using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartTourAPIController : ControllerBase
    {
        private readonly ICartTourRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;
        public CartTourAPIController(ICartTourRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;
        }

        [HttpGet("/getCartTours")]
        public async Task<IActionResult> getCartTours()
        {
            var c = await _repository.getCartTours();
            if (c == null)
            {
                return NotFound("No data");
            }
            return Ok(c);
        }

        [HttpGet("/getCartTourById/{cartTourId}")]
        public async Task<IActionResult> getCartTourById(int cartTourId)
        {
            var check = await _repository.getCartTourById(cartTourId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getCartTourByUserId")]
        public async Task<IActionResult> getCartTourByUserId()
        {
            var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);
            if (userId != null && userId != 0)
            {
                var check = await _repository.getCartTourByUserId(userId.Value);
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

        [HttpGet("/getCartTourByTourId/{tourId}")]
        public async Task<IActionResult> getCartTourByTourId(int tourId)
        {
            var check = await _repository.getCartTourByTourId(tourId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }


        [HttpPost("/createCartTour")]
        public async Task<IActionResult> createCartTour([FromBody] CartTour cartTour)
        {
            try
            {
                var cartTourExits = await _repository.checkCartTourExists(cartTour.UserId, cartTour.TourId);
                if (cartTourExits)
                {
                    return BadRequest("CartTour already exists for the specified userId and tourId");
                }

                await _repository.createCartTour(cartTour);
                return StatusCode(201, "Create Successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest("User or Tour not exits");
            }
        }

        [HttpPut("/updateCartTour")]
        public async Task<IActionResult> updateCartTour([FromBody] CartTour cartTour)
        {
            var checkCart = await _repository.getCartTourById(cartTour.CartTourId);
            if (checkCart == null)
            {
                return BadRequest("CartTour is not exits");
            }

            await _repository.updateCartTour(cartTour);
            return StatusCode(200, "Update Successfully!");
        }


        [HttpDelete("/deleteCartTour/{cartTourId}")]
        public async Task<IActionResult> deleteCartTour(int cartTourId)
        {
            var check = await _repository.getCartTourById(cartTourId);
            if (check == null)
            {
                return BadRequest("null empty or error input");
            }
            await _repository.deleteCartTour(cartTourId);
            return StatusCode(200, "Delele Successfully!");

        }
    }
}
