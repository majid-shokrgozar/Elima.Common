using Elima.Common.Domain.Entities;

namespace Elima.Common.WebApiSample.Domain.Entity
{
    public class SampleModel:Entity<Guid>
    {
        public SampleModel()
        {
                this.Id = Guid.NewGuid();
        }
        public string Titile { get; set; } = string.Empty;
    }
}
