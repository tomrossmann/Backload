using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Backload.Examples.Example01.Controllers
{
    using System.IO;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
