using Application.Common;
using Application.MappingProfiles;
using Application.Models.Post;
using Application.Models.User;
using Application.Services;
using Application.Services.DevImpl;
using Application.Services.Impl;
using Application.Validators.User;
using Application.Validators.Users;
using AutoMapper;
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
            services.Configure<CryptographyConfiguration>(configuration.GetSection("CryptographyConfiguration"));

            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IReCaptchaService, ReCaptchaService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<ICryptographicService, CryptographicService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITwoFactorAuthService, TwoFactorAuthService>();

            services.AddAutoMapper(typeof(IMappingProfilesMarker));

            if (env.IsProduction())
                services.AddScoped<IEmailService, EmailService>();
            else
                services.AddScoped<IEmailService, DevEmailService>();

            return services;
        }
    }
}
