using System.Web.Optimization;

namespace Moemisto.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/loader/jquery.loader.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/ajax").Include(
               // "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery.form.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap/bootstrap.css",
                "~/Content/font-awesome/font-awesome.css",
                "~/Scripts/loader/jquery.loader.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/pagination").Include(
                "~/Scripts/App/pagination.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                "~/Scripts/moment-with-locales.js",
                "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new StyleBundle("~/Content/datepicker").Include(
                "~/Content/datetimepicker/bootstrap-datetimepicker-build.css"));

            bundles.Add(new StyleBundle("~/Content/css-admin").Include(
                "~/Content/bootstrap/bootstrap.css",
                "~/Content/admin.css"));

            //bundles.Add(new ScriptBundle("~/bundles/js-admin").Include(
            //    "~/Scripts/editor.js"));
        }
    }
}
