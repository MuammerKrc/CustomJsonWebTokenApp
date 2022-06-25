using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Configurations;
using CoreLayer.Dtos;
using CoreLayer.IdentityModels;
using CoreLayer.IRepositories;
using CoreLayer.IServices;
using CoreLayer.IUnitOfWorks;
using CoreLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using SharedLayer.Dtos;

namespace ServiceLayer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepositories<UserRefreshToken,long> _userRefreshTokenService;
        public AuthenticationService(IOptions<List<Client>> optionsClient,ITokenService tokenService, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IBaseRepositories<UserRefreshToken, long> userRefreshTokenService)
        {
            _clients = optionsClient.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshTokenService = userRefreshTokenService;
        }

        public async  Task<Response<TokenDto>> CreateUserTokenAsync(UserLoginDto dto)
        {
            var errorResponse=Response<TokenDto>.ErrorResponse("User and password not matched");

            var user = await _userManager.FindByEmailAsync(dto.EmailOrUserName);
            if (user == null)
            {
                return errorResponse;
            }
            var checkedPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!checkedPassword)
            {
                return errorResponse;
            }
            var token=_tokenService.CreateTokenForUser(user);
            var userRefreshToken = await _userRefreshTokenService.Where((x => x.Id == user.Id)).FirstOrDefaultAsync();
            if (userRefreshToken != null)
            {
                userRefreshToken.RefreshToken = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }
            else
            {
                _userRefreshTokenService.Add(new UserRefreshToken()
                {
                    Id = user.Id,
                    Expiration = token.RefreshTokenExpiration,
                    RefreshToken = token.RefreshToken,
                });
            }
            await _unitOfWork.SaveChangeAsync();



            return Response<TokenDto>.SuccessResponse(token);
        }

        public async  Task<Response<TokenDto>> CreateUserTokenByRefreshToken(string refreshToken)
        {
            var errorResponse = Response<TokenDto>.ErrorResponse("Refresh token not useable");
            var existRefreshToken = await _userRefreshTokenService.Where(i => i.RefreshToken == refreshToken)
                .FirstOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return errorResponse;
            }

            if (existRefreshToken.Expiration > DateTime.Now)
            {
                return errorResponse;
            }

            var user = await _userManager.FindByIdAsync(existRefreshToken.Id.ToString());
            if (user == null)
            {
                return errorResponse;
            }
            var token=_tokenService.CreateTokenForUser(user);

            existRefreshToken.Expiration = token.RefreshTokenExpiration;
            existRefreshToken.RefreshToken = token.RefreshToken;

            await _unitOfWork.SaveChangeAsync();

            return Response<TokenDto>.SuccessResponse(token);
        }

        public async Task<Response<NoResponse>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(i => i.RefreshToken == refreshToken)
                .FirstOrDefaultAsync();

            if (existRefreshToken!=null)
            {
                existRefreshToken.RefreshToken = string.Empty;
                await _unitOfWork.SaveChangeAsync();
            }
            return Response<NoResponse>.SuccessResponse();
        }

        public async  Task<Response<ClientTokenDto>> CreateTokenByClient(ClientLoginDto dto)
        {
            var errorResponse = Response<ClientTokenDto>.ErrorResponse("Client and password not matched");

            var client = _clients.FirstOrDefault(i => i.ClientId == dto.ClientId);
            if (client == null)
            {
                return errorResponse;
            }

            if (client.ClientSecret != dto.ClientSecret)
            {
                return errorResponse;
            }
            var token=_tokenService.CreateClientToken(client);
            return Response<ClientTokenDto>.SuccessResponse(token);
        }
    }
}
