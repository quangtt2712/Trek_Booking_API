using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingAPIController : ControllerBase
    {
        private readonly IBookingRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;

        public BookingAPIController(IBookingRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;
        }

        [HttpGet("/getBookings")]
        public async Task<IActionResult> getBookings()
        {
            var c = await _repository.getBookings();
            return Ok(c);
        }

        [HttpGet("/getBookingById/{bookingId}")]
        public async Task<IActionResult> getBookingById(int bookingId)
        {
            var check = await _repository.getBookingById(bookingId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getBookingByUserId/{userId}")]
        public async Task<IActionResult> getBookingByUserId(int userId)
        {
            var check = await _repository.getBookingByUserId(userId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getBookingByHotelId/{hotelId}")]
        public async Task<IActionResult> getBookingByHotelId(int hotelId)
        {
            var check = await _repository.getBookingByHotelId(hotelId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getBookingByRoomId/{roomId}")]
        public async Task<IActionResult> getBookingByRoomId(int roomId)
        {
            var check = await _repository.getBookingByRoomId(roomId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpPost("/createBooking")]
        public async Task<IActionResult> createBooking([FromBody] Booking booking)
        {
                await _repository.createBooking(booking);
                return StatusCode(201, "Create Successfully!");
        }

        [HttpPut("/deleteBooking/{bookingId}")]
        public async Task<IActionResult> deleteBooking(int bookingId)
        {
            var checkCart = await _repository.getBookingById(bookingId);
            if (checkCart == null)
            {
                return BadRequest("Booking is not exits");
            }

            await _repository.deleteBooking(bookingId);
            return StatusCode(200, "Delete Successfully!");
        }

        [HttpPut("/recoverBookingDeleted/{bookingId}")]
        public async Task<IActionResult> recoverBookingDeleted(int bookingId)
        {
            var checkCart = await _repository.getBookingById(bookingId);
            if (checkCart == null)
            {
                return BadRequest("Booking is not exits");
            }

            await _repository.recoverBookingDeleted(bookingId);
            return StatusCode(200, "Recover Successfully!");
        }
        [HttpGet("/getBookingBySupplierId")]
        public async Task<IActionResult> getBookingBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            if (supplierId != null && supplierId != 0)
            {
                var check = await _repository.getBookingBySupplierId(supplierId.Value);
                if (check == null)
                {
                    return NotFound("Not Found");
                }
                return Ok(check);
            }
            else
            {
                return BadRequest(403);
            }
        }
    }
}
