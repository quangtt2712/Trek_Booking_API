using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateAPIController : ControllerBase
    {
        private readonly IRateRepository _repository;
        public RateAPIController(IRateRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("/rateHotel")]
        public async Task<IActionResult> rateHotel([FromBody] Rate rate)
        {
            
            var ratingHotel = await _repository.rateHotel(rate);
            return StatusCode(201, "Rating Successfully!");
        }

        [HttpGet("/getRateByHotelId/{hotelId}")]
        public async Task<IActionResult> getRateByHotelId(int hotelId)
        {

            var rates = await _repository.getRateByHotelId(hotelId);
            if (rates == null)
            {
                return NotFound("Not Found"); // Return OK with null data if no rates found
            }
            return Ok(rates);
        }



        [HttpGet("/getRateByUserId/{userId}")]
        public async Task<IActionResult> getRateByUserId(int userId)
        {
            var rates = await _repository.getRateByUserId(userId);
            if (rates == null)
            {
                return NotFound("Not Found"); // Return OK with null data if no rates found
            }
            return Ok(rates);
        }

        //Total rate of hotel
        [HttpGet("/getTotalRateValueByHotelId/{hotelId}")]
        public async Task<IActionResult> getTotalRateValueByHotelId(int hotelId)
        {
            float totalRateValue = await _repository.getTotalRateValueByHotelId(hotelId);
            if (totalRateValue == null)
            {
                return NotFound("Not Found"); // Return OK with null data if no rates found
            }
            return Ok(totalRateValue);
        }

    }
}
