using AutoMapper;
using DailyNewsServer.Api.ViewModels;
using DailyNewsServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyNewsServer.Api.Mappings
{
    public class UserVMProfile : Profile
    {
        public UserVMProfile()
        {
            CreateMap<User, UserVM>()
                .ForMember(dest => dest.Role, x => x.MapFrom(src => src.Role.Description));
        }        
    }
}
