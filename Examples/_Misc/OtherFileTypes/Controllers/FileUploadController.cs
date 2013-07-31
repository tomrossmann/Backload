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
            handler.FileUploadedStarted += FileUploadedStarted;
            handler.FileUploadedFinished += handler_FileUploadedFinished;
            handler.FileUploadException +=handler_FileUploadException;
            ActionResult result = await handler.HandleRequestAsync();

            return result;
        }

        void FileUploadedStarted(object sender, Backload.Eventing.UploadStartedEventArgs e)
        {
            // do something
        }
        
        void handler_FileUploadException(object sender, Eventing.UploadExceptionEventArgs e)
        {
            // do something
        }

        void handler_FileUploadedFinished(object sender, Eventing.UploadFinishedEventArgs e)
        {
            // error handling
        }
    }
}
