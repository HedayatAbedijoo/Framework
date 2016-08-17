using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Framework.UI.Core
{
  [PartCreationPolicy(CreationPolicy.Shared)]
  [Export(typeof(IServerMapPathProvider))]
  public class ServerMapPathProvider : IServerMapPathProvider
  {
    public string MapPath(string path)
    {
      return System.Web.Hosting.HostingEnvironment.MapPath(path);
    }
  }

  public interface IServerMapPathProvider
  {
    string MapPath(string path);
  }
}