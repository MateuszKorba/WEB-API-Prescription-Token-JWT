using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using WebAppi5.Models.DTO.Requests;
using WebAppi5.Services;

namespace WebAppi5.Controllers
{
    [ApiController]
    [Route("api/account")]

    public class AccountsController : ControllerBase
    {
        private IUserService _userService;

        public AccountsController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserRequestDto user)
        {
            // var userFromDb = ....

            var userFromDb = await _userService.IsUserExists(user);
            if (userFromDb.StatusCode == 404)
            {
                return NotFound(userFromDb.StatusDescription);
            }
            else
            {
                var token = await _userService.LogInUser(user);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token.Token),
                    refreshToken = token.RefreshedToken.ToString()
                });
            }
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task <IActionResult> RefreshToken([FromBody] RefreshRequestTokenDto refreshToken)
        {
            // Sprawdzenie czy w bazie istnieje taki refreshToken oraz czy jest aktywny
            // Jeżeli refreshToken wygasł lub go nie ma - 400 BadRequest
            // Jeżeli jest nadal aktywny - generujemy nowy token wraz z nowym refreshTokenem
            // Nadpisujemy refreshToken dla danego użytkownika tym nowym

            var tokenExist = await _userService.IsTokenExists(refreshToken);
            var tokenExpire = await _userService.TokenExpire(refreshToken);
            if (tokenExist.StatusCode == 400) {
                return BadRequest(tokenExist.StatusDescription);
            }
            else {
                if (tokenExpire.StatusCode == 400)
                {
                    return BadRequest(tokenExist.StatusDescription);
                }
                else
                {
                    var token = await _userService.UpdateToken(refreshToken);
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token.Token),
                        refreshToken = token.RefreshedToken.ToString()
                    });
                }
            }
        }


        [HttpPost("register")]
        [AllowAnonymous]

        public async Task <IActionResult> Register([FromBody] UserRequestDto registerUser)
        {
            // Starsza biblioteka
            // var hashedPassword = new PasswordHasher().HashPassword(register.Password);
            // VerifyPassword(hash, haslo) <- w wyniku otrzymujemy enuma z wynikiem
            // Nowsza wersja biblioteki (wbudowana)
            // Dodatkowo przekazujemy obiekt reprezentujący nam użytkownika dla którego chcemy zahashować hasło
            if(await _userService.RegisterNewUser(registerUser))
            {
                return Ok("Zarejestrowano Usera");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Blad Servera");
            }
        }

    }
}
