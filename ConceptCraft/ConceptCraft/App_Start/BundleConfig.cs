using System.Web;
using System.Web.Optimization;

namespace CRMAdmin
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/Basic/script").Include(
                        "~/Scripts/jquery-2.1.1.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/plugins/metisMenu/jquery.metisMenu.js",
                        "~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Main/css").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/font-awesome/css/font-awesome.css",
                      "~/Content/css/plugins/footable/footable.core.css",
                      "~/Content/css/plugins/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css",
                      "~/Content/css/plugins/select2/select2.min.css",
                      "~/Content/css/animate.css",
                      "~/Content/css/plugins/datapicker/datepicker3.css",
                      "~/Content/css/style.css"));
            bundles.Add(new StyleBundle("~/Datepicker/css").Include(
                      "~/Content/css/plugins/datapicker/datepicker3.css"));

            bundles.Add(new ScriptBundle("~/Datepicker/script").Include(
                     "~/Scripts/plugins/datapicker/bootstrap-datepicker.js",
                     "~/Scripts/plugins/iCheck/icheck.min.js"));
        }
    }
}
