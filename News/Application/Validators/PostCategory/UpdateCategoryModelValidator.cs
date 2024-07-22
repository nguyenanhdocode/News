using Application.Models.PostCategory;
using DataAccess.UnifOfWork;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.PostCategory
{
    public class UpdateCategoryModelValidator : AbstractValidator<UpdateCategoryModel>
    {
        private IUnitOfWork _uow;
        public UpdateCategoryModelValidator(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Vui lòng nhập tên.")
                .MaximumLength(255)
                .WithMessage(string.Format("Tên tối đa {0} ký tự.", PostCategoryConfiguration.MaximumNameLength));

            RuleFor(x => x.Slug)
                .NotEmpty()
                .WithMessage("Vui lòng nhập slug.")
                .MaximumLength(255)
                .WithMessage(string.Format("Slug tối đa {0} ký tự.", PostCategoryConfiguration.MaximumSlugLength));

            RuleFor(x => x.OrderIndex)
                .GreaterThan(0);
        }
    }
}
