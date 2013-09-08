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

namespace Backload.Examples.Example12.Controllers
{
    public class FileUploadController : Controller
    {
        private static string _logpattern = "<span class=\"{0}\">Server event: {1} at {2}</span><br />";

        public async Task<ActionResult> FileHandler()
        {
            FileUploadHandler handler = new FileUploadHandler(Request, this);
            handler.IncomingRequestStarted += handler_IncomingRequestStarted;

            handler.AuthorizeRequestStarted += handler_AuthorizeRequestStarted;
            handler.AuthorizeRequestFinished += handler_AuthorizeRequestFinished;

            handler.GetFilesRequestStarted += handler_GetFilesRequestStarted;
            handler.GetFilesRequestFinished += handler_GetFilesRequestFinished;
            handler.GetFilesRequestException += handler_GetFilesRequestException;

            handler.StoreFileRequestStartedAsync += handler_StoreFileRequestStartedAsync; // async event handler 
            handler.StoreFileRequestFinished += handler_StoreFileRequestFinished;
            handler.StoreFileRequestException += handler_StoreFileRequestException;

            handler.DeleteFilesRequestStarted += handler_DeleteFilesRequestStarted;
            handler.DeleteFilesRequestFinishedAsync += handler_DeleteFilesRequestFinishedAsync; // async event handler 
            handler.DeleteFilesRequestException += handler_DeleteFilesRequestException;

            handler.OutgoingResponseCreated += handler_OutgoingResponseCreated;

            handler.ProcessPipelineExceptionOccured += handler_ProcessPipelineExceptionOccured;


            ActionResult result = await handler.HandleRequestAsync();
            return result;
        }


        // Request (GET,POST,PUT,DELETE) comes in. Values (querystring, form) have been read and can be manipulated before the 
        // internal environment initializes. Request can be aborted if it does not match criteria.
        public void handler_IncomingRequestStarted(object sender, Eventing.Args.IncomingRequestEventArgs e)
        {
            // Demo 1: We forbid PUT requests
            // if (e.Context.HttpMethod == "PUT") e.Context.PipelineControl.ExecutePipeline = false;

            e.Context.PipelineControl.Message.MessageText = string.Format(_logpattern, "log-in", "IncomingRequestStarted", DateTime.Now.ToLongTimeString());
        }


        // Raised when the authorization process starts
        void handler_AuthorizeRequestStarted(object sender, Eventing.Args.AuthorizeRequestEventArgs e)
        {
            // Demo 2a: Force authorization of anonymous users:
            // e.Param.AllowAnonymous = true;

            // Demo 2b: Force authorization with roles:
            // e.Param.AllowAnonymous = false;
            // e.Param.IsAuthenticated = true;
            // e.Param.AllowedRoles.Add("SomeRole");
            // e.Param.CurrentUserRoles.Add("SomeRole");

            e.Context.PipelineControl.Message.MessageText += string.Format(_logpattern, "log-auth", "AuthorizeRequestStarted", DateTime.Now.ToLongTimeString());
        }

        // Raised when the authorization process finishes with a result in e.Param.IsAuthorized [true|false]
        void handler_AuthorizeRequestFinished(object sender, Eventing.Args.AuthorizeRequestEventArgs e)
        {
            // Demo 3: Deny authorization with e.Param.IsAuthorized
            // if (e.Param.CurrentUserRoles.Count == 0) e.Param.IsAuthorized = false;

            e.Context.PipelineControl.Message.MessageText += string.Format(_logpattern, "log-auth", "AuthorizeRequestFinished", DateTime.Now.ToLongTimeString());
        }


        // Begin of core GET execution method, values of the GET request can be edited, or GET request can be aborted
        void handler_GetFilesRequestStarted(object sender, Eventing.Args.GetFilesRequestEventArgs e)
        {
            // Demo 4: Allow only jpg files
            // e.Param.BackloadValues.FilesFilter = "*.jpg";

            e.Context.PipelineControl.Message.MessageText += string.Format(_logpattern, "log-get", "GetFilesRequestStarted", DateTime.Now.ToLongTimeString());
        }

        // List of files retrieved and FileUploadStatus.Files filled with FileUploadStatusItems
        void handler_GetFilesRequestFinished(object sender, Eventing.Args.GetFilesRequestEventArgs e)
        {
            // Demo 5: Limit the result of returned files to 5 items.
            // if (e.Context.RequestType == RequestType.Default) // GET requests for a file list, not a single file
            // {
            //     int limit = 5;
            //     int count = e.Param.FileStatus.Files.Count;
            //     if (count > limit) e.Param.FileStatus.Files.RemoveRange(limit, count - limit);
            // }

            e.Context.PipelineControl.Message.MessageText += string.Format(_logpattern, "log-get", "GetFilesRequestFinished", DateTime.Now.ToLongTimeString());
        }

        // Raised when an error within the core get method occurs (e.g. file not found or permission denied)
        void handler_GetFilesRequestException(object sender, Eventing.Args.GetFilesRequestEventArgs e)
        {
            e.Context.PipelineControl.Message.MessageText += string.Format(_logpattern, "log-error", "GetFilesRequestException", DateTime.Now.ToLongTimeString());
        }


