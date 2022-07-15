using BookingAPI.CostumObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Interfaces
{
    public interface IAvailabilityService
    {
        public Task<List<RoomAvailabilityCostum>> GetRoomsAvailability(int days);
    }
}
