using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLayer.Dtos;
using CoreLayer.IServices;
using Microsoft.VisualBasic;

namespace JWTApiSolution.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationsController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationsController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(UserLoginDto dto)
        {
            return BaseResponse(await _authenticationService.CreateUserTokenAsync(dto));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenForClient(ClientLoginDto dto)
        {
            return BaseResponse(await _authenticationService.CreateTokenByClient(dto));
        }
        [HttpPost]
        public async Task<IActionResult> RemoveRefreshToken(string refreshToken)
        {
            return BaseResponse(await _authenticationService.RevokeRefreshToken(refreshToken));
        }
        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(string refreshToken)
        {
            return BaseResponse(await _authenticationService.CreateUserTokenByRefreshToken(refreshToken));
        }


    }
}
