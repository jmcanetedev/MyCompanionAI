using MyCompanionAI.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCompanionAI.Data.Models;

public class Chat : _BaseEntity
{
    [ForeignKey("ConversationId")]
    public Guid ConversationId { get; set; }
    public string Message { get; set; } = string.Empty;
    public CompanionChatRole Role { get; set; } = CompanionChatRole.User;
   
    public Conversation Conversation { get; set; }
}
