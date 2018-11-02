using System;
using Demo.AspNetCore.ServerSentEvents.Model;

namespace Demo.AspNetCore.ServerSentEvents.Services.Bots
{
    public interface IChatBot
    {
        ChatMessage JoinChat(); ChatMessage 
        ProcessChatInteraction(ChatMessage msg);
    }
}
