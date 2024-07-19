using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentAPIController : ControllerBase
    {
        private readonly ICommentRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;
        public CommentAPIController(ICommentRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;
        }


        [HttpGet("/checkFeedback/{orderHotelHeaderId}")]
        public async Task<IActionResult> CheckFeedback(int orderHotelHeaderId)
        {
            var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);

            if (userId != null && userId != 0)
            {
                var hasFeedback = await _repository.checkFeedBack(orderHotelHeaderId, userId.Value);
                return Ok(hasFeedback);
            }
            else
            {
                return BadRequest(403);
            }

        }
        [HttpPost("/createComment")]
        public async Task<IActionResult> createComment([FromBody] Comment comment)
        {
            var create = await _repository.createComment(comment);
            return StatusCode(201, "Create Successfully!");
        }

        [HttpGet("/getCommentByHotelId/{hotelId}")]
        public async Task<IActionResult> getCommentByHotelId(int hotelId)
        {

            var comments = await _repository.getCommentByHotelId(hotelId);
            if (comments == null)
            {
                return NotFound("Not Found"); // Return OK with null data if no comments found
            }
            return Ok(comments);
        }

        [HttpGet("/getCommentByOrderHotelHeaderId/{OrderHotelHeaderId}")]
        public async Task<IActionResult> getCommentByOrderHotelHeaderId(int OrderHotelHeaderId)
        {

            var comments = await _repository.getCommentByOrderHotelHeaderId(OrderHotelHeaderId);
            if (comments == null)
            {
                return NotFound("Not Found"); // Return OK with null data if no comments found
            }
            return Ok(comments);
        }



        [HttpGet("/getCommentByUserId")]
        public async Task<IActionResult> getCommentByUserId()
        {
            var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);
          
            if (userId != null && userId != 0)
            {
                var comments = await _repository.getCommentByUserId(userId.Value);
                if (comments == null)
                {
                    return NotFound("Not Found");
                }
                return Ok(comments);
            }
            else
            {
                return BadRequest(403);
            }
        }

    }
}
