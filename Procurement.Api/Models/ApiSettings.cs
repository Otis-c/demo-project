using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.Api.Models
{
    public class ApiSettings
    {
        public string SenderAddress { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
    }
}
