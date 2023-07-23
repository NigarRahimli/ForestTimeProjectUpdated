using ForestTime.WebUI.Models;

namespace ForestTime.WebUI.ViewModels
{
    public class DetailVM
    {
        public Article Article { get; set;}
      
        public List <Comment> Comments { get; set; }
        public Article PrevArticle { get; set;} 
        public Article NextArticle { get; set;} 


    }
}
