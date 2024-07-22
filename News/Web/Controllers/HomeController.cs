using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IPostService _postService;

        public HomeController(ILogger<HomeController> logger
            , IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var hotPost = await _postService.GetPinnedPost(string.Empty);
            ViewData["HotPost"] = hotPost;
            ViewData["ShowCategoryBar"] = true;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search([FromQuery] Dictionary<string, string> param)
        {
            var posts = await _postService.Search(param);
            ViewData["ShowCategoryBar"] = true;
            return View(posts);
        }
    }
}
