using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateAPIController : ControllerBase
    {
        private readonly IRateRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;
        public RateAPIController(IRateRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;
        }

        [HttpPost("/rateHotel")]
        public async Task<IActionResult> rateHotel([FromBody] Rate rate)
        {
            
            var ratingHotel = await _repository.rateHotel(rate);
            return StatusCode(201, "Rating Successfully!");
        }

        [HttpGet("/getRateByOrderHotelHeaderId/{OrderHotelHeaderId}")]
        public async Task<IActionResult> getRateByOrderHotelHeaderId(int OrderHotelHeaderId)
        {

            var rates = await _repository.getRateByOrderHotelHeaderId(OrderHotelHeaderId);
            if (rates == null)
            {
                return NotFound("Not Found"); // Return OK with null data if no rates found
            }
            return Ok(rates);
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



        [HttpGet("/getRateByUserId")]
        public async Task<IActionResult> getRateByUserId()
        {
            var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);
            if (userId != null && userId != 0)
            {
                var rates = await _repository.getRateByUserId(userId.Value);
                if (rates == null)
                {
                    return NotFound("Not Found");
                }
                return Ok(rates);
            }
            else
            {
                return BadRequest(403);
            }


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
