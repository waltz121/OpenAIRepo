
using OpenAI_API.Chat;
using OpenAI_API.Models;
using OpenAI_API.Moderation;
using OpenAiCore;
using OpenAiCore.AzureOpenAi.Interface;
using System.ComponentModel.DataAnnotations;

namespace OpenAI.UnitTests
{
    [TestClass]
    public class SampleChatCompletionTests
    {
        IAzureOpenAiAuthorizationServices OpenAIAuthServices;

        public SampleChatCompletionTests() {
            OpenAIAuthServices = new AzureOpenAiAuthorizationServices();
            Config.Init("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");
        }

        [TestMethod]
        public void Sample_SimpleChat() {
            var api = OpenAIAuthServices.FromOpenAiOrg();
            var chat = api.Chat.CreateConversation();
            string response = "";
            // Give Instruction as System
            chat.AppendSystemMessage("You are an Assistant that helps the user with their questions and inquiries");

            chat.AppendUserInput("What is the day today?");
            
            Task.Run(async () =>
            {
                response = await chat.GetResponseFromChatbotAsync();
            }).GetAwaiter().GetResult();
            
            Console.WriteLine(response);
        }

        [TestMethod]
        public void Sample_AdvanceChat()
        {
            var api = OpenAIAuthServices.FromOpenAiOrg();
            var chat = api.Chat.CreateConversation();
            string response = "";

            Task.Run(async () =>
            {
                var result = await api.Chat.CreateChatCompletionAsync(new OpenAI_API.Chat.ChatRequest()
                {
                    Model = Model.GPT4,
                    Temperature = 0.5,
                    MaxTokens = 50,
                    Messages = new ChatMessage[] {
                        new ChatMessage(ChatMessageRole.User,"1 + 1 equals?")
                    }

                });
                var reply = result.Choices[0].Message;

                response = reply.Role + ": " + reply.Content.Trim();
            }).GetAwaiter().GetResult();
        }
    }
}
