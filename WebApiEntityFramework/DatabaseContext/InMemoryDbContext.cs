using Microsoft.EntityFrameworkCore;
using WebApiEntityFramework.Models;

namespace WebApiEntityFramework.DatabaseContext
{
    public class InMemoryDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "EmployeeDb");
        }

        public DbSet<Employee> Employees { get; set; } 
    }
}
