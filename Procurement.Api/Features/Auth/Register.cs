using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Procurement.Api.Models;
using Procurement.Api.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Api.Features.Auth
{
    public class Register : IRequest<bool>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNo { get; set; }
        public string Role { get; set; }
    }

    public class RegisterHandler : IRequestHandler<Register, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public RegisterHandler(UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> Handle(Register request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                CompanyName = request.CompanyName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNo
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                Log.Information("RegisterHandeler: User created a new accoun {@Request}", request);
                if (string.IsNullOrEmpty(request.Role))
                    request.Role = "Vendor";

                var res = await _userManager.AddToRoleAsync(user, request.Role);

                if (res.Succeeded)
                {
                    Log.Information("RegisterHandeler: User created a new accoun {@Request}: {@Result}", request, res);
                    return true;
                }

                Log.Error("RegisterHandeler: Failed to add user to role {@Request}", request);
                throw new Exception("Failed to add user role");

            }
            Log.Error("RegisterHandeler: Failed to create user to role {@Request}: {@Result}", request, result);
            throw new Exception("Failed to register user");
        }
    }

    public class RegisterValidator : AbstractValidator<Register>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
            RuleFor(x => x.ConfirmPassword).NotNull().NotEmpty();
            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
            RuleFor(x => x.CompanyName).NotNull().NotEmpty();
            RuleFor(x => x.PhoneNo).NotNull().NotEmpty();

            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.Password != x.ConfirmPassword)
                {
                    context.AddFailure(nameof(x.Password), "Passwords should match");
                }
            });
        }
    }
}
