using BookingAPI.CostumObjects;
using BookingAPI.Entities;
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
    public class RoomController : ControllerBase
    {

        private IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult<Response>> GetRooms()
        {
            try
            {
                List<Room> rooms = await _roomService.GetRooms();

                return Ok(new Response("", rooms));
            }
            catch (Exception ex)
            {

                return NotFound(new Response(ex.Message, new object()));
            }

            //return null;
        }

      

        [HttpPost]
        public async Task<ActionResult<Response>> AddRoom(RoomCostum room)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(room.HotelName))
                {
                    return BadRequest(new Response("Insert Hotel Name", new object()));
                }

                var roomNew = await _roomService.AddRoom(room);

                return Ok(new Response("", roomNew));
            }
            catch (Exception ex)
            {

                return NotFound(new Response(ex.Message, new object()));
            }

            //return null;
        }


    }
}
