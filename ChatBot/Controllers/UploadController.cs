using ChatBot.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            UploadViewModel vm = new UploadViewModel();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UploadViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // Do something
                vm.SaveUrl();
            }

            return View(vm);
        }
    }
}
