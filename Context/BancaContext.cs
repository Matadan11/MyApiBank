using Microsoft.EntityFrameworkCore;
using MyApiBank.Models;

namespace MyApiBank.Context
{
    public class BancaContext: DbContext
    {
        public BancaContext()
        {
        }

        public BancaContext(DbContextOptions<BancaContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}