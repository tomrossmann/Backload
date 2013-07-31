using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backload.Extension.Contoso.Plugin.Handler
{
    // Simple helper class for the json output. We can avoid using helper classes, if we use anonymous types.
    public class PlUploadFiles
    {
        public PlUploadFiles()
        {
            files = new List<PlUploadFileCore>();
        }

        public List<PlUploadFileCore> files { get; set; }
    }


    public class PlUploadFile : PlUploadFileCore // We use PlFileCore to return errors, so we do not have to return the full class.
    {
        public PlUploadFile(bool success, string name, long size, string type, string deleteUrl, string thumbnail, string fileUrl, string error) 
            : base(success, name, error)
        {
            this.size = size;
            this.type = type;
            this.deleteUrl = deleteUrl;
            this.thumbnail = thumbnail;
            this.fileUrl = fileUrl;
        }

        public long size { get; set; }
        public string type { get; set; }      // content type (e.g. image/jpeg)

        // THe following properties are not implemented in a standard PlUpload file object, 
        // but we need them to extend the functionality of PlUpload (e.g. server delete, image preview, file download)
        public string deleteUrl { get; set; }  // Url to delete the file
        public string thumbnail { get; set; }  // Embedded image (POST/PUT) or url (GET)
        public string fileUrl { get; set; }    // Download url of the file
    }


    // We use this class for returning errors
    public class PlUploadFileCore
    {
        public PlUploadFileCore(bool success, string name, string error)
        {
            this.success = success;
            this.name = name;
            this.error = error;
            this.status = 4; // plupload.FAILED
            if (success)
            {
                this.percent = 100;
                this.status = 5; // plupload.DONE
            }
        }

        public string name { get; set; }

        // THe following properties are not implemented in a standard PlUpload file object, 
        // but we need them to extend the functionality of PlUpload (e.g. server delete, image preview, file download)
        public bool success { get; set; }
        public string error { get; set; }     // Internal error message, you may want to throw an appropriate exception like internal server error, etc.
        public int percent { get; set; }
        public int status { get; set; }
    }
}
