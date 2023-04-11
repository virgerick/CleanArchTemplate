using CleanArchTemplate.Application.Common.Interfaces.Chat;
using CleanArchTemplate.Application.Common.Models.Chat;
using CleanArchTemplate.Shared.Responses.Identity;
using CleanArchTemplate.Shared.Wrapper;

namespace CleanArchTemplate.Application.Common.Interfaces.Services
{
    public interface IChatService
    {
        Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId);

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> message);

        Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId);
    }
}