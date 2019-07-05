using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Procurement.Api.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Api.Features.Admin
{
    public class EditUserRole : IRequest<bool>
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }

    public class EditUserRoleHandler : IRequestHandler<EditUserRole, bool>
    {
        private readonly UserManager<User> _userManager;

        public EditUserRoleHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> Handle(EditUserRole request, CancellationToken cancellationToken)
        {
            
            var user = await _userManager.FindByNameAsync(request.Username);
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            var result = await _userManager.AddToRoleAsync(user, request.Role);
            if (result.Succeeded)
            {
                Log.Error("EditUserRoleHandler: Successfully edited user roles {@Request}: {@Result}", request, result);
                return true;
            }
                

            Log.Error("EditUserRoleHandler: Failed to edit user roles {@Request}: {@Result}", request, result);
            throw new Exception("EditUserRoleHandler: Failed to edit user roles");
        }
    }

    public class EditUserRoleValidator : AbstractValidator<EditUserRole>
    {
        public EditUserRoleValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Role).NotEmpty();
        }
    }
}
