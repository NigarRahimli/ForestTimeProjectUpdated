using ForestTime.WebUI.Data;
using ForestTime.WebUI.DTOs;
using ForestTime.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var topArticles = _context.Articles.OrderByDescending(x => x.Views).Take(3).ToList();
            var recentArticles = _context.Articles.OrderByDescending(x => x.Id).Take(3).ToList();

            // Retrieve categories and their article counts
            var categoriesWithCounts = _context.Articles.Include(x=>x.Category)
                .GroupBy(a => a.Category.CategoryName)
                .Select(g => new CategoryWithCountDTO
                {
                    CategoryName = g.Key,
                    CategoryCount = g.Count()
                })
                .ToList();

            SideVM sideArticle = new SideVM
            {
                
                CategoriesWithCounts = categoriesWithCounts,
                TopArticles = topArticles,
                RecentAddedArticles = recentArticles
            };

            return View("Side", sideArticle);
        }

    }
}
