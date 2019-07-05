using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Procurement.Api.Data;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Api.Features.Requisitions.Commands
{
    public class ApproveQuote : IRequest<int>
    {
        public int Id { get; set; }
        public int ReqId { get; set; }
    }

    public class ApproveQuoteHandler : IRequestHandler<ApproveQuote, int>
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _context;
        private readonly ILogger _log;

        public ApproveQuoteHandler(AppDbContext db, IHttpContextAccessor context)
        {
            _db = db;
            _context = context;
            _log = Log.ForContext("UserName", _context.HttpContext.User.Identity.Name);
            _log.ForContext("Source", typeof(CreateQuotationHandler).Name);
        }
        public async Task<int> Handle(ApproveQuote request, CancellationToken cancellationToken)
        {
            var quote = await _db.Quotations.SingleOrDefaultAsync(x => x.Id == request.Id);
            quote.Status = "Approved";

            var quotes = _db.Quotations.Where(x => x.RequisitionId == request.ReqId && x.Id != request.Id);

            foreach (var q in quotes)
            {
                q.Status = "Rejected";
            }

            var req = await _db.Requisitions.SingleOrDefaultAsync(x => x.Id == request.ReqId);
            req.Status = "Waiting Authorisation";
            var result = await _db.SaveChangesAsync();

            return result;
        }
    }
}
