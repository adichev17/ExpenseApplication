using System.Text.Json.Serialization;

namespace FNSApi.Models.Communication
{
    public class BarcodeResponse
    {
        [JsonPropertyName("qr")]
        public string Barcode { get; set; }

        [JsonPropertyName("operation")]
        public BarcodeOperation Operation { get; set; }

        [JsonPropertyName("organization")]
        public BarcodeOrganization Organization { get; set; }

        [JsonPropertyName("ticket")]
        public BarcodeTicket Ticket { get; set; }

        public class BarcodeOrganization
        {

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("inn")]
            public string Inn { get; set; }
        }

        public class BarcodeOperation
        {
            [JsonPropertyName("type")]
            public int ActionType { get; set; }

            [JsonPropertyName("sum")]
            public int Amount { get; set; }

            [JsonPropertyName("date")]
            public DateTime Created { get; set; }
        }

        public class BarcodeTicket
        {
            [JsonPropertyName("document")]
            public BarcodeDocument Document { get; set; }

            public class BarcodeDocument
            {
                [JsonPropertyName("receipt")]
                public BarcodeReceipt Receipt { get; set; }

                public class BarcodeReceipt
                {
                    [JsonPropertyName("retailPlace")]
                    public string PlaceName { get; set; }

                    [JsonPropertyName("items")]
                    public List<ReceiptItem> Products { get; set; }

                    public class ReceiptItem
                    {
                        [JsonPropertyName("productType")]
                        public int ProductTypeId { get; set; }

                        [JsonPropertyName("name")]
                        public string ProductName { get; set; }

                        [JsonPropertyName("sum")]
                        public decimal ProductAmount { get; set; }

                        [JsonPropertyName("quantity")]
                        public int ProductQuantity { get; set; }
                    }
                }
            }
        }
    }
}
