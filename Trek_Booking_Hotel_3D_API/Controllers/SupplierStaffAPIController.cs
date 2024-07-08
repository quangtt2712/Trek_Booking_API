using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Hotel_3D_API.Service;
using Trek_Booking_Repository.Repositories;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierStaffAPIController : ControllerBase
    {
        private readonly ISupplierStaffRepository _repository;
        private readonly IJwtUtils _jwtUtils;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthenticationUserRepository _authenticationUserRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly AuthMiddleWare _authMiddleWare;


        public SupplierStaffAPIController(ISupplierStaffRepository repository, IJwtUtils jwtUtils,
            IRoleRepository roleRepository, IAuthenticationUserRepository authenticationUserRepository, ISupplierRepository supplierRepository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _jwtUtils = jwtUtils;
            _roleRepository = roleRepository;
            _authenticationUserRepository = authenticationUserRepository;
            _supplierRepository = supplierRepository;
            _authMiddleWare = authMiddleWare;
        }
        [HttpGet("/getSupplierStaffBySupplierIdAdmin/{supplierId}")]
        public async Task<IActionResult> getSupplierStaffBySupplierIdAdmin(int supplierId)
        {

            var check = await _repository.getSupplierStaffBySupplierId(supplierId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);


        }
        [HttpPut("ToggleSupplierStaff")]
        public async Task<IActionResult> ToggleStatus([FromBody] ToggleSupplierStaffRequest request)
        {
            return await _repository.ToggleStatus(request);
        }
        [HttpGet("/getSupplierStaffs")]
        public async Task<IActionResult> getSupplierStaffs()
        {
            var staffs = await _repository.getSupplierStaffs();
            if (staffs == null)
            {
                return NotFound("Not Found");
            }
            return Ok(staffs);
        }
        [HttpGet("/getSupplierStaffbyId/{staffId}")]
        public async Task<IActionResult> getSupplierStaffbyId(int staffId)
        {
            var check = await _repository.getSupplierStaffbyId(staffId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getSupplierStaffBySupplierId")]
        public async Task<IActionResult> getSupplierStaffBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            if (supplierId != null && supplierId != 0)
            {
                var check = await _repository.getSupplierStaffBySupplierId(supplierId.Value);
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
        [HttpPost("/createSupplierStaff")]
        public async Task<IActionResult> createSupplier([FromBody] SupplierStaff supplierStaff)
        {
            if (supplierStaff == null)
            {
                return BadRequest();
            }
            else if (await _repository.checkExitsEmail(supplierStaff.StaffEmail))
            {
                return BadRequest("SupplierEmail already exits");
            }
            var create = await _repository.createSupplierStaff(supplierStaff);
            return StatusCode(201, "Create Successfully!");
        }
        [HttpPut("/updateSupplierStaff")]
        public async Task<IActionResult> updateSupplierStaff([FromBody] SupplierStaff supplierStaff)
        {
            var check = await _repository.getSupplierStaffbyId(supplierStaff.StaffId);
            if (check == null)
            {
                return BadRequest("Not found Supplier");
            }
            var update = await _repository.updateSupplierStaff(supplierStaff);
            return Ok(update);
        }
        [HttpDelete("/deleteSupplierStaff/{staffId}")]
        public async Task<IActionResult> deleteSupplierStaff(int staffId)
        {
            var check = await _repository.getSupplierStaffbyId(staffId);
            if (check == null)
            {
                return NotFound("Not found Supplier");
            }
            await _repository.deleteSupplierStaff(staffId);
            return StatusCode(200, "Delele Successfully!");
        }
        [HttpPost("/loginSupplierStaff")]
        public async Task<IActionResult> loginSupplierStaff([FromBody] SupplierStaff supplierStaff)
        {
            if (supplierStaff == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _authenticationUserRepository.checkPasswordSupplierStaff(supplierStaff);
            if (result != null)
            {
                var checkBanned = await _repository.checkBannedSupplierStaff(result);
                if (checkBanned.Status == false)
                {
                    return BadRequest("The account of supplier staff is banned!");
                }
                var supplier = await _supplierRepository.getSupplierbyId(result.SupplierId);
                var role = await _roleRepository.getRoleById(result.RoleId);
                var tokenStaff = _jwtUtils.GenerateTokenSupplierStaff(result);
                var tokenSupplier = _jwtUtils.GenerateTokenSupplier(supplier);
                return Ok(new SupplierStaffResponse()
                {
                    IsAuthSuccessful = true,
                    ToKen = tokenStaff,
                    TokenSupplier = tokenSupplier,
                    StaffName = result.StaffName,
                    RoleName = role?.RoleName
                });
            }
            else
            {
                return Unauthorized(new SupplierStaffResponse
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Email or password is not correct!"
                });
            }
            return StatusCode(200);
        }
    }
}