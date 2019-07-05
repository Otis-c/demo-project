using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Procurement.Api.Pipeline
{
    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _context;

        public RequestLogger(ILogger<TRequest> logger, IHttpContextAccessor context)
        {
            _logger = logger;
            _context = context;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var name = typeof(TRequest).Name;

            // TODO: Add User Details

            _logger.LogInformation("Request: {Name} {@Request} {@UserName}", name, request, _context.HttpContext.User.Identity.Name);
            //Log.Information("Request: {@Name} {@Request}", name, request);

            return Task.CompletedTask;
        }
    }
}
