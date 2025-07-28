using LSE.TradeApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LSE.TradeApi.Services
{
    public interface ITradeService
    {
        Task AddTradeAsync(Trade trade);
        Task<decimal?> GetAveragePriceAsync(string ticker);
        Task<List<StockPriceResponse>> GetAllAveragePricesAsync();
        Task<List<StockPriceResponse>> GetAveragePricesForTickersAsync(List<string> tickers);
    }
}