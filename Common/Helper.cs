using Markdig;
using Microsoft.Extensions.AI;
using MyCompanionAI.Core.DTOs;
using MyCompanionAI.Core.Enums;
using System.Text.RegularExpressions;

namespace MyCompanionAI.Common;

public static class Helper
{
    /// <summary>
    /// Convert Chat History to ChatMessages
    /// </summary>
    /// <param name="chatDtos"></param>
    /// <returns></returns>
    public static List<ChatMessage> ConvertToChatMessages(List<ChatDto> chatDtos)
    {
        return chatDtos.Select(chat => new ChatMessage(chat.Role == CompanionChatRole.Assistant ? ChatRole.Assistant : ChatRole.User, chat.Message)).ToList();
    }
    /// <summary>
    /// Convert the content to html format
    /// </summary>
    /// <param name="markdown"></param>
    /// <returns></returns>
    public static string ToHtml(string markdown)
    {
        var codePattern = @"```(.*?)\n(.*?)```";
        var html = Regex.Replace(markdown, codePattern, match =>
        {
            var lang = match.Groups[1].Value;
            var code = match.Groups[2].Value;
            return $"<pre><code class=\"language-{lang}\">{System.Net.WebUtility.HtmlEncode(code)}</code></pre>";
        }, RegexOptions.Singleline);


        markdown = html
                       .Replace("<br>", "\n"); // Fix line breaks

        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        return Markdown.ToHtml(markdown, pipeline);
    }

}
