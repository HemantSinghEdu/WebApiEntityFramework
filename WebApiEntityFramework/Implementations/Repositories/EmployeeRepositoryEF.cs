using Microsoft.EntityFrameworkCore;
using WebApiEntityFramework.Contracts.Repositories;
using WebApiEntityFramework.DatabaseContext;
using WebApiEntityFramework.Models;

namespace WebApiEntityFramework.Implementations.Repositories
{
    public class EmployeeRepositoryEF : RepositoryEF<Employee>, IEmployeeRepository
    {
        public EmployeeRepositoryEF(InMemoryDbContext context) : base(context)
        {
        }
    }
}
