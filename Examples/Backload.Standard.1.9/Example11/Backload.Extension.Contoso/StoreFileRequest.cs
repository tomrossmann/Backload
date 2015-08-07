using Backload.Contracts;
using Backload.Contracts.Context;
using Backload.Contracts.Extension;
using Backload.Contracts.Params;
using System.ComponentModel.Composition;
using System.IO;

namespace Backload.Extension.Contoso
{
    [Export(typeof(IStoreFileRequest))]
    public class StoreFileRequest : IStoreFileRequest
    {
        public bool Execute(IBackloadContext context, IStoreFileRequestParam param)
        {
            // In this example we make 50 copies of the uploaded file manually. 
            //
            // Note: You do not need to make copies manually. Backload supports auto copies of uploaded files. 
            // You find the setting for auto copies in the config file (fileSystem: copiesPath).
            // In this demo Backload is configured to make additional copies within the ~/Files/_Backup folder.
            string localBackupPath = Helper.GetLocalFilePath(context.Request, param.FileStatusItem.FileUrl); // Helper method in Backload.Contracts to get the full local file path
            localBackupPath = localBackupPath.Replace("\\Uploads", "\\_Temp"); // Make sure ~/Files/_Temp folder exists

            // Because we want to post back the new path to the browser, we set the FileUrl property to the new path.
            // File status item halods the data send back to the client. You can change FileStatusItem.FileData which is the 
            // bayte array to be stored immediately after the extensions finish.
            param.FileStatusItem.FileUrl = localBackupPath; 
            
            // There are 5 locations within the processing pipeline to wait for the async operation to be finished:
            // 1. Use await:
            //      Example: await fileStream.WriteAsync(...);
            //      Info: Waits for the operation to be finished, but does not block the thread. The operating system can do other things.
            // 2. Use a Task and wait at the end of the extensions handler code (before other extensions are called, if any):
            //      Example: var task = fileStream.WriteAsync(...);
            //               PipelineControl.TaskManager.ExtensionTask.Add(task);
            //      Info: Finishes the execution of this extension and then waits for the task to be finished (Multiple tasks are supported).
            //            Other extensions whithin this extension point are not executed until this task finishes.
            // 3. Use a Task and wait at the end of this extension point (all extensions of this extension point are finished):
            //      Example: var task = fileStream.WriteAsync(...);
            //               PipelineControl.TaskManager.ExtensionPointTasks.Add(task);
            //      Info: Finishes the execution of all extensions within this extension point and then waits for all tasks to be finished.
            // 4. Use a Task and wait at the end the core pipeline:
            //      Example: var task = fileStream.WriteAsync(...);
            //               PipelineControl.TaskManager.PipelineCoreTasks.Add(task);
            //      Info: Waits at the end of pipeline core for the task to be finished. This is a point, when the core GET, POST/PUT, DELETE 
            //            method has done its job (e.g. saved a file) but before the final output was generated.    
            // 5. Use a Task and wait at the end of the processing pipeline:
            //      Example: var task = fileStream.WriteAsync(...);
            //               PipelineControl.TaskManager.PipelineExtendedTasks.Add(task);
            //      Info: Waits at the ultimative end of the handler for the task to be finished. This is a point immediately before 
            //            the return statement of the processing pipeline (Output for the client browser has been generated already).


            // Demo 7: In this demo we do our work in a separate thread and do not care about the result (fire&forget strategy). 
            // Because we cannot be sure that the thread finished the work successfully be careful with this approach.
            // If you rely on the results, use async/await or the task based approach or check if the files exist in another 
            // extension, a subsequent request, a different code or application (e.g.: if (!File.Exits(...)) File.Copy(...));
            // Comment out the following line:
            new System.Threading.Thread(new System.Threading.ThreadStart(new MuchToDo(param.FileStatusItem.FileData, localBackupPath).WriteFiles)).Start();

            return true; // We changed a property, so we set this.Processed to true
        }

    }

    public class MuchToDo
    {
        private const int COPIES = 50;

        public byte[] File;
        public string FilePath;
        public MuchToDo(byte[] file, string path)
        {
            this.File = file;
            this.FilePath = path;
        }
        public async void WriteFiles()
        {
            string ext = Path.GetExtension(this.FilePath);
            string filename = Path.GetFileNameWithoutExtension(this.FilePath);
            string dir = Path.GetDirectoryName(this.FilePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            for (int i = 1; i <= COPIES; i++)
            {
                string no = i.ToString().PadLeft(3, '0');
                string fullPath = Path.Combine(dir, filename + no +  ext);
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await fileStream.WriteAsync(File, 0, File.Length);
                }
                System.Diagnostics.Debug.WriteLine("Write asynchronous file no. {0} of {1} from an independent thread.", i, COPIES);
            }

            System.Diagnostics.Debug.WriteLine("Independent thread terminates.");
        }
    }
}