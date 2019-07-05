using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Procurement.Api.Features.Requisitions.Commands;
using Procurement.Api.Features.Requisitions.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.Api.Features.Requisitions
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequisitionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RequisitionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "User, Approver, Authoriser, Vendor")]
        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetRequisitions()
        {
            var userList = await _mediator.Send(new GetRequisitions());

            return Ok(userList);
        }

        [Authorize(Roles  = "User, Approver, Authoriser, Vendor")]
        [HttpGet, Route("[action]/{id}")]
        public async Task<IActionResult> GetRequisition(int id)
        {
            var userList = await _mediator.Send(new GetRequisition { Id = id });

            return Ok(userList);
        }

        [Authorize(Roles  = "Approver, Authoriser")]
        [HttpGet, Route("[action]/{id}")]
        public async Task<IActionResult> GetQuotations(int id)
        {
            var userList = await _mediator.Send(new GetQuotations { RequisitionId = id });

            return Ok(userList);
        }

        [Authorize(Roles = "User")]
        [HttpPost, Route("[action]")]
        public async Task<IActionResult> CreateRequisition([FromBody] CreateRequisition command)
        {
            var userList = await _mediator.Send(command);

            return Ok(userList);
        }

        [Authorize(Roles = "Vendor")]
        [HttpPost, DisableRequestSizeLimit]
        [Route("[action]")]
        public async Task<ActionResult> SubmitQuote()
        {
            try
            {
                var file = Request.Form.Files[0];

                int reqId;
                if (!int.TryParse(Request.Form["reqId"], out reqId))
                        throw new Exception("Invalid data for Requisition Id");

                double amount;
                if (!double.TryParse(Request.Form["cost"], out amount))
                    throw new Exception("Invalid data for amount");

                var description = Request.Form["description"];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + ".txt";//ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var quote = new CreateQuotation
                    {
                        RequisitionId = reqId,
                        Amount = amount,
                        Description = description,
                        FilePath = fileName,
                        Status = "Submitted"
                    };
                    await _mediator.Send(quote);

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Approver, Authoriser")]
        [HttpGet, Route("[action]/{fileName}")]
        public FileStream DownloadQuote(string fileName)
        {
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            currentDirectory = currentDirectory + "\\src\\assets";
            var file = Path.Combine(@"C:\Users\Otyce\source\repos\Procurement\Procurement.Api\Resources\Images\", fileName);
            return new FileStream(file, FileMode.Open, FileAccess.Read);
        }

        [Authorize(Roles = "Approver")]
        [HttpGet, Route("[action]/{id}/{reqId}")]
        public async Task<IActionResult> ApproveQuote(int id, int reqId)
        {
           var result = await _mediator.Send(new ApproveQuote { Id = id, ReqId = reqId });
            return Ok(result);
        }

        [Authorize(Roles = "Authoriser")]
        [HttpGet, Route("[action]/{id}")]
        public async Task<IActionResult> AuthoriseReq(int id)
        {
            var result = await _mediator.Send(new AuthoriseReq { ReqId = id });
            return Ok(result);
        }



    }
}
