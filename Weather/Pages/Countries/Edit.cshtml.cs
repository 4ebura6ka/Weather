using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Weather.Core;
using Weather.Data;

namespace Weather.Pages.Countries
{
    public class EditModel : PageModel
    {
        private readonly ICityData cityData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public City City { get; set; }
        public IEnumerable<SelectListItem> CityType { get; set; }

        public EditModel(ICityData cityData, IHtmlHelper htmlHelper)
        {
            this.cityData = cityData;
            this.htmlHelper = htmlHelper;
        }
        public IActionResult OnGet(int? cityId)
        {
            CityType = htmlHelper.GetEnumSelectList<CityType>();
            if (cityId.HasValue)
            {
                City = cityData.GetById(cityId.Value);
            }
            else {
                City = new City();
            }

            if (City == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                CityType = htmlHelper.GetEnumSelectList<CityType>();
                return Page();
            }

            if (City.Id > 0)
            {
                cityData.Update(City);
            }
            else
            {
                cityData.Add(City);
            }
            cityData.Commit();
            TempData["Message"] = "City saved!";
            return RedirectToPage("./Detail", new { cityId = City.Id });
        }
    }
}
