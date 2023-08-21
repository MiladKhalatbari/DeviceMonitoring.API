using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitoring.Services.DataTransferObjects;
public record MinMeasurmentValueAtDate
{
    public double MinValue { get; set; }
    public DateTime AtDate { get; set; }
}
