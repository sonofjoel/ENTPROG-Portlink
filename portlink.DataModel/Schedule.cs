using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Schedule
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ScheduleID { get; set; }

    [ForeignKey("Port")] // Specifies the foreign key relationship
    public int PortID { get; set; }
    public Port Port { get; set; } // Navigation property to the related Port

    public DateTime AvailableDate { get; set; }
    public TimeSpan TimeSlot { get; set; } // Use TimeSpan for TIME data type
    public string Status { get; set; }

    // Navigation property for related bookings
    public ICollection<Booking> Bookings { get; set; }
}
