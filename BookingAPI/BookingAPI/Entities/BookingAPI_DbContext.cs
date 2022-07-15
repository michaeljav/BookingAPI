using BookingAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI
{
    public class BookingAPI_DbContext:DbContext
    {
        private readonly AppSettings _appSettings;
        public BookingAPI_DbContext(DbContextOptions<BookingAPI_DbContext> options, IOptions<AppSettings> appSettings) :base(options)
        {
            _appSettings = appSettings.Value;

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (_appSettings.DataBaseInMemory != "yes" && _appSettings.DataBaseInMemory != "no")
            {
                _appSettings.DataBaseInMemory = "yes";
            }

            //If it is fisic database to create
            if (_appSettings.DataBaseInMemory == "no")
            {
                // Seed Data
                modelBuilder.Entity<EndUser>().HasData(
                    new EndUser
                    {
                        Id = 1,
                        FirstName = "Michael",
                        LastName = "Javier Mota",
                        Email = "michaeljaviermota@gmail.com"
                    },
                     new EndUser
                     {
                         Id = 2,
                         FirstName = "Joelina",
                         LastName = "Amador Sanchez",
                         Email = "Joelina@gmail.com"
                     }
                    );

                /*
                 state =1  : available
                 state =0  : not available
                 */
                modelBuilder.Entity<Room>().HasData(
                   new Room
                   {
                       Id = 11,
                       HotelName = "Cancun",
                       BedsNum = 2,
                       State = 1
                   },
                    new Room
                    {
                        Id = 12,
                        HotelName = "Cancun",
                        BedsNum = 1,
                        State = 0
                    }
                   );
            }
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<EndUser> EndUsers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
