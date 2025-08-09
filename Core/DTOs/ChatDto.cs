
using MyCompanionAI.Core.Enums;

namespace MyCompanionAI.Core.DTOs;

public class ChatDto : _BaseDto
{
    public Guid ConversationId { get; set; }
    public string Message { get; set; } = string.Empty;
    public CompanionChatRole Role { get; set; } = CompanionChatRole.User;
    public ConversationDto Conversation { get; set; } = new();
}
