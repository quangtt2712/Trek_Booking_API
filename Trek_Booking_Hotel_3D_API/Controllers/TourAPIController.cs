using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourAPIController : ControllerBase
    {
        private readonly ITourRepository _repository;

        public TourAPIController(ITourRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("/getTours")]
        public async Task<IActionResult> getTours()
        {
            var c = await _repository.getTours();
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }
        [HttpGet("/getTourById/{tourId}")]
        public async Task<IActionResult> getTourById(int tourId)
        {
            var check = await _repository.getTourById(tourId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getTourBySupplierId/{supplierId}")]
        public async Task<IActionResult> getTourBySupplierId(int supplierId)
        {
            var check = await _repository.getTourBySupplierId(supplierId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpPost("/createTour")]
        public async Task<IActionResult> createTour([FromBody] Tour tour)
        {
            if (tour == null)
            {
                return BadRequest();
            }
            else if (await _repository.checkExitsName(tour.TourName))
            {
                return BadRequest("TourName already exits");
            }
            var create = await _repository.createTour(tour);
            return StatusCode(201, "Create Successfully!");
        }
        [HttpPut("/updateTour")]
        public async Task<IActionResult> updateTour([FromBody] Tour tour)
        {
            var check = await _repository.getTourById(tour.TourId);
            if (check == null)
            {
                return BadRequest("Not found Tour");
            }
            var update = await _repository.updateTour(tour);
            return Ok(update);
        }
        [HttpDelete("/deleteTour/{tourId}")]
        public async Task<IActionResult> deleteTour(int tourId)
        {
            var check = await _repository.getTourById(tourId);
            if (check == null)
            {
                return NotFound("Not found Hotel");
            }
            await _repository.deleteTour(tourId);
            return StatusCode(200, "Delele Successfully!");
        }
    }
}
