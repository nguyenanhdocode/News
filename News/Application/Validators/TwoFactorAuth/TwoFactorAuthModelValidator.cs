using Application.Models.TwoFactorAuth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.TwoFactorAuth
{
    public class TwoFactorAuthModelValidator : AbstractValidator<TwoFactorAuthModel>
    {
        public TwoFactorAuthModelValidator()
        {
            RuleFor(p => p.Code)
                .NotNull()
                .WithMessage("Vui lòng nhập mã xác thực.")
                .NotEmpty()
                .WithMessage("Vui lòng nhập mã xác thực.");
        }
    }
}
