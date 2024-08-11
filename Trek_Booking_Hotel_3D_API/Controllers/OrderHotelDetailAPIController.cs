using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHotelDetailAPIController : ControllerBase
    {
        private readonly IOrderHotelDetailRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;

        public OrderHotelDetailAPIController(IOrderHotelDetailRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;
        }
        
        [HttpGet("/getOrderHotelDetailByOrderHotelHeaderId/{orderHotelHeaderId}")]
        public async Task<IActionResult> getOrderHotelDetailByOrderHotelHeaderId(int orderHotelHeaderId)
        {
            var check = await _repository.getOrderHotelDetailByOrderHotelHeaderId(orderHotelHeaderId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getTop5RoomOrders")]
        public async Task<IActionResult> getTop5RoomOrders()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getTop5RoomOrders(supplierId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getTop5RoomInWeek")]
        public async Task<IActionResult> getTop5RoomInWeek()
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getTop5RoomInWeek(supplierId.Value, startOfWeek, endOfWeek);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getMostFrequentlyRoomBySupplierIdAndDateRange")]
        public async Task<IActionResult> getMostFrequentlyRoomBySupplierIdAndDateRange(DateTime startDate, DateTime endDate)
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getMostFrequentlyRoomBySupplierIdAndDateRange(supplierId.Value, startDate, endDate);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
    }
}
