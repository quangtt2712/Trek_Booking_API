using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomServiceController : ControllerBase
    {
        private readonly IRoomServiceRepository _roomServiceRepository;

        public RoomServiceController(IRoomServiceRepository roomServiceRepository)
        {
            _roomServiceRepository = roomServiceRepository;
        }

        // GET: api/RoomService
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomService>>> GetRoomServices()
        {
            var roomServices = await _roomServiceRepository.getRoomServices();
            return Ok(roomServices);
        }

        // GET: api/RoomService/room/{roomId}/services

        [HttpGet("room/{roomId}/services")]
        public async Task<ActionResult<IEnumerable<Services>>> GetServicebyRoomId(int roomId)
        {
            var services = await _roomServiceRepository.getServicebyRoomId(roomId);
            if (services == null)
            {
                return NotFound();
            }

            return Ok(services);
        }

        // GET: api/RoomService/service/{serviceId}/rooms
        [HttpGet("service/{serviceId}/rooms")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomsByServiceId(int serviceId)
        {
            var rooms = await _roomServiceRepository.getRoombyServiceId(serviceId);
            if (rooms == null)
            {
                return NotFound();
            }

            return Ok(rooms);
        }

        // POST: api/RoomService
        [HttpPost("/createRoomService")]
        public async Task<ActionResult<RoomService>> CreateRoomService(RoomService roomService)
        {
            var existingRoomService = await _roomServiceRepository.GetRoomServiceByRoomIdAndServiceId(roomService.RoomId, roomService.ServiceId);

            if (existingRoomService != null)
            {
                
                await _roomServiceRepository.UpdateRoomService(existingRoomService);
                return Ok(existingRoomService);
            }
            else
            {
                // Create a new room service
                var createdRoomService = await _roomServiceRepository.createRoomService(roomService);
                return CreatedAtAction(nameof(GetRoomServices), new { roomId = createdRoomService.RoomId, serviceId = createdRoomService.ServiceId }, createdRoomService);
            }
        }

        // DELETE: api/RoomService
        [HttpDelete("/deleteRoomService")]
        public async Task<ActionResult<RoomService>> DeleteRoomService(RoomService roomService)
        {
            var deletedRoomService = await _roomServiceRepository.deleteRoomService(roomService);
            if (deletedRoomService == null)
            {
                return NotFound();
            }

            return Ok(new { message = "Delete successful" });
        }
    }
}
