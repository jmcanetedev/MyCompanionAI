using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Caching.Memory;
using MyCompanionAI.Core.DTOs;
using MyCompanionAI.Core.Interfaces;
using MyCompanionAI.Data;
using MyCompanionAI.Data.Models;

namespace MyCompanionAI.Core.Services;

public class ChatService : _BaseService, IChatService
{
    private const string CacheKey = "ChatsCache";
    public ChatService(IDbContextFactory<MyCompanionDbContext> context, IMapper mapper, IMemoryCache cache) : base(context, mapper, cache) { }
    public async Task<List<ChatDto>> GetChatsAsync(Guid conversationId)
    {
        using var dbContext = await context.CreateDbContextAsync();

        var chats = await dbContext.Chats
            .Where(c => c.ConversationId == conversationId).OrderBy(c => c.CreatedOn)
            .ToListAsync();
        var cachedChats = _mapper.Map<List<ChatDto>>(chats);


        return cachedChats.Any() ? cachedChats : [];
    }

    public async Task<bool> SaveAllChatAsync(List<ChatMessage> messages, Guid conversationId)
    {
        List<ChatDto> chatDtos = messages.Select(m => new ChatDto
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            Message = m.Text,
            CreatedOn = DateTime.UtcNow,
            Role = m.Role == ChatRole.Assistant ? Enums.CompanionChatRole.Assistant : Enums.CompanionChatRole.User,
            UpdatedOn = DateTime.UtcNow,
            Conversation = null,
        }).ToList();

       
        
        using var dbContext = await context.CreateDbContextAsync();
        
        var existingChats = await dbContext.Chats
            .Where(c => c.ConversationId == conversationId)
            .ToListAsync();

        var conversation = await dbContext.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation == null)
            return false;

        conversation.UpdatedOn = DateTime.UtcNow;

        conversation.Title = chatDtos.FirstOrDefault()?.Message ?? "New Conversation";

        var chats = _mapper.Map<List<Chat>>(chatDtos);
        
        dbContext.Conversations.Update(conversation);
        if (existingChats.Any())
        {
            dbContext.Chats.RemoveRange(existingChats); // Remove existing chats for the conversation
        }

        dbContext.Chats.AddRange(chats);

        var result = await dbContext.SaveChangesAsync();

        if (result > 0)
        {
            // Invalidate the cache since new chats were saved
            _cache.Remove(CacheKey);
        }
        return result > 0; // Return true if any changes were made
    }

    public async Task<bool> SaveChatAsync(ChatDto chatDto)
    {
        using var dbContext = await context.CreateDbContextAsync();
        var exist = await dbContext.Chats.AnyAsync(c => c.Id == chatDto.Id);

        if (exist)
        {
            return false; // Chat already exists
        }

        var chat = _mapper.Map<Chat>(chatDto);

        dbContext.Chats.Add(chat);

        var result = await dbContext.SaveChangesAsync();

        if (result > 0)
        {
            // Invalidate the cache since a new chat was saved
            _cache.Remove(CacheKey);
        }
        return result > 0; // Return true if any changes were made
    }

    public async Task<bool> UpdateChatAsync(Guid chatId, ChatDto chatDto)
    {
        using var dbContext = await context.CreateDbContextAsync();

        var existingChat = await dbContext.Chats.FirstOrDefaultAsync(c => c.Id == chatId);

        if (existingChat == null)
        {
            return false; // Chat not found
        }

        // Update the existing chat with new values
        existingChat.Message = chatDto.Message;
        existingChat.UpdatedOn = DateTime.UtcNow;

        dbContext.Chats.Update(existingChat);

        var result = await dbContext.SaveChangesAsync();

        if (result > 0)
        {
            // Invalidate the cache since a chat was updated
            _cache.Remove(CacheKey);
        }

        return result > 0; // Return true if any changes were made
    }
}
