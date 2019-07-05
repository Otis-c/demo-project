using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Procurement.Api.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.Api.Features.Auth
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody]Login command)
        {
            var token = await _mediator.Send(command);
            return Ok(new { Token = token });
        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> Register([FromBody]Register command)
        {
            var token = await _mediator.Send(command);
            return Ok(new { Token = token });
        }


    }
}
