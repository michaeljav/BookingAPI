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
    public class EndUserController : ControllerBase
    {


        private IEndUserService _endUserService;

        public EndUserController(IEndUserService endUserService)
        {
            _endUserService = endUserService;
        }

        [HttpGet]
        public async Task<ActionResult<Response>> GetEndUsers()
        {
            try
            {
                List<EndUser> endUsers = await _endUserService.GetEndUsers();

                return Ok(new Response("", endUsers));
            }
            catch (Exception ex)
            {

                return NotFound(new Response(ex.Message,new object()));
            }            
            
        }
        [HttpPost]
        public async Task<ActionResult<Response>> AddEndUser(EndUserCostum endUser)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(endUser.FirstName))
                {
                    return BadRequest(new Response("Insert Name", new object()));
                }

                EndUser endUsers = await _endUserService.AddEndUser(endUser);

                return Ok(new Response("", endUsers));
            }
            catch (Exception ex)
            {

                return NotFound(new Response(ex.Message, new object()));
            }

            
        }
    }
}
