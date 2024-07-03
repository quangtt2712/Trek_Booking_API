using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    public class HotelImageAPIController : Controller
    {
        private readonly IHotelImageRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;
        public HotelImageAPIController(IHotelImageRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;
        }
        [HttpGet("/getHotelImages")]
        public async Task<IActionResult> getHotelImages()
        {
            var c = await _repository.getHotelImages();
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }
        [HttpGet("/getHotelImagebyId/{roomImageId}")]
        public async Task<IActionResult> getHotelImagebyId(int hotelImageId)
        {
            var check = await _repository.getHotelImagebyId(hotelImageId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getHotelImagebyHotelId/{hotelId}")]
        public async Task<IActionResult> getHotelImagebyHotelId(int hotelId)
        {
            var check = await _repository.getHotelImageByHotelId(hotelId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpPost("/createHotelImage")]
        public async Task<IActionResult> createHotelImage([FromBody] HotelImage hotelImage)
        {
            if (hotelImage == null)
            {
                return BadRequest();
            }
            var create = await _repository.createHotelImage(hotelImage);
            return StatusCode(201, "Create Successfully!");
        }
        [HttpPut("/updateHotelImage")]
        public async Task<IActionResult> updateHotelImage([FromBody] HotelImage hotelImage)
        {
            var check = await _repository.getHotelImagebyId(hotelImage.HotelImageId);
            if (check == null)
            {
                return BadRequest("Not found RoomImage");
            }
            var update = await _repository.updateHotelImage(hotelImage);
            return Ok(update);
        }
        [HttpDelete("/deleteHotelImage/{hotelImageId}")]
        public async Task<IActionResult> deleteHotel(int hotelImageId)
        {
            var check = await _repository.getHotelImagebyId(hotelImageId);
            if (check == null)
            {
                return NotFound("Not found RoomImage");
            }
            await _repository.deleteHotelImage(hotelImageId);
            return StatusCode(200, "Delele Successfully!");
        }
    }
}
