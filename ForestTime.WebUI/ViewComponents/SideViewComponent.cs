using ForestTime.WebUI.Data;
using Microsoft.AspNetCore.Mvc;

namespace ForestTime.WebUI.ViewComponents
{
    public class SideViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public SideViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = _context.Categories.Take(5).ToList();
            return View("Category", categories);
        }
    }
}
