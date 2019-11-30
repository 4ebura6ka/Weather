using System;
using Microsoft.EntityFrameworkCore;
using Weather.Core;

namespace Weather.Data
{
    public class WeatherDbContext : DbContext
    {
        public DbSet<City> Cities { get; set;  }

        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
        {

        }
    }
}

/*
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Festivaio15-25" \
   -p 1433:1433 --name mssqllocaldb \
   -d mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04
*/