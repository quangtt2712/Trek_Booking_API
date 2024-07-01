using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trek_Booking_DataAccess;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IRoleRepository _repository;
        public RoleController(ApplicationDBContext context, IRoleRepository repository)
        {
            _context = context;
            _repository = repository;
        }
        [HttpGet("/getRoles")]
        public async Task<IActionResult> getTours()
        {
            var c = await _repository.getRoles();
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }

        [HttpPost("/createRole")]
        public async Task<IActionResult> createRole([FromBody] Role role)
        {
            if (role == null)
            {
                return BadRequest();
            }
    
            var create = await _repository.createRole(role);
            return StatusCode(201, "Create Successfully!");
        }
        [HttpPut("/updateRole")]
        public async Task<IActionResult> updateRole([FromBody] Role role)
        {
            var check = await _repository.getRoleById(role.RoleId);
            if (check == null)
            {
                return BadRequest("Not found Role");
            }
            var update = await _repository.updateRole(role);
            return Ok(update);
        }
        [HttpDelete("/deleteRole/{roleId}")]
        public async Task<IActionResult> deleteRole(int roleId)
        {
            var check = await _repository.getRoleById(roleId);
            if (check == null)
            {
                return NotFound("Not found role");
            }
            await _repository.deleteRole(roleId);
            return StatusCode(200, "Delete Successfully!");
        }
    }
}
