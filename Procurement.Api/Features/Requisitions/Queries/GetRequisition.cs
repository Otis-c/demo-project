using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Api.Data;
using Procurement.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Api.Features.Requisitions.Queries
{
    public class GetRequisition : IRequest<Requisition>
    {
        public int Id { get; set; }
    }

    public class GetRequisitionHandler : IRequestHandler<GetRequisition, Requisition>
    {
        private readonly AppDbContext _db;

        public GetRequisitionHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<Requisition> Handle(GetRequisition request, CancellationToken cancellationToken)
        {
            return await _db.Requisitions.SingleOrDefaultAsync(x => x.Id == request.Id);

        }
    }
}
