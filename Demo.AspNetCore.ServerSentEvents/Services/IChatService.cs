using System;
using System.Collections.Generic;
using Demo.AspNetCore.ServerSentEvents.Model;

namespace Demo.AspNetCore.ServerSentEvents.Services
{
    public interface IChatService
    {
        /// <summary>
        /// Gets the actors of the chat.
        /// </summary>
        /// <returns>The actors.</returns>
        Actor[] GetActors();
        /// <summary>
        /// Adds the chat message to transcript.
        /// </summary>
        /// <param name="msg">ChatMessage.</param>
        void AddMessageToTranscript(ChatMessage msg);
        /// <summary>
        /// Gets the last chat message to distribute.
        /// </summary>
        /// <returns>The chat message to distribute.</returns>
        ChatMessage GetChatMessageToDistribute();
    }
}
