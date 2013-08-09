using Backload.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Backload;
using System.Threading.Tasks;
using Backload.Eventing.Args;

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
            handler.OutgoingResponseCreated += handler_OutgoingResponseCreated;
            handler.ProcessPipelineExceptionOccured += handler_ProcessPipelineExceptionOccured;

            ActionResult result = await handler.HandleRequestAsync();
            return result;
        }

        void handler_StoreFileRequestStarted(object sender, StoreFileRequestEventArgs e)
        {
            // do something
        }

        void handler_StoreFileRequestFinished(object sender, StoreFileRequestEventArgs e)
        {
            var status = e.Param.FileStatusItem;
        }

        void handler_StoreFileRequestException(object sender, StoreFileRequestEventArgs e)
        {
            var exception = e.Param.FileStatusItem.Exception;
        }

        void handler_OutgoingResponseCreated(object sender, OutgoingResponseEventArgs e)
        {
            if (e.Context.RequestType == Contracts.Context.RequestType.Default)
            {
                JsonResult result = (JsonResult)e.Param.Result;
            }
        }

        void handler_ProcessPipelineExceptionOccured(object sender, ProcessPipelineExceptionEventArgs e)
        {
            var files = e.Param.Exception;
        }
    }
}
