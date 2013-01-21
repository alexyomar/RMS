using System.Web.Mvc;

namespace RMS.Areas.web
{
    public class webAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Web";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "web_secciones",
                "Web/{controller}/{action}/{id}/{name}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "web_default",
                "Web/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
