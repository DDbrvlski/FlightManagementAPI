using FlightManagementAPI.Interfaces.Services.Flights;
using FlightManagementDTO.DTOs.Flights;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementAPI.Controllers.Flights
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController(IFlightService flightService) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightDTO>>> GetFlightsAsync()
        {
            var flights = await flightService.GetAllFlightsAsync();
            return Ok(flights);
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<FlightDetailsDTO>> GetFlightDetailsAsync(int Id)
        {
            var flight = await flightService.GetFlightDetailsAsync(Id);
            return Ok(flight);
        }
        [HttpPost]
        public async Task<IActionResult> CreateFlightAsync(FlightPostDTO flightData)
        {
            await flightService.CreateFlightAsync(flightData);
            return Created();
        }
        [HttpPut]
        public async Task<IActionResult> EditFlightAsync(FlightPutDTO flightData)
        {
            await flightService.UpdateFlightAsync(flightData);
            return NoContent();
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeactivateFlightAsync(int Id)
        {
            await flightService.DeactivateFlightAsync(Id);
            return NoContent();
        }
    }
}
