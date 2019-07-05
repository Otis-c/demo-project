using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Procurement.Api.Features.Admin
{
    [Authorize(Roles ="Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetUsers()
        {
            var name = User.Identity.Name;
            var userList = await _mediator.Send(new GetUsers());

            return Ok(userList);
        }

        [HttpPost, Route("[action]")]
        public async Task<IActionResult> EditUserRole(EditUserRole command)
        {
            var userList = await _mediator.Send(command);
            return Ok(userList);
        }

        [HttpGet, Route("[action]")]
        public IActionResult GetPendingRequisitions()
        {
            return Ok("Only Managers can see this");
        }

        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _mediator.Send(new GetLogs());
            return Ok(logs);
        }
    }
}
