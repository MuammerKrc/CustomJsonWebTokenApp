using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Dtos;
using CoreLayer.IdentityModels;
using CoreLayer.IServices;
using Microsoft.AspNetCore.Identity;
using ServiceLayer.MapperSettings;
using SharedLayer.Dtos;

namespace ServiceLayer.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<UserDto>> CreateUserAsync(CreateUserDto userDto)
        {
            var user = ObjectMapper.Mapper.Map<AppUser>(userDto);
            var createResult = await _userManager.CreateAsync(user, userDto.Password);
            if (!createResult.Succeeded)
            {
                List<string> errorList = new List<string>();
                errorList = createResult.Errors.Select(i => i.Description).ToList();
                var errDto = new ErrorDto(errorList, true);
                return Response<UserDto>.ErrorResponse(errDto);
            }
            return Response<UserDto>.SuccessResponse();
        }

        public async  Task<Response<UserDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                return Response<UserDto>.SuccessResponse(ObjectMapper.Mapper.Map<UserDto>(user));
            }
            return Response<UserDto>.ErrorResponse("User not found",statusCode:404);
        }
    }
}
