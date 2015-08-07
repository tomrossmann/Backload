using Backload.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Backload.Examples.Example08.Data;
using System.Linq;
using Backload.Contracts.Context;
using Backload.Contracts.Extension;
using Backload.Contracts.Params;

namespace Backload.Extension.Contoso
{
    [Export(typeof(IIncomingRequest))]
    public class VeryBadRequest : IIncomingRequest
    {
        public bool Execute(IBackloadContext context, IIncomingRequestParam param)
        {
            if (context.HttpMethod == "POST")
            {
                // This extension validate if the artist is in the database, and returns a Bad Request status (400) if not.
                
                // IMPORTANT: Don't forget to rebuild the extension when you changed code. Otherwise you may use the old extensions code.
                using (var artists = new ArtistsLibrary())
                {
                    var artist = artists.Artists.FirstOrDefault(a => a.ArtistId == param.BackloadValues.ObjectContext);

                    if (artist == null)  // Artist not in list
                    {   
                        // Stop further processing of the pipeline but all extensions can do their work (maybe logging, etc.).
                        // The outgoing extension will also be called, so you have the chance to change the response to the client.
                        context.PipelineControl.ExecutePipeline = false;
                        // Because we prevented the execution of the core pipeline (where the core method for executing
                        // this request is) we do not generate a FileUploadStatus which holds the status of all files this request 
                        // handles. IIncomingRequest is the first extension point and FileUploadStatus is generated later in the core pipeline.
                        // This taken into account, we cannot send a message with each FileUploadStatusItem, instead we send a general error.
                        // If you want to send messages with the FileUploadStatus, do this in an extension like GetFilesRequest, StoreFileRequest 
                        // or in the OutgoingResponse extension.
                        context.Request.RequestContext.HttpContext.Response.StatusCode = 400; 
                        return true; // Return value is true, because the extension has changed properties.
                    }
                }
            }
            return false; // No properties have been changed, so false is returned.
        }
    }
}
