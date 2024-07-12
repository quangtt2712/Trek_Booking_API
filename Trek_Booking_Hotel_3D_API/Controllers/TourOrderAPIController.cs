using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourOrderAPIController : ControllerBase
    {
        private readonly ITourOrderRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;

        public TourOrderAPIController(ITourOrderRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;
        }

        [HttpGet("/getTourOrders")]
        public async Task<IActionResult> getTourOrders()
        {
            var c = await _repository.getTourOrders();
            if (c != null)
            {
                return Ok(c);
            }
            return NotFound("No data");
        }

        [HttpGet("/getTourOrderById/{tourOrderId}")]
        public async Task<IActionResult> getTourOrderById(int tourOrderId)
        {
            var check = await _repository.getTourOrderById(tourOrderId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getTourOrderByUserId")]
        public async Task<IActionResult> getTourOrderByUserId()
        {
            var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);
            var check = await _repository.getTourOrderByUserId(userId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getTourOrderByTourId/{tourId}")]
        public async Task<IActionResult> getTourOrderByTourId(int tourId)
        {
            var check = await _repository.getTourOrderByTourId(tourId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }


        [HttpGet("/getTourOrderBySupplierId/{supplierId}")]
        public async Task<IActionResult> getTourOrderBySupplierId(int supplierId)
        {
            var check = await _repository.getTourOrderBySupplierId(supplierId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }


        [HttpPost("/createTourOrder")]
        public async Task<IActionResult> createTourOrder([FromBody] TourOrder tourOrder)
        {
          
                await _repository.createTourOrder(tourOrder);
                return StatusCode(201, "Create Successfully!");
           
        }

        [HttpPut("/deleteTourOrder")]
        public async Task<IActionResult> deleteTourOrder(TourOrder tourOrder)
        {
            var checkCart = await _repository.getTourOrderById(tourOrder.TourOrderId);
            if (checkCart == null)
            {
                return BadRequest("TourOrder is not exits");
            }

            await _repository.deleteTourOder(tourOrder);
            return StatusCode(200, "Delete Successfully!");
        }
    }
}
