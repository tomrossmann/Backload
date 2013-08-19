using Backload.Contracts;
using Backload.Contracts.Context;
using Backload.Contracts.Extension;
using Backload.Contracts.Params;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Backload.Extension.Contoso
{
    // Multiple extensions demo: Each extension extends the path where the files should be stored
    // In a complex situation, you may have multiple extensions and each is responsible for calculating a single subfolder.

    // UploadContext is a subfolder of ObjectContext. A tree of subfolders can be achieved by seperating the subfolder names with a semicolon.
    // if there are multiple extensions manipulating the UploadContext or any other property, all can contributing to the value returned to 
    // the processing pipline. Logger holds information on the extensions called so far.

    // IMPORTANT: Don't forget to rebuild the extension when you changed code. Otherwise you may use the old extensions code.

    [Export(typeof(IIncomingRequest))]
    public class MultipleExtensions : IIncomingRequest
    {
        public string[] datetime = "2000-2099 2010-2019 yyyy MM dd HH mm ss".Split(' ');
        public string[] lorem = "Lorem ipsum dolor sit amet consectetur adipisicing elit sed do eiusmod tempor incididunt ut labore et dolore magna aliqua Ut enim ad minim veniam quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur Excepteur sint occaecat cupidatat non proident sunt in culpa qui officia deserunt mollit anim id est laborum".Split(' ');

        public bool Execute(IBackloadContext context, IIncomingRequestParam param)
        {

            // Demo 2: Comment out the following line and rebuild the solution 
             return false;

            if (context.HttpMethod == "POST")
            {
                int startIdx = context.PipelineControl.Message.MessageCode;
                string subfolder = string.Empty;
                if (startIdx < 8) subfolder = DateTime.Now.ToString(datetime[startIdx]);
                else subfolder = lorem[startIdx - 8];

                if (string.IsNullOrEmpty(param.BackloadValues.UploadContext) == false) param.BackloadValues.UploadContext += ";"; // Seperate subfolders with a semicolon
                param.BackloadValues.UploadContext += subfolder;

                // There are multiple ways sharing data between extensions. Here we use PipelineControl.Message to increase the index.
                // Note, that the ExtensionLogger logs the messages (subfolders) in PipelineControl.Message returned from the extension
                context.PipelineControl.Message.MessageText = subfolder;
                context.PipelineControl.Message.MessageCode += 1;

                return true; // Processed value is true, because the extension has changed properties.
            }

            return false; // No properties have been changed, so false is returned.
        }
    }
}
