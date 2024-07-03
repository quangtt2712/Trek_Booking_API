using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderTourDetailAPIController : ControllerBase
    {
        private readonly IOrderTourDetailRepository _repository;

        public OrderTourDetailAPIController(IOrderTourDetailRepository repository)
        {
            _repository = repository;
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
    }
}
