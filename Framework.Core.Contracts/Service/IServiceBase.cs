using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Contracts.Service
{
  public interface IServiceBase { }

  public interface IServiceBase<T> : IServiceBase
     where T : class, new()
  {
    ServiceContract<T> GetSingle(Expression<Func<T, bool>> whereCondition, params System.Linq.Expressions.Expression<Func<T, object>>[] includes);
    Task<ServiceContract<T>> GetSingleAsync(Expression<Func<T, bool>> whereCondition, params System.Linq.Expressions.Expression<Func<T, object>>[] includes);
    ServiceContract Create(T entity);
    ServiceContract Delete(T entity);
    ServiceContract<long> Count(Expression<Func<T, bool>> whereCondition);
    ServiceContract<long> Count();
    ServiceContract SaveChanges();
    Task<ServiceContract> SaveChangesAsync();
    ServiceContract<IQueryable<T>> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition);
    ServiceContract<IQueryable<T>> GetAll(params System.Linq.Expressions.Expression<Func<T, object>>[] includes);
    Task<ServiceContract<IEnumerable<T>>> GetAllAsync(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition);
    Task<ServiceContract<IEnumerable<T>>> GetAllAsync(params System.Linq.Expressions.Expression<Func<T, object>>[] includes);
    ServiceContract Update(T entity);
  }
}
