using System;

namespace portlink.Models
{
    public class DeliveryRequest
    {
        public int Id { get; set; }
        public string CargoType { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
