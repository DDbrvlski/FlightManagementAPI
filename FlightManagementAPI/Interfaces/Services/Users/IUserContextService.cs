using FlightManagementData.Models.Accounts;
using System.Linq.Expressions;

namespace FlightManagementAPI.Interfaces.Services.Users
{
    public interface IUserContextService
    {
        Task<User?> GetUserByDataAsync(Expression<Func<User, bool>> userExpression);
    }
}