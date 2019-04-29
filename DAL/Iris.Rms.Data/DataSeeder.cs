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
            if (!_context.RmsList.Any())
            {
                _context.RmsList.Add(new Models.RmsConfig { Description = "First and only RMS... for now" });

            }

            _context.SaveChanges();
        }
    }
}
