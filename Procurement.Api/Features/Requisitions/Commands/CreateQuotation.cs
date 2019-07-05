using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Procurement.Api.Data;
using Procurement.Api.Models;
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
    public class CreateQuotation : IRequest<bool>
    {
        public int RequisitionId { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
    }

    public class CreateQuotationHandler : IRequestHandler<CreateQuotation, bool>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _context;
        private readonly INotificationService _notificationService;
        private readonly ILogger _log;

        public CreateQuotationHandler(AppDbContext db, IMapper mapper, IHttpContextAccessor context,
                                       INotificationService notificationService)
        {
            _db = db;
            _mapper = mapper;
            _context = context;
            _notificationService = notificationService;
            _log = Log.ForContext("UserName", _context.HttpContext.User.Identity.Name);
            _log.ForContext("Source", typeof(CreateQuotationHandler).Name);
        }
        public async Task<bool> Handle(CreateQuotation request, CancellationToken cancellationToken)
        {
            var quote = _mapper.Map<Quotation>(request);
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Email == _context.HttpContext.User.Identity.Name);
            quote.Company = user.CompanyName;
            quote.SubmittedBy = user.Email;

            _db.Add(quote);
            await _db.SaveChangesAsync();
            _log.Information("Successfully save Quotation {@Request}", request);
            Sendmail();

            return true;
        }

        private void Sendmail()
        {
            var email = _context.HttpContext.User.Identity.Name;
            var message = "Your quotations has been received.";
            _notificationService.SendEmail(email, "Quotation Received", message, "alert");
        }
    }

    public class CreateQuotationValidator : AbstractValidator<CreateQuotation>
    {
        public CreateQuotationValidator()
        {
            RuleFor(x => x.RequisitionId).GreaterThan(0);
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.FilePath).NotEmpty();

        }
    }
}
