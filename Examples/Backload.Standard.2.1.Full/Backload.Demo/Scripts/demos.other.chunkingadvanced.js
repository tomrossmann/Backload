/*
 * jQuery File Upload Plugin JS Example 8.9.1
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * Copyright 2010, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * http://www.opensource.org/licenses/MIT
 */

/* global $, window */

$(function () {
    'use strict';

    var web_app = '/';                                                   // Name of a web application (usually in full IIS mode). Can be found in Properties/Web/Server/Project-Url. Example: http://localhost/Demo (Name of web application is "Demo")

    // We use the upload handler integrated into Backload:
    // In this example we set an objectContect (id) in the url query (or as form parameter). You can use a user id as 
    // objectContext give users only access to their own uploads. ObjectContext can also be set server side.
    var url = web_app + 'Backload/FileHandler?objectContext=C5F260DD3787';


    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        url: url,
        maxChunkSize: 1000000,                                           // Size of file chunks (data packets): 1MB. Note: In a real world scenario chunk size should be 5-30MB, depending on target infrastructure or use cases.
        acceptFileTypes: /(jpg)|(jpeg)|(png)|(gif)|(tif)|(pdf)$/i,             // Allowed file types
        cache: false,

        // the add function is called immediately after a file was added to the client ui list, but not uploaded so far.
        add: function (e, data) {
            var that = this;

            //  The following call requests meta data for the file just added to the client ui list from the server.
            //  context: "fileInfo" (get file or chunk meta data); 
            //  ts: timestamp (prevents caching)
            //  Release 2.2+ syntax: $.getJSON(url, { execute: "getfileinfo(" + data.files[0].name + ")", t: e.timeStamp },
            $.getJSON(url, { fileName: data.files[0].name, context: "fileInfo", t: e.timeStamp }, function (result) {  

                // Server response
                var uuid = data.files[0].size.toString(16);                                     // Create new unique id based on the file size or any other value like user id, etc. This second file characteristics helps to identify the file
            
                if (result.files.length > 0) {                                                  // If result.files.length is 0 no previous partial uploads
                    for (var i = 0; i < result.files.length; i++) {                             // Iterate over the list of partial uploads for this file name, usually only one exist
                        var file = result.files[i];
                        var chunkInfo = file.extra.chunkInfo;                                   // Chunked file info (not used in this demo)
                        if ((chunkInfo != null) && (chunkInfo.uuid == uuid)) {                  // if chunkinfo is not null and has the same uuid we can resume the upload
                            data.uploadedBytes = file.size;             // Set number of bytes already uploaded
                        } else if (data.files[0].size == file.size) {                           // if true, file already uploaded
                            if (!confirm('File already uploaded. Overwrite anyway?')) {
                                return;                                                         // Do not add file to list, because file is already fully uploaded
                            }
                        }
                    }
                }

                data.formData = { 'uuid': uuid };                                               // When uploading the file, the uuid prevents the file to be overwritten by a file with different size
                $.blueimp.fileupload.prototype.options.add.call(that, e, data);                 // Add file with the uuid as form parameter to client ui list 
            })
        },
        send: function (e, data) {
            uploadingFiles += 1;
        },
        always: function (e, data) {
            uploadingFiles -= 1;
        }
    });



    // Warn user if upload is currently in progess. 
    var uploadingFiles = 0;
    $(window).bind('beforeunload', function () {
        if (uploadingFiles > 0) {
            return "You are currently uploading files. If you leave this page, your upload may not complete successfully.";
        }
    });



    // Optional: Load existing files:
    $('#fileupload').addClass('fileupload-processing');
    $.ajax({
        // xhrFields: {withCredentials: true},                                                  // Uncomment to send cross-domain cookies
        url: url,
        dataType: 'json',
        context: $('#fileupload')[0]
    }).always(function () {
        $(this).removeClass('fileupload-processing');
    }).done(function (result) {
        $(this).fileupload('option', 'done')
            .call(this, $.Event('done'), { result: result });
    });

});
