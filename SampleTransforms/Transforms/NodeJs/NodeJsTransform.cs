using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web.Optimization;

namespace SampleTransforms.Transforms.NodeJs
{
    public class NodeJsTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            var output = new StringBuilder();
            var errors = new StringBuilder();
            var applicationFolder = context.HttpContext.Server.MapPath(@"~/");
            var nodejs = Path.GetFullPath(applicationFolder + @"..\nodejs"); 
            foreach (var file in response.Files)
            {
                 using (var process = new Process())
                 {
                        process.StartInfo = new ProcessStartInfo
                        {
                            FileName = Path.Combine(nodejs, "node.exe"),
                            Arguments = Path.Combine(nodejs, @"node_modules\less\bin\lessc") + " " + file.FullName,
                            CreateNoWindow = true,
                            RedirectStandardError = true,
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            WorkingDirectory = applicationFolder,
                        };
                        process.OutputDataReceived += (sender, e) => output.AppendLine(e.Data);
                        process.ErrorDataReceived += (sender, e) => { if (!String.IsNullOrWhiteSpace(e.Data)) errors.AppendLine(e.Data); };
                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.WaitForExit();
                    }
                }
            
                if (!String.IsNullOrEmpty(errors.ToString())) throw new LessParsingException(errors.ToString());
                response.ContentType = "text/css";
                response.Content = output.ToString();
            }            
        }
    }
