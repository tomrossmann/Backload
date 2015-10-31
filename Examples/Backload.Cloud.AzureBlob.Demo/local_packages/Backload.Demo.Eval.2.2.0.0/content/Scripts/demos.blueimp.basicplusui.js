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

    // We use the upload handler integrated into Backload:
    // In this example we set an objectContect (id) in the url query (or as form parameter).
    // You can use a user id as objectContext give users only access to their own uploads.
    var url = '/Backload/FileHandler?objectContext=C5F260DD3787';

    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        url: url,
        maxChunkSize: 4000000,                                                  // Optional: file chunking with 4MB chunks (default Azure blob storage)
        acceptFileTypes: /(jpg)|(jpeg)|(png)|(gif)$/i                           // Allowed file types

        /*
        // Optional: Try to resume a partially uploaded chunked file of a previous upload attempt
        add: function (e, data) {
            var that = this;
            // Get fileInfo request parameters: 
            //    fileName: Name of the file; 
            //    context: fileInfo (get file or chunks meta data); 
            //    t: timestamp (prevents caching)
            $.getJSON(url, { fileName: data.files[0].name, context: "fileInfo", t: e.timeStamp },
                function (result) {
                    // Response:
                    if (result.files.length != 0) {
                        var chunkInfo = result.files[0].extra.chunkInfo;
                        if (chunkInfo != null) {                                // To resume a partially uploaded file,
                            data.uploadedBytes = chunkInfo.uploadedSize;        // set the length of bytes already uploaded
                        }
                    }
                    $.blueimp.fileupload.prototype
                        .options.add.call(that, e, data);                       // Add file to list
                }) 
        }*/
    });


    // Load existing files:
    $('#fileupload').addClass('fileupload-processing');
    $.ajax({
        // Uncomment the following to send cross-domain cookies:
        // xhrFields: {withCredentials: true},
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
