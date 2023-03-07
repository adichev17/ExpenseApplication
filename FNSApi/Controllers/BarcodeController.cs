using FNSApi.Common.Errors;
using FNSApi.Models.Communication;
using FNSApi.Models.Dtos;
using FNSApi.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace FNSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BarcodeController : ControllerBase
    {
        private readonly IFnsHttpService _fnsHttpService;
        public BarcodeController(IFnsHttpService fnsHttpService)
        {
            _fnsHttpService = fnsHttpService;
        }

        [HttpPost]
        public async Task<IActionResult> SendCode()
        {
            try
            {
                await _fnsHttpService.SendCodeAsync();
                return NoContent();
            }
            catch (HttpRequestException httpEx)
            {
                if (!httpEx.StatusCode.HasValue)
                    throw;

                switch (httpEx.StatusCode)
                {
                    case HttpStatusCode.TooManyRequests:
                        return Problem(statusCode: (int)HttpStatusCode.TooManyRequests);
                    default:
                        throw;
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(string qrCode)
        {
            try
            {
                var ticketSerializable = await _fnsHttpService.GetFromBarcodeAsync(qrCode);
                if (string.IsNullOrWhiteSpace(ticketSerializable))
                    return Problem(statusCode: (int)HttpStatusCode.NotFound);

                var ticket = JsonSerializer.Deserialize<BarcodeResponse>(ticketSerializable);
                var barcodeResponseDto = 
                    new BarcodeResponseDto(
                        ticket.Ticket.Document.Receipt.Products.First().ProductTypeId, 
                        ticket.Operation.Amount,
                        ticket.Ticket.Document.Receipt.PlaceName);

                return Ok(barcodeResponseDto);
            }
            catch (HttpRequestException httpEx)
            {
                if (!httpEx.StatusCode.HasValue)
                    throw;

                switch (httpEx.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Problem(statusCode: (int)HttpStatusCode.NotFound);
                    default:
                        throw;
                }
            }
            catch(NullReferenceException)
            {
                return Problem(statusCode: (int)HttpStatusCode.InternalServerError, title: Errors.PhoneCodeNotFound);
            }
        }
    }
}
