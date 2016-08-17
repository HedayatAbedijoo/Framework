using Framework.DomainModel;
using System.Data.Entity.ModelConfiguration;

namespace Framework.Context.Configurations
{
  public class LogMap : EntityTypeConfiguration<Log>
  {
    public LogMap()
    {
      this.HasKey(t => t.Id);
    }
  }
}
