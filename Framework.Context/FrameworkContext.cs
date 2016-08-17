using Framework.Context.Configurations;
using Framework.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Context
{
  public class FrameworkContext : DbContext
  {
    public FrameworkContext(string connection)
        : base(connection)
    {


    }
  
    public DbSet<Product> Products { get; set; }

    public DbSet<Log> Logs { get; set; }
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Configurations.Add(new ProductMap());
      modelBuilder.Configurations.Add(new LogMap());
    }


  }
}
