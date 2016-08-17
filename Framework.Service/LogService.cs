using Framework.DomainModel;
using Framework.Repository.Contracts;
using Framework.Service.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Service
{
  [Export(typeof(ILogService))]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public class LogService : ILogService
  {
    ILogRepository repository;
    [ImportingConstructor]
    public LogService(ILogRepository repository)
    {
      this.repository = repository;
    }


    public void WriteLog(LogType LogType, string Event, string Message, string ExtraInfo = null, string ObjectId = null)
    {
      this.repository.WriteLog(LogType, Event: Event, Message: Message, ExtraInfo: ExtraInfo, ObjectId: ObjectId);
    }

    public void WriteLog(LogType LogType, Exception exp, string Event = null, string ExtraInfo = null, string ObjectId = null)
    {
      this.repository.WriteLog(LogType, Event: Event, Message: exp.Message, ExtraInfo: ExtraInfo, ObjectId: ObjectId);

    }

    public void WriteLog(Log log)
    {
      this.repository.WriteLog(log);
    }


    public IQueryable<Log> GetAll(params Expression<Func<Log, object>>[] includes)
    {
      return repository.GetAll(includes);
    }

  }
}
