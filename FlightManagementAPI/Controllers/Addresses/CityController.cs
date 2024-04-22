using FlightManagementAPI.Helpers.BaseController;
using FlightManagementAPI.Interfaces.Helpers;
using FlightManagementData.Models.Address;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementAPI.Controllers.Addresses
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : BaseController<City>
    {
        public CityController(IBaseService<City> baseService) : base(baseService)
        {
            
        }
    }
}
