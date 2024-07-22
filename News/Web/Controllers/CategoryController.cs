using Application.Exceptions;
using Application.Models.PostCategory;
using Application.Services;
using Application.Validators.PostCategory;
using AutoMapper;
using Core.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Common;

namespace Web.Controllers
{
    [Route("Categories")]
    [Authorize(Roles = Roles.Administrator)]
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;
        private IMapper _mapper;
        private IClaimService _claimService;

        public CategoryController(ICategoryService categoryService
            , IMapper mapper
            , IClaimService claimService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _claimService = claimService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cates = await _categoryService.GetAll();
            return View(cates);
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            int maxOrderIndex = await _categoryService.GetMaxOrderIndex();

            return View(new CreateCategoryModel { OrderIndex = maxOrderIndex + 1 });
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(
            [CustomizeValidator(Skip = true)] CreateCategoryModel model
            , [FromServices] IValidator<CreateCategoryModel> validator)
        {
            var val = validator.Validate(model);
            if (!val.IsValid)
            {
                val.AddToModelState(ModelState);
                return View();
            }
            
            if (await _categoryService.GetCategoryBySlug(model.Slug) != null)
            {
                ModelState.AddModelError("Slug", "Slug này đã được sử dụng.");
                return View();
            }

            await _categoryService.Create(model);

            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        [Route("{id}/Update")]
        public async Task<IActionResult> Update(string id)
        {
            var cate = await _categoryService.GetCategoryById(id);

            if (cate == null)
                throw new NotFoundException("Post not found");

            if (!cate.CreatedUserId.Equals(_claimService.GetUserId()))
                throw new ForbiddenException();

            return View(_mapper.Map<Category, UpdateCategoryModel>(cate));
        }

        [HttpPost]
        [Route("{id}/Update")]
        public async Task<IActionResult> Update(
            [CustomizeValidator(Skip = true)] UpdateCategoryModel model
            , [FromServices] IValidator<UpdateCategoryModel> validator)
        {
            var cate = await _categoryService.GetCategoryById(model.Id.ToString());

            var val = validator.Validate(model);
            if (!val.IsValid)
            {
                val.AddToModelState(ModelState);
                return View();
            }

            if (!model.Slug.Equals(cate.Slug)
                && await _categoryService.GetCategoryBySlug(model.Slug) != null)
            {
                ModelState.AddModelError("Slug", "Slug này đã được sử dụng.");
                return View();
            }

            await _categoryService.Update(model);

            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        [Route("{id}/Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var cate = await _categoryService.GetCategoryById(id);

            if (cate == null)
                throw new NotFoundException("Category not found");

            if (!cate.CreatedUserId.Equals(_claimService.GetUserId()))
                throw new ForbiddenException();

            return View(_mapper.Map<Category, DeleteCategoryModel>(cate));
        }

        [HttpPost]
        [Route("{id}/Delete")]
        public async Task<IActionResult> Delete(DeleteCategoryModel model)
        {
            await _categoryService.Delete(model.Id);

            return RedirectToAction("Index", "Category");
        }

    }
}
