using Backload.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Backload;
using System.Threading.Tasks;

namespace Backload.Demo.OtherFileTypes.Controllers
{

    public class FileUploadController : Controller
    {
        public async Task<ActionResult> FileHandler()
        {
            FileUploadHandler handler = new FileUploadHandler(Request, this);
            handler.StoreFileRequestStarted += handler_StoreFileRequestStarted;
            handler.StoreFileRequestFinished += handler_StoreFileRequestFinished;
            handler.StoreFileRequestException += handler_StoreFileRequestException;
            ActionResult result = await handler.HandleRequestAsync();

            return result;
        }


        void handler_StoreFileRequestStarted(object sender, Eventing.Args.StoreFileRequestEventArgs e)
        {
            // do something
        }

        void handler_StoreFileRequestFinished(object sender, Eventing.Args.StoreFileRequestEventArgs e)
        {
            // do something
        }

        void handler_StoreFileRequestException(object sender, Eventing.Args.StoreFileRequestEventArgs e)
        {
            // do something
        }
    }
}
