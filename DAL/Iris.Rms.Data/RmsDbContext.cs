using Iris.Rms.Models;
using Microsoft.EntityFrameworkCore;

namespace Iris.Rms.Data
{
    public class RmsDbContext :DbContext
    {
        public RmsDbContext(DbContextOptions<RmsDbContext> options) : base(options) { }
        public DbSet<RmsConfig> RmsList { get; set; }
        public DbSet<Light> Lights { get; set; }
        public DbSet<Hvac> Hvacs { get; set; }
        public DbSet<WebHook> WebHooks { get; set; }
    }
}
