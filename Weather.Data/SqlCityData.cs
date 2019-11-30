using System.Linq;
using System.Collections.Generic;
using Weather.Core;
using Microsoft.EntityFrameworkCore;

namespace Weather.Data
{

    public class SqlCityData : ICityData
    {
        private readonly WeatherDbContext db;

        public SqlCityData(WeatherDbContext db)
        {
            this.db = db;
        }

        public City Add(City newCity)
        {
            db.Add(newCity);
            return newCity;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public City Delete(int id)
        {
            var city = GetById(id);
            if (city != null)
            {
                db.Cities.Remove(city);
            }
            return city;
        }

        public City GetById(int id)
        {
            return db.Cities.Find(id);
        }

        public IEnumerable<City> GetCityByName(string name)
        {
            var cities = from c in db.Cities
                         where (c.Name.Contains(name) || string.IsNullOrEmpty(name))
                         orderby c.Name
                         select c;
            return cities;
        }

        public City Update(City updateCity)
        {
            var entity = db.Cities.Attach(updateCity);
            entity.State = EntityState.Modified;
            return updateCity;
        }
    }
}
