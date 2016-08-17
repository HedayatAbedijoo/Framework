using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Common.Extensions;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Framework.Core.Contracts.Repository;
using System.Data.Entity;
using System.ComponentModel.Composition;
using System.Data.Entity.Validation;
using Framework.Repository.Contracts;
using System.Reflection;
using Framework.DomainModel;
namespace SiutSupply.Repository
{
  public abstract class RepositoryBase<T> : IRepositoryBase<T>, IDisposable
      where T : class, new()
  {
    private DbContext _context;
    public DbContext Context
    {
      get { return _context; }
      private set { _context = value; }
    }
    private IDbSet<T> _dbSet;
    public IDbSet<T> DbSet
    {
      get
      {
        return _dbSet;
      }
    }

    ILogRepository log;

    [ImportingConstructor]
    public RepositoryBase(IUnitOfWork unitOfWork, ILogRepository log)
    {
      _context = unitOfWork.Context;
      this.log = log;
      _dbSet = _context.Set<T>();
    }
    public T GetSingle(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition, params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
    {
      try
      {
        IQueryable<T> query = _dbSet;
        if (includes != null && includes.Any())
        {
          query = includes.Aggregate(query, (current, include) => current.Include(include));
        }
        var entity = query.FirstOrDefault(whereCondition);
        RefreshEntity(entity);
        return entity;
      }
      catch (Exception exp)
      {

        if (log != null)
        {
          log.WriteLog(LogType.Emergency, Message: exp.GetInnermostException(), Event: this.GetType().Name + "=>GetSingle", ExtraInfo: getOriginalValues());
        }
        throw exp;
      }


    }
    public void Add(T entity)
    {
      _dbSet.Add(entity);
    }
    public void Update(T entity)
    {
      // Existing entity
      Context.Entry(entity).State = EntityState.Modified;
    }
    public void Delete(T entity)
    {
      _dbSet.Remove(entity);
    }
    public void Attach(T entity)
    {
      _dbSet.Attach(entity);
    }
    public IQueryable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition)
    {
      try
      {
        IQueryable<T> query = _dbSet;
        return query.Where(whereCondition);
      }
      catch (Exception exp)
      {

        if (log != null)
        {
          log.WriteLog(LogType.Emergency, Message: exp.GetInnermostException(), Event: this.GetType().Name + "=>GetAll", ExtraInfo: getOriginalValues());
        }
        throw exp;
      }
    }
    public long Count(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition)
    {
      return _dbSet.Count(whereCondition);
    }
    public long Count()
    {
      return _dbSet.Count();
    }
    public int SaveChanges()
    {
      try
      {
        var changes = getOriginalValues() + getCurrentValues();
        var result = Context.SaveChanges();
        if (log != null)
          log.WriteLog(LogType.Informational, this.GetType().Name + "=>Save Changes", "", ExtraInfo: changes);
        return result;

      }
      catch (DbEntityValidationException ex)
      {
        StringBuilder error = new StringBuilder();
        foreach (var eve in ex.EntityValidationErrors)
        {
          foreach (var ve in eve.ValidationErrors)
          {
            error.AppendLine(string.Format("\"{0}\" : {1}" + Environment.NewLine,
                ve.PropertyName, ve.ErrorMessage));
          }
        }
        if (log != null)
        {
          log.WriteLog(LogType.Error, Message: error.ToString(), Event: this.GetType().Name + "=>Save Changes", ExtraInfo: getOriginalValues());
        }
        throw new Exception(error.ToString());
      }
      catch (Exception ex)
      {
        if (log != null)
        {
          log.WriteLog(LogType.Error, Message: ex.GetInnermostException(), Event: this.GetType().Name + "=>Save Changes", ExtraInfo: getOriginalValues());
        }
        throw ex;
      }

    }
    public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
    {
      try
      {
        IQueryable<T> query = _dbSet;
        foreach (var include in includes)
        {
          query = query.Include(include);
        }
        return query;
      }
      catch (Exception exp)
      {

        if (log != null)
        {
          log.WriteLog(LogType.Emergency, Message: exp.GetInnermostException(), Event: this.GetType().Name + "=>GetAll", ExtraInfo: getOriginalValues());
        }
        throw exp;
      }
    }

    private void RefreshEntity(T entity)
    {
      if (entity != null)
        _context.Entry(entity).Reload();
    }

    private string getCurrentValues()
    {
      try
      {
        var changeInfo = this.Context.ChangeTracker.Entries()
               .Where(t => t.State == EntityState.Modified || t.State == EntityState.Deleted || t.State == EntityState.Added)
               .Select(t => new
               {
                 Current = t.CurrentValues.PropertyNames.ToDictionary(pn => pn, pn => t.CurrentValues[pn]),
                 Name = t.Entity.GetType().Name,
                 Operation = t.State
               });

        if (changeInfo == null || changeInfo.Count() == 0)
          return string.Empty;

        StringBuilder info = new StringBuilder();
        foreach (var item in changeInfo)
        {
          info.AppendLine(item.Name + "=>");

          info.AppendLine(item.Operation + "=>");

          info.AppendLine("Current Values: =>");
          foreach (var crt in item.Current)
          {
            info.AppendLine(crt.Key + " =  " + crt.Value);
          }
          info.AppendLine();
        }
        return info.ToString();
      }
      catch (Exception exp)
      {

        return string.Empty;
      }
    }
    private string getOriginalValues()
    {
      try
      {
        var changeInfo = this.Context.ChangeTracker.Entries()
               .Where(t => t.State == EntityState.Modified || t.State == EntityState.Deleted || t.State == EntityState.Added)
               .Select(t => new
               {
                 Original = t.OriginalValues.PropertyNames.ToDictionary(pn => pn, pn => t.OriginalValues[pn]),
                 Name = t.Entity.GetType().Name,
                 Operation = t.State
               });

        if (changeInfo == null || changeInfo.Count() == 0)
          return string.Empty;

        StringBuilder info = new StringBuilder();
        foreach (var item in changeInfo)
        {
          info.AppendLine(item.Name + "=>");

          info.AppendLine(item.Operation + "=>");

          info.AppendLine("Original Values: =>");
          foreach (var org in item.Original)
          {
            info.AppendLine(org.Key + " =  " + org.Value);
          }
          info.AppendLine();
        }
        return info.ToString();
      }
      catch (Exception)
      {

        return string.Empty;
      }
    }

    public void Dispose()
    {
      this._context.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
      try
      {
        var changes = getOriginalValues() + getCurrentValues(); // Just to show auditing is working. It is enough to have auditing on exception block.
        var result = await Context.SaveChangesAsync();
        if (log != null)
          log.WriteLog(LogType.Informational, this.GetType().Name + "=>Save Changes", "", ExtraInfo: changes);
        return result;

      }
      catch (DbEntityValidationException ex)
      {
        StringBuilder error = new StringBuilder();
        foreach (var eve in ex.EntityValidationErrors)
        {
          foreach (var ve in eve.ValidationErrors)
          {
            error.AppendLine(string.Format("\"{0}\" : {1}" + Environment.NewLine,
                ve.PropertyName, ve.ErrorMessage));
          }
        }
        if (log != null)
        {
          log.WriteLog(LogType.Error, Message: error.ToString(), Event: this.GetType().Name + "=>Save Changes", ExtraInfo: getOriginalValues());
        }
        throw new Exception(error.ToString());
      }
      catch (Exception ex)
      {
        if (log != null)
        {
          log.WriteLog(LogType.Error, Message: ex.GetInnermostException(), Event: this.GetType().Name + "=>Save Changes", ExtraInfo: getOriginalValues());
        }
        throw ex;
      }
    }

    public async Task<IEnumerable<T>> GetAllAsyc(Expression<Func<T, bool>> whereCondition)
    {
      try
      {
        IQueryable<T> query = _dbSet;
        return await query.Where(whereCondition).ToListAsync();
      }
      catch (Exception exp)
      {

        if (log != null)
        {
          log.WriteLog(LogType.Emergency, Message: exp.GetInnermostException(), Event: this.GetType().Name + "=>GetAll", ExtraInfo: getOriginalValues());
        }
        throw exp;
      }
    }

    public async Task<IEnumerable<T>> GetAllAsyc(params Expression<Func<T, object>>[] includes)
    {
      try
      {
        IQueryable<T> query = _dbSet;
        foreach (var include in includes)
        {
          query = query.Include(include);
        }
        return await query.ToListAsync();
      }
      catch (Exception exp)
      {

        if (log != null)
        {
          log.WriteLog(LogType.Emergency, Message: exp.GetInnermostException(), Event: this.GetType().Name + "=>GetAll", ExtraInfo: getOriginalValues());
        }
        throw exp;
      }
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> whereCondition, params Expression<Func<T, object>>[] includes)
    {
      try
      {
        IQueryable<T> query = _dbSet;
        if (includes != null && includes.Any())
        {
          query = includes.Aggregate(query, (current, include) => current.Include(include));
        }
        var entity = await query.FirstOrDefaultAsync(whereCondition);
        RefreshEntity(entity);
        return entity;
      }
      catch (Exception exp)
      {

        if (log != null)
        {
          log.WriteLog(LogType.Emergency, Message: exp.GetInnermostException(), Event: this.GetType().Name + "=>GetSingle", ExtraInfo: getOriginalValues());
        }
        throw exp;
      }
    }
  }


}
