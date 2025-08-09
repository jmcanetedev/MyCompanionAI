namespace MyCompanionAI.Data.Models;

public class Conversation : _BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<Chat> Chats { get; set; } = [];
}
