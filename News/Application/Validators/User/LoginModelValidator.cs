using Application.Models.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Users
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .WithMessage("Vui lòng nhập email")
                .NotNull()
                .WithMessage("Vui lòng nhập email");

            RuleFor(p => p.Password)
                .NotNull()
                .WithMessage("Vui lòng nhập mật khẩu")
                .NotEmpty()
                .WithMessage("Vui lòng nhập mật khẩu");
        }
    }
}
