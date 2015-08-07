using Backload.Contracts;
using Backload.Contracts.Context;
using Backload.Contracts.Extension;
using Backload.Contracts.Params;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Backload.Extension.Contoso
{
    [Export(typeof(IDeleteFilesRequest))]
    public class DeleteFilesRequest : IDeleteFilesRequest
    {
        public bool Execute(IBackloadContext context, IDeleteFilesRequestParam param)
        {
            // In this extension we delete the copies we've created within the _Backup and _Temp folder.
            // Backload has made the copies within the _Backup folder automatically, because it is configured to do so (config file) 
            // The copies within the _Temp folder were created for demo purposes in the StoreFileRequest extension.
            string dirBackup = string.Empty;
            string dirTemp = string.Empty;

            if (param.DeleteFiles == null) return false; // in the case something went wrong.
            if (param.DeleteFiles.Files.Count > 0)
            {
                dirTemp = Path.GetDirectoryName(Helper.GetLocalFilePath(context.Request, param.DeleteFiles.First().FileUrl).Replace("\\Uploads", "\\_Temp")); // Helper method in Backload.Contracts to get the full local file path
                dirBackup = Path.GetDirectoryName(Helper.GetLocalFilePath(context.Request, param.DeleteFiles.First().FileUrl).Replace("\\Uploads", "\\_Backup")); // Helper method in Backload.Contracts to get the full local file path
            }

            foreach (var file in param.DeleteFiles.Files)
            {
                // Synchronous deletion:
                // Delete the backup file Backload created (see config file)
                string pathBackupFile = Path.Combine(dirBackup, file.FileName);
                if (File.Exists(pathBackupFile)) File.Delete(pathBackupFile);

                // Asynchronous deletion with Tasks:
                // Demo 6: Running and awaiting a method asynchronously in a thread of the AppDomains thread pool.
                // Note: We do not use await here, because we have implemented the synchronous interface.
                // The Tasks will be awaited at the end of the extension point for the FilesDelete extension. If we do not await the 
                // result, the task may be aborted and does not finish file deletion.
                var t = Task.Run(() => DeleteTempFiles(dirTemp, file.FileName));
                context.PipelineControl.TaskManager.ExtensionPointTasks.Add(t);
            }
            return (param.DeleteFiles.Files.Count > 0);
        }


        public void DeleteTempFiles(string dir, string fileName)
        {
            string placeholder = Path.GetFileNameWithoutExtension(fileName) + "*" + Path.GetExtension(fileName);
            string[] fileList = Directory.GetFiles(dir, placeholder);
            foreach (string file in fileList)
            {
                string fullPath = Path.Combine(dir, file);
                if (File.Exists(fullPath)) File.Delete(fullPath);
            }
            return;
        }

    }
}