using System;
using BusinessChat.Domain.Common;

namespace BusinessChat.Domain.Entities
{
    public class ChatMessage : BaseEntity
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime SendAt { get; set; }

        public ChatMessage()
        {
        }
    }
}