using Backload.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using System.Linq;

namespace Backload.Extension.Contoso.Plugin.PlUpload
{
    [Export(typeof(IOutgoingResponse))]
    public class OutgoingResponse : IOutgoingResponse
    {
        public JsonResult Result  { get; set; }
        public List<ExtensionLogger> Logger { get; set; }

        public bool ProcessStep(System.Web.HttpRequestBase request, string httpMethod)
        {
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
            //    string control = request.QueryString["control"];
            //    if ((string.IsNullOrEmpty(control)) || (control != "plupload")) return false;


            // Just for demo purposes we use a dynamic object to get the current response data (which is in JsonResult.Data).
            // Note: dynamic means no Intellisense and type checking at compile time.
            // Otherwise can simply cast Result.Data to Backload.FileUploadStatus to access the strongly typed values at debug time.
            dynamic data = this.Result.Data;
            var processed = false;  // Flag. Set to true, if the extension has manipulated the result.
            if (httpMethod != "DELETE")
            {
                var result = new PlFiles();

                for (int i = 0; i < data.files.Count; i++)
                {
                    var file = data.files[i];
                    result.files.Add(new PlFile(file.extra_info.original_name, file.size, file.type, file.delete_url, file.thumbnail_url, file.url, file.error));
                    processed = true;
                }
                this.Result = Helper.Json(result);

            }
            else
            {
                // Here we use an anonymous type to return some data. PlUpload does not handle server side file deletion, but we made the 
                // ajax request and so we can return whatever we want.
                var result = new { success = string.IsNullOrEmpty(data.files[0].error), message = data.files[0].error, name = data.files[0].name };
                this.Result = Helper.Json(result);
                processed = true;
            }
            return processed;
        }
    }


    // Simple helper class for the json output. We can avoid using helper classes, if we use anonymous types.
    public class PlFiles
    {
        public PlFiles()
        {
            files = new List<PlFile>();
        }

        public List<PlFile> files { get; set; }
    }

    public class PlFile
    {
        public PlFile(string name, long size, string type, string deleteUrl, string thumbnail, string fileUrl, string error)
        {
            this.name = name;
            this.percent = 100;
            this.size = size;
            this.type = type; 
            this.deleteUrl = deleteUrl;  
            this.thumbnail = thumbnail; 
            this.fileUrl = fileUrl;
            this.error = error;
        }

        public string  name  { get; set; }
        public int  percent  { get; set; }
        public long  size  { get; set; }
        public string  type  { get; set; }      // content type (e.g. image/jpeg)

        // THe following properties are not implemented in a standard PlUpload file object, 
        // but we need them to extend the functionality of PlUpload (e.g. server delete, image preview, file download)
        public string  deleteUrl { get; set; }  // Url to delete the file
        public string  thumbnail { get; set; }  // Embedded image (POST/PUT) or url (GET)
        public string  fileUrl { get; set; }    // Download url of the file
        public string  error  { get; set; }     // Internal error message, you may want to throw an appropriate exception like internal server error, etc.
    }
}