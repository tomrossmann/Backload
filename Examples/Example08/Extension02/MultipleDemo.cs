using Backload.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Backload.Extension.Contoso
{
    [Export(typeof(IIncomingRequest))]
    public class MultipleDemo : IIncomingRequest
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
                // UploadContext is a subfolder of ObjectContext. 
                // A tree of subfolders can be achieved by seperating the subfolder names with a semicolon.
                // if their are multiple extensions manipulating the UploadContext or any other property,
                // all can contibuting to the value returned to the processing pipline.
                if (string.IsNullOrEmpty(this.UploadContext) == false) this.UploadContext += ";";
                this.UploadContext += "subfolder_" + Logger.Count.ToString(); // Logger lists all extensions call so far

                return true; // Return value is true, because the extension has changed properties.
            }

            return false; // No properties have been changed, so false is returned.
        }
    }
}
