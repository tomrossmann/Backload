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

    // Name of a web application (usually in full IIS mode). Can be found in Properties/Web/Server/Project-Url. Example: http://localhost/Demo (Name of web application is "Demo")
    var web_app = '/';

    // We use the upload handler integrated into Backload:
    // In this example we set an objectContect (id) in the url query (or as form parameter). You can use a user id as 
    // objectContext give users only access to their own uploads. ObjectContext can also be set server side.
    var url = web_app + 'Backload/FileHandler?objectContext=C5F260DD3787';


    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        url: url,
        maxChunkSize: 1000000,                                           // Size of file chunks (data packets): 1MB. Note: In a real world scenario chunk size should be 5-30MB, depending on target infrastructure or use cases.
        acceptFileTypes: /(jpg)|(jpeg)|(png)|(gif)|(pdf)$/i,             // Allowed file types

        
        // Optional: Resume a partially uploaded file or prevent uploading a file that already exists on the server
        add: function (e, data) {
            var that = this;

            //  The following call requests meta data for the file just added to the client ui list from the server.
            //  context: "fileInfo" (get file or chunk meta data); 
            //  ts: timestamp (prevents caching)
            //  Release 2.2+ syntax: $.getJSON(url, { execute: "getfileinfo(" + data.files[0].name + ")", t: e.timeStamp },
            $.getJSON(url, { fileName: data.files[0].name, context: "fileInfo", t: e.timeStamp }, function (result) {

                // Server response
                if (result.files.length > 0) {                                      // If result.files.length is 0 no previous partial uploads
                    var file = result.files[0];
                    var chunkInfo = file.extra.chunkInfo;                           // Chunked file info (not used in this demo)
                    if (data.files[0].size == file.size) {                          // If true, file is already uploaded completely
                        alert("File already uploaded.");                            // Present a dialog to the user.
                        return;                                                     // Do not add the file to the upload list.
                    } else {                                                        // To resume a partially uploaded file,
                        data.uploadedBytes = file.size;                             // Set the position to the size of already uploaded bytes                       
                    }
                }

                $.blueimp.fileupload.prototype.options.add.call(that, e, data);     // Add file with updated data to list in the ui 
            }) 
        }
    })




    // Optional: Load existing files:
    $('#fileupload').addClass('fileupload-processing');
    $.ajax({
        // xhrFields: {withCredentials: true},                                          // Uncomment to send cross-domain cookies
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
