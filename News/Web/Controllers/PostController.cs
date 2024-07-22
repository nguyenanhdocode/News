using Application.Common;
using Application.Models.Common;
using Application.Models.Post;
using Application.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarkdownSharp;
using AutoMapper;
using Core.Entities;
using Application.Exceptions;
using Core.Common;
using Application.Services.Impl;

namespace Web.Controllers
{
    [Route("/Posts")]
    [Authorize(Roles = Roles.Administrator 
        + "," + Roles.Creator
        + "," + Roles.Moderator)]
    public class PostController : Controller
    {
        private IPostService _postService;
        private IAssetService _assetService;
        private IMapper _mapper;
        private ICryptographicService _cryptographicService;
        private IClaimService _claimService;
        private IRoleService _roleService;

        public PostController(IPostService postService
            , IAssetService assetService
            , IMapper mapper
            , ICryptographicService cryptographicService
            , IClaimService claimService
            , IRoleService roleService)
        {
            _postService = postService;
            _assetService = assetService;
            _mapper = mapper;
            _cryptographicService = cryptographicService;
            _claimService = claimService;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] Dictionary<string, string> param)
        {
            var userRoles = await _roleService.GetRolesOfUser(_claimService.GetUserId());
            if (userRoles.Contains(Roles.Administrator) || userRoles.Contains(Roles.Moderator))
            {
                var posts = await _postService.GetAll(string.Empty, param);
                return View(posts);
            }
            else
            {
                var posts = await _postService.GetAll(_claimService.GetUserId(), param);
                return View(posts);
            }
        }

        [HttpGet]
        [Route("Create")]
        [Authorize(Roles = Roles.Administrator + ","
            + "," + Roles.Creator
            + "," + Roles.Moderator)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = Roles.Administrator + ","
            + "," + Roles.Creator
            + "," + Roles.Moderator)]
        public async Task<IActionResult> Create(CreatePostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await _postService.Create(model);
                ViewData["Alert"] = new Alert(AlertTypes.Success, "Tạo bài đăng thành công.");
            }
            catch (Exception)
            {
                throw;
            }

            return View();
        }

        [HttpGet]
        [Route("{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> View(string slug)
        {
            var post = await _postService.GetPostBySlug(slug);

            await _postService.IncreaseViewCounter(post.Id);
            post.Content = new Markdown().Transform(post.Content);

            ViewData["ShowCategoryBar"] = true;

            return View(post);
        }

        [HttpGet]
        [Route("{id}/Update")]
        [Authorize]
        public async Task<IActionResult> Update(string id)
        {
            var post = await _postService.GetPostById(id);

            if (post == null)
                throw new NotFoundException("Post not found");

            if (post.CreatedUserId != _claimService.GetUserId())
                throw new ForbiddenException();

            var result = _mapper.Map<Post, UpdatePostModel>(post);
            result.Content = _cryptographicService.Decrypt(result.Content);
            result.Content = new Markdown().Transform(result.Content);

            return View(result);
        }

        [HttpPost]
        [Route("{id}/Update")]
        [Authorize]
        public async Task<IActionResult> Update(UpdatePostModel model)
        {
            var post = await _postService.GetPostById(model.Id);

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await _postService.Update(model);
                ViewData["Alert"] = new Alert(AlertTypes.Success, "Cập nhật thành công!");
            }
            catch (Exception)
            {
                ViewData["Alert"] = new Alert(AlertTypes.Success, "Đã xảy ra lỗi!");
            }

            return RedirectToAction("Index", "Post");
        }

        [HttpGet]
        [Route("{id}/Delete")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var post = await _postService.GetPostById(id);

            if (post == null)
                throw new NotFoundException("Post not found");

            var userRoles = await _roleService.GetRolesOfUser(_claimService.GetUserId());
            if (post.CreatedUserId != _claimService.GetUserId()
                && !(userRoles.Contains(Roles.Administrator) || userRoles.Contains(Roles.Moderator)))
                throw new ForbiddenException();

            var viewResult = _mapper.Map<Post, DeletePostModel>(post);
                
            return View(viewResult);
        }

        [HttpPost]
        [Route("{id}/Delete")]
        [Authorize]
        public async Task<IActionResult> Delete(DeletePostModel model)
        {
            var post = await _postService.GetPostById(model.Id);

            await _postService.Delete(model.Id);
            return RedirectToAction("Index", "Post");
        }
    }
}
