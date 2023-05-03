using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Models.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public DbSet<AppUser> AppUsers { get; set; }

        internal Task FindAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
