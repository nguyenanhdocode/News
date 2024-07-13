using Application.Common;
using Application.MappingProfiles;
using Application.Models.User;
using Application.Services;
using Application.Services.DevImpl;
using Application.Services.Impl;
using Application.Validators.User;
using Application.Validators.Users;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services
            , IConfiguration configuration, IWebHostEnvironment env)
        {
            services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
            services.Configure<SmtpConfiguration>(configuration.GetSection("SmtpConfiguration"));
            services.Configure<CloudinaryConfiguration>(configuration.GetSection("CloudinaryConfiguration"));
            services.Configure<ReCaptchaConfiguration>(configuration.GetSection("ReCaptchaConfiguration"));

            //services.AddScoped<IValidator<LoginModel>, LoginModelValidator>();
            //services.AddScoped<IValidator<ForgotPasswordModel>, ForgotPasswordModelValidator>();

            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IReCaptchaService, ReCaptchaService>();

            services.AddAutoMapper(typeof(IMappingProfilesMarker));

            if (env.IsProduction())
                services.AddScoped<IEmailService, EmailService>();
            else
                services.AddScoped<IEmailService, DevEmailService>();

            return services;
        }
    }
}
