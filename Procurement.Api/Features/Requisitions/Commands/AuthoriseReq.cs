using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Procurement.Api.Data;
using Procurement.Api.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Api.Features.Requisitions.Commands
{
    public class AuthoriseReq : IRequest<int>
    {
        public int ReqId { get; set; }
    }

    public class AuthoriseReqHandler : IRequestHandler<AuthoriseReq, int>
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _context;
        private readonly INotificationService _notificationService;
        private readonly ILogger _log;

        public AuthoriseReqHandler(AppDbContext db, IHttpContextAccessor context,
                                   INotificationService notificationService)
        {
            _db = db;
            _context = context;
            _notificationService = notificationService;
            _log = Log.ForContext("UserName", _context.HttpContext.User.Identity.Name);
            _log.ForContext("Source", typeof(CreateQuotationHandler).Name);
        }
        public async Task<int> Handle(AuthoriseReq request, CancellationToken cancellationToken)
        {
            var req = await _db.Requisitions.SingleOrDefaultAsync(x => x.Id == request.ReqId);
            req.Status = "Authorised";

            var quote = await _db.Quotations.SingleOrDefaultAsync(x => x.RequisitionId == request.ReqId && x.Status != "Rejected");
            quote.Status = "Authorised";
            Sendmail(quote.SubmittedBy);

            var result = await _db.SaveChangesAsync();

            return result;
        }

        private void Sendmail(string email)
        {
            // var email = _context.HttpContext.User.Identity.Name;
            var message = "Your quotations has been authorised.";
            _notificationService.SendEmail(email, "Quotation Authorised", message, "alert");
        }
    }
}
