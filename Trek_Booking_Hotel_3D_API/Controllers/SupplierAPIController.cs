using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Service;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierAPIController : ControllerBase
    {
        private readonly ISupplierRepository _repository;
        private readonly IAuthenticationUserRepository _authenticationUserRepository;
        private readonly IJwtUtils _jwtUtils;
        private readonly IRoleRepository _roleRepository;

        public SupplierAPIController(ISupplierRepository repository, IAuthenticationUserRepository authenticationUserRepository,
            IJwtUtils jwtUtils, IRoleRepository roleRepository)
        {
            _repository = repository;
            _authenticationUserRepository = authenticationUserRepository;
            _jwtUtils = jwtUtils;
            _roleRepository = roleRepository;
        }
        [HttpGet("/getSuppliers")]
        public async Task<IActionResult> getSuppliers()
        {
            var c = await _repository.getSuppliers();
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }
        [HttpGet("/getSupplierbyId/{supplierId}")]
        public async Task<IActionResult> getSupplierbyId(int supplierId)
        {
            var check = await _repository.getSupplierbyId(supplierId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpPost("/createSupplier")]
        public async Task<IActionResult> createSupplier([FromBody] Supplier supplier)
        {
            if (supplier == null)
            {
                return BadRequest();
            }
            else if (await _repository.checkExitsEmail(supplier.Email))
            {
                return BadRequest("SupplierEmail already exits");
            }
            var create = await _repository.createSupplier(supplier);
            return StatusCode(201, "Create Successfully!");
        }
        [HttpPut("/updateSupplier")]
        public async Task<IActionResult> updateSupplier([FromBody] Supplier supplier)
        {
            var check = await _repository.getSupplierbyId(supplier.SupplierId);
            if (check == null)
            {
                return BadRequest("Not found Supplier");
            }
            var update = await _repository.updateSupplier(supplier);
            return Ok(update);
        }
        [HttpDelete("/deleteSupplier/{supplierId}")]
        public async Task<IActionResult> deleteSupplier(int supplierId)
        {
            var check = await _repository.getSupplierbyId(supplierId);
            if (check == null)
            {
                return NotFound("Not found Supplier");
            }
            await _repository.deleteSupplier(supplierId);
            return StatusCode(200, "Delele Successfully!");
        }

        [HttpPost("/loginSupplier")]
        public async Task<IActionResult> loginSupplier([FromBody] Supplier supplier)
        {
            if (supplier == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _authenticationUserRepository.checkPasswordSupplier(supplier);
            if (result != null)
            {
                var checkBanned = await _repository.checkBannedSupplier(result);
                if (checkBanned.Status == false)
                {
                    return BadRequest("The account of supplier is banned!");
                }
                var role = await _roleRepository.getRoleById(result.RoleId);
                var token = _jwtUtils.GenerateTokenSupplier(result);
                return Ok(new SupplierResponse()
                {
                    IsAuthSuccessful = true,
                    ToKen = token,
                    Supplier = new Supplier()
                    {
                        SupplierName = result.SupplierName,
                        SupplierId = result.SupplierId,
                        Email = result.Email,
                        Phone = result.Phone,
                        RoleId = result.RoleId,
                    },
                    SupplierName = result.SupplierName,
                    RoleId = result.RoleId,
                    RoleName = role?.RoleName
                });
            }
            else
            {
                return Unauthorized(new UserResponse
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Email or password is not correct!"
                });
            }
            return StatusCode(200);
        }
        [HttpPost("/registerSupplier")]
        public async Task<IActionResult> RegisterSupplier([FromBody] Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _repository.createSupplier(supplier);
            return StatusCode(200);
        }
    }
}
