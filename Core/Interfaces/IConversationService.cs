using MyCompanionAI.Core.DTOs;

namespace MyCompanionAI.Core.Interfaces;

public interface IConversationService
{
    Task<List<ConversationDto>> GetConversationsAsync();
    Task<ConversationDto> GetConversationByIdAsync(Guid conversationId);
    Task<bool> SaveConversation(ConversationDto conversationDto);
    Task<bool> UpdateConversationAsync(Guid conversationId, ConversationDto conversationDto);
    Task<bool> DeleteConversationAsync(Guid conversationId);
}
