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
            handler.FileUploadedStarted += FileUploadedStarted;
            handler.FileUploadedFinished += handler_FileUploadedFinished;
            handler.FileUploadException +=handler_FileUploadException;
            ActionResult result = await handler.HandleRequestAsync();

            return result;
        }

        void FileUploadedStarted(object sender, Backload.Eventing.UploadStartedEventArgs e)
        {
            // Example: Prevent files of type gif to be uploaded
            if (e.ContentType == "image/gif")
            {
                // Cancel uploading processing of the incoming file
                e.CancelAction = true;
                e.CancelMessage = "Upload canceled, File type not accepted";
            }
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
