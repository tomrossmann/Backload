using Backload.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Backload;

namespace JQueryFileUpload.Example02.Controllers
{
    // Method 1: Derive from BackloadController
    //public class FileUploadController : BackloadController
    //{
    //    public override JsonResult UploadHandler()
    //    {
    //        // Call base class to handle the file upload
    //        JsonResult result = HandleRequest();

    //        return result;
    //    }
    //}

    // Method 2: Use a common controller, instantiate an object from BackloadController and call HandleRequest(Request) on it
    public class FileUploadController : Controller
    {
        public JsonResult UploadHandler()
        {
            FileUploadHandler handler = new FileUploadHandler(Request);
            handler.FileUploadedStarted += FileUploadedStarted;
            JsonResult result = handler.HandleRequest();

            return result;
        }

        void FileUploadedStarted(object sender, Backload.Eventing.UploadStartedEventArgs e)
        {
            // Example: Prevent files of type tif to be processed
            if (e.ContentType == "image/tiff")
            {
                // Cancel processing of the incoming file
                e.CancelAction = true;
                e.CancelMessage = "Upload canceled, image file not accepted";
            }
        }
    }
}
