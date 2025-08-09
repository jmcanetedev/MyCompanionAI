namespace MyCompanionAI.Core.DTOs;

public class ConversationDto : _BaseDto
{
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<ChatDto> Chats { get; set; } = [];
}
