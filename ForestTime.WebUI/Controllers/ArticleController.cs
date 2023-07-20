using ForestTime.WebUI.Data;
using ForestTime.WebUI.Models;
using ForestTime.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ForestTime.WebUI.Controllers
{
    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public ArticleController(AppDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Detail(Comment comment)
        {
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Comment newComment = new();

            newComment.CreatedDate = DateTime.Now;
            newComment.UserId = userId;
            newComment.ArticleId = comment.ArticleId;
            newComment.Message = comment.Message;

            var article = _context.Articles.FirstOrDefault(x => x.Id == comment.ArticleId);

            await _context.Comments.AddAsync(newComment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", new { id = article.Id});
        }
        
        public IActionResult Detail(int id)
        {
            try
            {
            var article=_context.Articles.Include(x=>x.User).Include(x=>x.Category).Include(x=>x.ArticleTags).ThenInclude(x=>x.Tag).FirstOrDefault(x=>x.Id == id);
                var topArticles = _context.Articles.OrderByDescending(x=>x.Views).Take(3).ToList();
                var recentArticles=_context.Articles.OrderByDescending(x=>x.Id).Take(3).ToList();
                var currentArticle = _context.Articles.Include(x => x.Category).FirstOrDefault(x => x.Id == id);

                var prevArticle = _context.Articles
                    .Include(x => x.Category)
                    .Where(x => x.Id != id && x.Id < currentArticle.Id)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                var nextArticle = _context.Articles
                    .Include(x => x.Category)
                    .Where(x => x.Id != id && x.Id > currentArticle.Id)
                    .OrderBy(x => x.Id)
                    .FirstOrDefault();
                //eger bos olarsa ilk article kimi basa dusur ve siralayib axrinci article i goturur
                if (prevArticle == null)
                {
                    prevArticle = _context.Articles
                        .Include(x => x.Category)
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefault();
                }
                //eger bos olarsa son article kimi basa dusur ve siralayib ilk article i goturur
                if (nextArticle == null)
                {
                    nextArticle = _context.Articles
                        .Include(x => x.Category)
                        .OrderBy(x => x.Id)
                        .FirstOrDefault();
                }


                if (article == null)
            {
                return NotFound();
            }
                var comments = _context.Comments.Include(x => x.User).Where(x=>x.ArticleId==id).ToList();
                DetailVM vm = new()
                {
                    Article = article,
                    TopArticles = topArticles,
                    RecentAddedArticles= recentArticles,
                    Comments = comments,
                    PrevArticle=prevArticle,
                    NextArticle=nextArticle

                };
                article.Views += 1;
                _context.Articles.Update(article);
                _context.SaveChanges();
            return View(vm);
            }
            catch (Exception)
            {

                return NotFound();
            }
        }
        public IActionResult Category(int categoryId)
        {
            var getByCategory = _context.Articles.Include(x => x.Category).Where(x => x.Category.Id == categoryId).ToList();
            var vm = new CategoryVM()
            {
                GetByCategory=getByCategory

            };

            return View(vm);
        }
    }

}
