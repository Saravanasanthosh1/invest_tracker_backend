
// Data/AppDbContext.cs
using investtracker.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace investtracker.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AuthUser> AuthUsers { get; set; }
        public DbSet<Portfolio> Portfolio { get; set; }
        public DbSet<MonthlyCommitment> MonthlyCommitments { get; set; }
    }

    public class Portfolio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } = "";
        public decimal Value { get; set; }
    }

    public class MonthlyCommitment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Month { get; set; } = "";
        public decimal Amount { get; set; }
    }

}
