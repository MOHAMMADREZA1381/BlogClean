﻿using Domain.IRepositories;
using Domain.Models;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class ViewCountRepository:IViewCountRepository
{
    #region Context
    private readonly BlogContext _context;
    public ViewCountRepository(BlogContext context)
    {
        _context = context;
    }
    #endregion
    public async Task AddView(ContentViews contentViews)
    {
        await _context.AddAsync(contentViews);
            await _context.SaveChangesAsync();
    }

    public async Task<bool> IsAnyIp(string UserIp,int ContentId)
    {
        return await _context.Views.AnyAsync(a=>a.UserIp==UserIp && a.ContentId==ContentId);
    }
}