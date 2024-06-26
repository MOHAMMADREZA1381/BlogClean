﻿using Application.ImageTools;
using Application.Interfaces;
using Domain.ViewModels.Content;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.ImageTools.Common;
using Domain.Models;
using Domain.ViewModels.Message;
using Domain.ViewModels.Report;
using Microsoft.AspNetCore.HttpOverrides;

namespace BlogClean.Controllers
{

    public class ContentController : Controller
    {
        #region Services
        private readonly IContentService _contentService;
        private readonly ICategoryService _categoryService;
        private readonly IViewCountService _viewCountService;
        private readonly IFollowService _followService;
        private readonly IBookmarkService _bookmarkService;
        private readonly IReportContentService _reportContent;
        public ContentController(IContentService contentService, ICategoryService categoryService, IViewCountService viewCountService, IFollowService followService, IBookmarkService bookmarkService, IReportContentService reportContent)
        {
            _contentService = contentService;
            _categoryService = categoryService;
            _viewCountService = viewCountService;
            _followService = followService;
            _bookmarkService = bookmarkService;
            _reportContent = reportContent;
        }
        #endregion

        #region ContentList
        [Route("ContentList")]
        public async Task<IActionResult> Index(FilterContentViewModel viewModel)
        {

            var Content = await _contentService.GetContentWithFilter(viewModel);

            return View(Content);
        }
        #endregion

        #region ContentDetails
        [Route("Content-Details")]
        public async Task<IActionResult> ContentDetails(int id, int? HowManyCaseShow, string? state)
        {


            TempData["MessageType"] = state;
            ///for load more case message
            if (HowManyCaseShow == null || HowManyCaseShow == 0)
            {
                HowManyCaseShow = 4;
            }
            TempData["HowManyShowCase"] = HowManyCaseShow.Value;

            bool ContentExist = await _contentService.IsAnyContent(id);
            var ContentViewModel = new ContentDetailsViewModel();
            if (ContentExist == true)
            {
                var AddViewCount = new ViewCountViewModel();
                AddViewCount.ContentId = id;
                AddViewCount.UserIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                await _viewCountService.AddView(AddViewCount);
                ;
                var Content = await _contentService.GetContentById(id);
                if (User.Identity.IsAuthenticated)
                {
                    var UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    bool Exist = await _followService.FollowedBefor(UserId, Content.UserId);
                    TempData["FollowBefor"] = Exist.ToString();
                    bool AddToBookmarkBefor = await _bookmarkService.AddBefor(Content.id, UserId);
                    TempData["AddToBookamrk"] = AddToBookmarkBefor.ToString();
                }


                var Message = new MessageViewModel();
                Message.ContentId = Content.id;
                ContentViewModel.MessageViewModel = Message;

                var caseMessageViewModels = Content.CaseList.Take(HowManyCaseShow.Value);
                Content.CaseList = caseMessageViewModels.ToList();
                ContentViewModel.Content = Content;
                return View(ContentViewModel);
            }

            return PartialView("_NotFoundError");
        }

        #endregion

        #region Create Content

        [Authorize]
        [HttpGet("Add-Content")]
        public async Task<IActionResult> CreateContent()
        {

            var ContentViewModel = new ContentViewModel();
            ContentViewModel.CategoryViewModels = await _categoryService.GetAllCategories();
            ContentViewModel.UserId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(ContentViewModel);
        }
        [Authorize]
        [HttpPost("Add-Content")]
        public async Task<IActionResult> CreateContent(ContentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _contentService.CreateContentTask(viewModel);
                return RedirectToAction("Index");
            }
            viewModel.CategoryViewModels = await _categoryService.GetAllCategories();
            viewModel.UserId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(viewModel);
        }

        [Authorize]
        public JsonResult UploadImagesContentCkEditorTask()
        {
            var MyFiles = Request.Form.Files;
            if (MyFiles.Count == 0)
            {
                var FileNotFound = new
                {
                    uploaded = false,
                    url = string.Empty,
                };
                return Json(FileNotFound);
            }
            var File = MyFiles[0];
            var FileName = Guid.NewGuid() + File.FileName;
            File.AddImageToServer(FileName, PathExtensions.ContentImgOrginServer, 300, 300, PathExtensions.ContentImgThumbServer);
            var pathFile = "./images/ContentImages/origin/" + FileName;
            var Success = new
            {
                uploaded = true,
                Url = pathFile,
            };
            return Json(Success);
        }
        #endregion

        #region EditContent
        [HttpGet("Edit-Content")]
        public async Task<IActionResult> EditContent(int id)
        {
            var Content = await _contentService.GetContentForEdit(id);
            Content.CategoryViewModels = await _categoryService.GetAllCategories();

            return View(Content);
        }
        [HttpPost("Edit-Content")]
        public async Task<IActionResult> EditContent(EditContentViewModel model)
        {

            if (ModelState.IsValid)
            {
                await _contentService.Edit(model);
                return RedirectToAction("Index");

            }
            var Categories = await _categoryService.GetAllCategories();
            model.CategoryViewModels = Categories;
            return View(model);
        }
        #endregion

        #region DeleteContent
        [Authorize]
        public async Task<IActionResult> DeleteContent(int id)
        {
            var UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var Result = await _contentService.DeletContent(id, UserId);
            if (Result == State.Success)
            {
                return RedirectToAction("Index");
            }
            // todo : return redirect to ContentDetails if State=Failed
            return RedirectToAction("Index");
        }

        #endregion

        #region Report-Content
        [Route("Report-Content")]
        [Authorize]
        public async Task<IActionResult> ReportContent(AddReportViewModel model)
        {
            bool ExistContent = await _contentService.IsAnyContent(model.ContentId);
            if (ExistContent == false) return PartialView("_NotFoundError");
            if (ModelState.IsValid)
            {
                await _reportContent.AddReport(model);
                return RedirectToAction("ContentDetails", new { id = model.ContentId, state = "Success" });
            }
            return RedirectToAction("ContentDetails", new { id = model.ContentId, state = "Error" });
        }
        #endregion

        #region MyContents
        [Authorize]
        [Route("my-contents")]
        public async Task<IActionResult>MyContents(UserPanelContents contents)
        {
            contents.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var Contents = await _contentService.GetUserContents(contents);
            return View(Contents);
        }

        #endregion

        public async Task<IActionResult> ContentUser(UserPanelContents contents,int UserId)
        {
            if (UserId==0 || UserId==null)
            {
                return PartialView("_NotFoundError");
            }

            contents.UserId = UserId;
            var Contents = await _contentService.GetUserContents(contents);
            return View(Contents);
        }

    }
}
