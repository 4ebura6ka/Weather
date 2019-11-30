using System;
using System.Collections.Generic;
using Weather.Core;
using System.Linq;

namespace Weather.Data
{
    public class InMemoryCitiesData : ICityData
    {
        List<City> cities;

        public InMemoryCitiesData()
        {
            cities = new List<City>()
            {
                new City { Id = 1, CityType = CityType.Capitals, Country = "Lithuania", Name = "Klaipeda" },
                new City { Id = 2, CityType = CityType.Atomic, Country = "Lithuania", Name = "Visaginas" },
                new City { Id = 3, CityType = CityType.Port, Country = "Lithuania", Name = "Vilnius" }
            };
        }

        public IEnumerable<City> GetCityByName(string name = null)
        {
            return cities.FindAll(x => (string.IsNullOrEmpty(name) || x.Name.Contains(name)));
        }

        public City GetById(int id)
        {
            return cities.SingleOrDefault<City>(c => c.Id == id);
        }

        public City Update(City updateCity)
        {
            var city = cities.SingleOrDefault(c => c.Id == updateCity.Id);
            if (city != null)
            {
                city.Name = updateCity.Name;
                city.Country = updateCity.Country;
                city.CityType = updateCity.CityType;
            }
            return city;
        }

        public City Add(City newCity)
        {
            cities.Add(newCity);
            newCity.Id = cities.Max(c => c.Id) + 1;

            return newCity;
        }

        public City Delete(int id)
        {
            var city = cities.FirstOrDefault(c => c.Id == id);
            if (city != null)
            {
                cities.Remove(city);
            }
            return city;
        }
        public int Commit()
        {
            return 0;
        }
    }
}
