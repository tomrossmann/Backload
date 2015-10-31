/*jslint unparam: true */
/*global window, $ */
$(function () {
    'use strict';

    // We use the upload handler integrated into Backload:
    // In this example we set an objectContect (id) in the url query (or as form parameter).
    // You can use a user id as objectContext give users only access to their own uploads.
    var url = '/Backload/FileHandler?objectContext=C5F260DD3787';

    $('#fileupload').fileupload({
        url: url,
        dataType: 'json',
        cache: false,
        autoUpload: false,
        disableImageResize: /Android(?!.*Chrome)|Opera/
            .test(window.navigator && navigator.userAgent),
        previewCrop: true,
        maxChunkSize: 4000000                                          // Optional: file chunking with 4MB chunks (Azure blob storage default)
    })

        // if the following init method call causes problems bind event handlers manually 
        // like in blueimps basic plus example (https://blueimp.github.io/jQuery-File-Upload/basic-plus.html)
    .data('blueimp-fileupload').initTheme("BasicPlus");
});
