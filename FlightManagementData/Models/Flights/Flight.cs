using FlightManagementData.Models.Airports;
using FlightManagementData.Models.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FlightManagementData.Models.Flights
{
    public class Flight : BaseEntity
    {
        public string FlightNumber { get; set; }
        public DateTime DepartureDate { get; set; }

        //DepartureAirpot
        public int DepartureAirpotID { get; set; }

        [ForeignKey("DepartureAirpotID")]
        [JsonIgnore]
        public virtual Airport? DepartureAirpot { get; set; }

        //ArrivalAirport
        public int ArrivalAirportID { get; set; }

        [ForeignKey("ArrivalAirportID")]
        [JsonIgnore]
        public virtual Airport? ArrivalAirport { get; set; }

        //AirplaneType
        public int AirplaneTypeID { get; set; }

        [ForeignKey("AirplaneTypeID")]
        [JsonIgnore]
        public virtual AirplaneType? AirplaneType { get; set; }
    }
}
