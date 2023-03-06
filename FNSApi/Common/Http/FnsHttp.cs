using Microsoft.AspNetCore.Http;

namespace FNSApi.Common.Http
{
    public class FnsHttp
    {
        public const string AuthPhone = "/auth/phone/request";
        public const string Verify = "/auth/phone/verify";
        public const string BarcodeId = "/ticket";
        public const string Barcode = "/tickets";
        public const string RefreshToken = "/mobile/users/refresh";
    }
}
