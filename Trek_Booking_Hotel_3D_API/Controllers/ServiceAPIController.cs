using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceAPIController : ControllerBase
    {
        private readonly IServiceRepository _repository;

        public ServiceAPIController(IServiceRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("/getServices")]
        public async Task<IActionResult> getServices()
        {
            var c = await _repository.getServices();
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }
        [HttpGet("/getServicebyId/{serviceId}")]
        public async Task<IActionResult> getServicebyId(int serviceId)
        {
            var check = await _repository.getServicebyId(serviceId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpPost("/createService")]
        public async Task<IActionResult> createService([FromBody] Services service)
        {
            if (service == null)
            {
                return BadRequest();
            }
            else if (await _repository.checkExitsName(service.ServiceName))
            {
                return BadRequest("ServiceName already exits");
            }
            var create = await _repository.createService(service);
            return StatusCode(201, "Create Successfully!");
        }
        [HttpPut("/updateService")]
        public async Task<IActionResult> updateService([FromBody] Services service)
        {
            var check = await _repository.getServicebyId(service.ServiceId);
            if (check == null)
            {
                return BadRequest("Not found Service");
            }
            var update = await _repository.updateService(service);
            return Ok(update);
        }
        [HttpDelete("/deleteService/{serviceId}")]
        public async Task<IActionResult> deleteService(int serviceId)
        {
            var check = await _repository.getServicebyId(serviceId);
            if (check == null)
            {
                return NotFound("Not found Service");
            }
            await _repository.deleteService(serviceId);
            return StatusCode(200, "Delele Successfully!");
        }
    }
}
