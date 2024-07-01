using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomImageAPIController : ControllerBase
    {
        private readonly IRoomImageRepository _repository;

        public RoomImageAPIController(IRoomImageRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("/getRoomImages")]
        public async Task<IActionResult> getRoomImages()
        {
            var c = await _repository.getRoomImages();
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }
        [HttpGet("/getRoomImagebyId/{roomImageId}")]
        public async Task<IActionResult> getRoomImagebyId(int roomImageId)
        {
            var check = await _repository.getRoomImagebyId(roomImageId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getRoomImagebyRoomId/{roomId}")]
        public async Task<IActionResult> getRoomImagebyRoomId(int roomId)
        {
            var check = await _repository.getRoomImageByRoomId(roomId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpPost("/createRoomImage")]
        public async Task<IActionResult> createRoomImage([FromBody] RoomImage roomImage)
        {
            if (roomImage == null)
            {
                return BadRequest();
            }
            var create = await _repository.createRoomImage(roomImage);
            return StatusCode(201, "Create Successfully!");
        }
        [HttpPut("/updateRoomImage")]
        public async Task<IActionResult> updateHotel([FromBody] RoomImage roomImage)
        {
            var check = await _repository.getRoomImagebyId(roomImage.RoomImageId);
            if (check == null)
            {
                return BadRequest("Not found RoomImage");
            }
            var update = await _repository.updateRoomImage(roomImage);
            return Ok(update);
        }
        [HttpDelete("/deleteRoomImage/{roomImageId}")]
        public async Task<IActionResult> deleteHotel(int roomImageId)
        {
            var check = await _repository.getRoomImagebyId(roomImageId);
            if (check == null)
            {
                return NotFound("Not found RoomImage");
            }
            await _repository.deleteRoomImage(roomImageId);
            return StatusCode(200, "Delele Successfully!");
        }
    }
}
