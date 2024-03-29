﻿using Domain.Models;
using Domain.ViewModels.Bookmark;

namespace Application.Interfaces;

public interface IBookmarkService
{
    public Task AddBookmark(BookmarkViewModel model);
    public Task<FilterBookmarkViewModel> GettBookmarkList(FilterBookmarkViewModel viewModel);
    public Task<bool> AddBefor(int ContentId, int UserId);
    public Task RemoveFromBookmark(Bookmark model);
    public Task<Bookmark> getBookmark(int ContentId, int UserId);

}