using FlightManagementAPI.Helpers.BaseController;
using FlightManagementAPI.Interfaces.Helpers;
using FlightManagementData.Models.Address;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementAPI.Controllers.Addresses
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : BaseController<Country>
    {
        public CountryController(IBaseService<Country> baseService) : base(baseService)
        {

        }
    }
}
