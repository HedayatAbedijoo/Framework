using Framework.DomainModel;
using System.Data.Entity.ModelConfiguration;

namespace Framework.Context.Configurations
{
  public class ProductMap : EntityTypeConfiguration<Product>
  {
    public ProductMap()
    {
      this.HasKey(t => t.Id);
      this.Property(t => t.Name).HasMaxLength(50).IsRequired();
      this.Property(t => t.Price).IsRequired();
      this.Property(t => t.Photo).HasMaxLength(255);
      this.Property(t => t.RowVersion).IsRequired().IsConcurrencyToken();
    }
  }
}
