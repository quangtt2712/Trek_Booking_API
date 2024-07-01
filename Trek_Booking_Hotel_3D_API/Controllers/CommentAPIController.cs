using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentAPIController : ControllerBase
    {
        private readonly ICommentRepository _repository;
        public CommentAPIController(ICommentRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("/createComment")]
        public async Task<IActionResult> createComment([FromBody] Comment comment)
        {
            //if (comment == null)
            //{
            //    return BadRequest();
            //}
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



        [HttpGet("/getCommentByUserId/{userId}")]
        public async Task<IActionResult> getCommentByUserId(int userId)
        {
            var comments = await _repository.getCommentByUserId(userId);
            if(comments == null)
            {
                return NotFound("Not Found"); // Return OK with null data if no comments found
            }
            return Ok(comments);
        }

    }
}
