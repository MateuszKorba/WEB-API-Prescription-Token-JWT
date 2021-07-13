using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAppi5.Models.DTO.Requests;
using WebAppi5.Models.DTO.Responses;

namespace WebAppi5.Services
{
    public interface IUserService
    {
        public Task<bool> RegisterNewUser(UserRequestDto newUser);
        public Task<TokensResponseDto> LogInUser(UserRequestDto loginUserRequestDto);
        public Task<TokensResponseDto> UpdateToken(RefreshRequestTokenDto refreshRequestTokenDto);
        public Task<HttpStatusCodeResult> IsUserExists(UserRequestDto userRequestDto);
        public Task<HttpStatusCodeResult> IsTokenExists(RefreshRequestTokenDto refreshTokenDto);
        public Task<HttpStatusCodeResult> TokenExpire(RefreshRequestTokenDto refreshTokenDto);
    }
}
