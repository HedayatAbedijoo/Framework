using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common.Extensions
{
  public static class ExceptionExtensions
  {
    public static string GetInnermostException(this Exception e)
    {
      StringBuilder message = new StringBuilder();
      if (e == null)
      {
        throw new ArgumentNullException("e");
      }
      message.AppendLine(e.Message);
      message.AppendLine(e.StackTrace);
      if (e.InnerException != null)
      {
        message.AppendLine(GetInnermostException(e.InnerException));
      }
      return message.ToString();
    }
  }
}
