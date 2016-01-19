/*jslint unparam: true */
/*global window, $ */
$(function () {
    'use strict';

    // Name of a web application (usually in full IIS mode). Can be found in Properties/Web/Server/Project-Url. Example: http://localhost/Demo (Name of web application is "Demo")
    var web_app = '/';

    // We use the upload handler integrated into Backload:
    var url = web_app + 'Backload/FileHandler';
    var userId = '030357B624D9';
    var fine = '/Backload/Client/widen/fineuploader/';


    // Initialize the plugin:
    var uploader = new qq.FineUploader({
        element: document.getElementById("fine-uploader"),
        template: 'qq-template-gallery',
        request: {
            endpoint: url,
            params: {                                                                       // Send a plugin param or set Fine Uploader in 
                plugin: "FineUploader",                                                     // Web.Backload.config as the default client plugin                                                    
                objectContext: userId                                                       // ObjectContext (e.g. user id) for GET/POST requests
            }
        },

        deleteFile: {
            enabled: true,
            endpoint: url,
            params: {                                                                       // Send a plugin param or set Fine Uploader in 
                plugin: "FineUploader",                                                     // Web.Backload.config as the default client plugin                                                    
                objectContext: userId                                                       // ObjectContext (e.g. user id) for DELETE requests 
            }
        },

        session: {                                                                          // Initial GET request to load existing files
            endpoint: url,
            params: {                                                                       // Send a plugin param or set Fine Uploader in 
                plugin: "FineUploader",                                                     // Web.Backload.config as the default client plugin                                                      
                objectContext: userId                                                       // ObjectContext (e.g. user id) for initial GET request
            }
        },

        thumbnails: {
            placeholders: {
                waitingPath: fine + 'placeholders/waiting-generic.png',
                notAvailablePath: fine + 'placeholders/not_available-generic.png'
            }
        },

        chunking: {
            enabled: true,                                                                  // true to enable file chunking
            partSize: 10000000                                                              // 10MB chunks (usually to small, but this is a demo)
        },

        validation: {
            allowedExtensions: ['jpeg', 'jpg', 'gif', 'png', 'pdf']
        },

        callbacks: {
            onComplete: function (id, name, response, xhr) {                                // Within normal file uploads the uuid provided
                if ((response) && (response.success)) {                                     // by the server is automatically applied to the file
                    var uuid = uploader.getUuid(id);                                        // by Fine Uploader. In file chunk mode this must be 
                    if (uuid != response.newUuid)                                           // done by hand. If we don't do this, the server side
                        uploader.setUuid(id, response.newUuid);                             // component cannot associate the file on delete requests.
                }
            }
        }
    });

});
