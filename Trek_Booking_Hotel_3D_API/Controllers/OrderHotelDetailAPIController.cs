using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHotelDetailAPIController : ControllerBase
    {
        private readonly IOrderHotelDetailRepository _repository;

        public OrderHotelDetailAPIController(IOrderHotelDetailRepository repository)
        {
            _repository = repository;
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
    }
}
