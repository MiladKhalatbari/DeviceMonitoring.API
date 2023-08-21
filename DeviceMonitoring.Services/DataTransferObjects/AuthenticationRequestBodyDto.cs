using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitoring.Services.DataTransferObjects;
public record AuthenticationRequestBodyDto
{
    [MaxLength(256)]
    public string UserName { get; set; }
    [MaxLength(256)]
    public string Password { get; set; }
}
