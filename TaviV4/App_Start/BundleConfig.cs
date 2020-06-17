using System.Web;
using System.Web.Optimization;

namespace TaviV4
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/admin-lte/js").Include(
                      "~/AdminLTE/plugins/jquery/jquery.js",
                      "~/AdminLTE/plugins/jquery/jquery.min.js",
                      "~/Scripts/jquery.validate.js",
                      "~/AdminLTE/plugins/jquery-ui/jquery-ui.min.js",
                      "~/AdminLTE/plugins/bootstrap/js/bootstrap.bundle.min.js",
                      "~/AdminLTE/plugins/jsgrid/jsgrid.min.js",
                      "~/AdminLTE/plugins/sweetalert2/sweetalert2.min.js",
                      "~/AdminLTE/plugins/toastr/toastr.min.js",
                      "~/AdminLTE/plugins/chart.js/Chart.min.js",
                      "~/AdminLTE/plugins/jquery-knob/jquery.knob.min.js",
                      "~/AdminLTE/plugins/moment/moment.min.js",
                      "~/AdminLTE/plugins/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",
                      "~/AdminLTE/plugins/daterangepicker/daterangepicker.js",
                      "~/AdminLTE/plugins/bootstrap-datepicker/dist/locales/bootstrap-datepicker.vi.min.js", // language for datepicker
                      "~/AdminLTE/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
                      "~/AdminLTE/plugins/summernote/summernote-bs4.min.js",
                      "~/AdminLTE/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js",
                      "~/AdminLTE/dist/js/adminlte.js",
                       //"~/AdminLTE/dist/js/pages/dashboard.js",
                       "~/AdminLTE/plugins/select2/js/select2.full.js",
                        "~/AdminLTE/plugins/inputmask/jquery.inputmask.bundle.js",
                        "~/AdminLTE/plugins/chart.js/Chart.js",
                        "~/Scripts/jquery.slimscroll.min.js",
                       "~/AdminLTE/dist/js/demo.js",
                      "~/Scripts/Site.js",
                      "~/Scripts/jquery.validate.min.js",
                      "~/Scripts/jsTree/jstree.min.js"
                      ));
            bundles.Add(new StyleBundle("~/admin-lte/css").Include(
                     "~/AdminLTE/plugins/fontawesome-free/css/all.min.css",
                     "~/AdminLTE/plugins/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css",
                     "~/AdminLTE/plugins/jsgrid/jsgrid.min.css",
                     "~/AdminLTE/plugins/jsgrid/jsgrid-theme.min.css",
                     "~/AdminLTE/plugins/toastr/toastr.min.css",
                      "~/AdminLTE/plugins/overlayScrollbars/css/OverlayScrollbars.min.css",
                      "~/AdminLTE/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css",
                      "~/AdminLTE/plugins/icheck-bootstrap/icheck-bootstrap.min.css",
                      "~/AdminLTE/plugins/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css",
                      "~/AdminLTE/plugins/daterangepicker/daterangepicker.css",
                      "~/AdminLTE/plugins/summernote/summernote-bs4.css",
                      "~/AdminLTE/plugins/select2/css/select2.css",
                      "~/AdminLTE/plugins/select2-bootstrap4-theme/select2-bootstrap4.css",
                      "~/AdminLTE/dist/css/adminlte.min.css",
                      "~/Scripts/jsTree/style.min.css",
                      "~/Content/PagedList.css",
                     "~/Content/Site.css"
                     ));
            bundles.Add(new StyleBundle("~/admin-lte/calendar/css").Include(
                "~/AdminLTE/plugins/fullcalendar/main.min.css",
                "~/AdminLTE/plugins/fullcalendar-interaction/main.min.css",
                "~/AdminLTE/plugins/fullcalendar-daygrid/main.min.css",
                "~/AdminLTE/plugins/fullcalendar-timegrid/main.min.css",
                "~/AdminLTE/plugins/fullcalendar-bootstrap/main.min.css"
                ));
            bundles.Add(new StyleBundle("~/admin-lte/calendar/js").Include(
                "~/AdminLTE/plugins/moment/moment.min.js",
                "~/AdminLTE/plugins/fullcalendar/main.min.js",
                "~/AdminLTE/plugins/fullcalendar-daygrid/main.min.js",
                "~/AdminLTE/plugins/fullcalendar-timegrid/main.min.js",
                "~/AdminLTE/plugins/fullcalendar-interaction/main.min.js",
                "~/AdminLTE/plugins/fullcalendar-bootstrap/main.min.js",
                "~/AdminLTE/plugins/fullcalendar/locales-all.min.js",
                "~/AdminLTE/plugins/fullcalendar/locales/vi.js"
                ));
        }
    }
}
