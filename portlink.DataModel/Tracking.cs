using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Tracking
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TrackingID { get; set; }

    [ForeignKey("Booking")]
    public int BookingID { get; set; }
    public Booking Booking { get; set; } 

    public string CurrentLocation { get; set; }
    public DateTime EstimatedArrival { get; set; }
    public DateTime UpdatedAt { get; set; }
}
