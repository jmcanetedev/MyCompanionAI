using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyCompanionAI.Data;

namespace MyCompanionAI.Core.Services;

public class _BaseService
{
    public IDbContextFactory<MyCompanionDbContext> context;
    public IMapper _mapper;
    public IMemoryCache _cache;
    public _BaseService(IDbContextFactory<MyCompanionDbContext> context, IMapper mapper, IMemoryCache cache)
    {
        this.context = context;
        this._mapper = mapper;
        this._cache = cache;
    }
}
