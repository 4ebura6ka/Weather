using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Weather.Core;
using Weather.Data;

namespace Weather.Pages.Countries
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly ICityData cityData;

        public string Message { get; set; }
        public IEnumerable<City> Cities { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public ListModel(IConfiguration config, ICityData cityData)
        {
            this.config = config;
            this.cityData = cityData;
        }

        public void OnGet()
        {
            Message = config["Message"];
            Cities = cityData.GetCityByName(SearchTerm);
        }
    }
}
