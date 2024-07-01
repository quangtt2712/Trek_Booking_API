using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelAPIController : ControllerBase
    {
        private readonly IHotelRepository _repository;
        public HotelAPIController(IHotelRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("/getHotels")]
        public async Task<IActionResult> getHotels()
        {
            var c = await _repository.getHotels();
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }

        [HttpGet("/getHotelsBySupplierId/{supplierId}")]
        public async Task<IActionResult> getHotelsBySupplierId(int supplierId)
        {
            var c = await _repository.getHotelsBySupplierId(supplierId);
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }

        [HttpGet("/searchHotelByName/{key}")]
        public async Task<IActionResult> searchHotelByName(string key)
        {
            var c = await _repository.searchHotelByName(key);
            if (c == null || !c.Any())
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }

        [HttpGet("/getHotelbyId/{hotelId}")]
        public async Task<IActionResult> getHotelbyId(int hotelId)
        {
            var check = await _repository.getHotelbyId(hotelId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpPost("/createHotel")]
        public async Task<IActionResult> createHotel([FromBody] Hotel hotel)
        {
            if (hotel == null)
            {
                return BadRequest();
            }
            else if (await _repository.checkExitsName(hotel.HotelName))
            {
                return BadRequest("HotelName already exits");
            }
            else if (await _repository.checkExitsEmail(hotel.HotelEmail))
            {
                return BadRequest("HotelEmail already exits");
            }
            var create = await _repository.createHotel(hotel);
            return StatusCode(201, "Create Successfully!");
        }
        [HttpPut("/updateHotel")]
        public async Task<IActionResult> updateHotel([FromBody] Hotel hotel)
        {
            var check = await _repository.getHotelbyId(hotel.HotelId);
            if (check == null)
            {
                return BadRequest("Not found Hotel");
            }
            var update = await _repository.updateHotel(hotel);
            return Ok(update);
        }

        [HttpPut("/updateHotelAvatar")]
        public async Task<IActionResult> updateHotelAvatar([FromBody] Hotel hotel)
        {
            var check = await _repository.getHotelbyId(hotel.HotelId);
            if (check == null)
            {
                return BadRequest("Not found Hotel");
            }
            var update = await _repository.updateHotelAvatar(hotel);
            return Ok(update);
        }

        [HttpPut("/deleteHotel/{hotelId}")]
        public async Task<IActionResult> deleteHotel(int hotelId)
        {
            var check = await _repository.getHotelbyId(hotelId);
            if (check == null)
            {
                return NotFound("Not found Hotel");
            }
            await _repository.deleteHotel(hotelId);
            return StatusCode(200, "Delele Successfully!");
        }

        [HttpPut("/recoverHotelDeleted/{hotelId}")]
        public async Task<IActionResult> recoverHotelDeleted(int hotelId)
        {
            var check = await _repository.getHotelbyId(hotelId);
            if (check == null)
            {
                return NotFound("Not found Hotel");
            }
            await _repository.recoverHotelDeleted(hotelId);
            return StatusCode(200, "Recover Successfully!");
        }


        [HttpGet("/searchHotelByCity")]
        public async Task<IActionResult> SearchHotelByCity([FromQuery] string city)
        {
            var hotels = await _repository.SearchHotelByCity(city);
            if (hotels == null || !hotels.Any())
            {
                return NotFound("No hotels found in the specified city.");
            }
            return Ok(hotels);
        }

        [HttpGet("/searchHotelSchedule")]
        public async Task<IActionResult> SearchHotelSchedule([FromQuery] DateTime checkInDate, [FromQuery] DateTime checkOutDate, [FromQuery] string city)
        {
            var hotels = await _repository.SearchHotelSchedule(checkInDate, checkOutDate, city);
            if (hotels == null || !hotels.Any())
            {
                return NotFound("No hotels found with available rooms for the specified dates and city.");
            }
            return Ok(hotels);
        }
    }
}
