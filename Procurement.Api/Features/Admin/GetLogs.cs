using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Procurement.Api.Data;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Api.Features.Admin
{
    public class GetLogs : IRequest<IEnumerable<AppLog>>
    {

    }

    public class GetLogsHandler : IRequestHandler<GetLogs, IEnumerable<AppLog>>
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _context;
        private readonly ILogger _log;

        public GetLogsHandler(AppDbContext db, IHttpContextAccessor context)
        {
            _db = db;
            _context = context;
            _log = Serilog.Log.ForContext("UserName", _context.HttpContext.User.Identity.Name);
            _log.ForContext("Source", typeof(GetLogsHandler).Name);
        }
        public async Task<IEnumerable<AppLog>> Handle(GetLogs request, CancellationToken cancellationToken)
        {
            using (var con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Procurement;Integrated Security=True;"))
            {
                var query = @"SELECT  Distinct([TimeStamp]),
                              JSON_VALUE(LogEvent, '$.Properties.Name') AS [Action],
                              JSON_VALUE(LogEvent, '$.Properties.UserName') AS UserName
                              FROM [Procurement].[dbo].[Logs]
                              WHERE [Message] like 'Request:%' ";

                var logs = await con.QueryAsync<AppLog>(query);
                return logs;
            }
        }
    }

    public class AppLog
    {
        public DateTime TimeStamp { get; set; }
        public string Action { get; set; }
        public string UserName { get; set; }
    }
}

