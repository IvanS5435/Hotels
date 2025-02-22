using Hotels.Services;
using Hotels.ViewData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotels.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly int pageSize = 5;

        public HotelsController(IHotelService hotelService) {
            _hotelService = hotelService;
        }

        [HttpPost]
        public IActionResult CreateHotel([FromBody] HotelCreate hotel)
        {
            return Ok(_hotelService.Add(hotel));
        }

        [HttpGet]
        public IActionResult GetHotels(int page = 1,double latitude=0, double longitude=0)
        {
            return Ok(_hotelService.GetByDistance(page, pageSize, latitude, longitude));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateHotel(int id, [FromBody] HotelCreate hotelData)
        {
            return Ok(_hotelService.Update(id, hotelData));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteHotel(int id)
        {
            var hotelDeleted = _hotelService.Delete(id);
            if (hotelDeleted)
            {
                return Ok("Hotel deleted successfully");
            }
            else
            {
                return BadRequest("Hotel deleted failed");
            }

        }
    }
}
