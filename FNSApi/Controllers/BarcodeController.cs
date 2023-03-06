using FNSApi.Services.IServices;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> Get(string qrCode)
        {
            var tickets = await _fnsHttpService.GetFromBarcodeAsync(qrCode);
            return Ok(tickets);
        }
    }
}
