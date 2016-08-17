using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.Test.Base
{
  public class RepositoryTestBase
  {
    protected CompositionContainer Container;
    public RepositoryTestBase()
    {
      Container = Framework.Repository.Bootstrap.MEFLoader.Init();
    }
  }
}
