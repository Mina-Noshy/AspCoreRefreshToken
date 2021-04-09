using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Services.Account;
using MyWebAPI.Services.Other;
using MyWebModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebAPI.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> RegisterAsync(RegisterVM model)
        {
            string message = await _userService.RegisterAsync(model);

            ResponseVM response = new ResponseVM
            {
                Status = TaskHelper.GetResponceStatus(message),
                Message = message
            };
            return Ok(response);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost("register-user")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> RegisterAsUserAsync(RegisterVM model)
        {
            string message = await _userService.RegisterWithRoleAsync(model, RoleVM.User);

            ResponseVM response = new ResponseVM
            {
                Status = TaskHelper.GetResponceStatus(message),
                Message = message
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost("register-moderator")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> RegisterAsModeratorAsync(RegisterVM model)
        {
            string message = await _userService.RegisterWithRoleAsync(model, RoleVM.Moderator);

            ResponseVM response = new ResponseVM
            {
                Status = TaskHelper.GetResponceStatus(message),
                Message = message
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> RegisterAsAdminAsync(RegisterVM model)
        {
            string message = await _userService.RegisterWithRoleAsync(model, RoleVM.Admin);

            ResponseVM response = new ResponseVM
            {
                Status = TaskHelper.GetResponceStatus(message),
                Message = message
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("update-user")]
        public async Task<ActionResult> UpdateUserInfoAsync(RegisterVM model)
        {
            string message = await _userService.UpdateUserInfoAsync(model);

            ResponseVM response = new ResponseVM
            {
                Status = TaskHelper.GetResponceStatus(message),
                Message = message
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("update-user-role")]
        public async Task<ActionResult> UpdateUserRoleAsync(string email, string role)
        {
            string message = await _userService.UpdateUserRoleAsync(email, role);

            ResponseVM response = new ResponseVM
            {
                Status = TaskHelper.GetResponceStatus(message),
                Message = message
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("remove-user-role")]
        public async Task<ActionResult> RemoveUserFromRoleAsync(string email, string role)
        {
            string message = await _userService.RemoveUserRoleAsync(email, role);

            ResponseVM response = new ResponseVM
            {
                Status = TaskHelper.GetResponceStatus(message),
                Message = message
            };

            return Ok(response);
        }


        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync(TokenRequestVM model)
        {
            var result = await _userService.GetTokenAsync(model);

            if (result.IsAuthenticated)
            {
                SetRefreshTokenInCookie(result.RefreshToken);
                return Ok(result);
            }

            return Unauthorized(result);

        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRoleAsync(AddUserRoleVM model)
        {
            string message = await _userService.AddRoleAsync(model);

            ResponseVM response = new ResponseVM
            {
                Status = TaskHelper.GetResponceStatus(message),
                Message = message
            };

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _userService.RefreshTokenAsync(refreshToken);

            if (!string.IsNullOrEmpty(response.RefreshToken))
                SetRefreshTokenInCookie(response.RefreshToken);

            return Ok(response);
        }


        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequestVM model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new ResponseVM {Status = "Error", Message = "Token is required" });

            var response = await _userService.RevokeToken(token);

            if (!response)
                return NotFound(new ResponseVM{Status = "Error", Message = "Token not found" });

            return Ok(new ResponseVM{Status = "Success", Message = "Token revoked" });
        }

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10),
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        [Authorize]
        [HttpPost("tokens/{id}")]
        public async Task<IActionResult> GetRefreshTokens(string id)
        {
            var user = await _userService.GetById(id);
            return Ok(user.RefreshTokens);
        }

    }
}
