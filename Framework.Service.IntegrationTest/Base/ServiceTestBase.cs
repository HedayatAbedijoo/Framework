using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Service.IntegrationTest.Base
{
  [TestClass]
  public class ServiceTestBase
  {
    protected CompositionContainer Container;

    public ServiceTestBase()
    {
      Container = Framework.Service.Bootstrap.MEFLoader.Init();
    }
  }
}
