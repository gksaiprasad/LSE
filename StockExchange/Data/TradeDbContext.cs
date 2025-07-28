using Microsoft.EntityFrameworkCore;
using LSE.TradeApi.Models;

namespace LSE.TradeApi.Data
{
    public class TradeDbContext : DbContext
    {
        public TradeDbContext(DbContextOptions<TradeDbContext> options) : base(options) { }

        public DbSet<Trade> Trades { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Broker> Brokers { get; set; }

    }
}