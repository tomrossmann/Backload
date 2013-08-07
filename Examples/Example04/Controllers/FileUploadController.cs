using Backload.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Backload;
using System.Threading.Tasks;

namespace Backload.Examples.Example04.Controllers
{
    // For all demos below open the file  ~/Scripts/main.js and
    // change the url of the ajax call for the fileupload plugin

    // Demo 1: Derive from BackloadController and call the base class
    public class FileUploadDerivedController : BackloadController
    {
        // Since version 1.9 you can call the asynchronous handler method 
        public async Task<ActionResult> FileHandler()
        {
            // Call base class method to handle the file upload asynchronously
            ActionResult result = await base.HandleRequestAsync();
            return result;
        }
    }

    // Demo 2: Use a common controller, instantiate a FileUploadHandler object 
    // call HandleRequestAsync, (since version 1.9+) on it.
    // NOTE: In this example we registered an event handler to cancel a file upload.
    public class FileUploadInstanceController : Controller
    {
        public async Task<ActionResult> FileHandler()
        {
            FileUploadHandler handler = new FileUploadHandler(Request, this);
            handler.IncomingRequestStarted += handler_IncomingRequestStarted;

            ActionResult result = await handler.HandleRequestAsync();
            return result;
        }

        void handler_IncomingRequestStarted(object sender, Eventing.Args.IncomingRequestEventArgs e)
        {
            // Demo: Disallow PUT request within the event handler.
            if (e.Context.HttpMethod == "PUT") e.Context.PipelineControl.ExecutePipeline = false;
        }
    }
}
