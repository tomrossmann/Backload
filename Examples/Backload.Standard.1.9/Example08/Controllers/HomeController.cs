//using Backload.Examples.Example08.Models;
using System.Web.Mvc;
using System.Linq;
using Backload.Examples.Example08.Data;
using Backload.Examples.Example08.Data.Models;
using System.Collections.Generic;

namespace Backload.Examples.Example08.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var artists = new ArtistsLibrary())
            {
                return View(artists.Artists.ToList<Artist>());
            }
        }
    }
}
