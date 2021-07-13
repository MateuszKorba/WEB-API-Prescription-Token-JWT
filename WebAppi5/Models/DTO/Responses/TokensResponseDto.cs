using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppi5.Models.DTO.Responses
{
    public class TokensResponseDto
    {
        public JwtSecurityToken Token { get; set; }
        public Guid RefreshedToken { get; set; }
    }
}
