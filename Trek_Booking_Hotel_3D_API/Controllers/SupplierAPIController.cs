using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Helper;
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
        private readonly AuthMiddleWare _authMiddleWare;

        public SupplierAPIController(ISupplierRepository repository, IAuthenticationUserRepository authenticationUserRepository,
            IJwtUtils jwtUtils, IRoleRepository roleRepository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authenticationUserRepository = authenticationUserRepository;
            _jwtUtils = jwtUtils;
            _roleRepository = roleRepository;
            _authMiddleWare = authMiddleWare;
        }
        [HttpPut("/changePasswordSupplier")]
        public async Task<IActionResult> changePasswordSupplier([FromBody] Supplier supplier)
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getSupplierbyId(supplierId.Value);
            if (check == null)
            {
                return BadRequest("Not found Supplier");
            }
            await _repository.changePasswordSupplier(supplier);
            return StatusCode(200, "Change Password Successfully!");
        }
        [HttpPost("/checkPasswordSupplier")]
        public async Task<IActionResult> checkPasswordSupplier([FromBody] Supplier supplier)
        {
            var checkPass = await _repository.checkPasswordSupplier(supplier);
            if (checkPass == null)
            {
                return BadRequest("Password is incorrect");
            }
            return Ok();
        }
        [HttpPut("/toggleSupplierStatus")]
        public async Task<IActionResult> ToggleStatus([FromBody] ToggleSupplierRequest request)
        {
            var result = await _repository.ToggleStatus(request);
            if (result is NotFoundResult)
            {
                return NotFound();
            }
            return NoContent();
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
        [HttpGet("/getSupplierbyId")]
        public async Task<IActionResult> getSupplierbyId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            if (supplierId != null && supplierId != 0)
            {
                var check = await _repository.getSupplierbyId(supplierId.Value);
                if (check == null)
                {
                    return NotFound("Not Found");
                }
                return Ok(check);
            }
            else
            {
                return BadRequest(403);
            }
                
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
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getSupplierbyId(supplierId.Value);
            if (check == null)
            {
                return BadRequest("Not found Supplier");
            }
            await _repository.updateSupplier(supplier);
            return StatusCode(200, "Update Successfully!");
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
