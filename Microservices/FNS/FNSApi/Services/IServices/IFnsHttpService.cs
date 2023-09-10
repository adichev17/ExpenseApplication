using FNSApi.Models.Fns.Response;

namespace FNSApi.Services.IServices
{
    public interface IFnsHttpService
    {
        Task SendCodeAsync();
        Task<string> GetTokenAsync();
        Task<string> GetFromBarcodeAsync(string qr);
    }
}
