using FNSApi.Common;
using FNSApi.Common.Http;
using FNSApi.Models.Fns.Requests;
using FNSApi.Models.Fns.Response;
using FNSApi.Services.IServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net;

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
            try
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
            catch (HttpRequestException httpEx)
            {
                if (!httpEx.StatusCode.HasValue)
                    throw;

                switch (httpEx.StatusCode)
                {
                    case HttpStatusCode.TooManyRequests:
                        await Task.Delay(TimeSpan.FromMinutes(3));
                        break;
                    default:
                        throw;
                }
            }
        }

        public async Task<string> GetTokenAsync(string phoneCode = "2373")
        {
            if (string.IsNullOrWhiteSpace(phoneCode))
                throw new ArgumentException(phoneCode, nameof(phoneCode));

            string token = string.Empty;
            if (!_cache.TryGetValue("token", out token))
            {
                if (_httpClient.DefaultRequestHeaders.Contains(HttpContextItemKeys.SessionId))
                    _httpClient.DefaultRequestHeaders.Remove(HttpContextItemKeys.SessionId);
              
                if (!_cache.TryGetValue("refreshToken", out string refreshToken))
                {
                    string url = $"{_httpClient.BaseAddress}{FnsHttp.Verify}";
                    var fnsTokenRequest = new FnsTokenRequest {
                        PhoneNumber = FnsSettings.PhoneNumber,
                        SecretKey = FnsSettings.SecretKey,
                        OperationSystem = FnsSettings.OperationSystem,
                        PhoneCode = phoneCode
                    };
                    fnsTokenRequest.PhoneCode = phoneCode;

                    var response = await _httpClient.PostAsJsonAsync(url, fnsTokenRequest);
                    var fnsTokenResponse = await response.Content.ReadFromJsonAsync<FnsTokenResponse>();

                    SetTokenCache(fnsTokenResponse.Token, fnsTokenResponse.RefreshToken);
                    token = fnsTokenResponse.Token;
                } else
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
            }

            return token;
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
            var fnsBarcodeId = await fnsBarcodeIdResponse.Content.ReadFromJsonAsync<FnsBarcodeIdResponse>();
            return fnsBarcodeId.BarcodeId;
        }

        private void SetTokenCache(string token, string refreshToken)
        {
            _cache.Set("token", token,
                      new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(14)));
            _cache.Set("refreshToken", refreshToken,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(60)));
        }
    }
}
