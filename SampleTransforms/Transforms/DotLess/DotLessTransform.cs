using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;
using dotless.Core;

namespace SampleTransforms.Transforms.DotLess
{
    public class DotLessTransform : IBundleTransform
    {
        public static Regex ImportPattern = new Regex("@import\\s+\\\"(?<File>.*)\\\"", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        public void Process(BundleContext context, BundleResponse response)
        {           
            var engine = new LessEngine() { Logger = new ExceptionLogger() };
            var output = new StringBuilder();
            foreach (var file in response.Files)
            {
                var content = File.ReadAllText(file.FullName);
                content = ResolveImportPaths(content, context.HttpContext);                
                var css = engine.TransformToCss(content, null);
                output.Append(css);
            }
            
            response.ContentType = "text/css";
            response.Content = output.ToString();
        }


        // We need this hack as DotLess doesn't seem to handle relative paths well. Workbench and Nodejs do not have this problem.
        private static string ResolveImportPaths(string content, HttpContextBase context)
        {
            var fullPathToContentFolder = context.Server.MapPath("~/Content");
            var imports = ImportPattern.Matches(content);
            foreach (Match import in imports)
            {
                var valueToReplace = import.Groups["File"].Value;
                var newValue = Path.Combine(fullPathToContentFolder, valueToReplace);
                content = content.Replace(valueToReplace, newValue);
            }
            return content;
        }
    }
}