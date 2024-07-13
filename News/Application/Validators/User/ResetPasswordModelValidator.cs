using Application.Models.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.User
{
    public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordModelValidator()
        {
            RuleFor(p => p.NewPassword)
                .NotNull()
                .WithMessage("Vui lòng nhập mật khẩu.")
                .NotEmpty()
                .WithMessage("Vui lòng nhập mật khẩu.");

            RuleFor(p => p.ConfirmPassword)
                .NotNull()
                .WithMessage("Vui lòng nhập lại mật khẩu.")
                .NotEmpty()
                .WithMessage("Vui lòng nhập lại mật khẩu.")
                .Equal(p => p.NewPassword)
                .WithMessage("Mật khẩu không trùng khớp.");
        }
    }
}
