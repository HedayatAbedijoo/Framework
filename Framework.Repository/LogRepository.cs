using Framework.Core.Contracts.Repository;
using Framework.DomainModel;
using Framework.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository
{
  [Export(typeof(ILogRepository))]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public class LogRepository : ILogRepository
  {
    IDbContextFactory contextFactory;
    [ImportingConstructor]
    public LogRepository(IDbContextFactory contextFactory)
    {
      this.contextFactory = contextFactory;

    }


    #region Log in text file if there is any problem in logging functionality
    /// <summary>
    /// If for any reason the logging system can not create log in SQL Server database, it will log inside a text file
    /// </summary>
    /// <param name="ex"></param>
    public void WroteLogTofile(Exception ex, string sourceMessageLog)
    {
      try
      {
        string errorMessage = string.Empty;
        if (ex != null)
        {
          errorMessage = "Logging Exception: " + ex.Source + " --> " + ex.Message + "<-- Logging System Message: " + sourceMessageLog;
        }
        else
        {
          errorMessage = sourceMessageLog;
        }

        string executingPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).ToLower().Replace(@"file:\", "");

        string filename = String.Format("{0:yyyy-MM-dd}__{1}", DateTime.Now, "log.txt");

        if (!String.IsNullOrEmpty(executingPath))
        {
          System.IO.File.AppendAllText(executingPath + "\\" + filename, Environment.NewLine + "----- D ----".Replace("D", DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString()) + Environment.NewLine);
          System.IO.File.AppendAllText(executingPath + "\\" + filename, errorMessage + Environment.NewLine);
        }
      }
      catch (Exception)
      {
      }
    }
    public void WroteLogTofile(string sourceMessageLog)
    {
      try
      {
        WroteLogTofile(null, sourceMessageLog);
      }
      catch (Exception)
      {
      }
    }
    #endregion

    public void WriteLog(LogType LogType, string Event, string Message, string ExtraInfo = null, string ObjectId = null)
    {
      Log newLog = new Log();
      newLog.Type = LogType;
      newLog.Event = Event;
      newLog.Message = Message;
      newLog.ExtraInfo = ExtraInfo;
      newLog.ObjectId = ObjectId;
      newLog.Created = DateTime.UtcNow;
      this.WriteLog(newLog);
    }

    public void WriteLog(LogType LogType, Exception exp, string Event = null, string ExtraInfo = null, string ObjectId = null)
    {
      this.WriteLog(LogType, Event, exp.Message, ExtraInfo, ObjectId);
    }



    public void WriteLog(Log log)
    {
      try
      {
        var newContext = this.contextFactory.GetDbContext() as DbContext;
        newContext.Set<Log>().Add(log);
        newContext.SaveChanges();
      }
      catch (Exception exp)
      {
        WroteLogTofile("Original Problem: " + log.Message);
        WroteLogTofile(exp, "Logger System Problem: " + log.Message);
      }
    }


    public IQueryable<Log> GetAll(params Expression<Func<Log, object>>[] includes)
    {
      try
      {
        var newContext = this.contextFactory.GetDbContext() as DbContext;

        IQueryable<Log> query = newContext.Set<Log>();
        foreach (var include in includes)
        {
          query = query.Include(include);
        }
        return query;
      }
      catch (Exception exp)
      {

        throw exp;
      }
    }
  }

}
