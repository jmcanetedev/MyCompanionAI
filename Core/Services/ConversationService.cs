using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyCompanionAI.Core.DTOs;
using MyCompanionAI.Core.Interfaces;
using MyCompanionAI.Data;
using MyCompanionAI.Data.Models;

namespace MyCompanionAI.Core.Services;

public class ConversationService : _BaseService, IConversationService
{
   
    private const string CacheKey = "ConversationsCache";
    public ConversationService(IDbContextFactory<MyCompanionDbContext> context, IMapper mapper, IMemoryCache cache) : base(context, mapper, cache) { }
    public async Task<bool> DeleteConversationAsync(Guid conversationId)
    {
        using var dbContexxt = await context.CreateDbContextAsync();

        var conversation = await dbContexxt.Conversations.FirstOrDefaultAsync(c=>c.Id == conversationId);

        if (conversation == null)
        {
            return false; // Conversation not found
        }
        
        dbContexxt.Conversations.Remove(conversation);
        
        var result = await dbContexxt.SaveChangesAsync();

        if (result > 0)
        {
            // Invalidate the cache since a conversation was deleted
            _cache.Remove(CacheKey);
        }

        return result > 0; // Return true if any changes were made

    }

    public async Task<ConversationDto> GetConversationByIdAsync(Guid conversationId)
    {
        using var dbContext = await context.CreateDbContextAsync();
        var conversation = await dbContext.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation == null)
            return null;

        return _mapper.Map<ConversationDto>(conversation);
    }

    public async Task<List<ConversationDto>> GetConversationsAsync()
    {
        // Check if the conversations are cached
        if (!_cache.TryGetValue(CacheKey, out List<ConversationDto> cachedConversations))
        {
            using var dbContext = await context.CreateDbContextAsync();

            var conversations = await dbContext.Conversations.OrderByDescending(c=>c.CreatedOn).ToListAsync();

            cachedConversations = _mapper.Map<List<ConversationDto>>(conversations);

            // Update the cache with the latest conversations
            _cache.Set(CacheKey, cachedConversations, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Cache for 10 minutes
            });

        }

        return cachedConversations.Count != 0 ? cachedConversations : [];
    }

    public async Task<bool> SaveConversation(ConversationDto conversationDto)
    {
        using var dbContext = await context.CreateDbContextAsync();
        
        var conversation = _mapper.Map<Conversation>(conversationDto);

        var existingConversation = await dbContext.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversation.Id);

        if (existingConversation != null)
            return false; // Conversation already exists

        existingConversation = await dbContext.Conversations.FirstOrDefaultAsync(c => string.IsNullOrEmpty(c.Title));

        if (existingConversation != null)
            return false; // Conversation with empty title already exists
        
        conversation.CreatedOn = DateTime.UtcNow;

        dbContext.Conversations.Add(conversation);

        var result = await dbContext.SaveChangesAsync();

        if (result > 0)
        {
            // Invalidate the cache since a new conversation was added
            _cache.Remove(CacheKey);
        }

        return result > 0; // Return true if any changes were made
    }

    public async Task<bool> UpdateConversationAsync(Guid conversationId, ConversationDto conversationDto)
    {
        using var dbContext = await context.CreateDbContextAsync();
        var existingConversation = await dbContext.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId);
        if (existingConversation == null)
            return false;
        // Map the updated properties from conversationDto to existingConversation
        existingConversation.Title = conversationDto.Title;
        existingConversation.UpdatedOn = DateTime.UtcNow;

        // You can map other properties as needed
        dbContext.Conversations.Update(existingConversation);
        
        var result = await dbContext.SaveChangesAsync();

        if (result > 0)
        {
            // Invalidate the cache since a conversation was updated
            _cache.Remove(CacheKey);
        }

        return result > 0; // Return true if any changes were made

    }
}
