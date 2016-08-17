using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel
{
  public class Product
  {
    public System.Guid Id { get; set; }
    public string Name { get; set; }
    public string Photo { get; set; }
    public decimal Price { get; set; }
    public DateTime LastUpdated { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; }
  }
}
