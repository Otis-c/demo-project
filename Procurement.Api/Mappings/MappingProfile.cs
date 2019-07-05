using AutoMapper;
using Procurement.Api.Features.Requisitions.Commands;
using Procurement.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateRequisition, Requisition>();
            CreateMap<CreateQuotation, Quotation>();

            //CreateMap<CreateRequistion, Requisition>();
            //CreateMap<CreateReqItem, RequisitionItem>();
            //CreateMap<Requisition, CreateRequistion>();
            //CreateMap<RequisitionItem, CreateReqItem>();
        }
    }
}
