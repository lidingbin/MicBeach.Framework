using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Site.Cms.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Scripts

            bundles.Add(new ScriptBundle("~/scripts/jquery").Include("~/Content/scripts/jquery.js"));
            bundles.Add(new ScriptBundle("~/scripts/jqueryui").Include("~/Content/scripts/jquery-ui.js"));
            bundles.Add(new ScriptBundle("~/scripts/jqueryval").Include("~/Content/scripts/jquery.validate.js", "~/Content/scripts/jquery.validate.unobtrusive.js", "~/Content/scripts/jquery.unobtrusive-ajax.js"));
            bundles.Add(new ScriptBundle("~/scripts/bootstrap").Include("~/Content/scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/scripts/mousewheel").Include("~/Content/scripts/jquery.mousewheel.js"));
            bundles.Add(new ScriptBundle("~/scripts/slimscroll").Include("~/Content/scripts/jquery.slimscroll.js"));
            bundles.Add(new ScriptBundle("~/scripts/index").Include("~/Content/scripts/index.js"));
            bundles.Add(new ScriptBundle("~/scripts/tool").Include("~/Content/scripts/Tool.js"));
            bundles.Add(new ScriptBundle("~/scripts/uimain").Include("~/Content/scripts/ui.js"));
            bundles.Add(new ScriptBundle("~/scripts/dialog").Include("~/Content/scripts/dialog/artDialog.js", "~/Content/scripts/dialog/iframeTools.js"));
            bundles.Add(new ScriptBundle("~/scripts/editor").Include("~/Content/scripts/editor/ueditor.config.js", "~/Content/scripts/editor/ueditor.all.js"));
            bundles.Add(new ScriptBundle("~/scripts/ztree").Include("~/Content/scripts/ztree/jquery.ztree.all.js", "~/Content/scripts/ztree/ztree.exhide.js"));
            bundles.Add(new ScriptBundle("~/scripts/upload").Include("~/Content/scripts/editor/third-party/webuploader/webuploader.js"));
            bundles.Add(new ScriptBundle("~/scripts/date").Include("~/Content/scripts/datepicker/WdatePicker.js"));
            bundles.Add(new ScriptBundle("~/scripts/color/jsfile").Include("~/Content/scripts/color/js/colorpicker.js"));
            bundles.Add(new ScriptBundle("~/scripts/table").Include("~/Content/scripts/table/jquery.dataTables.js", "~/Content/scripts/table/dataTables.bootstrap.js", "~/Content/scripts/table/dataTables.fixedColumns.js", "~/Content/scripts/moment.js"));

            #endregion

            #region Css

            bundles.Add(new StyleBundle("~/style/login").Include("~/Content/Style/css/bootstrap.css", "~/Content/Style/css/login.css"));
            bundles.Add(new StyleBundle("~/style/bootstrap").Include("~/Content/Style/css/bootstrap.css", "~/Content/Style/css/customerbootstrap.css"));
            bundles.Add(new StyleBundle("~/style/common").Include("~/Content/Style/css/common.css", "~/Content/Style/css/margin.css", "~/Content/Style/css/padding.css", "~/Content/Style/css/position.css", "~/Content/Style/css/wh.css", "~/Content/Style/css/border.css"));
            bundles.Add(new StyleBundle("~/style/index").Include("~/Content/Style/css/iconfont.css", "~/Content/Style/css/index.css"));
            bundles.Add(new StyleBundle("~/style/main").Include("~/Content/Style/css/main.css"));
            bundles.Add(new StyleBundle("~/style/table").Include("~/Content/Style/css/table/dataTables.bootstrap.css", "~/Content/Style/css/table/fixedColumns.bootstrap.css"));
            bundles.Add(new StyleBundle("~/style/ztree").Include("~/Content/scripts/ZTree/skin.css"));
            bundles.Add(new StyleBundle("~/style/upload").Include("~/Content/scripts/editor/third-party/webuploader/webuploader.css"));
            bundles.Add(new StyleBundle("~/style/jqui").Include("~/Content/Style/css/jquery-ui.css"));
            bundles.Add(new StyleBundle("~/style/color").Include("~/Content/scripts/color/css/colorpicker.css"));
            bundles.Add(new StyleBundle("~/style/error").Include("~/Content/Style/css/error.css"));

            #endregion
        }
    }
}