using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitoring.Services.DataTransferObjects;
public record MaxMeasurmentValueAtDate
{
    public double MaxValue { get; set; }
    public DateTime AtDate { get; set; }
}