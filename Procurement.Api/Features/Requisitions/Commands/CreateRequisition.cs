using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Procurement.Api.Data;
using Procurement.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Api.Features.Requisitions.Commands
{
    public class CreateRequisition : IRequest<bool>
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime SubmissionDueDate { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateRequisitionHandler : IRequestHandler<CreateRequisition, bool>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private static IHttpContextAccessor _contextAccessor;

        public CreateRequisitionHandler(AppDbContext db, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> Handle(CreateRequisition request, CancellationToken cancellationToken)
        {
            var req = _mapper.Map<Requisition>(request);
            req.DateCreated = DateTime.Today;
            req.SubmissionDueDate = DateTime.Today.AddDays(7);
            req.CreatedBy = _contextAccessor.HttpContext.User.Identity.Name;
            req.ReferenceNo = "PO-" + RandomNumber().ToString();
            req.Status = "Created";

            _db.Add(req);
            await _db.SaveChangesAsync();
            return true;

        }

        public int RandomNumber()
        {
            Random random = new Random();
            return random.Next(10000, 99999);
        }
    }

}
