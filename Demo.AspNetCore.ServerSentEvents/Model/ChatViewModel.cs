using System;
using System.Collections.Generic;
using Demo.AspNetCore.ServerSentEvents.Services;
using Newtonsoft.Json;

namespace Demo.AspNetCore.ServerSentEvents.Model
{
    public class ChatViewModel
    {
        public ChatMessage Message { get; set; }
        public IChatService chatService { get; set; }
        public ChatViewModel()
        {
           
        }
        public ChatMessage ParseMessage(string json)
        {
            ChatMessage message = JsonConvert.DeserializeObject<ChatMessage>(json);
            return message;
        }
        public string SerializeMessage(ChatMessage msg)
        {
            string message = JsonConvert.SerializeObject(msg);
            return message;
        }
    }
    public class ChatMessage
    {
        public DateTime TimeStamp;
        public int SequenceNumber;
        public Actor ChatActor;
        public Message TheMessage;
    }
    public class Actor
    {
        public string ActorName;
        public string ActorType;
        public string ActorId;
        public object ActorAvatar;
    }
    public class Message
    {
        public string MessageType;
        public string MessageContent;
        public object MessageBinary;

    }

}
