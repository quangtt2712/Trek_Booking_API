using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Room3DImageAPIController : ControllerBase
    {
        private readonly IRoom3DImageRepository _repository;

        public Room3DImageAPIController(IRoom3DImageRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("/getRoom3DImages")]
        public async Task<IActionResult> getRoom3DImages()
        {
            var room3DImages = await _repository.getRoom3DImages();

            if (room3DImages == null)
            {
                return NotFound("No data");
            }

            return Ok(room3DImages);
        }

        [HttpGet("/getRoom3DImagebyId/{room3DImageId}")]
        public async Task<IActionResult> getRoom3DImagebyId(int room3DImageId)
        {
            var check = await _repository.getRoom3DImagebyId(room3DImageId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getRoom3DImagebyRoomId/{roomId}")]
        public async Task<IActionResult> getRoom3DImagebyRoomId(int roomId)
        {
            var check = await _repository.getRoom3DImageByRoomId(roomId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpPost("/createRoom3DImage")]
        public async Task<IActionResult> createRoom3DImage([FromBody] Room3DImage room3DImage)
        {
            if (room3DImage == null)
            {
                return BadRequest();
            }
            var create = await _repository.createRoom3DImage(room3DImage);
            return StatusCode(201, "Create Successfully!");
        }
        [HttpPut("/updateRoom3DImage")]
        public async Task<IActionResult> updateRoom3DImage([FromBody] Room3DImage room3DImage)
        {
            var check = await _repository.getRoom3DImagebyId(room3DImage.RoomImage3DId);
            if (check == null)
            {
                return BadRequest("Not found Room3DImage");
            }
            var update = await _repository.updateRoom3DImage(room3DImage);
            return Ok(update);
        }
        [HttpDelete("/deleteRoom3DImage/{room3DImageId}")]
        public async Task<IActionResult> deleteRoom3DImage(int room3DImageId)
        {
            var check = await _repository.getRoom3DImagebyId(room3DImageId);
            if (check == null)
            {
                return NotFound("Not found Room3DImage");
            }
            await _repository.deleteRoom3DImage(room3DImageId);
            return StatusCode(200, "Delele Successfully!");
        }
    }
}
