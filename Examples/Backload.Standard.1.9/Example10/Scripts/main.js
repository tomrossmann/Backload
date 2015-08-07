$(function () {
    var backloadController = "/Backload/UploadHandler";
    $("#uploader").plupload({
        runtimes: 'html5,html4',
        url: backloadController,
        max_file_size: '5mb',
        max_file_count: 50, // user can add no more then 10 files at a time
        rename: true,
        multiple_queues: true,
        filters: [{ title: "Image files", extensions: "jpg,gif,png" }], // Specify what files to browse for
        preinit: { Init: function (up) { } },  // Do init stuff here 
        views: { list: true, thumbs: true, active: 'thumbs' }
    });
    var uploader = $('#uploader').plupload('getUploader');
    var $uploaderwidget = $('#uploader').data("ui-plupload");


    // PlUpload doesn't send a delete request to the server automatically, so we do it with an jquery ajax request
    uploader.bind("FilesRemoved", function (up, files) {
        $.each(files, function (i, file) {
            if ((typeof file.deleteUrl !== "undefined") && (file.deleteUrl != "")) {
                $.ajax({
                    url: file.deleteUrl, 
                    type: "DELETE",
                    dataType: "json"
                }).done(function (data, textStatus, jqXHR) {
                    if ((data.files == null) || (data.files[0].success != true)) {
                        // Add error handling.
                    }
                });
            }
        });
    });


    // If the HTTP status code is >= 400 the Error handler will be called
    uploader.bind('Error', function (up, err) {
        var file = err.file;
        if ((typeof err.response === "string") && (err.response != "")) resp = $.parseJSON(err.response); // if string convert to an object
        else resp = err.response;                           // We have a response object, if we use graceful exception handling in our extension. Otherwise, if we throw an error, this is null
        $('.plupload_header .plupload_message').remove();   //remove existing error message
        $uploaderwidget.notify("error", "<strong>HTTP Error " + err.status + "</strong> <br /><i>An error has occured during the last operation " + ((file != null) ? file.name : "") + ".</i>");
    });


    // If the HTTP status code is < 400 the FileUploaded will be called
    uploader.bind('FileUploaded', function (up, files, result) {
        var remoteFiles = [];
        if (result.response != null) remoteFiles = JSON.parse(result.response).files;
        if (remoteFiles.length == 0) {
            $('.plupload_header .plupload_message').remove(); //remove existing error message
            $uploaderwidget.notify("error", "<strong>HTTP Error</strong> <br /><i>An error has occured during the last operation.</i>");
        } else {
            if (!$.isArray(files)) files = [files]; // For simplicity, if files is only a single file object we put it in an array so we can handle it the same way.
            $.each(files, function (i, file) {
                if (remoteFiles[i].success) {
                    file.deleteUrl = remoteFiles[i].deleteUrl;
                    file.fileUrl = remoteFiles[i].fileUrl;
                    file.thumbnail = remoteFiles[i].thumbnail;
                    attachColorbox(file.id, file.fileUrl); // Use colorbox to show the original image
                } else {
                    // Do stuff
                    // We are here because in Example 1 of our server side exception extension the HTTP status code is set to 200 (default)
                    // In Example 2 and 3 we set the HTTP status code 500 within the extension, so this handler will not be called and we do not
                    // need to switch on the file.success condition.
                    file.loaded = 0; // reset all progress
                    file.status = plupload.FAILED; // set failed status
                    up.trigger('Error', { code: plupload.HTTP_ERROR, message: remoteFiles[i].error, file: file, response: file, status: 500, responseHeaders: "" });
                }
            });
        }
    });



    // Load existing files, if any.
    $.ajax({
        url: backloadController,
        type: "GET",
        dataType: "json"
    }).done(function (data, textStatus, jqXHR) {
        var upFiles = data.files;
        var files = [];
        for (var i = 0; i < upFiles.length; i++) {
            var file = new plupload.File("", upFiles[i].name, upFiles[i].size);
            file.percent = upFiles[i].percent + "%";
            file.name = upFiles[i].name;
            file.loaded = upFiles[i].size;
            file.size = upFiles[i].size;
            file.origSize = upFiles[i].size;
            file.status = plupload.DONE;
            file.type = upFiles[i].type;
            file.thumbnail = upFiles[i].thumbnail;
            file.fileUrl = upFiles[i].fileUrl;
            file.deleteUrl = upFiles[i].deleteUrl;
            files.push(file);
        }
        if (uploader) uploader.addFile(files);
        // PlUpload needs some time to insert the nodes. We must add the thumbnail images by hand,
        // because PlUpload does usually not handle existing files and does not show thumbnails for those files.
        setTimeout(function () {
            $.each(files, function (i, file) {
                var $thumb = $('li#' + file.id + ' div.plupload_file_thumb');
                $thumb.html('<img src="' + file.thumbnail + '" title="Existing file, click me!" />');
                attachColorbox(null, file.fileUrl, $thumb);  // Optional: Use colorbox to show the original image on click
            });
        }, 50);
    }).error(function (jqXHR, status, error) {
            uploader.trigger('Error', { code: plupload.HTTP_ERROR, message: error, file: null, response: jqXHR.responseText, status: status, responseHeaders: "" });
        })
    ;

    function attachColorbox(id, url, $thumb) {
        if (!$thumb) var $thumb = $('li#' + id + ' div.plupload_file_thumb');
        $thumb.colorbox({photo: true, href: url }); // attach colorbox
    }
});