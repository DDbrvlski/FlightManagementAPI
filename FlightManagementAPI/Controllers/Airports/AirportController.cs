using FlightManagementAPI.Interfaces.Services.Airports;
using FlightManagementDTO.DTOs.Airports;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementAPI.Controllers.Airports
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController(IAirportService airportService) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AirportDTO>>> GetAirportsAsync()
        {
            var flights = await airportService.GetAllAirportsAsync();
            return Ok(flights);
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<AirportDetailsDTO>> GetAirportDetailsAsync(int Id)
        {
            var flight = await airportService.GetAirportDetailsAsync(Id);
            return Ok(flight);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAirportAsync(AirportPostDTO airportData)
        {
            await airportService.CreateAirportAsync(airportData);
            return Created();
        }
        [HttpPut]
        public async Task<IActionResult> EditAirportAsync(AirportPutDTO airportData)
        {
            await airportService.UpdateAirportAsync(airportData);
            return NoContent();
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeactivateAirportAsync(int Id)
        {
            await airportService.DeactivateAirportAsync(Id);
            return NoContent();
        }
    }
}
