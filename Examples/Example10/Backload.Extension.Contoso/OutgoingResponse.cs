using Backload.Contracts;
using Backload.Contracts.Context;
using Backload.Contracts.Extension;
using Backload.Contracts.Params;
using Backload.Extension.Contoso.Plugin.Handler;
using System.ComponentModel.Composition;

namespace Backload.Extension.Contoso
{
    [Export(typeof(IOutgoingResponse))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class OutgoingResponse : IOutgoingResponse
    {
        public bool Execute(IBackloadContext context, IOutgoingResponseParam param)
        {
            // Important Note: Since Version 1.9 PlUpload can be handled internally (Plugin attribute in the configuration).

            // Don't forget to rebuid the extension if you made changes to your extension, otherwise you may use the old extension assembly.
            if (context.RequestType == RequestType.Default)
            {
                var result = new PlUploadFiles();
                foreach (var file in param.FileStatus.Files)
                {
                    if (context.HttpMethod != "DELETE")
                        result.files.Add(new PlUploadFile(file.Success, file.FileName, file.FileSize, file.ContentType, file.DeleteUrl, file.ThumbnailUrl, file.FileUrl, file.ErrorMessage));
                    else
                        result.files.Add(new PlUploadFileCore(file.Success, file.FileName, file.ErrorMessage)); //We only return values we need on client side, so we return a PlFileCode object
                }
                param.Result = Helper.Json(result);

                return (param.FileStatus.Files.Count > 0); // we have processed the response, so we notify the pipeline
            }
            return false;
        }
    }
}