using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomAPIController : ControllerBase
    {
        private readonly IRoomRepository _repository;

        public RoomAPIController(IRoomRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("/getRooms")]
        public async Task<IActionResult> getRooms()
        {
            var c = await _repository.getRooms();
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }
        [HttpGet("/getRoombyId/{roomId}")]
        public async Task<IActionResult> getRoombyId(int roomId)
        {
            var check = await _repository.getRoombyId(roomId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getRoombyHotelId/{hotelId}")]
        public async Task<IActionResult> getRoombyHotelId(int hotelId)
        {
            var check = await _repository.getRoombyHotelId(hotelId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpPost("/createRoom")]
        public async Task<IActionResult> createRoom([FromBody] Room room)
        {
            if (room == null)
            {
                return BadRequest();
            }
            else if (await _repository.checkExitsName(room.RoomName,room.HotelId))
            {
                return BadRequest("RoomName already exits");
            }
            var create = await _repository.createRoom(room);
            return StatusCode(201, "Create Successfully!");
        }
        [HttpPut("/updateRoom")]
        public async Task<IActionResult> updateRoom([FromBody] Room room)
        {
            var check = await _repository.getRoombyId(room.RoomId);
            if (check == null)
            {
                return BadRequest("Not found Room");
            }
            var update = await _repository.updateRoom(room);
            return Ok(update);
        }
        [HttpPut("/deleteRoom/{roomId}")]
        public async Task<IActionResult> deleteRoom(int roomId)
        {
            var check = await _repository.getRoombyId(roomId);
            if (check == null)
            {
                return NotFound("Not found Room");
            }
            await _repository.deleteRoom(roomId);
            return StatusCode(200, "Delele Successfully!");
        }

        [HttpPut("/recoverRoomDeleted/{roomId}")]
        public async Task<IActionResult> recoverRoomDeleted(int roomId)
        {
            var check = await _repository.getRoombyId(roomId);
            if (check == null)
            {
                return NotFound("Not found Room");
            }
            await _repository.recoverRoomDeleted(roomId);
            return StatusCode(200, "Recover Successfully!");
        }

        [HttpGet("/getRoomAvailability")]
        public async Task<ActionResult<IEnumerable<RoomAvailabilityDto>>> GetRoomAvailability(int hotelId, DateTime checkInDate, DateTime checkOutDate)
        {
            try
            {
                var availableRooms = await _repository.SearchRoomSchedule(hotelId, checkInDate, checkOutDate);
                if (availableRooms == null || !availableRooms.Any())
                {
                    return NotFound("No available rooms found for the given dates.");
                }
                return Ok(availableRooms);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for simplicity)
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
