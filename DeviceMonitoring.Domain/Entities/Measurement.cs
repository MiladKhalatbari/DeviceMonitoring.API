namespace DeviceMonitoring.Domain.Entities;

public class Measurement
{
    public int Id { get; set; }

    public double? Value { get; set; }

    public DateTime CreatedOn { get; set; }

    public int DeviceId { get; set; }


    public Device Device { get; set; } = null!;
}
