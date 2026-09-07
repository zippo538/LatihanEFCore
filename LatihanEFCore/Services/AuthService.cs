using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using LatihanEFCore.Commons;
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

        public async Task<ServiceResult<UserDto>> GetCurrentUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ServiceResult<UserDto>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var userDto = _mapper.Map<UserDto>(user);
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Roles = roles.ToList();

                return new ServiceResult<UserDto>
                {
                    Success = true,
                    Data = userDto
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<UserDto>
                {
                    Success = false,
                    Message = $"Error retrieving user: {ex.Message}"
                };
            }
        }


        public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null)
                {
                    return ServiceResult<AuthResponseDto>.ErrorResult("Invalid email or password");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
                if (!result.Succeeded)
                {
                    return ServiceResult<AuthResponseDto>.ErrorResult("Invalid email or password");
                }

                var authResponse = await GenerateJwtToken(user);
                return ServiceResult<AuthResponseDto>.SuccessResult(authResponse, "Login successful");
            }
            catch (Exception ex)
            {
                return ServiceResult<AuthResponseDto>.ErrorResult($"Login error: {ex.Message}");
            }
        }

        public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDTO)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
                if (existingUser != null)
                {
                    return ServiceResult<AuthResponseDto>.ErrorResult("User with this email already exists");
                }

                var user = _mapper.Map<ApplicationUser>(registerDTO);
                var result = await _userManager.CreateAsync(user, registerDTO.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ServiceResult<AuthResponseDto>.ErrorResult("Registration failed", errors);
                }

                // Add user to default role
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    var errors = roleResult.Errors
                        .Select(e => e.Description)
                        .ToList();

                    return ServiceResult<AuthResponseDto>
                        .ErrorResult("Failed to assign default role", errors);
                }

                var authResponse = await GenerateJwtToken(user);
                return ServiceResult<AuthResponseDto>.SuccessResult(authResponse, "Registration successful");
            }
            catch (Exception ex)
            {
                return ServiceResult<AuthResponseDto>.ErrorResult($"Registration error: {ex.Message}");
            }
        }
        private async Task<AuthResponseDto> GenerateJwtToken(
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

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = roles.ToList();

            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = expiration,
                User = userDto
            };
        }
    }
}