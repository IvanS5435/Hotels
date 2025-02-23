using Hotels.Services;
using Hotels.ViewData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

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

        /// <summary>
        /// Creates a new hotel.
        /// </summary>
        /// <param name="hotel">Hotel details.</param>
        /// <returns>Returns the created hotel.</returns>
        /// <response code="200">Hotel created successfully.</response>
        /// <response code="400">Invalid hotel data.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHotel([FromBody] HotelCreate hotel)
        {
            return Ok(_hotelService.Add(hotel));
        }

        /// <summary>
        /// Retrieves a list of hotels, optionally sorted by proximity to a given location.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="latitude">Latitude for sorting by distance (optional).</param>
        /// <param name="longitude">Longitude for sorting by distance (optional).</param>
        /// <returns>Returns a paginated list of hotels.</returns>
        /// <response code="200">Returns a list of hotels.</response>
        /// <response code="400">Invalid current location latitude or longitude.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetHotels(int page = 1, double? latitude = null, double? longitude = null)
        {
            if (latitude.HasValue && longitude.HasValue)
            {
                if ((latitude > 90 || latitude < -90) && (longitude > 180 || longitude < -180))
                {
                    return BadRequest("Invalid latitude/longitude.");
                }

                return Ok(_hotelService.GetByDistance(page, pageSize, latitude.Value, longitude.Value));
            }else
            {
                return Ok(_hotelService.GetAll(page, pageSize));
            }
        }

        /// <summary>
        /// Updates an existing hotel.
        /// </summary>
        /// <param name="id">Hotel ID.</param>
        /// <param name="hotelData">Updated hotel data.</param>
        /// <returns>Returns the updated hotel.</returns>
        /// <response code="200">Returned the hotel data.</response>
        /// <response code="400">Invalid request or hotel not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Deletes a hotel.
        /// </summary>
        /// <param name="id">Hotel ID.</param>
        /// <returns>Returns success or failure message.</returns>
        /// <response code="200">Returned success or failure message.</response>
        /// <response code="400">Hotel deletion failed.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
