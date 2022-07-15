using BookingAPI.CostumObjects;
using BookingAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Interfaces
{
   public  interface IEndUserService
    {
        public  Task<List<EndUser>> GetEndUsers();
        public  Task<EndUser> AddEndUser(EndUserCostum endUser);
    }
}
