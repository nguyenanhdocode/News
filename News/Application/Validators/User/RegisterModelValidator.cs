using Application.Models.User;
using Core.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.User
{
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        private UserManager<AppUser> _userManager;
        public RegisterModelValidator(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

            RuleFor(p => p.FirstName)
                .NotEmpty()
                .WithMessage("Vui lòng nhập tên.")
                .MaximumLength(UserValidatorConfiguration.MaximumFirstNameLength)
                .WithMessage(string.Format("Tên quá dài (tối đa {0})", UserValidatorConfiguration.MaximumFirstNameLength));

            RuleFor(p => p.LastName)
                .NotEmpty()
                .WithMessage("Vui lòng nhập họ.")
                .MaximumLength(UserValidatorConfiguration.MaximumLastNameLength)
                .WithMessage(string.Format("Họ quá dài (tối đa {0})", UserValidatorConfiguration.MaximumLastNameLength));

            RuleFor(p => p.Email)
                .NotEmpty()
                .WithMessage("Vui lòng nhập email.")
                .Matches(UserValidatorConfiguration.EmailRegex)
                .WithMessage("Email không hợp lệ.")
                .Must(BeUserNotExisted)
                .WithMessage("Email đã được sử dụng.");

            RuleFor(p => p.Password)
                .NotEmpty()
                .WithMessage("Vui lòng nhập mật khẩu.")
                .Matches(UserValidatorConfiguration.PasswordRegex)
                .WithMessage("Mật khẩu không hợp lệ.");

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Vui lòng nhập lại mật khẩu.")
                .Equal(p => p.Password)
                .WithMessage("Mât khẩu xác nhận không khớp.");
        }

        private bool BeUserNotExisted(string email)
        {
            return _userManager.Users.SingleOrDefault(p => p.Email.Equals(email)) == null;
        }
    }
}
