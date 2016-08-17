using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Contracts.Service
{
  public enum OperationResult : byte
  {
    Success,
    Information,
    Warning,
    Error
  }

  public class ServiceContract
  {
    public ServiceException Exception { get; set; }

    public string Message { get; set; }

    public OperationResult Result { get; set; }

    public object ExtraData { get; set; }
  }


  public class ServiceContract<T> : ServiceContract
  {
    public ServiceContract()
    { }
    public ServiceContract(T item)
    {
      this.Item = item;
    }

    public T Item { get; set; }

  }
}
