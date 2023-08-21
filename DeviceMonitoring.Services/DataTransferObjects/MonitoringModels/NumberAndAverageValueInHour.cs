using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitoring.Services.DataTransferObjects;
public record NumberAndAverageValueInHour
{
    public int AtHour { get; set; }
    public int NumberOfMeasurments { get; set; }
    public double AverageValue { get; set; }
}
