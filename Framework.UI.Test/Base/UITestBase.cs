using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.UI.Test.Base
{
  [TestClass]
  public class UITestBase
  {
    protected CompositionContainer Container;
    public UITestBase()
    {
      Container = Framework.Service.Bootstrap.MEFLoader.Init();
    }
  }
}
