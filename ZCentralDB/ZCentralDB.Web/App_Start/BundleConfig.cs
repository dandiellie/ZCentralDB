using System.Web;
using System.Web.Optimization;

namespace ZCentralDB.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-ui-router.js",
                        "~/app/app.module.js",
                        "~/app/app.state.js",
                        "~/app/app.constant.js",
                        // services
                        //"~/app/components/employees/employeesService.js",
                        //"~/app/components/location/locationService.js",
                        //"~/app/components/login/loginService.js",
                        //"~/app/components/orders/ordersService.js",
                        //"~/app/components/runner/runnerService.js",
                        //"~/app/components/vendor/vendorService.js",
                        // controllers
                        //"~/app/components/employees/employeesController.js",
                        "~/app/components/index/indexController.js"));
        }
    }
}
