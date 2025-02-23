using Hotels.Services;
using Hotels.ViewData;
using Microsoft.AspNetCore.Authorization;
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

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateHotel([FromBody] HotelCreate hotel)
        {
            return Ok(_hotelService.Add(hotel));
        }

        [HttpGet]
        public IActionResult GetHotels(int page = 1, double? latitude = null, double? longitude = null)
        {
            if (latitude.HasValue && longitude.HasValue)
            {
                return Ok(_hotelService.GetByDistance(page, pageSize, latitude.Value, longitude.Value));
            }else
            {
                return Ok(_hotelService.GetAll(page, pageSize));
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateHotel(int id, [FromBody] HotelCreate hotelData)
        {
            try
            {
                return Ok(_hotelService.Update(id, hotelData));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteHotel(int id)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
