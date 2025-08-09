using AutoMapper;
using MyCompanionAI.Core.DTOs;
using MyCompanionAI.Data.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyCompanionAI.Core;

public class AutoMapperProfile : Profile
{
    readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = true
    };
    public AutoMapperProfile()
    {
        _ = CreateMap<Conversation, ConversationDto>().ReverseMap();
        _ = CreateMap<Chat, ChatDto>().ReverseMap();
        _ = CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
    }
}