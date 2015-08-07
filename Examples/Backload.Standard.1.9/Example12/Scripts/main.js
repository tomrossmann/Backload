$(function () {
    // Note: We do not use Backloads internal controller as the url below links to our own controller
    var backloadController = "/FileUpload/FileHandler";

    $("#uploader").plupload({
        runtimes: 'html5,html4',
        url: backloadController,
        max_file_size: '20mb',
        max_file_count: 64, // user can add no more then 10 files at a time
        filters: [{ title: "Image files", extensions: "jpg,gif,png" }], // Specify what files to browse for
        preinit: { Init: function (up) {  } },  // Do init stuff here 
        views: { list: true, thumbs: true, active: 'thumbs' }
    });
    var uploader = $('#uploader').plupload('getUploader');


    // PlUpload doesn't send a delete request to the server automatically, so we do it with an jquery ajax request
    uploader.bind("FilesRemoved", function (up, files) {
        $.each(files, function (i, file) {
            if ((typeof file.deleteUrl !== "undefined") && (file.deleteUrl != "")) {
                $.ajax({
                    url: file.deleteUrl, 
                    type: "DELETE",
                    dataType: "json"
                }).done(function (data, textStatus, jqXHR) {
                    if (data.success != true) {
                        // Add error handling.
                    }

                    var log = $('#eventlog').html() + data.eventlog;
                    $('#eventlog').html(log);
                });
            }
        });
    });


    // After a file was uploaded we extend the internal file class in the plupload.files array with a delete url.
    uploader.bind('FileUploaded', function (up, files, result) {
        var data = { files: [], eventlog: "" };
        if (typeof result.response !== "undefined") data = JSON.parse(result.response);
        var remoteFiles = data.files;
        if (!$.isArray(files)) files = [files];
        $.each(files, function (i, file) {
            file.deleteUrl = remoteFiles[i].deleteUrl;
            file.fileUrl = remoteFiles[i].fileUrl;
            file.thumbnail = remoteFiles[i].thumbnail;
            attachColorbox(file.id, file.fileUrl) // Use colorbox to show the original image
        });

        var log = $('#eventlog').html() + data.eventlog;
        $('#eventlog').html(log);
    });


    // We do not use the file added event but if you need to manipulate the dom, give PlUpload a little time to add the files to the list
    uploader.bind("FilesAdded", function (up, files) {
        setTimeout(function () {
            $.each(files, function (i, file) {
            });
        }, 50);
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

        if ((typeof data.eventlog !== "undefined") && (data.eventlog != null)) $('#eventlog').html(data.eventlog);
    });

    function attachColorbox(id, url, $thumb) {
        if (!$thumb) var $thumb = $('li#' + id + ' div.plupload_file_thumb');
        $thumb.colorbox({ href: url }); // attach colorbox
    }
});