using FNSApi.Models.Fns.Response;

namespace FNSApi.Services.IServices
{
    public interface IFnsHttpService
    {
        Task SendCodeAsync();
        Task<string> GetTokenAsync(string phoneCode);
        Task<string> GetFromBarcodeAsync(string qr);
    }
}
