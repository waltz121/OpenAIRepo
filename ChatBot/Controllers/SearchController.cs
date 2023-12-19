using ChatBot.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            SearchViewModel vm = new SearchViewModel();
            // vm.InsertTestData();
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(SearchViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // Handle the Search Logic Here
            }

            return View(vm);
        }
    }
}
