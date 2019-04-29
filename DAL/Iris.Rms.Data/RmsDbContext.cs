using Iris.Rms.Models;
using Microsoft.EntityFrameworkCore;

namespace Iris.Rms.Data
{
    public class RmsDbContext :DbContext
    {
        public RmsDbContext(DbContextOptions<RmsDbContext> options) : base(options) { }
        public DbSet<RmsConfig> RmsList { get; set; }
        public DbSet<RmsDevice> Devices { get; set; }
    }
}
