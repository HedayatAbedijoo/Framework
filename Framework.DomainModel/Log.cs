using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel
{

  public enum LogType
  {
    Emergency = 0,
    Alert = 1,
    Critical = 2,
    Error = 3,
    Warning = 4,
    Notice = 5,
    Informational = 6,
    Debug = 7

  }

  public class Log
  {
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public LogType Type { get; set; }
    public string Event { get; set; }
    public string Message { get; set; }
    public string ObjectId { get; set; }
    public string ExtraInfo { get; set; }
  }

}
