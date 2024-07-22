using Application.Models.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.User
{
    public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordModelValidator()
        {
            RuleFor(p => p.OldPassword)
                .NotEmpty()
                .WithMessage("Vui lòng nhập mật khẩu cũ.");

            RuleFor(p => p.NewPassword)
                .NotEmpty()
                .WithMessage("Vui lòng nhập mật khẩu mới.")
                .Matches(UserValidatorConfiguration.PasswordRegex)
                .WithMessage("Mật khẩu không hợp lệ.");

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Vui lòng nhập lại mật khẩu.")
                .Equal(p => p.NewPassword)
                .WithMessage("Mât khẩu xác nhận không khớp.");
        }
    }
}
