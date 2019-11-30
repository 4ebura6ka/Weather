using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Weather.Core;
using Weather.Data;

namespace Weather.Pages.Countries
{
    public class DetailModel : PageModel
    {
        private readonly ICityData cityData;

        public City City { get; set; }

        [TempData]
        public string Message { get; set; }

        public DetailModel(ICityData cityData)
        {
            this.cityData = cityData;
        }

        public IActionResult OnGet(int cityId)
        {
            City = cityData.GetById(cityId);
            if (City == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
    }
}
