using AutoMapper;
using Registration.Data;
using Registration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registration.Configuration
{
    public class MapperInitializer :Profile
    {
        public void MapperInitilizer()     
        {            
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<ApiUser, User>()
                .ForMember(dto => dto.Image, opt => opt.Ignore());
                
            CreateMap<User, ApiUser>()
                .ForMember(dto => dto.Role, dto => dto.Ignore())
                .ForMember(dto => dto.Image, opt => opt.Ignore());



        }
    }
}
