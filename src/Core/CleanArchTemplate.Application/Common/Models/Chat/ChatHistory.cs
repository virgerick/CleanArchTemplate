using System;

using CleanArchTemplate.Application.Common.Interfaces.Chat;

namespace CleanArchTemplate.Application.Common.Models.Chat
{
    public partial class ChatHistory<TUser> : IChatHistory<TUser> where TUser : IChatUser
    {
        
        public long Id { get; set; }
        public required string FromUserId { get; set; }
        public required string ToUserId { get; set; }
        public required string Message { get; set; }
        public DateTimeOffset CreateAt { get ; set ; }
        public  TUser? FromUser { get; set; }
        public  TUser? ToUser { get; set; }
    }
}