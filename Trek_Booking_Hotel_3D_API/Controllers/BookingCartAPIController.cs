using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingCartAPIController : ControllerBase
    {
        private readonly IBookingCartRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;
        public BookingCartAPIController(IBookingCartRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;
        }

        [HttpGet("/getBookingCarts")]
        public async Task<IActionResult> getBookingCarts()
        {
            var c = await _repository.getBookingCarts();
            return Ok(c);
        }

        [HttpGet("/getBookingCartbyId/{bookingCartId}")]
        public async Task<IActionResult> getBookingCartbyId(int bookingCartId)
        {
            var check = await _repository.getBookingCartbyId(bookingCartId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getBookingCartbyUserId")]
        public async Task<IActionResult> getBookingCartbyUserId()

        {
            var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);
            if (userId != null && userId != 0)
            {
                var check = await _repository.getBookingCartbyUserId(userId.Value);
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

        [HttpGet("/getBookingCartbyHotelId/{hotelId}")]
        public async Task<IActionResult> getBookingCartbyHotelId(int hotelId)
        {
            var check = await _repository.getBookingCartbyHotelId(hotelId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getBookingCartbyRoomId/{roomId}")]
        public async Task<IActionResult> getBookingCartbyRoomId(int roomId)
        {
            var check = await _repository.getBookingCartbyRoomId(roomId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpPost("/createBookingCart")]
        public async Task<IActionResult> CreateBookingCart([FromBody] BookingCart bookingCart)
        {
            var bookingCartExists = await _repository.CheckBookingCartExists(bookingCart.UserId, bookingCart.RoomId);
            if (bookingCartExists)
            {
                return BadRequest("BookingCart already exists for the specified userId and roomId.");
            }

            await _repository.createBookingCart(bookingCart);
            return StatusCode(201, "Create Successfully!");
        }

        [HttpPut("/updateBookingCart")]
        public async Task<IActionResult> updateBookingCart([FromBody] BookingCart bookingCart)
        {
            var checkCart = await _repository.getBookingCartbyId(bookingCart.BookingCartId);
            if (checkCart == null)
            {
                return BadRequest("BookingCart is not exits");
            }

            await _repository.updateBookingCart(bookingCart);
            return StatusCode(200, "Update Successfully!");
        }


        [HttpDelete("/deleteBookingCart/{bookingCartId}")]
        public async Task<IActionResult> deleteBookingCart(int bookingCartId)
        {
            var check = await _repository.getBookingCartbyId(bookingCartId);
            if (check == null)
            {
                return BadRequest("null empty or error input");
            }
            await _repository.deleteBookingCart(bookingCartId);
            return StatusCode(200, "Delele Successfully!");

        }
    }
}
