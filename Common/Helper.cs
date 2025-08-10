using Markdig;
using Microsoft.Extensions.AI;
using MyCompanionAI.Core.DTOs;
using MyCompanionAI.Core.Enums;

namespace MyCompanionAI.Common;

public static class Helper
{
    public static List<ChatMessage> ConvertToChatMessages(List<ChatDto> chatDtos)
    {
        return chatDtos.Select(chat => new ChatMessage(chat.Role == CompanionChatRole.Assistant ? ChatRole.Assistant : ChatRole.User, chat.Message)).ToList();
    }
    public static string ToHtml(string markdown)
    {
        markdown = markdown.Replace("###", "##") // Normalize headings
                       .Replace("<br>", "\n") // Fix line breaks
                       .Replace("**", "__");  // Optional bold tweak

        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        return Markdown.ToHtml(markdown, pipeline);
    }

}
