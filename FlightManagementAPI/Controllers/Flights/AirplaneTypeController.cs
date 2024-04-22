using FlightManagementAPI.Helpers.BaseController;
using FlightManagementAPI.Interfaces.Helpers;
using FlightManagementData.Models.Flights;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementAPI.Controllers.Flights
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirplaneTypeController : BaseController<AirplaneType>
    {
        public AirplaneTypeController(IBaseService<AirplaneType> baseService) : base(baseService)
        {

        }
    }
}
