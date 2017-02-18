using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;
using System.Web.Optimization;

namespace ASP.MetopeNspace
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            var cssTransformer = new StyleTransformer();
            var jsTransformer = new ScriptTransformer();
            var nullOrderer = new NullOrderer();

            var cssBundle = new StyleBundle("~/bundles/css");
            //cssBundle.Include("~/Content/Site.less", "~/Content/bootstrapSlate.css", "~/Content/PagedList.css,", "~/Content/myCustom.css");
            cssBundle.Include("~/Content/Site.less", "~/Content/bootstrap/bootstrap.less", "~/Content/PagedList.css,", "~/Content/myCustom.css");
            cssBundle.Transforms.Add(cssTransformer);
            cssBundle.Orderer = nullOrderer;
            bundles.Add(cssBundle);

            bundles.Add(new StyleBundle("~/Content/datetime").Include(
            "~/Content/bootstrap-datetimepicker*"));
           
            bundles.Add(new StyleBundle("~/Content/GridMvc").Include(
                "~/Content/Gridmvc.*"));
            //var cssBundle2= new StyleBundle("~/bundles/cssCustom");
            //cssBundle2.Include("~/Content/Site.less", "~/Content/Template/myCustom.css");
            //cssBundle2.Transforms.Add(cssTransformer);
            //cssBundle2.Orderer = nullOrderer;
            //bundles.Add(cssBundle2);

            var jqueryBundle = new ScriptBundle("~/bundles/jqueryThings");
            jqueryBundle.Include("~/Scripts/jquery-{version}.js", "~/Scripts/jquery-ui-{version}.js", "~/Scripts/jquery.validate*");
            jqueryBundle.Transforms.Add(jsTransformer);
            jqueryBundle.Orderer = nullOrderer;
            bundles.Add(jqueryBundle);

            var jqueryvalBundle = new ScriptBundle("~/bundles/jqueryval");
            jqueryvalBundle.Include("~/Scripts/jquery.validate*");
            jqueryvalBundle.Transforms.Add(jsTransformer);
            jqueryvalBundle.Orderer = nullOrderer;
            bundles.Add(jqueryvalBundle);

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            var modernizrBundle = new ScriptBundle("~/bundles/modernizr");
            modernizrBundle.Include("~/Scripts/modernizr-*");
            modernizrBundle.Transforms.Add(jsTransformer);
            modernizrBundle.Orderer = nullOrderer;
            bundles.Add(modernizrBundle);


            bundles.Add(new ScriptBundle("~/bundles/datetime").Include(
            "~/Scripts/moment*",
            "~/Scripts/bootstrap-datetimepicker*",
            "~/Scripts/common/datetimepicker-init.js")); 

            bundles.Add(new ScriptBundle("~/bundles/bootstrapWizard").Include(
            "~/Scripts/jquery.bootstrap.wizard*",
            "~/Scripts/common/wizard-init.js"));

            bundles.Add(new ScriptBundle("~/bundles/GridMvc").Include(
            "~/Scripts/gridmvc.min.js",
            "~/Scripts/bootstrap-datepicker.js"));  
              
            var bootstrapBundle = new ScriptBundle("~/bundles/bootstrap");
            bootstrapBundle.Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js");
            bootstrapBundle.Transforms.Add(jsTransformer);
            bootstrapBundle.Orderer = nullOrderer;
            bundles.Add(bootstrapBundle);


        }
    }
}