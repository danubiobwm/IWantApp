using Dapper;
using IWantApp.Endpoints.Employees;
using Microsoft.Data.SqlClient;

namespace IWantApp.Infra.Data;

public class QueryAllUsersWithClaimName
{
    private readonly IConfiguration configuration;

    public QueryAllUsersWithClaimName(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public IEnumerable<EmployeResponse> Execute(int? page, int? rows)
    {
        var db = new SqlConnection(configuration["ConnectionStrings:IWantDb"]);
        var query = @"select Email, ClaimValue as Name
                from AspNetUsers u inner join 
                AspNetUserClaims c on u.Id = c.UserId and ClaimType = 'Name'
                order by Name offset (@page - 1)* @rows ROWS fetch next @rows rows only";

        return db.Query<EmployeResponse>(query, new { page, rows });

    }
}
