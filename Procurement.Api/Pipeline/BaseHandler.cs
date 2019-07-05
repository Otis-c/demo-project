using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.Api.Pipeline
{
    public class BaseHandler 
    {
        private readonly IHttpContextAccessor _context;
        public readonly ILogger _log;

        public BaseHandler(IHttpContextAccessor context)
        {
            _context = context;
            _log = Log.ForContext("UserName", _context.HttpContext.User.Identity.Name);
        }
    }
}
