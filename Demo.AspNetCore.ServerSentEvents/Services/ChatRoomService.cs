using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.AspNetCore.ServerSentEvents.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Demo.AspNetCore.ServerSentEvents.Services
{
    public class ChatRoomService : IChatRoomService
    {
        public Dictionary<string, Actor> ChatRoom { get; set; }
        public List<ChatMessage> ChatTranscript { get; set; }
        private INotificationsService notificationsService;
        public ChatRoomService(INotificationsService _notificationsService)
        {
            ChatRoom = new Dictionary<string, Actor>();
            ChatTranscript = new List<ChatMessage>();
            notificationsService = _notificationsService;
        }
        public void AddActor(Actor a)
        {
            string id = string.Format("{0}_{1}", a.ActorId, a.ActorName);
            if(!ChatRoom.ContainsKey(id))
                ChatRoom.Add(id, a);
        }
        public void RemoveActor(Actor actor)
        {
            string id = string.Format("{0}_{1}",actor.ActorId,actor.ActorName);
            if(ChatRoom.ContainsKey(id))
                ChatRoom.Remove(id);
        }
        public void AddMessage(ChatMessage message)
        {
            ChatTranscript.Add(message);
        }
        public IDictionary<string,string> AddMessageToTranscript(ChatMessage msg)
        {
            bool isRoomChanged = false;
            switch (msg.TheMessage.MessageType)
            {
                case "join":
                    AddActor(msg.ChatActor);
                    isRoomChanged = true;
                    break;
                case "left":
                    RemoveActor(msg.ChatActor);
                    isRoomChanged = true;
                    break;
                default:
                    break;
            }
            AddMessage(msg);
            Dictionary<string,string> result = new Dictionary<string, string>();
            if(isRoomChanged)
            {
                result.Add("chat-room", JsonConvert.SerializeObject(ChatRoom));
            }
            result.Add("chat-message", JsonConvert.SerializeObject(msg));
            return result;

        }
    }
}
