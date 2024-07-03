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

        [HttpGet("/getOrderHotelDetailByOrderHotelHeaderId/{orderTourHeaderId}")]
        public async Task<IActionResult> getOrderHotelDetailByOrderHotelHeaderId(int orderTourHeaderId)
        {
            var check = await _repository.getOrderHotelDetailByOrderHotelHeaderId(orderTourHeaderId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
    }
}
