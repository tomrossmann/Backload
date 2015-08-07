using Backload.Contracts.Context;
using Backload.Contracts.Extension;
using Backload.Contracts.Params;
using Backload.Contracts.Status;
using System.ComponentModel.Composition;

namespace Backload.Extension.Contoso
{
    [Export(typeof(IProcessPipelineException))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class ProcessPipelineException : IProcessPipelineException
    {
        public bool Execute(IBackloadContext context, IProcessPipelineExceptionParam param)
        {
            //
            // Example 1: Graceful exception. We send an error message in our response
            // Extensions responsible for the outgoing response (IOutgoingResponse) are executed as usual.
            // Note: In this case the error is not handled in our client side error handler as we do not 
            // send a HTTP status error code (see examples 2 and 3). Instead the response was send to our 
            // client side FilesAdded handler, so we need an if statement reading the file.success property there.
            // This could lead to a lot of messy code, because you have to notify the client uploader yourself that something 
            // went wrong. PlUpload isn't very good in changing the state from your code. Example 2 provides a cleaner solution.
            //
            // Don't forget to rebuild your extension if you have not set a project dependency in your MVC app
            context.Request.RequestContext.HttpContext.Response.StatusCode = 200; // Backload has set 500 Internal Server Error, we revert this for demo purposes
            if ((param.FileStatus != null) && (param.FileStatus.Files.Count > 0))
            {
                foreach (var file in param.FileStatus.Files)
                {
                    file.ErrorMessage = param.Exception.Message; // for simplicity we set the error to all FileUploadStatusItems
                    file.Success = false; // for simplicity we set the error to all FileUploadStatusItems
                }
            }
            else
            {   // For demo only. We build our own FileStatus. FileStatusSimple has no logic implemented, its only a property bag.
                FileStatusSimple status = new FileStatusSimple();
                for (int i = 0; i < context.Request.Files.Count; i++)
                {
                    var file = context.Request.Files[i];
                    status.Files.Add(new FileStatusSimpleItem(file.FileName, file.ContentType, file.ContentLength, "", param.Exception.Message, false));
                }
                // Now that we have a simple IFileUploadStatus object it can be transformed in our outgoing extension 
                // into a PlUpload friendly Json format. 
                param.FileStatus = status;
            }



            //
            // Example 2: Set Response code to a valid http status code (e.g. 500 Internal server error.
            // Extensions responsible for the outgoing response (IOutgoingResponse) are executed as usual and can build a custom Json response
            // which will be sent with the HTTP status code. The client error handler can use the Json object.
            // Comment in the following line (Don't forget to rebuild your extension if you have not set a project dependency in your MVC app):
            //
            // request.RequestContext.HttpContext.Response.StatusCode = 500; // Note since v1.9 Backload sets this status code automatically on exceptions



            //
            // Example 3: We throw an exception, so ASP.MVC sends out an http status code.
            // Extensions responsible for the outgoing response (IOutgoingResponse) are not executed anymore.
            // The response body is null in this case, because no JsonResult has been created.
            // Comment in the following line (Don't forget to rebuild your extension if you have not set a project dependency in your MVC app):
            //
            // throw new HttpException(500, param.Exception.Message, Exception);


            return true;  // Set to true, if the extension has manipulated the result.
        }

    }
}