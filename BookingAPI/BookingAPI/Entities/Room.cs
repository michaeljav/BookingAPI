using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Entities
{
    public class Room
    {
       
        public int Id { get; set; }

        public string HotelName { get; set; }
        public int BedsNum { get; set; }
        public int State { get; set; }




    }
}
