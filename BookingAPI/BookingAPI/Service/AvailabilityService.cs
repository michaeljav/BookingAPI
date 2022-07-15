using BookingAPI.CostumObjects;
using BookingAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Service
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly BookingAPI_DbContext _bookingAPI_DbContext;
        private readonly IRoomService _roomService;
        private readonly IReservationService _reservationService;
        public AvailabilityService(BookingAPI_DbContext bookingAPI_DbContext, IRoomService roomService, IReservationService reservationService)
        {

            _bookingAPI_DbContext = bookingAPI_DbContext;
            _roomService = roomService;
            _reservationService = reservationService;
        }

        public async Task<List<RoomAvailabilityCostum>> GetRoomsAvailability(int days)
        {          

            try
            {
                //I will show the availability of a room for a maximum of 30 days because it is the range of time that a person has to schedule.

                ///find the available rooms
                var rooms =  await _roomService.GetRoomsActive();

                List<RoomAvailabilityCostum> roomAvailabilityCostumList = new List<RoomAvailabilityCostum>();

                foreach (var room in rooms)
                {

                    for (int index = 1; index <= days; index++)
                    {
                        //Goin  through the days
                        var daysElapsed = DateTime.Now.AddDays(index);
                        RoomAvailabilityCostum roomAvailabilityCostumNew = new RoomAvailabilityCostum();
                        roomAvailabilityCostumNew.Room = room;
                        roomAvailabilityCostumNew.AvailabilityFrom = new DateTime(daysElapsed.Year,daysElapsed.Month,daysElapsed.Day, 0, 0 , 0);
                        roomAvailabilityCostumNew.AvailabilityTo = new DateTime(daysElapsed.Year,daysElapsed.Month,daysElapsed.Day, 23, 59 , 59);
                        roomAvailabilityCostumList.Add(roomAvailabilityCostumNew);
                    }       

                }

                //Create new list only with availability
                List<RoomAvailabilityCostum> AvailabilityList = new List<RoomAvailabilityCostum>();
                foreach (var roomAvailability in roomAvailabilityCostumList)
                {
                    ReservationCostum reservationCostum = new ReservationCostum();
                    reservationCostum.RoomId = roomAvailability.Room.Id;
                    reservationCostum.ReservationDateFrom = roomAvailability.AvailabilityFrom.ToString();
                    reservationCostum.ReservationDateTo = roomAvailability.AvailabilityTo.ToString();


                    //Check if the proposed range is not within any reservation already made.
                    //the second parameter is true so that a day is not added to the checking date, because the availability is for days
                    dynamic validRange = await _reservationService.ValidNoConflictDateReservation(reservationCostum,true);
                    //If it is not within a range of reservations already made, I add it as available
                    if (!validRange.returnMesage)
                    {
                        AvailabilityList.Add(roomAvailability);
                    }


                }

                return AvailabilityList;
            }
            catch (Exception)
            {

                throw;
            }




        }


       


    }
}
