using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Procurement.Api.Data;
using Procurement.Api.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Api.Features.Requisitions.Queries
{
    public class GetQuotations : IRequest<IEnumerable<Quotation>>
    {
        public int RequisitionId { get; set; }
    }

    public class GetQuotationsHandler : IRequestHandler<GetQuotations, IEnumerable<Quotation>>
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _context;
        private readonly ILogger _log;

        public GetQuotationsHandler(AppDbContext db, IHttpContextAccessor context)
        {
            _context = context;
            _log = Log.ForContext("UserName", _context.HttpContext.User.Identity.Name);
            _log.ForContext("Source", typeof(GetQuotationsHandler).Name);
            _db = db;
        }

        public async Task<IEnumerable<Quotation>> Handle(GetQuotations request, CancellationToken cancellationToken)
        {
            var quotaions= await _db.Quotations.Where(x => x.RequisitionId == request.RequisitionId).ToListAsync();
            _log.Information("Successfully Retrived Quotations {@Request}", request);
            
            return quotaions;
        }
    }
}
