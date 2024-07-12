using Application.MappingProfiles;
using Application.Models.User;
using Application.Services;
using Application.Services.Impl;
using Application.Validators.Users;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginModel>, LoginModelValidator>();
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IUserService, UserService>();
            services.AddAutoMapper(typeof(IMappingProfilesMarker));

            return services;
        }
    }
}
