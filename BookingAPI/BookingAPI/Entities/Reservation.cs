using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Entities
{
    public class Reservation
    {
      
        public int Id { get; set; }
     
        public virtual EndUser EndUser { get; set; }    
        public virtual Room Room { get; set; }
        public DateTime ReservationDateTimeFrom { get; set; }
        public DateTime ReservationDateTimeTo { get; set; }
        public DateTime CheckInDateTime { get; set; }
        public DateTime CheckOutDateTime { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int State { get; set; }

    }
}
