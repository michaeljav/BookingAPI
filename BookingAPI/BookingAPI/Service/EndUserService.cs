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
    public class EndUserService : IEndUserService
    {
        private readonly BookingAPI_DbContext _bookingAPI_DbContext;
        public EndUserService(BookingAPI_DbContext bookingAPI_DbContext) {

            _bookingAPI_DbContext = bookingAPI_DbContext;
        }

        public async Task<EndUser> AddEndUser(EndUserCostum endUser)
        {
            try
            {
                //New object to create
                EndUser enduseNew = new EndUser()
                {
                    FirstName = endUser.FirstName,
                    LastName = endUser.LastName,
                    Email = endUser.Email,
                };

                    await _bookingAPI_DbContext.EndUsers.AddAsync(enduseNew);
                 await _bookingAPI_DbContext.SaveChangesAsync();
                   var endUsers = await _bookingAPI_DbContext.EndUsers.Where(f => f.Id == enduseNew.Id).SingleOrDefaultAsync();
                  return endUsers;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<EndUser>> GetEndUsers()
        {
        
            try
            {
                var endUsers = await _bookingAPI_DbContext.EndUsers.ToListAsync();
                return endUsers;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
