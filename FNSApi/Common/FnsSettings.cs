using System.Text.Json.Serialization;

namespace FNSApi.Common
{
    public class FnsSettings
    {
        public string BaseAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string SecretKey { get; set; }
        public string OperationSystem {  get; set; }
    }
}
