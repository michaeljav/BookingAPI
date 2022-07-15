using BookingAPI.CostumObjects;
using BookingAPI.Entities;
using BookingAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {



        private IReservationService _reservationService;

        public BookingController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }


        [HttpGet]
        public async Task<ActionResult<List<Reservation>>> GetReservations()
        {
            try
            {
                List<Reservation> reservations = await _reservationService.GetReservations();

                return Ok(new Response("", reservations));
            }
            catch (Exception ex)
            {

                return NotFound(new Response(ex.Message, new object()));
            }


          
        }
        
       // [HttpPut("{reservation}")]
        [HttpPut]
        public async Task<ActionResult<List<Reservation>>> GetUpdateReservation(Reservation reservation)
        {
            try
            {
                Reservation reservations = await _reservationService.UpdateReservations(reservation);

                return Ok(new Response("", reservations));
            }
            catch (Exception ex)
            {

                return NotFound(new Response(ex.Message, new object()));
            }


          
        }
        [HttpDelete("{reservationId}")]
        public async Task<ActionResult<List<Reservation>>> CancelReservation(int reservationId)
        {
            try
            {
                Reservation reservation = await _reservationService.CancelReservation(reservationId);

                return Ok(new Response("", reservation));
            }
            catch (Exception ex)
            {

                return NotFound(new Response(ex.Message, new object()));
            }


          
        }
        [HttpPost]
        public async Task<ActionResult<List<Reservation>>> AddReservation(ReservationCostum reservationCostum)
        {
            try
            {
               
                //Validations
                dynamic valid = _reservationService.Valid(reservationCostum);
                if (valid.returnMesage)
                {
                    return BadRequest(valid.resp);
                }
                //Check if the proposed range is not within any reservation already made.
                dynamic validRange =  await _reservationService.ValidNoConflictDateReservation(reservationCostum);
                if (validRange.returnMesage)
                {
                    return BadRequest(validRange.resp);
                }
                



                Reservation reservation = await _reservationService.AddReservation(reservationCostum);

                return Ok(new Response("", reservation));
            }
            catch (Exception ex)
            {

                return NotFound(new Response(ex.Message, new object()));
            }
            
        }





        //[HttpGet("{id}")]
        //public async Task<ActionResult<Reservation>> GetReservationById(int id)
        //{
        //    var reservation = reservations.Find(r => r.Id ==id);
        //    if (reservation == null)
        //    {
        //        return BadRequest("Reservation not Found");
        //    }
        //    return Ok(reservation);
        //}



        //[HttpPut]
        //public async Task<ActionResult<List<Reservation>>> UpdateReservation(Reservation request)
        //{
        //    var reservation = reservations.Find(r => r.Id == request.Id);
        //    if (reservation == null)
        //    {
        //        return BadRequest("Reservation not Found");
        //    }

        //    reservations.Add(reservation);

        //    return Ok(reservations);
        //}
    }
}
