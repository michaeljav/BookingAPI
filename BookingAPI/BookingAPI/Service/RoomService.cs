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
    public class RoomService : IRoomService
    {
        private readonly BookingAPI_DbContext _bookingAPI_DbContext;
        public RoomService(BookingAPI_DbContext bookingAPI_DbContext) {

            _bookingAPI_DbContext = bookingAPI_DbContext;
        }

     
        public async Task<Room> AddRoom(RoomCostum roomCostum)
        {
            try
            {
                //New object to create
                Room roomNew = new Room()
                {
                    HotelName = roomCostum.HotelName,
                    BedsNum = roomCostum.BedsNum,
                    State = 0 /*because the instruction says only one room. You can insert but those would be not available to reserve*/
                };

                await _bookingAPI_DbContext.Rooms.AddAsync(roomNew);
                await _bookingAPI_DbContext.SaveChangesAsync();
                var room = await _bookingAPI_DbContext.Rooms.Where(f => f.Id == roomNew.Id).SingleOrDefaultAsync();
                return room;
            }
            catch (Exception)
            {

                throw;
            }
        }       

        public async Task<List<Room>> GetRooms()
        {
            try
            {
                var rooms = await _bookingAPI_DbContext.Rooms.ToListAsync();
                return rooms;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Room>> GetRoomsActive()
        {
            try
            {
                var rooms = await _bookingAPI_DbContext.Rooms.Where(r => r.State == 1).ToListAsync();
                return rooms;
            }
            catch (Exception)
            {

                throw;
            }
        }






    }
}
