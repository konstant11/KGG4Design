using System;
using System.Collections.Generic;
using Demo.AspNetCore.ServerSentEvents.Model;

namespace Demo.AspNetCore.ServerSentEvents.Services
{
    public class ChatService : IChatService
    {
        public IDictionary<string, Actor> ChatRoom { get; set; }
        List<ChatMessage> Transcript { get; set; }
        public ChatService()
        {
            ChatRoom = new Dictionary<string, Actor>();
            Transcript = new List<ChatMessage>();
        }
        public void AddMessageToTranscript(ChatMessage msg)
        {
            switch (msg.TheMessage.MessageType)
            {
                case "join":
                    AddActor(msg);
                    break;
                case "left":
                    RemoveActor(msg);
                    break;
                default:
                    break;
            }
            Transcript.Add(msg);
        }
        private void AddActor(ChatMessage msg)
        {
            string id = string.Format("{0}_{1}", msg.ChatActor.ActorId, msg.ChatActor.ActorName);
            if (!ChatRoom.ContainsKey(id))
                ChatRoom.Add(id, msg.ChatActor);
            else
                Console.WriteLine("Client with that ID already in chatroom...");
        }
        public Actor[] GetActors()
        {
            List<Actor> res = new List<Actor>();
            foreach (Actor a in ChatRoom.Values)
                res.Add(a);
            return res.ToArray();
        }
        private void RemoveActor(ChatMessage msg)
        {
            string id = string.Format("{0}_{1}", msg.ChatActor.ActorId, msg.ChatActor.ActorName);
            if (ChatRoom.ContainsKey(id))
                ChatRoom.Remove(id);
        }
        private ChatMessage GetLastMessage()
        {
            return Transcript.FindLast(AllwaysTrue);
        }

        Predicate<ChatMessage> AllwaysTrue = s => true;
        public ChatMessage GetChatMessageToDistribute()
        {
            return GetLastMessage();
        }
 
    }
}
