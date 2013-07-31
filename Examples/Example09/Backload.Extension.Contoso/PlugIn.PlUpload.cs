using Backload.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using System.Linq;
using Backload.Extension.Contoso.Plugin.Handler;
using Backload.Contracts.Extension;
using Backload.Contracts.Context;
using Backload.Contracts.Params;

namespace Backload.Extension.Contoso.Plugin.PlUpload
{
    [Export(typeof(IOutgoingResponse))]
    public class OutgoingResponse : IOutgoingResponse
    {
        public bool Execute(IBackloadContext context, IOutgoingResponseParam param)
        {
            // IMPORTANT NOTE:
            // Backload v1.9 and above can handle PlUpload internally now. Just send plugin=plupload. You do not need an extension anymore.
            //
            // In our example we have a PlUpload Plugin client side, so we transform the output to a PlUpload friendly format.
            // Important Note: Since Version 1.9 PlUpload can be handled internally (Plugin attribute in the configuration).

            // We have two upload plugins in our form, but this extension is only for PlUpload. We have two options to ensure that our
            // extension only handles PlUpload requests: Convention based handling or conditional handling.
            // In our example we use handling based on convention, where the convention is: 
            //    1. In our request we set a "plugin" querystring to plupload (plugin=plupload)
            //    2. The namespace of our extension contains Plugin.PlUpload (not case sensitive)
            // If you do not want to use convention based handling, just do not send a "plugin" querystring or leave it empty (plugin=)
            //
            // You may use conditional handling instead. The extension code is called, and you make the decision based on a condition 
            // if to handle the request or not in your code. Typically you parse the request (Querystring or body) for a condition:
            // Example:
            //    string control = context.Request.QueryString["control"];
            //    if ((string.IsNullOrEmpty(control)) || (control != "plupload")) return false;

            //
            // Remarks: Don't forget to rebuild your extension if you made changes to your extension, otherwise you may use the old extension assembly.
            if (param.FileStatus == null) return false; // In case something went wrong

            if (context.HttpMethod != "DELETE")
            {
                var result = new PlUploadFiles();

                for (int i = 0; i < param.FileStatus.Files.Count; i++)
                {
                    var file = param.FileStatus.Files[i];
                    result.files.Add(new PlUploadFile(file.Success, file.OriginalName, file.FileSize, file.ContentType, file.DeleteUrl, file.ThumbnailUrl, file.FileUrl, file.ErrorMessage));
                    
                }
                param.Result = Helper.Json(result);
            }
            else
            {
                // Here we use an anonymous type to return some data. PlUpload does not handle server side file deletion, but we made the 
                // ajax request and so we can return whatever we want.
                var file = param.FileStatus.Files[0];
                var result = new { success = file.Success, message = file.ErrorMessage, name = file.FileName };
                param.Result = Helper.Json(result);
            }
            
            return (param.FileStatus.Files.Count > 0); // if > 0 we've changed properties.
        }
    }
}