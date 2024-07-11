using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderTourDetailAPIController : ControllerBase
    {
        private readonly IOrderTourDetailRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;

        public OrderTourDetailAPIController(IOrderTourDetailRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;

        }

        [HttpGet("/GetOrderTourDetailByOrderTourHeaderId/{orderTourHeaderId}")]
        public async Task<IActionResult> GetOrderTourDetailByOrderTourHeaderId(int orderTourHeaderId)
        {
            var check = await _repository.GetOrderTourDetailByOrderTourHeaderId(orderTourHeaderId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getTop5TourOrders")]
        public async Task<IActionResult> getTop5TourOrders()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getTop5TourOrders(supplierId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }


        [HttpGet("/getTop5TourInWeek")]
        public async Task<IActionResult> getTop5TourInWeek()
        {
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getTop5TourInWeek(supplierId.Value, startDate, endDate);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

    }
}
