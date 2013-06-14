using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.BackloadMVC.AppStart_Initializer), "Initialize")]
 
namespace $rootnamespace$.BackloadMVC {
    public static class AppStart_Initializer {
        public static void Initialize() {
            // Use bundeling for client files (scripts (js) and styles (css)). You can include/exclude the image gallery provided by the default JQuery file upload ui demo implementaion
            // Comment this out, if you manually include the files in your page
            // Overload: RegisterBundles(BundleTable.Bundles, inclImageGallery, pathToScripts, pathToStyles) 
            // Example: Backload.Configuration.FileUploadBundles.RegisterBundles(BundleTable.Bundles, false, "~/Scripts/FileUpload/", "~/Content/FileUpload/css/");
            Backload.Configuration.FileUploadBundles.RegisterBundles(BundleTable.Bundles, false);
        }
    }
}