using ChatBot.Models;
using ChatBot.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OpenAiCore.OpenAiRepository.DTO.OpenAi;
using System.Diagnostics;

namespace ChatBot.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SendMessage([FromBody]ChatViewModel chatvm)
        {
            var reply = await chatvm.GetBotReply_WithContext();
            return Json(new { message = reply.Choices[0].Message.Content });
        }

        [HttpGet]
        public async Task<JsonResult> GetInitialChatViewModel()
        {
            ChatViewModel chatvm = new ChatViewModel();
            MessagesDTO tpm = await chatvm.SetInitialMessage();
            chatvm.Messages = new List<MessagesDTO>();
            chatvm.Messages.Add(tpm);
            return Json(chatvm);
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
    }
}
