using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreLayer.Dtos;
using CoreLayer.IdentityModels;
using CoreLayer.Models;

namespace ServiceLayer.MapperSettings
{
    public class MapperSetting:Profile
    {
        public MapperSetting()
        {
            CreateMap<ProductDto, Product>().ReverseMap();

            CreateMap<UserDto, AppUser>().ReverseMap();
        }
    }
}
