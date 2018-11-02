using System;
using System.Collections.Generic;
using Demo.AspNetCore.ServerSentEvents.Model;

namespace Demo.AspNetCore.ServerSentEvents.Services
{
    public interface IChatRoomService
    {
        Dictionary<string, Actor> ChatRoom { get; set; }
        List<ChatMessage> ChatTranscript { get; set; }
        void AddActor(Actor a);
        void RemoveActor(Actor actor);
        void AddMessage(ChatMessage message);
        IDictionary<string, string> AddMessageToTranscript(ChatMessage msg);
    }
}