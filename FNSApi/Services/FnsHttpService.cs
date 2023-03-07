using FNSApi.Common;
using FNSApi.Common.Cache;
using FNSApi.Common.Http;
using FNSApi.Models.Fns.Requests;
using FNSApi.Models.Fns.Response;
using FNSApi.Services.IServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace FNSApi.Services
{
    public sealed class FnsHttpService : IFnsHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly FnsSettings FnsSettings;
        private readonly IMemoryCache _cache;
        public FnsHttpService(
            IOptions<FnsSettings> fnsHttp,
            HttpClient httpClient,
            IMemoryCache cache)
        {
            _httpClient = httpClient;
            FnsSettings = fnsHttp.Value;
            _cache = cache;
        }

        public async Task SendCodeAsync()
        {
            string url = $"{_httpClient.BaseAddress}{FnsHttp.AuthPhone}";
            var payload = new FnsAuthRequest
            {
                PhoneNumber = FnsSettings.PhoneNumber,
                SecretKey = FnsSettings.SecretKey,
                OperationSystem = FnsSettings.OperationSystem
            };
            var responseMessage = await _httpClient.PostAsJsonAsync(url, payload);
            responseMessage.EnsureSuccessStatusCode();
        }

        public async Task<string> GetTokenAsync()
        {
            string token = string.Empty;

            if (_cache.TryGetValue(CacheItemKeys.Token, out token))
            {
                return token;
            }
            if (_httpClient.DefaultRequestHeaders.Contains(HttpContextItemKeys.SessionId))
                _httpClient.DefaultRequestHeaders.Remove(HttpContextItemKeys.SessionId);

            if (!_cache.TryGetValue(CacheItemKeys.RefreshToken, out string refreshToken))
            {
                if (!_cache.TryGetValue(CacheItemKeys.PhoneCode, out string phoneCode))
                {
                    throw new NullReferenceException(nameof(phoneCode));
                }
                token = await GetTokenByPhoneCode(phoneCode);
            }
            else
            {
                string url = $"{_httpClient.BaseAddress}{FnsHttp.RefreshToken}";
                var fnsRefreshTokenRequest = new FnsRefreshTokenRequest
                {
                    RefreshToken = refreshToken,
                    SecretKey = FnsSettings.SecretKey,
                };

                var response = await _httpClient.PostAsJsonAsync(url, fnsRefreshTokenRequest);
                var fnsTokenResponse = await response.Content.ReadFromJsonAsync<FnsRefreshTokenResponse>();

                SetTokenCache(fnsTokenResponse.Token, fnsTokenResponse.RefreshToken);
                token = fnsTokenResponse.Token;
            }

            return token;
        }

        protected async Task<string> GetTokenByPhoneCode(string phoneCode)
        {
            if (string.IsNullOrWhiteSpace(phoneCode)) throw new ArgumentException(nameof(phoneCode));

            string url = $"{_httpClient.BaseAddress}{FnsHttp.Verify}";
            var fnsTokenRequest = new FnsTokenRequest
            {
                PhoneNumber = FnsSettings.PhoneNumber,
                SecretKey = FnsSettings.SecretKey,
                OperationSystem = FnsSettings.OperationSystem,
                PhoneCode = phoneCode
            };
            fnsTokenRequest.PhoneCode = phoneCode;

            var response = await _httpClient.PostAsJsonAsync(url, fnsTokenRequest);
            var fnsTokenResponse = await response.Content.ReadFromJsonAsync<FnsTokenResponse>();

            SetTokenCache(fnsTokenResponse.Token, fnsTokenResponse.RefreshToken);
            return fnsTokenResponse.Token;
        }

        public async Task<string> GetFromBarcodeAsync(string qr)
        {
            if (string.IsNullOrWhiteSpace(qr)) throw new ArgumentException(qr, nameof(qr));

            var token = await GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Add(HttpContextItemKeys.SessionId, token);
            var barcodeId = await GetBarcodeIdAsync(qr, token);
            string url = $"{_httpClient.BaseAddress}{FnsHttp.Barcode}/{barcodeId}";
            var responseMessage = await _httpClient.GetAsync(url);
            var response = await responseMessage.Content.ReadAsStringAsync();
            return response;
        }


        protected async Task<string> GetBarcodeIdAsync(string qr, string token)
        {
            if (string.IsNullOrWhiteSpace(qr)) throw new ArgumentException(nameof(qr));
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentException(nameof(token));

            string url = $"{_httpClient.BaseAddress}{FnsHttp.BarcodeId}";
            var fnsBarcodeIdResponse = await _httpClient.PostAsJsonAsync(url, new FnsBarcodeIdRequest { Barcode = qr });
            fnsBarcodeIdResponse.EnsureSuccessStatusCode();
            var fnsBarcodeId = await fnsBarcodeIdResponse.Content.ReadFromJsonAsync<FnsBarcodeIdResponse>();
            return fnsBarcodeId.BarcodeId;
        }

        private void SetTokenCache(string token, string refreshToken)
        {
            _cache.Set(CacheItemKeys.Token, token,
                      new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(14)));
            _cache.Set(CacheItemKeys.RefreshToken, refreshToken,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(60)));
        }
    }
}
