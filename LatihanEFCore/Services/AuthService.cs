using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LatihanEFCore.DTO.Responses;
using LatihanEFCore.DTOs;
using LatihanEFCore.DTOs;
using LatihanEFCore.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LatihanEFCore.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ApiResponseDto<UserDTO>> GetCurrentUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponseDto<UserDTO>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var userDto = _mapper.Map<UserDTO>(user);
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Roles = roles.ToList();

                return new ApiResponseDto<UserDTO>
                {
                    Success = true,
                    Data = userDto
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<UserDTO>
                {
                    Success = false,
                    Message = $"Error retrieving user: {ex.Message}"
                };
            }
        }


        public async Task<ApiResponseDto<AuthResponseDTO>> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null)
                {
                    return ApiResponseDto<AuthResponseDTO>.ErrorResult("Invalid email or password");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
                if (!result.Succeeded)
                {
                    return ApiResponseDto<AuthResponseDTO>.ErrorResult("Invalid email or password");
                }

                var authResponse = await GenerateJwtToken(user);
                return ApiResponseDto<AuthResponseDTO>.SuccessResult(authResponse, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<AuthResponseDTO>.ErrorResult($"Login error: {ex.Message}");
            }
        }

        public async Task<ApiResponseDto<AuthResponseDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
                if (existingUser != null)
                {
                    return ApiResponseDto<AuthResponseDTO>.ErrorResult("User with this email already exists");
                }

                var user = _mapper.Map<ApplicationUser>(registerDTO);
                var result = await _userManager.CreateAsync(user, registerDTO.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ApiResponseDto<AuthResponseDTO>.ErrorResult("Registration failed", errors);
                }

                // Add user to default role
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    var errors = roleResult.Errors
                        .Select(e => e.Description)
                        .ToList();

                    return ApiResponseDto<AuthResponseDTO>
                        .ErrorResult("Failed to assign default role", errors);
                }

                var authResponse = await GenerateJwtToken(user);
                return ApiResponseDto<AuthResponseDTO>.SuccessResult(authResponse, "Registration successful");
            }
            catch (Exception ex)
            {
                return ApiResponseDto<AuthResponseDTO>.ErrorResult($"Registration error: {ex.Message}");
            }
        }
        private async Task<AuthResponseDTO> GenerateJwtToken(
    ApplicationUser user)
        {
            var jwtSection = _configuration.GetSection("Jwt");

            var jwtKey = jwtSection["Key"]
                ?? throw new InvalidOperationException(
                    "JWT Key tidak ditemukan.");

            var jwtIssuer = jwtSection["Issuer"]
                ?? throw new InvalidOperationException(
                    "JWT Issuer tidak ditemukan.");

            var jwtAudience = jwtSection["Audience"]
                ?? throw new InvalidOperationException(
                    "JWT Audience tidak ditemukan.");

            var expiresInMinutes =
                int.TryParse(jwtSection["ExpiresInMinutes"], out var minutes)
                    ? minutes
                    : 60;

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
    {
        new Claim(
            ClaimTypes.NameIdentifier,
            user.Id),

        new Claim(
            ClaimTypes.Name,
            user.UserName ?? string.Empty),

        new Claim(
            ClaimTypes.Email,
            user.Email ?? string.Empty)
    };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expiration =
                DateTime.UtcNow.AddMinutes(expiresInMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                Expires = expiration,
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Roles = roles.ToList();

            return new AuthResponseDTO
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = expiration,
                User = userDto
            };
        }
    }
}