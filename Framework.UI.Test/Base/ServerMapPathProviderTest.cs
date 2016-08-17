using Framework.UI.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.UI.Test.Base
{
  public class ServerMapPathProviderTest : IServerMapPathProvider
  {
    public string MapPath(string path)
    {
      var uploadedPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadPhotos");
      if (!System.IO.Directory.Exists(uploadedPath))
        System.IO.Directory.CreateDirectory(uploadedPath);
      return uploadedPath;
    }
  }
}
