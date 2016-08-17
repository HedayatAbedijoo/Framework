using Framework.Service.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.UI.Controllers
{
  [Export]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public class HomeController : Controller
  {
    IProductService service;
    [ImportingConstructor]
    public HomeController(IProductService service)
    {
      this.service = service;

    }
    public ActionResult Index()
    {
      return RedirectToAction("Products", "Home");

    }

    public ActionResult Products()
    {
      return View();
    }
  }
}
