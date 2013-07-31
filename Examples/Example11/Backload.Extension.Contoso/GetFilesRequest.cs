using Backload.Contracts.Context;
using Backload.Contracts.Extension;
using Backload.Contracts.Params;
using Backload.Contracts.Status;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Backload.Extension.Contoso
{
    /// <summary>
    /// IGetFilesRequest (asynchronous) extension 1
    /// </summary>
    [Export(typeof(IGetFilesRequestAsync))]
    public class GetFilesRequest : IGetFilesRequestAsync
    {
        public async Task<bool> ExecuteAsync(IBackloadContext context, IGetFilesRequestParam param)
        {
            if (param.FileStatus == null) return false; // in the case something went wrong.

            // In this example we want the user to work with a backup file we created on file upload.
            // Note: We do not need to use async/await or Tasks in this simple example. We only simulate a long running task.
            // Important notes: In order to test these demos set breakpoints in all extensions at the beginning of the 
            // ProcessStep/ProcessStepAsync method. After changing the code always rebuild this extension or set a project dependency 
            // for the extension (context menu or Project menu) in your MVC project as we did, so the extension class is automatically 
            // rebuilded. The output build path of the extension is set to the main MVC project in this solution (folder ~/bin/extensions/). 
            // If you do not rebuild the extension, the main MVC application may uses the old extension.

            // Demo 1: The following line simulates a long running task. We do not care about (await) the results of this task, so it 
            // does not block. The task runs as a background thread within the AppDomains thread pool. You should not use this technique 
            // with file IO, because the thread may be aborted when Backload returns. (If you do not want to await the results of a long 
            // running File I/O operation take a look in the StoreFile extension were we're using a separate thread).
            var t = Task.Run(() => Task.Delay(System.TimeSpan.FromSeconds(10)));
            
            // Demo 2: Uncomment the line below, set breakpoints (also in the GetFilesRequest2 extension) and rebuild the extension. 
            // You'll notice that debugging stops and awaits the result of the task.
            // The subsequent code and extensions are not executed until the task is finished. But, the ui thread is not blocked and tied up.
            // You can see this, because the web page comes back. Usually, when ececuting and debug synchronous code, the web page 
            // doesn't come back in long running tasks.
            // await t;

            // Demo 3: Comment out the await line above, uncomment the following line, set breakpoints and rebuild the extension. 
            // The debugging now doesn't stop here and the rest of this extensions code (and the extension handling code in the extension manager) 
            // are executed. Before calling the next extension whithin this extension point, the extension manager awaits the result. Note that 
            // the second extension below isn't called until the task is finished.
            // context.PipelineControl.TaskManager.ExtensionTasks.Add(t);

            // Demo 4: Comment out the ExtensionTasks.Add line above, uncomment the following line, set breakpoints and rebuild the extension. 
            // When debugging the application you'll see, that the second extension also can execute and finish its code, before the 
            // extension manager awaits at the end of the extension point (before code execution returns to the pipeline) the result.
            // context.PipelineControl.TaskManager.ExtensionPointTasks.Add(t);

            // Demo 5: Comment out the ExtensionPointTasks.Add line above, uncomment the following line, set breakpoints and rebuild the extension. 
            // Make sure you have set a breakpoint in the OutgoingResponse extension were the response for the PlUpload plugin is generated. 
            // The difference to the code above is, that the OutgoingResponse was called before the Pipeline waits for the result of the task. 
            // PipelineExtendedTasks is ultimately the last point in the processing pipeline before the Pipeline returns the results to the client.
            // context.PipelineControl.TaskManager.PipelineExtendedTasks.Add(t);

            // Note: we do not have mentioned PipelineCoreTasks so far. This point is reached when pipeline core ends. The core methods responsible
            // for doing the GET/POST/PUT/DELETE request have done their job, but before the output was generated and the OutgoingResponse extensions 
            // were called. In this demo there would be no difference to Demo 4. If you're using the early extension points (e.g. IncomingRequest) 
            // multiple extension points are between this extension point and the end of pipeline core and may better see the difference.
            return false;
        }
    }


    /// <summary>
    /// IGetFilesRequest (asynchronous) extension 2
    /// </summary>
    [Export(typeof(IGetFilesRequestAsync))]
    public class GetFilesRequest2 : IGetFilesRequestAsync
    {
        public async Task<bool> ExecuteAsync(IBackloadContext context, IGetFilesRequestParam param)
        {
            if (param.FileStatus == null) return false; // if cace something went wrongt.

            // We use this extension only to demonstrate you different examples of blocked and parallel requests.

            // IMPORTANT NOTE: The FileStatusSimple here is only to show, that you can create your FileStatus from scratch.
            // You may use FileStatusSimple as we did here or implement your own iFileUploadStatus class if you need it.
            // In this example it'll be sufficient to simply set the file.FileUrl of the FileStatus property, no need to use FileStatusSimple.
            FileStatusSimple status = new FileStatusSimple();
            foreach (var file in param.FileStatus.Files)
            {
                file.FileUrl = file.FileUrl.Replace("/Uploads", "/_Backup");
                status.Files.Add(file);
            }
            param.FileStatus = status;

            return (param.FileStatus.Files.Count > 0); // if FileStatus.files.Count > 0 we've processed the response, so we need notify the pipeline
        }

    }
}