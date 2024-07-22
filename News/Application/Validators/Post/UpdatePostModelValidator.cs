using Application.Models.Post;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Post
{
    public class UpdatePostModelValidator : AbstractValidator<UpdatePostModel>
    {
        public UpdatePostModelValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty()
                .WithMessage("Vui lòng nhập tiêu đề.");

            RuleFor(p => p.Slug)
                .NotEmpty()
                .WithMessage("Vui lòng nhập slug.");

            RuleFor(p => p.Description)
                .NotEmpty()
                .WithMessage("Vui lòng nhập mô tả.");

            RuleFor(p => p.Content)
                .NotEmpty()
                .WithMessage("Vui lòng nhập nội dung.");

            RuleFor(p => p.CoverPhoto)
                .Must(BeValidImageFile)
                .WithMessage("Ảnh không hợp lệ.");
        }

        private bool BeValidImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return true;

            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!PostValidatorConfiguration.AcceptImageFormats
                    .Any(p => p.Equals(extension)))
                return false;

            return true;
        }
    }
}
