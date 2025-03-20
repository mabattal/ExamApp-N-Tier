﻿using ExamApp.Services.Authentication;
using ExamApp.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers
{
    public class AuthController(IUserService userService, IAuthService authService, JwtService jwtService) : CustomBaseController
    {
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var result = await authService.ValidateUserAsync(loginRequest.Email, loginRequest.Password);
            return CreateActionResult(result);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request)
        {
            var result = await authService.RegisterAsync(request);
            return CreateActionResult(result);
        }
    }
}