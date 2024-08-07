using Microsoft.AspNetCore.Http;
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
    public class UserAPIController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IAuthenticationUserRepository _authenticationUserRepository;
        private readonly IJwtUtils _jwtUtils;
        private readonly IRoleRepository _roleRepository;
        private readonly AuthMiddleWare _authMiddleWare;

        public UserAPIController(IUserRepository repository, IAuthenticationUserRepository authenticationUserRepository,
            IJwtUtils jwtUtils, IRoleRepository roleRepository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authenticationUserRepository = authenticationUserRepository;
            _jwtUtils = jwtUtils;
            _roleRepository = roleRepository;
            _authMiddleWare = authMiddleWare;
        }

        [HttpPut("/changePasswordUser")]
        public async Task<IActionResult> changePasswordUser([FromBody] User user)
        {
            var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);
            var check = await _repository.getUserById(userId.Value);
            if (check == null)
            {
                return BadRequest("Not found Supplier");
            }
            await _repository.changePasswordUser(user);
            return StatusCode(200, "Change Password Successfully!");
        }
        [HttpPost("/checkPasswordUser")]
        public async Task<IActionResult> checkPasswordUser([FromBody] User user)
        {
            var checkPass = await _repository.checkPasswordUser(user);
            if (checkPass == null)
            {
                return BadRequest("Password is incorrect");
            }
            return Ok();
        }

        [HttpPut("/toggleUserStatus")]
        public async Task<IActionResult> ToggleStatus([FromBody] ToggleUserRequest request)
        {
            var result = await _repository.ToggleStatus(request);
            if (result is NotFoundResult)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet("/getUsers")]
        public async Task<IActionResult> getUsers()
        {
            var c = await _repository.getUsers();
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }
        [HttpGet("/getUserById")]
        public async Task<IActionResult> getUserById()
        {
            var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);
            if (userId != null && userId != 0)
            {
                var check = await _repository.getUserById(userId.Value);
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

        [HttpGet("/getUserByRoleId/{roleId}")]
        public async Task<IActionResult> getUserByRoleId(int roleId)
        {
            var check = await _repository.getUserByRoleId(roleId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpPost("/createUser")]
        public async Task<IActionResult> createUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            else if (await _repository.checkExitsEmail(user.Email))
            {
                return BadRequest("Email already exits");
            }
            var create = await _repository.createUser(user);
            return StatusCode(201, "Create Successfully!");
        }

        [HttpPut("/deleteUser/{userId}")]
        public async Task<IActionResult> deleteUser(int userId)
        {
            var check = await _repository.getUserById(userId);
            if (check == null)
            {
                return NotFound("Not found User");
            }
            await _repository.deleteUser(userId);
            return StatusCode(200, "Delele Successfully!");
        }
        [HttpPut("/recoverUserDeleted/{userId}")]
        public async Task<IActionResult> recoverUserDeleted(int userId)
        {
            var check = await _repository.getUserById(userId);
            if (check == null)
            {
                return NotFound("Not found User");
            }
            await _repository.recoverUserDeleted(userId);
            return StatusCode(200, "Recover Successfully!");
        }
        [HttpPost("/loginAdmin")]
        public async Task<IActionResult> LoginAdmin([FromBody] User user)
        {
            if (user == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _authenticationUserRepository.checkPasswordClient(user);
            if (result != null)
            {
                var checkBanned = await _repository.checkBannedUser(result);
                if (checkBanned.Status == false)
                {
                    return BadRequest("The account of user is banned!");
                }

                var isAdmin = await _repository.checkIsAdmin(result);
                if (!isAdmin)
                {
                    return BadRequest("The account is not admin!");
                }

                var role = await _roleRepository.getRoleById(result.RoleId);
                return Ok(new UserResponse()
                {
                    IsAuthSuccessful = true,
                    UserName = result.UserName,
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
        }

        [HttpPost("/loginClient")]
        public async Task<IActionResult> loginClient([FromBody] User user)
        {
            if (user == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _authenticationUserRepository.checkPasswordClient(user);
            if (result != null)
            {
                var checkBanned = await _repository.checkBannedUser(result);
                if (checkBanned.Status == false)
                {
                    return BadRequest("The account of user is banned!");
                }
                var role = await _roleRepository.getRoleById(result.RoleId);
                var token = _jwtUtils.GenerateTokenClient(result);
                return Ok(new UserResponse()
                {
                    IsAuthSuccessful = true,
                    ToKen = token,
                    UserName = result.UserName,
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
        [HttpPost("/registerClient")]
        public async Task<IActionResult> RegisterClient([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _repository.createUser(user);
            return StatusCode(200);
        }

        [HttpPut("/updateUser")]
        public async Task<IActionResult> updateUser([FromBody] User user)
        {
            var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);
            var check = await _repository.getUserById(userId.Value);
            if (check == null)
            {
                return NotFound("Not found User");
            }
            await _repository.updateUser(user);
            return StatusCode(200, "Update Successfully!");
        }
    }
}
