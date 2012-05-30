using System;
using System.Linq;
using System.Text;
using System.Web.Optimization;

namespace SampleTransforms.Transforms.Workbench
{
    /// <summary>
    /// To be able to to compile this code you need to buy Workbench Pro from Mindscape which comes with command line compiler. 
    /// It works with 32bit host processes only. If you need to run it from a 64bit process use the same technique as the one used in NodeJsTransform.
    /// Make sure that you either install http://www.microsoft.com/en-us/download/details.aspx?id=5555 on the server or 
    /// include 32bit version of msvcr100.dll with your application.
    /// </summary>
    public class WorkbenchTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            var output = new StringBuilder();
            var errors = new StringBuilder();
            // This won't compile because the code uses Workbench Pro which couldn't be included.
            Mindscape.VisualSass.CommandLine.CommandLineCompiler.Run( response.Files.Select(file => file.FullName).ToArray(), 
                                                                    (path, content) => output.Append(content), 
                                                                    error => errors.Append(error));

            if (!String.IsNullOrEmpty(errors.ToString())) throw new LessParsingException(errors.ToString());
            response.ContentType = "text/css";
            response.Content = output.ToString();
        }
    }
}