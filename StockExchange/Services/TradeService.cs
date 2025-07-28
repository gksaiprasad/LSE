using LSE.TradeApi.Data;
using LSE.TradeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LSE.TradeApi.Services
{
    public class TradeService : ITradeService
    {
        private readonly TradeDbContext _context;

        public TradeService(TradeDbContext context)
        {
            _context = context;
        }

        public async Task AddTradeAsync(Trade trade)
        {
            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal?> GetAveragePriceAsync(string ticker)
        {
            if (string.IsNullOrWhiteSpace(ticker)) return null;
            return await _context.Trades
                .Where(t => t.TickerSymbol == ticker)
                .AverageAsync(t => (decimal?)t.Price);
        }

        public async Task<List<StockPriceResponse>> GetAllAveragePricesAsync()
        {
            return await _context.Trades
                .GroupBy(t => t.TickerSymbol)
                .Select(g => new StockPriceResponse
                {
                    TickerSymbol = g.Key,
                    AveragePrice = g.Average(t => t.Price)
                })
                .ToListAsync();
        }

        public async Task<List<StockPriceResponse>> GetAveragePricesForTickersAsync(List<string> tickers)
        {
            return await _context.Trades
                .Where(t => tickers.Contains(t.TickerSymbol))
                .GroupBy(t => t.TickerSymbol)
                .Select(g => new StockPriceResponse
                {
                    TickerSymbol = g.Key,
                    AveragePrice = g.Average(t => t.Price)
                })
                .ToListAsync();
        }
    }
}