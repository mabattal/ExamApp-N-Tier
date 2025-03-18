﻿using System.Net;
using Microsoft.AspNetCore.Mvc;
using ExamApp.Services.Authentication;
using ExamApp.Services;
using ExamApp.Services.User;

namespace ExamApp.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IUserService userService, JwtService jwtService) : CustomBaseController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var user = await userService.ValidateUserAsync(loginRequest.Email, loginRequest.Password);
            if (user is null)
            {
                return CreateActionResult(ServiceResult.Fail("Invalid email or password.", HttpStatusCode.Unauthorized));
            }

            var token = jwtService.GenerateToken(user.UserId, user.Role.ToString());
            return CreateActionResult(ServiceResult<string>.Success(token));
        }
    }
}