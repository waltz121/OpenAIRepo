using ChatBot.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            SearchViewModel vm = new SearchViewModel();
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Index(SearchViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await vm.GetSearchResults();
                
            }

            return View(vm);
        }
    }
}
