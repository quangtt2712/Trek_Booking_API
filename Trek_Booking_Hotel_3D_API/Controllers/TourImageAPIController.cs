using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourImageAPIController : ControllerBase
    {
        private readonly ITourImageRepository _repository;

        public TourImageAPIController(ITourImageRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("/getTourImages")]
        public async Task<IActionResult> getTourImages()
        {
            var c = await _repository.getTourImages();
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }
        [HttpGet("/getTourImageById/{tourImageId}")]
        public async Task<IActionResult> getTourImageById(int tourImageId)
        {
            var check = await _repository.getTourImageById(tourImageId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getTourImageByTourId/{tourId}")]
        public async Task<IActionResult> getTourImageByTourId(int tourId)
        {
            var check = await _repository.getTourImageByTourId(tourId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }   

        [HttpPost("/createTourImage")]
        public async Task<IActionResult> createTourImage([FromBody] TourImage tourImage)
        {
            if (tourImage == null)
            {
                return BadRequest();
            }
            var create = await _repository.createTourImage(tourImage);
            return StatusCode(201, "Create Successfully!");
        }
        [HttpPut("/updateTourImage")]
        public async Task<IActionResult> updateTour([FromBody] TourImage tourImage)
        {
            var check = await _repository.getTourImageById(tourImage.TourImageId);
            if (check == null)
            {
                return BadRequest("Not found Tour Image");
            }
            var update = await _repository.updateTourImage(tourImage);
            return Ok(update);
        }
        [HttpDelete("/deleteTourImage/{tourImageId}")]
        public async Task<IActionResult> deleteTourImage(int tourImageId)
        {
            var check = await _repository.getTourImageById(tourImageId);
            if (check == null)
            {
                return NotFound("Not found Hotel");
            }
            await _repository.deleteTourImage(tourImageId);
            return StatusCode(200, "Delele Successfully!");
        }
    }
}
