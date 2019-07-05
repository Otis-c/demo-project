using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.Api.Models
{
    public class Requisition
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime SubmissionDueDate { get; set; }
        public string Status { get; set; }
        public ICollection<Quotation> Quotations { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }
}
