using Backload.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using System.Linq;
using Backload.Extension.Contoso.Plugin.Handler;
using Backload.Contracts.Context;
using System.Web;
using System.Threading.Tasks;
using Backload.Contracts.Extension;
using Backload.Contracts.Params;

namespace Backload.Extension.Contoso.Plugin.PlUpload
{
    [Export(typeof(IOutgoingResponse))]
    public class OutgoingResponse : IOutgoingResponse
    {
        public bool Execute(IBackloadContext context, IOutgoingResponseParam param)
        {
            // Note: We use convention based plugin handling (see Example 09). 
			// Backload v1.9 and above can handle this internally. Just send plugin=plupload in the querystring or in the config file.
            //
            // In our example we have a PlUpload Plugin client side, so we transform the output to a PlUpload friendly format.
            // Important Note: Since Version 1.9 PlUpload can be handled internally (Plugin attribute in the configuration).
            //
            // Remarks: Don't forget to rebuild your solution if you made changes to your extension, otherwise you may use the old extension assembly.

            if (param.FileStatus == null) return false; // in the case something went wrong.
            var result = new PlUploadFiles();
            foreach (var file in param.FileStatus.Files)
            {
                if (context.HttpMethod != "DELETE")
                    result.files.Add(new PlUploadFile(file.Success, file.FileName, file.FileSize, file.ContentType, file.DeleteUrl, file.ThumbnailUrl, file.FileUrl, file.ErrorMessage));
                else
                    result.files.Add(new PlUploadFileCore(file.Success, file.FileName, file.ErrorMessage)); //We only return values we need on client side, so we instatiate a PlFileCore object
            }
            param.Result = Helper.Json(result);
            return true; // We've processed the response
        }

    }
}