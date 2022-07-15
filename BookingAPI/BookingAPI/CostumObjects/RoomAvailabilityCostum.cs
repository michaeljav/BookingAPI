using BookingAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.CostumObjects
{
    public class RoomAvailabilityCostum
    {
        public Room Room { get; set; }
        public DateTime AvailabilityFrom { get; set; }
        public DateTime AvailabilityTo { get; set; }
    }
}
