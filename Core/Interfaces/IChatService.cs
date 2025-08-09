using Microsoft.Extensions.AI;
using MyCompanionAI.Core.DTOs;

namespace MyCompanionAI.Core.Interfaces;

public interface IChatService
{
    Task<List<ChatDto>> GetChatsAsync(Guid conversationId);
    Task<bool> SaveChatAsync(ChatDto chatDto);
    Task<bool> SaveAllChatAsync(List<ChatMessage> messages, Guid conversationId);
    Task<bool> UpdateChatAsync(Guid chatId, ChatDto chatDto);
}