        // Begin of core PUT/POST execution method, values can be edited, or request can be aborted
        async Task handler_StoreFileRequestStartedAsync(object sender, Eventing.Args.StoreFileRequestEventArgs e)
        {
            // Demo 6: Change the file to be stored
            string filepath = Server.MapPath("~/Resources/SomeImage.jpg");
            // var file = e.Param.FileStatusItem;
            // if (System.IO.File.Exists(filepath))
            // {
            //     byte[] bytes = null;
            //     using (FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            //     {
            //         bytes = new byte[stream.Length];
            //         await stream.ReadAsync(bytes, 0, (int)stream.Length);
            //     }
            //     file.FileData = bytes;
            //     file.FileSize = bytes.LongLength;
            //     file.ContentType = "image/jpeg";
            // }

            e.Context.PipelineControl.Message.MessageText += string.Format(_logpattern, "log-post", "StoreFileRequestStartedAsync", DateTime.Now.ToLongTimeString());
        }

        // Core PUT/POST execution method has saved the file
        void handler_StoreFileRequestFinished(object sender, Eventing.Args.StoreFileRequestEventArgs e)
        {
            // Demo 7: Include a message text
            // e.Param.FileStatusItem.Message = "Changed the storag ";
            e.Context.PipelineControl.Message.MessageText += string.Format(_logpattern, "log-post", "StoreFileRequestFinished", DateTime.Now.ToLongTimeString());
        }

        // Raised when an error within the core post/put method occurs (e.g. file cannot be saved or permission denied)
        void handler_StoreFileRequestException(object sender, Eventing.Args.StoreFileRequestEventArgs e)
        {
            e.Context.PipelineControl.Message.MessageText += string.Format(_logpattern, "log-error", "StoreFileRequestException", DateTime.Now.ToLongTimeString());
        }


        // Begin of core DELETE execution method, values can be edited, files removed/added from deletion list, logging,etc
        void handler_DeleteFilesRequestStarted(object sender, Eventing.Args.DeleteFilesRequestEventArgs e)
        {
            // Demo 8: We've setup the copiesRoot attribute in the config to "Backup" which makes copies of uploaded files 
            // in the ~/Files/Backup folder. On a delete request we now want to delete the backup files too. In this demo we add 
            // the additional files to delete to the DeleteFiles list and let Backload delete the files.
            // (Note: The DeleteFilesRequestFinished handler below shows another way to do this).
            // IFileUploadStatus copy = e.Param.DeleteFiles.Clone(); // Clones the files to delete list
            // foreach (var file in copy.Files) file.UploadContext = "Backup"; // Sets the delete folder for the clone to the Backup folder
            // e.Param.DeleteFiles.Files.AddRange(copy.Files); // Adds the clone to the delete list

            e.Context.PipelineControl.Message.MessageText += string.Format(_logpattern, "log-delete", "DeleteFilesRequestStarted", DateTime.Now.ToLongTimeString());
        }

        // End of core DELETE execution method with results of the deletion
        async Task handler_DeleteFilesRequestFinishedAsync(object sender, Eventing.Args.DeleteFilesRequestEventArgs e)
        {
            // Demo 9: In the DeleteFilesRequestStarted event handler we added the additional files to the DeleteFiles list.
            // In this demo we delete the files manually in the event handler. We do not want to block the ui thread, so we do it asynchronous.
            // await Task.Factory.StartNew(
            //    () =>
            //    {
            //        foreach (var file in e.Param.DeleteFiles.Files)
            //        {
            //            if (System.IO.File.Exists(file.StorageInfo.CopyPath)) System.IO.File.Delete(file.StorageInfo.CopyPath);
            //        }
            //    });

            e.Context.PipelineControl.Message.MessageText += string.Format(_logpattern, "log-delete", "DeleteFilesRequestFinished", DateTime.Now.ToLongTimeString());
        }

        // Raised when an error within the core delete method occurs (e.g. file cannot be deleted
        void handler_DeleteFilesRequestException(object sender, Eventing.Args.DeleteFilesRequestEventArgs e)
        {
            e.Context.PipelineControl.Message.MessageText += string.Format(_logpattern, "log-error", "DeleteFilesRequestException", DateTime.Now.ToLongTimeString());
        }


        // Outgoing response is created. you can completely override the results in e.Param.Result
        void handler_OutgoingResponseCreated(object sender, Eventing.Args.OutgoingResponseEventArgs e)
        {
            e.Context.PipelineControl.Message.MessageText += string.Format(_logpattern, "log-out", "OutgoingResponseCreated", DateTime.Now.ToLongTimeString()) + "&nbsp;<br />";

            // Demo 10: Because we want to return extra data (event log) to the client we need to add this to the JSON output as an additional JSON object.
            // The easiest way to do this is to create an anonymous object and assign it to the Data property of the JsonResult object:
            var result = (JsonResult)e.Param.Result;
            var plupload = (PlUpload)result.Data; // JsonResult.Data is always of type object. We convert it to the underlying PlUpload object.
            result.Data = new { files = plupload.files, eventlog = e.Context.PipelineControl.Message.MessageText }; // Assing anonymous object
        }

        // Single point exception handler
        void handler_ProcessPipelineExceptionOccured(object sender, Eventing.Args.ProcessPipelineExceptionEventArgs e)
        {
            e.Context.PipelineControl.Message.MessageText += string.Format(_logpattern, "log-error", "ProcessPipelineExceptionOccured", DateTime.Now.ToLongTimeString());
        }
    }
}
