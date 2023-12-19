using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
