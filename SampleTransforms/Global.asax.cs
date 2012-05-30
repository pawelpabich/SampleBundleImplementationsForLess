using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SampleTransforms.Transforms.DotLess;
using SampleTransforms.Transforms.NodeJs;
using SampleTransforms.Transforms.Workbench;

namespace SampleTransforms
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            SetupBundle("~/css/nodejs", new NodeJsTransform());
            SetupBundle("~/css/dotless", new DotLessTransform());
            SetupBundle("~/css/workbench", new WorkbenchTransform());
        }

        private static void SetupBundle(string path, IBundleTransform transform)
        {
            var cssBundle = new Bundle(path, transform);
            cssBundle.AddDirectory("~/Content", "*.less", true);
            BundleTable.Bundles.Add(cssBundle);
        }
    }
}