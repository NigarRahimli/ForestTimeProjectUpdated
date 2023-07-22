using ForestTime.WebUI.Models;

namespace ForestTime.WebUI.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<Article> Articles { get; set; }
    }
}
