using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }
    public string CompanyName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Designation { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLogin { get; set; }

    
    public ICollection<Booking> Bookings { get; set; }
}
