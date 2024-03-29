﻿using Application.Interfaces;
using Application.SenderEmail;
using Application.Services;
using Domain.IRepositories;
using Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace Infra.Ioc
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //    <---Application LAYER--->
            services.AddScoped<IRenderService, RenderViewToString>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<IContactUsService, ContactUsService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<ICaseMessageService, CaseMessageService>();
            services.AddScoped<IViewCountService, ViewCountService>();
            services.AddScoped<IFollowService, FollowService>();
            services.AddScoped<IBookmarkService, BookmarkService>();
            services.AddScoped<IReportContentService, ReportContentService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IUseFulLinksService, UseFulLinksService>();


            //    <---Data Layer --->
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<ICategoryRepository,CategoryRepository>();
            services.AddScoped<IContentRepository, ContentRepository>();
            services.AddScoped<IContactUsRepository, ContactUsRepository>();
            services.AddScoped<ICaseMessageRepository, CaseMessageRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IViewCountRepository, ViewCountRepository>();
            services.AddScoped<IFollowRepository, FollowRepository>();
            services.AddScoped<IBookmarkRepository, BookmarkRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IUseFulLinksRepository, UseFulLinksRepository>();


        }
    }
}
