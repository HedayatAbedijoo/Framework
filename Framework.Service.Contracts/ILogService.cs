using Framework.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Service.Contracts
{
  public interface ILogService
  {
    void WriteLog(LogType LogType, string Event, string Message, string ExtraInfo = null, string ObjectId = null);
    void WriteLog(LogType LogType, Exception exp, string Event = null, string ExtraInfo = null, string ObjectId = null);
    void WriteLog(Log log);

    IQueryable<Log> GetAll(params Expression<Func<Log, object>>[] includes);

  }
}
