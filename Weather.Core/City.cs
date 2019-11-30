using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weather.Core
{
    public class City
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }

        [Required, StringLength(255)]
        public string Country { get; set; }

        public double? Temperature { get; set; }

        public CityType CityType { get; set; }
    }
}
