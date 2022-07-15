using BookingAPI.CostumObjects;
using BookingAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Interfaces
{
   public  interface IRoomService
    {
        public Task<List<Room>> GetRooms();
     
        public  Task<Room> AddRoom(RoomCostum endUser);
        public  Task<List<Room>> GetRoomsActive();
    }
}
