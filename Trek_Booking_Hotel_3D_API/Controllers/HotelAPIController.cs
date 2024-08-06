using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelAPIController : ControllerBase
    {
        private readonly IHotelRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;
        public HotelAPIController(IHotelRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;
        }

        [HttpPut("/toggleHotelStatus")]
        public async Task<IActionResult> ToggleStatus([FromBody] ToggleHotelRequest request)
        {
            var result = await _repository.ToggleHotel(request);
            if (result is NotFoundResult)
            {
                return NotFound();
            }
            return NoContent();
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
        [HttpGet("/getHotelsAdmin")]
        public async Task<IActionResult> getHotelsAdmin()
        {
            var c = await _repository.getHotelsByAdmin();
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }

        [HttpGet("/getHotelsBySupplierId")]
        public async Task<IActionResult> getHotelsBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            if (supplierId != null && supplierId != 0)
            {
                var c = await _repository.getHotelsBySupplierId(supplierId.Value);

                if (c == null)
                {
                    return NotFound("Not Found");
                }
                return Ok(c);
            }
            else
            {
                return BadRequest(403);
            }
                
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
            else if (await _repository.checkExitsName(hotel.HotelName, hotel.SupplierId))
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
