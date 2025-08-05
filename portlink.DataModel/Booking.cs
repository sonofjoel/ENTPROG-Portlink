using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Booking
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BookingID { get; set; }

    [ForeignKey("User")]
    public int UserID { get; set; }
    public User User { get; set; }

    [ForeignKey("Schedule")]
    public int ScheduleID { get; set; }
    public Schedule Schedule { get; set; } 

    public string DropOff { get; set; }
    public decimal Price { get; set; }
    public DateTime BookingDate { get; set; }
    public string Status { get; set; }
    public int? Rating { get; set; } 
    public DateTime DateModified { get; set; }

    
    public Tracking Tracking { get; set; }
}
