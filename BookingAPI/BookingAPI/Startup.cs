using BookingAPI.Entities;
using BookingAPI.Interfaces;
using BookingAPI.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI
{
    public class Startup
    {
       // string DataBaseInMemory ;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            //Get variable from appsetting.json
            var DataBaseInMemory= Configuration.GetSection("AppSettings")["DataBaseInMemory"];

            //To create a default value to work with database
            if (DataBaseInMemory != "yes" && DataBaseInMemory != "no")
            {
                DataBaseInMemory = "yes";
            }

            if (DataBaseInMemory == "yes")
            {
                //Context
                services.AddDbContext<BookingAPI_DbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName: "_BookingAPI_5142TE_MJM_ST42024245_842472115157_");

                });

            }
            //if  fisic Database created in Microsoft Sql Server intance
            else if (DataBaseInMemory == "no")
            {
                //Context
                services.AddDbContext<BookingAPI_DbContext>(options =>
                {
                    //Fisic Database  sqlserver 
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"));
                });
            }

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddScoped<IEndUserService, EndUserService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IAvailabilityService, AvailabilityService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {


            var DataBaseInMemory = Configuration.GetSection("AppSettings")["DataBaseInMemory"];
            //To create a default value to work with database
            if (DataBaseInMemory != "yes" && DataBaseInMemory != "no")
            {
                DataBaseInMemory = "yes";
            }

            //load data from database in memory
            if (DataBaseInMemory == "yes")
            {
                // var context = app.ApplicationServices.GetService<BookingAPI_DbContext>();
                var context = serviceProvider.GetService<BookingAPI_DbContext>();
                AddTestData(context);
            }
            else if (DataBaseInMemory == "no")
            {
                //delete and created Database and tablets
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<BookingAPI_DbContext>();
                    context.Database.EnsureDeleted();
                    context.Database.Migrate();
                }
            }



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookingAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //Add data to database in memory
        private static void AddTestData(BookingAPI_DbContext context)
        {



            var testEndUser = new EndUser
            {
                Id = 1,
                FirstName = "Michael",
                LastName = "Javier Mota",
                Email = "michaeljaviermota@gmail.com"
            };
            context.EndUsers.Add(testEndUser);

            var testEndUser2 = new EndUser
            {
                Id = 2,
                FirstName = "Joelina",
                LastName = "Amador Sanchez",
                Email = "Joelina@gmail.com"
            };
            context.EndUsers.Add(testEndUser2);

            /*
             state =1  : available
             state =0  : not available
             */

            var testRoom = new Room
            {
                Id = 11,
                HotelName = "Cancun",
                BedsNum = 2,
                State = 1
            };
            context.Rooms.Add(testRoom);

            var testRoom2 = new Room
            {
                Id = 12,
                HotelName = "Cancun",
                BedsNum = 1,
                State = 0
            };
            context.Rooms.Add(testRoom2);

            context.SaveChanges();
        }

    }
}
