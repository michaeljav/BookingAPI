using BookingAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {

        private IAvailabilityService _availabilityService;

        public AvailabilityController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpGet("roomsavailability")]
        public async Task<ActionResult<Response>> GetRoomsAvailability()
        {
            try
            {
                //                30 days  is  maximum days to book in advance
                var availability = await _availabilityService.GetRoomsAvailability(30);

                return Ok(new Response("", availability));
            }
            catch (Exception ex)
            {

                return NotFound(new Response(ex.Message, new object()));
            }

            //return null;
        }


    }
}
