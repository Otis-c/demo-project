using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.Api.Models
{
    public class Quotation
    {
        public int Id { get; set; }
        public int RequisitionId { get; set; }
        public Requisition Requisition { get; set;}
        public string Description { get; set; }
        public string Company { get; set; }
        public string SubmittedBy { get; set; }
        public double Amount { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }

    }
}
