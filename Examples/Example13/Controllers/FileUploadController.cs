using Backload.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Backload;
using System.Threading.Tasks;
using Backload.Plugin.Handler;
using System.Threading;

namespace Backload.Examples.Example13.Controllers
{
    public class FileUploadController : Controller
    {
        public async Task<ActionResult> FileHandler()
        {
            FileUploadHandler handler = new FileUploadHandler(Request, this);
            handler.IncomingRequestStarted += handler_IncomingRequestStarted;

            ActionResult result = await handler.HandleRequestAsync();
            return result;
        }


        public void handler_IncomingRequestStarted(object sender, Eventing.Args.IncomingRequestEventArgs e)
        {
            // throw new Exception("Demo exception to be shown in the trace output");
        }

    }
}
