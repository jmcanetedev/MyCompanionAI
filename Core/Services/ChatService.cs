using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyCompanionAI.Core.DTOs;
using MyCompanionAI.Core.Interfaces;
using MyCompanionAI.Data;

namespace MyCompanionAI.Core.Services;

public class ChatService : _BaseService, IChatService
{
    public ChatService(IDbContextFactory<MyCompanionDbContext> context, IMapper mapper, IMemoryCache cache) : base(context, mapper, cache)
    {
       
    }
    public Task<bool> DeleteChatAsync(Guid chatId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ChatDto>> GetChatsAsync(Guid conversationId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveChatAsync(ChatDto chatDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateChatAsync(Guid chatId, ChatDto chatDto)
    {
        throw new NotImplementedException();
    }
}
