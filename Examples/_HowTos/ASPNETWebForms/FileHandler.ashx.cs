using Backload.Plugin.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Backload.HowTo.ASPNET
{
    public class FileHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequestBase request = new HttpRequestWrapper(context.Request);      // Wrap the request into a HttpRequestBase type
            context.Response.ContentType = "application/json; charset=utf-8";

            // Important note: we reference the .NET 4.0 assembly here, 
            // not the .NET 4.5 version! If you use the NuGet package, make sure
            // to set the correct reference
            FileUploadHandler handler = new FileUploadHandler(request, null);       // Get an instance of the handler class
            handler.IncomingRequestStarted += handler_IncomingRequestStarted;       // Register event handler for demo purposes

            var jsonResult = (JsonResult)handler.HandleRequest();                   // Call the handler method
            JQueryFileUpload result = (JQueryFileUpload)jsonResult.Data;            // JsonResult.Data is of type object and must be casted 

            context.Response.Write(JsonConvert.SerializeObject(result));            // Serialize the JQueryFileUpload object to a Json string
        }

        // Event handler for demo purposes
        void handler_IncomingRequestStarted(object sender, Eventing.Args.IncomingRequestEventArgs e)
        {
            var values = e.Param.BackloadValues;
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}