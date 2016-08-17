using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Core.Contracts.Repository
{
  public interface IRepositoryBase { }
  public interface IRepositoryBase<T> : IRepositoryBase, IDisposable
      where T : class, new()
  {
    T GetSingle(Expression<Func<T, bool>> whereCondition, params System.Linq.Expressions.Expression<Func<T, object>>[] includes);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> whereCondition, params System.Linq.Expressions.Expression<Func<T, object>>[] includes);
    void Add(T entity);
    void Delete(T entity);
    void Attach(T entity);
    IQueryable<T> GetAll(Expression<Func<T, bool>> whereCondition);
    IQueryable<T> GetAll(params System.Linq.Expressions.Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> GetAllAsyc(Expression<Func<T, bool>> whereCondition);
    Task<IEnumerable<T>> GetAllAsyc(params System.Linq.Expressions.Expression<Func<T, object>>[] includes);
    long Count(Expression<Func<T, bool>> whereCondition);
    long Count();
    void Update(T entity);
    int SaveChanges();
    Task<int> SaveChangesAsync();
  }
}
