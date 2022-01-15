using Microsoft.AspNetCore.Mvc;

namespace Persento.Areas.Admin.ViewComponents
{
    public class SidebarViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
