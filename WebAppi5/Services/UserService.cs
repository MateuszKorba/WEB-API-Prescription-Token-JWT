using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAppi5.Models;
using WebAppi5.Models.DTO.Requests;
using WebAppi5.Models.DTO.Responses;

namespace WebAppi5.Services
{
    public class UserService : IUserService
    {
        private readonly MyContext _myContext;
        private readonly IConfiguration _configuration;

        public object KeyDeriviation { get; private set; }

        public UserService(IConfiguration configuration, MyContext context)
        {
            _myContext = context;
            _configuration = configuration;
        }

        public async Task<bool> RegisterNewUser(UserRequestDto newUser)
        {
            byte[] salts = new byte[128 / 8];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salts);
            }
            await _myContext.Users.AddAsync(new User
            {
                Login = newUser.Login,
                Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: newUser.Password,
                        salt: salts,
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 10000,
                        numBytesRequested: 256/8)),
                RefreshedToken = Guid.NewGuid().ToString(),
                RefreshTokenExpireDate = DateTime.Now.AddDays(1),
                Salt = Convert.ToBase64String(salts)
            });
            await _myContext.SaveChangesAsync();
            return true;
        }

        public async Task<TokensResponseDto> LogInUser(UserRequestDto loginUserRequestDto)
        {
            // Pobieranie z bazy użytkownika o danym loginie i haśle 
            var users = await _myContext.Users.Where(x => x.Login == loginUserRequestDto.Login).ToListAsync();
            User userLogged = null;
            foreach (User user in users) {
                var u = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: loginUserRequestDto.Password,
                        salt: Convert.FromBase64String(user.Salt),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8));
                if(u == user.Password)
                {
                    userLogged = user;
                    break;
                }
            }
            return await UpdateToken(new RefreshRequestTokenDto
            {
                RefreshedToken = userLogged.RefreshedToken
            });
        }

        public async Task<TokensResponseDto> UpdateToken(RefreshRequestTokenDto refreshRequestTokenDto)
        {
            User userLogged = await _myContext.Users.Where(x => x.RefreshedToken.Equals(refreshRequestTokenDto.RefreshedToken)).FirstAsync();
            Claim[] userClaims = 
            {
                new Claim(ClaimTypes.NameIdentifier,userLogged.IdUser.ToString()),
                new Claim(ClaimTypes.Name,userLogged.Login),
                new Claim(ClaimTypes.Role,"user"),
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );
            Guid newGuidGenerator = Guid.NewGuid();
            userLogged.RefreshedToken = newGuidGenerator.ToString();
            userLogged.RefreshTokenExpireDate = DateTime.Now.AddDays(1);
            await _myContext.SaveChangesAsync(); // nadpisujemy refreshToken
            return new TokensResponseDto
            {
                Token = token,
                RefreshedToken = newGuidGenerator
            };
        }

        public async Task<HttpStatusCodeResult> IsUserExists(UserRequestDto urdto)
        {
            var userData = await _myContext.Users.Where(x => x.Login == urdto.Login).ToListAsync();
            foreach (User user in userData) {
                var u = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: urdto.Password,
                        salt: Convert.FromBase64String(user.Salt),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8));
                if(u == user.Password)
                {
                    return new HttpStatusCodeResult(200);
                }
            }
            return new HttpStatusCodeResult(404, "Brak Usera w bazie");
        }

        public async Task<HttpStatusCodeResult> IsTokenExists(RefreshRequestTokenDto refreshTokenDto)
        {
            var tokenExists = await _myContext.Users.Where(x => x.RefreshedToken == refreshTokenDto.RefreshedToken).CountAsync();
            if (tokenExists > 0)
            {
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return new HttpStatusCodeResult(404, "Zły token");
            }
        }

        public async Task<HttpStatusCodeResult> TokenExpire(RefreshRequestTokenDto refreshTokenDto)
        {
            var token = await _myContext.Users.Where(x => x.RefreshedToken.Equals(refreshTokenDto.RefreshedToken)).FirstAsync();
            if(token.RefreshTokenExpireDate > DateTime.Now)
            {
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return new HttpStatusCodeResult(400, "Token wygasł");
            }
        }
    }
}
