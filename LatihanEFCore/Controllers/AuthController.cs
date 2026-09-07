using FluentValidation;
using LatihanEFCore.DTO.Responses;
using LatihanEFCore.DTOs;
using LatihanEFCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LatihanEFCore.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterDTO> _registerValidator;
        private readonly IValidator<LoginDTO> _loginValidator;

        public AuthController(IAuthService authService,
            IValidator<RegisterDTO> registerValidator,
            IValidator<LoginDTO> loginValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }




        // [HttpPost("register")]
        // public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        // {
        //     var validationResult = await _registerValidator.ValidateAsync(registerDto);
        //     if (!validationResult.IsValid)
        //     {
        //         var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        //         var errorResponse = ApiResponseDto<AuthResponseDTO>.ErrorResult("Validation failed", errors);
        //         return BadRequest(errorResponse);
        //     }

        //     var result = await _authService.RegisterAsync(registerDto);

        //     if (!result.Success)
        //     {
        //         return BadRequest(result);
        //     }

        //     return Ok(result);
        // }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var errorResponse = ApiResponseDto<AuthResponseDTO>.ErrorResult("Validation failed", errors);
                return BadRequest(errorResponse);
            }

            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }



            var result = await _authService.GetCurrentUserAsync(userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


    }
}