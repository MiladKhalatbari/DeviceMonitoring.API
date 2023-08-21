using DeviceMonitoring.Data.Repositories;
using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitoring.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<User> _userService;

        public AuthenticationService(IRepository<User> userService)
        {
            _userService = userService;
        }
        public async Task<User> ValidateUserCredentials(AuthenticationRequestBodyDto user)
        {
            Expression<Func<User, bool>> filter = u =>
             u.Password.Trim().ToLower() == user.Password.Trim().ToLower() && u.UserName.Trim().ToLower() == user.UserName.Trim().ToLower();

            IEnumerable<User> users = await _userService.GetAllAsync(filter);

            if (users.Any())
                return users.Single();
            else
            {
                throw new UnauthorizedAccessException($"Invalid credentials for {user.UserName} {user.Password}");
            }
        }
    }
}
