using Microsoft.EntityFrameworkCore;

namespace Practice2.Data
{
    public class BankContext :DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options)
        {

        }

        public DbSet<Models.Bank> Banks { get; set; }
    }
}
