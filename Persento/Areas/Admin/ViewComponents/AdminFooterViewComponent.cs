using Microsoft.AspNetCore.Mvc;

namespace Persento.Areas.Admin.ViewComponents
{
    public class AdminFooterViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
