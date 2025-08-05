using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Port
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PortID { get; set; }
    public string PortName { get; set; }
    public string Location { get; set; }
    public string Status { get; set; }

    
    public ICollection<Schedule> Schedules { get; set; }
}
