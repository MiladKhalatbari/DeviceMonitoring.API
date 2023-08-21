using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitoring.Services.DataTransferObjects;
public record DeviceNameAndNumberOfMeasurements
{
   public required string DeviceName { get; set; }
   public int NumberOfMeasurements { get; set; }
}
