using Application.Models.TwoFactorAuth;
using AutoMapper;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MappingProfiles
{
    public class TwoFactorAuthProfile : Profile
    {
        public TwoFactorAuthProfile()
        {
            CreateMap<CreateTwoFactorAuthModel, TwoFactorAuth>();
        }
    }
}
