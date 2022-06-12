using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Dtos;
using SharedLayer.Dtos;

namespace CoreLayer.IServices
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateUserTokenAsync(UserLoginDto dto);
        Task<Response<TokenDto>> CreateUserTokenByRefreshToken(string refreshToken);
        Task<Response<NoResponse>> RevokeRefreshToken(string refreshToken);
        Task<Response<ClientTokenDto>> CreateTokenByClient(ClientLoginDto dto);
    }
}
