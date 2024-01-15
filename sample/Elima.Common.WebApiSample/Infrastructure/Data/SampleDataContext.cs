using Elima.Common.EntityFramework.EntityFrameworkCore;
using Elima.Common.WebApiSample.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Elima.Common.WebApiSample.Infrastructure.Data
{
    public class SampleDataContext : DatabaseContext<SampleDataContext>
    {
        public DbSet<SampleModel> Samples { get; set; }
        public SampleDataContext(DbContextOptions<SampleDataContext> options) : base(options)
        {
        }
    }
}
