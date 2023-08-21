using DeviceMonitoring.Domain.Entities;
using DeviceMonitoring.Services.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitoring.Services;
public interface IAuthenticationService
{
    public Task<User> ValidateUserCredentials(AuthenticationRequestBodyDto user);
}
