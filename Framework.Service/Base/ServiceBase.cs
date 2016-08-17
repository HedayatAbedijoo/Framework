using Framework.Core.Contracts.Service;
using Framework.Service.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Framework.Common.Extensions;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Contracts.Repository;
using System.Linq.Expressions;
using System.Data.SqlClient;

namespace Framework.Service.Base
{
  public abstract class ServiceBase<T> : ServiceBase, IServiceBase<T>, IDisposable
       where T : class, new()
  {
    IRepositoryBase<T> repository;
    public ServiceBase(IRepositoryBase<T> repository)
    {
      this.repository = repository;
    }
    public virtual ServiceContract<T> GetSingle(Expression<Func<T, bool>> whereCondition, params Expression<Func<T, object>>[] includes)
    {
      return RunServiceCode(() =>
      {
        return repository.GetSingle(whereCondition, includes);
      });

    }
    public virtual ServiceContract<IQueryable<T>> GetAll(params Expression<Func<T, object>>[] includes)
    {
      return RunServiceCode(() =>
      {
        return repository.GetAll(includes);
      });
    }

    public virtual ServiceContract Create(T entity)
    {
      var item = RunServiceCode(() =>
      {
        repository.Add(entity);

      });
      return item;

    }

    public virtual ServiceContract Delete(T entity)
    {
      var item = RunServiceCode(() =>
      {
        repository.Delete(entity);
      });
      return item;
    }

    public virtual ServiceContract<IQueryable<T>> GetAll(Expression<Func<T, bool>> whereCondition)
    {
      return RunServiceCode(() =>
      {
        return repository.GetAll(whereCondition);
      });
    }

    public virtual ServiceContract<long> Count(Expression<Func<T, bool>> whereCondition)
    {
      return RunServiceCode<long>(() =>
      {
        return repository.Count(whereCondition);
      });
    }

    public virtual ServiceContract<long> Count()
    {
      return RunServiceCode<long>(() =>
      {
        return repository.Count();
      });
    }

    public virtual ServiceContract Update(T entity)
    {
      var item = RunServiceCode(() =>
      {
        repository.Update(entity);
      });

      return item;
    }

    public virtual ServiceContract SaveChanges()
    {

      var contract = RunServiceCode(() =>
      {
        repository.SaveChanges();
      });
      if (contract.Result == OperationResult.Success)
        contract.Message = "The operation has been done successfully.";

      return contract;
    }

    public async Task<ServiceContract> SaveChangesAsync()
    {
      var contract = new ServiceContract();
      try
      {
        await repository.SaveChangesAsync();
        contract.Result = OperationResult.Success;
      }
      catch (Exception exp)
      {
        contract.Result = OperationResult.Error;
        contract.Exception = ServiceException.Factory(exp);
      }

      return contract;

    }

    public void Dispose()
    {
      this.repository.Dispose();
    }

    public async Task<ServiceContract<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>> whereCondition)
    {
      var contract = new ServiceContract<IEnumerable<T>>();

      try
      {
        contract.Item = await repository.GetAllAsyc(whereCondition);
        contract.Result = OperationResult.Success;
      }
      catch (Exception exp)
      {

        contract.Result = OperationResult.Error;
        contract.Message = exp.Message;
      }

      return contract;
    }

    public async Task<ServiceContract<IEnumerable<T>>> GetAllAsync(params Expression<Func<T, object>>[] includes)
    {
      var contract = new ServiceContract<IEnumerable<T>>();

      try
      {
        contract.Item = await repository.GetAllAsyc(includes);
        contract.Result = OperationResult.Success;
      }
      catch (Exception exp)
      {

        contract.Result = OperationResult.Error;
        contract.Message = exp.Message;
      }

      return contract;
    }

    public async Task<ServiceContract<T>> GetSingleAsync(Expression<Func<T, bool>> whereCondition, params Expression<Func<T, object>>[] includes)
    {
      var contract = new ServiceContract<T>();

      try
      {
        contract.Item = await repository.GetSingleAsync(whereCondition, includes);
        contract.Result = OperationResult.Success;
      }
      catch (Exception exp)
      {

        contract.Result = OperationResult.Error;
        contract.Message = exp.Message;
      }
      return contract;
    }
  }

  public abstract class ServiceBase
  {
    [Import]
    ILogService log;
    protected ServiceContract<T> RunServiceCode<T>(Func<T> codetoExecute)
    {
      ServiceContract<T> contract = new ServiceContract<T>();

      try
      {
        contract.Item = codetoExecute.Invoke();
        contract.Result = OperationResult.Success;
      }
      catch (Exception ex)
      {
        contract.Result = OperationResult.Error;
        contract.Exception = ServiceException.Factory(ex);
        contract.Message = "The operation could not be done.";

        if (log != null)
        {
          log.WriteLog(DomainModel.LogType.Emergency, Message: ex.GetInnermostException(), Event: this.GetType().Name + "=> RunServiceCode");
        }
      }

      return contract;
    }

    protected ServiceContract RunServiceCode(Action codetoExecute)
    {
      ServiceContract contract = new ServiceContract();

      try
      {
        codetoExecute.Invoke();
        contract.Result = OperationResult.Success;
      }
      catch (SqlException ex)
      {

        contract.Result = OperationResult.Error;
        contract.Exception = ServiceException.Factory(ex);
        if (log != null)
        {
          log.WriteLog(DomainModel.LogType.Emergency, Message: ex.GetInnermostException(), Event: this.GetType().Name + "=> RunServiceCode");
        }
      }
      catch (Exception ex)
      {
        contract.Result = OperationResult.Error;
        contract.Exception = ServiceException.Factory(ex);
        contract.Message = "The operation could not be done.";
        if (log != null)
        {
          log.WriteLog(DomainModel.LogType.Emergency, Message: ex.GetInnermostException(), Event: this.GetType().Name + "=> RunServiceCode");
        }
      }

      return contract;
    }

    protected ServiceContract RunCodeOtherData<T>(Func<T> codetoExecute)
    {
      ServiceContract contract = new ServiceContract();

      try
      {
        contract.ExtraData = codetoExecute.Invoke();
        contract.Result = OperationResult.Success;

      }
      catch (Exception ex)
      {
        contract.Result = OperationResult.Error;
        contract.Exception = ServiceException.Factory(ex);
        contract.Message = "The operation could not be done.";
        if (log != null)
        {
          log.WriteLog(DomainModel.LogType.Emergency, Message: ex.GetInnermostException(), Event: this.GetType().Name + "=> RunServiceCode");
        }
      }

      return contract;
    }


  }
}
