using Backload.Contracts.Context;
using Backload.Contracts.Extension;
using Backload.Contracts.Params;
using System;
using System.ComponentModel.Composition;

namespace Backload.Extension.Contoso
{
    [Export(typeof(IIncomingRequest))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class IncomingRequest : IIncomingRequest
    {

        // This extension is only for demo purposes, as we throw an exception here
        // Don't forget to rebuid your solution if you made changes to your extension, otherwise you may use the old extension assembly.
        public bool Execute(IBackloadContext context, IIncomingRequestParam param)
        {
            if (context.HttpMethod == "POST")
            {
                for (int i = 0; i < context.Request.Files.Count; i++)
                {
                    var file = context.Request.Files[i];
                    if (file.FileName.ToLower().Contains("badfile"))
                    {
                        context.PipelineControl.Message.MessageText = "Bad file name.";
                        throw new Exception(context.PipelineControl.Message.MessageText);
                    }
                }
            }
            return false; // No properties have been changed, so we set return false.
        }
    }
}
