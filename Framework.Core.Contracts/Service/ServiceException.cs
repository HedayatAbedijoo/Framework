using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Contracts.Service
{
  public class ServiceException
  {
    public Exception Exception { get; set; }

    public int ErrorCode { get; set; }

    public string ErrorMessage { get; set; }

    public static ServiceException Factory(Exception exp)
    {
      var item = new ServiceException();
      item.Exception = exp;
      item.ErrorMessage = exp.Message;
      item.ErrorCode = new Random().Next(); /// log Exception Somewhere
      return item;
    }
  }
}
