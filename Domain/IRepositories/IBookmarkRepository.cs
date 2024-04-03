﻿using Domain.Models;
using Domain.ViewModels.Bookmark;

namespace Domain.IRepositories;

public interface IBookmarkRepository
{
    public Task AddBookmark(Bookmark bookmark);
    public Task<FilterBookmarkViewModel> FiltertBookmarks(FilterBookmarkViewModel model);
    public Task<bool> AddeBefor(int ContentId, int UserId);
    public Task RemoveFromBookmark(Bookmark model);
    public Task<Bookmark> getBookmark(int ContentId, int UserId);
    public Task SaveAsync();
}