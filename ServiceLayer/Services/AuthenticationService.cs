using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Dtos;
using CoreLayer.IServices;
using SharedLayer.Dtos;

namespace ServiceLayer.Services
{
   public class AuthenticationService:IAuthenticationService
    {
        public Task<Response<TokenDto>> CreateUserTokenAsync(UserLoginDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<TokenDto>> CreateUserTokenByRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<Response<NoResponse>> RevokeRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ClientTokenDto>> CreateTokenByClient(ClientLoginDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
