using System;
using Demo.AspNetCore.ServerSentEvents.Model;

namespace Demo.AspNetCore.ServerSentEvents.Services.Bots
{
    public class ChatBot :IChatBot
    {
        Actor myInfo { get; set; }
        public ChatBot()
        {
            myInfo = new Actor
            {
                ActorId = "000000000",
                ActorName = "Home Bot",
                ActorType = "System",
                ActorAvatar = "/images/chat/bot_avatar.png"
            };

        }
        public ChatMessage JoinChat()
        {
            ChatMessage message = new ChatMessage()
            {
                ChatActor = myInfo,
                TheMessage = new Message
                {
                    MessageType = "join",
                    MessageContent = "I am a home Bot and this is my room",
                }

            };
            return message;
        }
        public ChatMessage ProcessChatInteraction(ChatMessage msg)
        {
            ChatMessage message = new ChatMessage()
            {
                ChatActor = myInfo,
                TheMessage = new Message
                {
                    MessageType = "text" 
                }

            };
            if (msg.TheMessage.MessageType == "join")
            {
                message.TheMessage.MessageContent = string.Format("Dear {0} I glad to see you in my  chat room. I will copy your messages as long as you will post them.", msg.ChatActor.ActorName);
            }
            else if(msg.TheMessage.MessageType == "left")
            {
                message.TheMessage.MessageContent = string.Format("Dear {0} I hope you found this chat helpful. Have a good day and hope to see you next time in my room.", msg.ChatActor.ActorName);
            }
            else
            message.TheMessage.MessageContent = string.Format("Good job, {0}! I've got your message as following:\"{1}\" I hope this is correct.", msg.ChatActor.ActorName, msg.TheMessage.MessageContent);
            return message;

        }
    }
}
