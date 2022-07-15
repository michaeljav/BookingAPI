using BookingAPI.CostumObjects;
using BookingAPI.Entities;
using BookingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Service
{
    public class ReservationService : IReservationService
    {
        private readonly BookingAPI_DbContext _bookingAPI_DbContext;
        public ReservationService(BookingAPI_DbContext bookingAPI_DbContext) {

            _bookingAPI_DbContext = bookingAPI_DbContext;
        }

        //Get checkinDate and checkoutDate
        public  List<DateTime> GetCheckinCheckout(ReservationCostum reservationCostum)
        {
            //ReservationDateFrom;          0 index
            //DateTime ReservationDateTo;   1 index
            //checkin                       2 index
            //checkout                      3 index



            //convertirn from string to  datetime
            DateTime ReservationDateFrom;
            DateTime ReservationDateTo;
            DateTime.TryParse(reservationCostum.ReservationDateFrom, out ReservationDateFrom);
            DateTime.TryParse(reservationCostum.ReservationDateTo, out ReservationDateTo);

            //-All reservations start at least the next day of booking,
            //-To simplify the use case, a “DAY’ in the hotel room starts from 00:00 to 23:59:59.
            var dateNextDayBookingFrom = ReservationDateFrom;
                dateNextDayBookingFrom = dateNextDayBookingFrom.AddDays(1);
            var dateNextDayBookingTo = ReservationDateTo;
            var stayfrom = new DateTime(dateNextDayBookingFrom.Year, dateNextDayBookingFrom.Month, dateNextDayBookingFrom.Day, 0, 0, 0);
            var stayto = new DateTime(dateNextDayBookingTo.Year, dateNextDayBookingTo.Month, dateNextDayBookingTo.Day, 23, 59, 59);

            return new List<DateTime> { ReservationDateFrom, dateNextDayBookingTo, stayfrom, stayto };
        }



      public async Task<Reservation> AddReservation(ReservationCostum reservationCostum)
        {
            try
            {
                //search user
                EndUser reservationUser = await _bookingAPI_DbContext.EndUsers.Where(f => f.Id == reservationCostum.EndUserId).SingleOrDefaultAsync();
                if (reservationUser == null)
                {
                     throw new InvalidOperationException("This user does not exist."); 
                }
                //search room
                Room reservationRoom = await _bookingAPI_DbContext.Rooms.Where(f => f.Id == reservationCostum.RoomId).SingleOrDefaultAsync();
                if (reservationUser == null || reservationRoom.State == 0)
                {
                    throw new InvalidOperationException("this room either does not exist or is not available (status =0); status= 1 means available");
                }

          
                //get reservation date and check in and out
                List<DateTime> CheckinCheckout = GetCheckinCheckout(reservationCostum);


                //New object to create
                Reservation reervationNew = new Reservation()
                {
                    EndUser = reservationUser,
                    Room = reservationRoom,
                    ReservationDateTimeFrom = CheckinCheckout[0],
                    ReservationDateTimeTo = CheckinCheckout[1],
                    CheckInDateTime = CheckinCheckout[2],
                    CheckOutDateTime = CheckinCheckout[3],
                    Adults = (int)reservationCostum.Adults,
                    Children = (int)reservationCostum.Children,
                    State = 1 //Active the reservervation                    

                };

                await _bookingAPI_DbContext.Reservations.AddAsync(reervationNew);
                await _bookingAPI_DbContext.SaveChangesAsync();
                var reservation = await _bookingAPI_DbContext.Reservations.Where(f => f.Id == reervationNew.Id).SingleOrDefaultAsync();
                return reservation;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

       
        public async Task<List<Reservation>> GetReservations()
        {
            try
            {
                
               // var reservations = await _bookingAPI_DbContext.Reservations.ToListAsync();
                var reservations = await (from p in  _bookingAPI_DbContext.Reservations.Include("Room").Include("EndUser")
                                          select p
                                          ).ToListAsync();
                return reservations;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<Reservation>> GetReservationsByRoom(int RoomId)
        {
            try
            {
                
               // var reservations = await _bookingAPI_DbContext.Reservations.ToListAsync();
                var reservations = await (from p in  _bookingAPI_DbContext.Reservations.Include("Room").Include("EndUser")
                                          where (p.Room.Id == RoomId)
                                          select p
                                          ).ToListAsync();
                return reservations;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Reservation> GetReservationsById(int reservationId)
        {
            try
            {

                // var reservations = await _bookingAPI_DbContext.Reservations.ToListAsync();
                var reservation = await (from p in _bookingAPI_DbContext.Reservations.Include("Room").Include("EndUser")
                                          where (p.Id== reservationId && p.State == 1)
                                          select p
                                          ).SingleOrDefaultAsync();
                return reservation;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Reservation> UpdateReservations(Reservation reservationObj)
        {
            try
            {

                // var reservations = await _bookingAPI_DbContext.Reservations.ToListAsync();
                var reservation = await GetReservationsById(reservationObj.Id);
                if (reservation == null)
                {
                    return new Reservation();
                }
                reservation.EndUser.FirstName = reservationObj.EndUser.FirstName;
                reservation.EndUser.LastName = reservationObj.EndUser.LastName;
                reservation.EndUser.Email = reservationObj.EndUser.Email;
                reservation.Room.HotelName = reservationObj.Room.HotelName;
                reservation.Room.BedsNum = reservationObj.Room.BedsNum;
                reservation.ReservationDateTimeFrom = reservationObj.ReservationDateTimeFrom;
                reservation.ReservationDateTimeTo = reservationObj.ReservationDateTimeTo;
                reservation.CheckInDateTime = reservationObj.CheckInDateTime;
                reservation.CheckOutDateTime = reservationObj.CheckOutDateTime;
                reservation.Adults = reservationObj.Adults;
                reservation.Children = reservationObj.Children;
               await _bookingAPI_DbContext.SaveChangesAsync();




                return reservation;
            }
            catch (Exception)
            {

                throw;
            }
        }


        
             public async Task<Reservation> CancelReservation(int reservationId)
        {
            try
            {

                // var reservations = await _bookingAPI_DbContext.Reservations.ToListAsync();
                var reservation = await GetReservationsById(reservationId);
                if (reservation == null)
                {
                    return new Reservation();
                }
               
                reservation.State = 0;
                await _bookingAPI_DbContext.SaveChangesAsync();

                return reservation;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Validate there is no conflict of dates in the reservations.
        /// </summary>
        /// <param name="reservationCostum"></param>
        /// <returns></returns>
        public async Task<object> ValidNoConflictDateReservation(ReservationCostum reservationCostum,bool availability =false)
        {

           

            //Valid Correct date Format
            DateTime ReservationDateFrom;
            DateTime ReservationDateTo;

            if (!DateTime.TryParse(reservationCostum.ReservationDateFrom, out ReservationDateFrom))
            {

                return new { returnMesage = true, resp = new Response("Insert ReservationDateFrom with format yyyy-mm-dd", new object()) };
            }
            if (!DateTime.TryParse(reservationCostum.ReservationDateTo, out ReservationDateTo))
            {

                return new { returnMesage = true, resp = new Response("Insert ReservationDateTo with format yyyy-mm-dd", new object()) };
            }



            //Get all  Reservations for a Room by id
            var ReservationsForARoom = await GetReservationsByRoom((int)reservationCostum.RoomId);

            //Get the check in and check out  format datatime         
            List<DateTime> CheckinCheckout = GetCheckinCheckout(reservationCostum);

            //If the method for room availability is being called, I do not add a day.
            //The method is called from AvailabilityService class
            if (availability)
            {
                CheckinCheckout[2] = CheckinCheckout[2].AddDays(-1);                
            }

            bool hasConflictOneOrBothDates = false;
            DateTime? CheckInWithingRange = null ;
            DateTime? CheckOutWithingRange = null;
            //Valid there is no conflict 
            //that is, verify if one of the dates is within a reservation already made.
            foreach (var reservation in ReservationsForARoom)
            {
                //If one of the dates proposed for a new reservation is within a range of those already saved, that date cannot be taken.
                bool CheckInDateTimeIsInsideRange = false;
                bool CheckOutDateTimeIsInsideRange = false;
                if (CheckinCheckout[2]  >= reservation.CheckInDateTime && CheckinCheckout[2]  <= reservation.CheckOutDateTime  )
                {
                    CheckInDateTimeIsInsideRange = true;
                    CheckInWithingRange = CheckinCheckout[2];
                }
                if (CheckinCheckout[3]  >= reservation.CheckInDateTime && CheckinCheckout[3] <= reservation.CheckOutDateTime )
                {
                    CheckOutDateTimeIsInsideRange = true;
                    CheckOutWithingRange = CheckinCheckout[3];
                }
               
                if (CheckInDateTimeIsInsideRange ||  CheckOutDateTimeIsInsideRange)
                {
                    hasConflictOneOrBothDates = true;
                    break;
                }
            }

            if (hasConflictOneOrBothDates)
            {
                string checkin = CheckInWithingRange != null ? CheckInWithingRange.ToString() :"" ;
                string checkout = CheckOutWithingRange != null ? CheckOutWithingRange.ToString() :"" ;
                return new { returnMesage = true, resp = new Response("this reservation is within a time range already booked, please choose another date range: range used ("+ checkin+" --- "+ checkout+")", new object()) };
            }          

            //Not Entered 
            return new { returnMesage = false, resp = new Response("", new object()) }; ;

        }

        public object Valid(ReservationCostum reservationCostum)
        {

            //Should have
            if (reservationCostum.EndUserId < 1)
            {
                return new { returnMesage = true, resp = new Response("Insert an User", new object()) }; 
            }
            if (reservationCostum.RoomId < 1)
            {
                
                return new { returnMesage = true, resp = new Response("Insert an Room", new object()) };
            }

            if (reservationCostum.Adults < 1)
            {
              
                return new { returnMesage = true, resp = new Response("Insert an Adults quantity", new object()) };
            }

            //Valid Correct date Format
            DateTime ReservationDateFrom;
            DateTime ReservationDateTo;

            if (!DateTime.TryParse(reservationCostum.ReservationDateFrom, out ReservationDateFrom))
            {
             
                return new { returnMesage = true, resp = new Response("Insert ReservationDateFrom with format yyyy-mm-dd", new object()) };
            }
            if (!DateTime.TryParse(reservationCostum.ReservationDateTo, out ReservationDateTo))
            {
                
                return new { returnMesage = true, resp = new Response("Insert ReservationDateTo with format yyyy-mm-dd", new object()) };
            }

            
            //The ReservationDateFrom date must be equal to or greater than the current date
            if (ReservationDateFrom < DateTime.Now.Date)
            {

                return new { returnMesage = true, resp = new Response("The ReservationDateFrom date must be  greater than the current date", new object()) };
            }

            //validate that ReservationDateFrom is not greater than ReservationDateTo

            if (ReservationDateFrom >= ReservationDateTo)
            {
                
                return new { returnMesage = true, resp = new Response("ReservationDateFrom should be  greater  than ReservationDateTo", new object()) };
            }


            //can’t be reserved more than 30 days in advance
            if ((ReservationDateFrom - DateTime.Now.AddDays(-1)).Days > 30)
            {
                
                return new { returnMesage = true, resp = new Response("Can’t be reserved more than 30 days in advance", new object()) };
            }

            // Reservations cannot be longer than 3 days
            if ((ReservationDateTo - ReservationDateFrom).Days > 3)
            {
                
                return new { returnMesage = true, resp = new Response("The stay can’t be longer than 3 days", new object()) };
            }

            //Not Entered 
            return   new { returnMesage = false, resp = new Response("", new object()) }; ;

        }
    }
}
