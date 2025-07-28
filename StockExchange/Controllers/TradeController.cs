using Microsoft.AspNetCore.Mvc;
using LSE.TradeApi.Models;
using LSE.TradeApi.Services;

namespace LSE.TradeApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;
        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitTrade([FromBody] Trade trade)
        {
            if (trade == null)
                return BadRequest("Trade data is required.");
            await _tradeService.AddTradeAsync(trade);
            return Ok();
        }

        [HttpGet("stock/{ticker}")]
        public async Task<IActionResult> GetAveragePrice(string ticker)
        {
            if (string.IsNullOrWhiteSpace(ticker))
                return BadRequest("Ticker is required.");
            var price = await _tradeService.GetAveragePriceAsync(ticker);
            return Ok(new { Ticker = ticker, AveragePrice = price });
        }

        [HttpGet("stocks/all")]
        public async Task<IActionResult> GetAllAveragePrices()
        {
            return Ok(await _tradeService.GetAllAveragePricesAsync());
        }

        [HttpPost("stocks/range")]
        public async Task<IActionResult> GetPricesForTickers([FromBody] List<string> tickers)
        {
            return Ok(await _tradeService.GetAveragePricesForTickersAsync(tickers));
        }
    }
}