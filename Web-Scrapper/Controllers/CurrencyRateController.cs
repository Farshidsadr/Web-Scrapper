using Core.Application.Model.CurrencyRate;
using Core.Application.Services.Contracts;
using Core.Infrastructure.Domain.Entities;
using Core.Infrastructure.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Web_Scrapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyRateController : ControllerBase
    {
        #region Properties

        private readonly ICurrencyRateService _currencyRateService;

        #endregion

        #region Ctor

        public CurrencyRateController(ICurrencyRateService currencyRateService)
        {
            _currencyRateService = currencyRateService;
        }

        #endregion

        #region GetAllAsync

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<CurrencyRate>>> GetAllAsync()
        {
            var model = await _currencyRateService.GetAllAsync();
            return Ok(model);
        }

        #endregion

        #region GetAverageRateAsync

        [HttpPost("GetAverageRate")]
        public async Task<ActionResult<CurrencyRateModel?>> GetAverageRateAsync(DateTime? startDate, DateTime? endDate, 
            CurrencySymbol currencySymbol = CurrencySymbol.Dollar)
        {
            if (startDate == null || endDate == null)
            {
                return BadRequest("Dates are required.");
            }

            var result = await _currencyRateService.GetAverageRateAsync(startDate.Value, endDate.Value);
            return Ok(result);
        }

        #endregion

    }
}