using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Configurations;
using CoreLayer.Dtos;
using CoreLayer.IdentityModels;

namespace CoreLayer.IServices
{
    public interface ITokenService
    {
        TokenDto CreateTokenForUser(AppUser appUser);
        ClientTokenDto CreateClientToken(ClientTokenDto client);
    }
}
