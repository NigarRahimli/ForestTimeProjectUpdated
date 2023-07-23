using ForestTime.WebUI.DTOs;
using ForestTime.WebUI.Models;

namespace ForestTime.WebUI.ViewModels
{
    public class SideVM
    {
        public List<CategoryWithCountDTO> CategoriesWithCounts { get; set; }
        public List<Article> Articles { get; set; }
        public List<Article> TopArticles { get; set; }
        public List<Article> RecentAddedArticles { get; set; }

    }
}
