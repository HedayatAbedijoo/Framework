using System.Data.Entity;
using System.Threading.Tasks;

namespace Framework.Core.Contracts.Repository
{
  public interface IUnitOfWork
  {
    DbContext Context { get; }   
  }
}
