using BookingAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.CostumObjects
{
    public class ReservationCostum
    {

        public int? EndUserId { get; set; }
        public int? RoomId { get; set; }
        public string ReservationDateFrom { get; set; }
        public string ReservationDateTo { get; set; }   
        public int? Adults { get; set; }
        public int? Children { get; set; }
       
    }
}
