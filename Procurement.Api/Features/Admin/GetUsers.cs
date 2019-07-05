using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Api.Data;
using Procurement.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Api.Features.Admin
{
    public class GetUsers : IRequest<IEnumerable<QueriedUsers>>
    {

    }

    public class GetUsersHandler : IRequestHandler<GetUsers, IEnumerable<QueriedUsers>>
    {
        private readonly AppDbContext _db;

        public GetUsersHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<QueriedUsers>> Handle(GetUsers request, CancellationToken cancellationToken)
        {
            using (var con = new SqlConnection(
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Procurement;Integrated Security=True"))
            {
                var query = @"SELECT AspNetUsers.UserName, AspNetUsers.Email, AspNetUsers.PhoneNumber, 
                            AspNetUsers.CompanyName, AspNetUsers.FirstName, AspNetUsers.LastName, 
                            AspNetUsers.Id, AspNetRoles.Name AS Role
                            FROM AspNetRoles INNER JOIN
                            AspNetUserRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId RIGHT OUTER JOIN
                            AspNetUsers ON AspNetUserRoles.UserId = AspNetUsers.Id";

                var result = await con.QueryAsync<QueriedUsers>(query);
                return result;
            }
        }

        private  string GetRoleName(ICollection<UserRole> userRoles)
        {
            var roleId = userRoles.FirstOrDefault().RoleId;
            return _db.Roles.Single(x => x.Id == roleId).Name ?? string.Empty;
           
        }
    }


    public class QueriedUsers
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }
}
