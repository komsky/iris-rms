using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Iris.Rms.Data
{
    public class DataSeeder
    {
        private readonly RmsDbContext _context;

        public DataSeeder(RmsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Seed()
        {
            _context.Database.Migrate();
            _context.SaveChanges();
            _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.RmsList ON;");
            _context.SaveChanges();
        }
    }
}
