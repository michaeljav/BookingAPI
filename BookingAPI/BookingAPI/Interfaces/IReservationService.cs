using BookingAPI.CostumObjects;
using BookingAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Interfaces
{
   public  interface IReservationService
    {
        public  Task<List<Reservation>> GetReservations();
        public  object Valid(ReservationCostum reservationCostum);
        public Task<object> ValidNoConflictDateReservation(ReservationCostum reservationCostum,bool availability=false);
        public Task<Reservation> GetReservationsById(int reservationId);
        public Task<Reservation> UpdateReservations(Reservation reservation);
        public Task<Reservation> CancelReservation(int reservationId);
        
        public  Task<Reservation> AddReservation(ReservationCostum reservationCostum);
    }
}
