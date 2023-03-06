using System.Text.Json.Serialization;

namespace FNSApi.Models.Fns.Requests
{
    public class FnsBarcodeIdRequest
    {
        [JsonPropertyName("qr")]
        public string Barcode { get; set; }
    }
}
