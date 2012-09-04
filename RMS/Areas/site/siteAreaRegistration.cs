using System.Web.Mvc;

namespace RMS.Areas.site
{
    public class siteAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "site";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "site_default",
                "site/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
