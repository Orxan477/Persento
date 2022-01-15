using Microsoft.AspNetCore.Mvc;

namespace Persento.Areas.Admin.ViewComponents
{
    public class NavbarViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
