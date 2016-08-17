using Framework.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.Contracts
{
  public interface ILogRepository
  {
    void WriteLog(LogType LogType, string Event, string Message, string ExtraInfo = null, string ObjectId = null);
    void WriteLog(LogType LogType, Exception exp, string Event = null, string ExtraInfo = null, string ObjectId = null);
    void WriteLog(Log log);
    void WroteLogTofile(Exception ex, string sourceMessageLog);
    void WroteLogTofile(string sourceMessageLog);

    IQueryable<Log> GetAll(params Expression<Func<Log, object>>[] includes);
  }
}
