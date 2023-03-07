namespace FNSApi.Models.Dtos
{
    public record class BarcodeResponseDto(
        int CategoryId,
        decimal Amount,
        string PlaceName);
}
