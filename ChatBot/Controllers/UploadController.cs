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
                await vm.SaveUrl();

                // Code for waiting 1 minute
                //await Task.Delay(3000);
                //vm.ResultMessage = "Success";
            }

            return View(vm);
        }

        public IActionResult BatchUpload()
        {
            BatchUploadViewModel vm = new BatchUploadViewModel();
            return View(vm);
        }
    }
}
