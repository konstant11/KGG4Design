using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.AspNetCore.ServerSentEvents.Model;
using Demo.AspNetCore.ServerSentEvents.Services;
using Demo.AspNetCore.ServerSentEvents.Services.Bots;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Owin;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Demo.AspNetCore.ServerSentEvents.Controllers
{
    public class ChatController : Controller
    {
        // GET: /<controller>/
        private IChatService chatService;
        private INotificationsService notificationsService;
        private IChatBot chatBot;
        public ChatController(IChatService _chatService, INotificationsService _notificationsService, IChatBot _chatBot)
        {
            chatService = _chatService;
            notificationsService = _notificationsService;
            chatBot = _chatBot;
        }


        [ActionName("chat-session")]
        [AcceptVerbs("GET")]

        public IActionResult ChatInitRestore(ChatViewModel viewModel)
        {
            new Thread(() =>
            {
                Thread.Sleep(1000);
                ChatMessage cm = chatBot.JoinChat();
                chatService.AddMessageToTranscript(cm);
                SendServerSideEventRoomMessage(chatService.GetActors());
                SendServerSideEventMessage(cm);
            }).Start();
            return View("StartChat");
        }

        [ActionName("chat-message")]
        [AcceptVerbs("POST")]
        public async Task<IActionResult> ChatMessage()
        {
            Stream req = Request.Body;
            //req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            ChatViewModel viewModel = null;
            try
            {
                viewModel = JsonConvert.DeserializeObject<ChatViewModel>(json);
            }
            catch(Exception ex){
                Console.WriteLine(ex);
            }
            if (ModelState.IsValid)
            {
                if(viewModel != null && viewModel.Message != null && viewModel.Message.TheMessage != null)
                {

                    chatService.AddMessageToTranscript(viewModel.Message);
                    await SendServerSideEventMessage(chatService.GetChatMessageToDistribute());
                    if(viewModel.Message.TheMessage.MessageType == "join" || viewModel.Message.TheMessage.MessageType == "left")
                    await SendServerSideEventRoomMessage(chatService.GetActors());
                    ChatMessage replyBot = chatBot.ProcessChatInteraction(viewModel.Message);
                    await SendServerSideEventMessage(replyBot);
                }
            }
            return Ok();
        }
        private async Task<IActionResult> SendServerSideEventMessage(ChatMessage message)
        {
            string ss = JsonConvert.SerializeObject(message);
            await notificationsService.SendNotificationAsync("chat-message", ss);
            return Ok();
        }
        private async Task<IActionResult> SendServerSideEventRoomMessage(Actor[] message)
        {
            string ss = JsonConvert.SerializeObject(message);
            await notificationsService.SendNotificationAsync("chat-room", ss);
            return Ok();
        }
    }
}
