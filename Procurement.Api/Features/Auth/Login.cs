using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Procurement.Api.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Api.Features.Auth
{
    public class Login : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginHandler : IRequestHandler<Login, string>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginHandler(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> Handle(Login request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email);

            if (user == null)
            {
                Log.Error("Login Handler: Failed to Log in user, User not found {@Request}", request);
                throw new Exception("Login Failed");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                Log.Information("Login Handler: Successfully Logged in user {@Request}", request);
                var token = await GenerateToken(user);
                return token;
            }

            Log.Error("Login Handler: Failed to Log in user {@Request}: {@Result} ", request, result);
            throw new Exception("Login Failed");
        }

        private async Task<string> GenerateToken(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);

            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Email, user.UserName),
                            new Claim(ClaimTypes.GivenName, user.UserName),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "http://localhost:5000",
                Audience = "http://localhost:5000",
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = signinCredentials
            };

            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }

    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
