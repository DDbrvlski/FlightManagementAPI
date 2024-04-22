using FlightManagementData.Models.Address;
using FlightManagementData.Models.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FlightManagementData.Models.Airports
{
    public class Airport : BaseEntity
    {
        public string AirportName { get; set; }
        //City
        [Display(Name = "Miasto")]
        public int CityID { get; set; }

        [ForeignKey("CityID")]
        [JsonIgnore]
        public virtual City? City { get; set; }

        //Country
        [Display(Name = "Kraj")]
        public int CountryID { get; set; }

        [ForeignKey("CountryID")]
        [JsonIgnore]
        public virtual Country? Country { get; set; }
    }
}
