using Backload.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Backload.Examples.Example08.Data;
using System.Linq;

namespace Backload.Extension.Contoso
{
    [Export(typeof(IIncomingRequest))]
    public class IncomingRequest : IIncomingRequest
    {
        public string ObjectContext { get; set; }
        public string UploadContext { get; set; }
        public string QueryFileName { get; set; }
        public string QueryExtraFolder { get; set; }
        public StopProcessingType StopProcessing { get; set; }
        public string ProcessingMessage { get; set; }
        public List<ExtensionLogger> Logger { get; set; }

        public bool ProcessStep(System.Web.HttpRequestBase request, string httpMethod)
        {
            if (httpMethod == "POST")
            {
                // Validate if the artist is in the database
                using (var artists = new ArtistsLibrary())
                {
                    var artist = artists.Artists.FirstOrDefault(a => a.ArtistId == this.ObjectContext);

                    if (artist == null)  // Artist not in list
                    {   
                        // Stop further processing of the pipeline but all extensions can do their work (maybe logging, etc.).
                        this.StopProcessing = StopProcessingType.StopProcessingPipelineOnly; 
                        this.ProcessingMessage = "Artist not in list";
                        return true; // Return value is true, because the extension has changed properties.
                    }
                }
            }

            return false; // No properties have been changed, so false is returned.
        }
    }
}
