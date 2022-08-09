using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetWebAPI.Dto;
using TweetWebAPI.Services;

namespace TweetWebAPI.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost("/api/v1.0/tweets/register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var response = await this._userService.Register(userDto, userDto.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("/api/v1.0/tweets/login")]
        public async Task<IActionResult> Login(LoginIdDto loginIdDto)
        {
            var response = await this._userService.Login(loginIdDto.LoginId, loginIdDto.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("/api/v1.0/tweets/{userName}/forgot")]
        public async Task<IActionResult> ForgotPassword(string userName, ForgotDto forgotDto)
        {
            var response = await this._userService.ForgotPassword(userName, forgotDto.Password, forgotDto.ConfirmPassword);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("/api/v1.0/tweets/users/all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await this._userService.GetAllUsers();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("/api/v/1.0/tweets/user/search/{userName}")]
        public async Task<IActionResult> GetUsersByName(string userName)
        {
            var response = await this._userService.SearchUsers(userName);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
