using Bogus;
using LSE.TradeApi.Models;

namespace LSE.TradeApi.Services
{
    public class FakeTradeService : ITradeService
    {
        private readonly List<Trade> _trades;

        public FakeTradeService()
        {
            _trades = GenerateFakeTrades();
        }

        public static List<Trade> GenerateFakeTrades(int count = 10)
        {
            var brokers = new List<Guid>
            {
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
            };

            var tickers = new[] { "AAPL", "GOOG", "MSFT", "AMZN", "TSLA" };

            var faker = new Faker<Trade>()
                .RuleFor(t => t.TickerSymbol, f => f.PickRandom(tickers))
                .RuleFor(t => t.Price, f => Math.Round(f.Random.Decimal(100, 1000), 2))
                .RuleFor(t => t.Shares, f => Math.Round(f.Random.Decimal(10, 500), 2))
                .RuleFor(t => t.BrokerId, f => f.PickRandom(brokers))
                .RuleFor(t => t.Timestamp, f => f.Date.RecentOffset(5).UtcDateTime);

            return faker.Generate(count);
        }

        public Task AddTradeAsync(Trade trade)
        {
            _trades.Add(trade);
            return Task.CompletedTask;
        }

        public Task<decimal?> GetAveragePriceAsync(string ticker)
        {
            var trades = _trades.Where(t => t.TickerSymbol == ticker);
            var avg = trades.Any() ? (decimal?)trades.Average(t => t.Price) : null;
            return Task.FromResult(avg);
        }

        public Task<List<StockPriceResponse>> GetAllAveragePricesAsync()
        {
            var results = _trades
                .GroupBy(t => t.TickerSymbol)
                .Select(g => new StockPriceResponse
                {
                    TickerSymbol = g.Key,
                    AveragePrice = Math.Round(g.Average(t => t.Price), 2)
                }).ToList();

            return Task.FromResult(results);
        }

        public Task<List<StockPriceResponse>> GetAveragePricesForTickersAsync(List<string> tickers)
        {
            var results = _trades
                .Where(t => tickers.Contains(t.TickerSymbol))
                .GroupBy(t => t.TickerSymbol)
                .Select(g => new StockPriceResponse
                {
                    TickerSymbol = g.Key,
                    AveragePrice = Math.Round(g.Average(t => t.Price), 2)
                }).ToList();

            return Task.FromResult(results);
        }
    }
}
