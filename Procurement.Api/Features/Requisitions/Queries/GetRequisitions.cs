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
    public class GetRequisitions : IRequest<IEnumerable<Requisition>>
    {

    }

    public class GetRequisitionsHandler : IRequestHandler<GetRequisitions, IEnumerable<Requisition>>
    {
        private readonly AppDbContext _db;

        public GetRequisitionsHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Requisition>> Handle(GetRequisitions request, CancellationToken cancellationToken)
        {
            return await _db.Requisitions.ToListAsync();

        }
    }
}
