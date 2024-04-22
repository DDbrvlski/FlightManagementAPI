using FlightManagementAPI.Interfaces.Services.Users;
using FlightManagementData.Data;
using FlightManagementData.Models.Accounts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FlightManagementAPI.Services.Users
{
    public class UserContextService
        (FlightManagementContext context) : IUserContextService
    {
        public async Task<User?> GetUserByDataAsync(Expression<Func<User, bool>> userExpression)
        {
            return await context.User.Where(x => x.IsActive).FirstOrDefaultAsync(userExpression);
        }
    }
}
