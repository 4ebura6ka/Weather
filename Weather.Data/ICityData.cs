using System.Collections.Generic;
using Weather.Core;
using System.Linq;

namespace Weather.Data
{
    public interface ICityData
    {
        IEnumerable<City> GetCityByName(string name);
        City GetById(int id);
        City Update(City updateCity);
        City Add(City newCity);
        City Delete(int id);
        int Commit();
    }

    
}