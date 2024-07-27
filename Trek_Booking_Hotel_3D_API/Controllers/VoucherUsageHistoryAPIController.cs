using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherUsageHistoryAPIController : ControllerBase
    {
        private readonly IVoucherUsageHistoryRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;

        public VoucherUsageHistoryAPIController(IVoucherUsageHistoryRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;
        }
        [HttpGet("/getVoucherUsageHistories")]
        public async Task<IActionResult> getVoucherUsageHistories()
        {
            var c = await _repository.getVoucherUsageHistories();
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }
        [HttpGet("/getVoucherUsageHistoryById/{UserVoucherId}")]
        public async Task<IActionResult> getVoucherUsageHistoryById(int UserVoucherId)
        {
            var check = await _repository.getVoucherUsageHistoryById(UserVoucherId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpPost("/createVoucherUsageHistory")]
        public async Task<IActionResult> createVoucherUsageHistory([FromBody] VoucherUsageHistory voucherUsageHistory)
        {
            if (voucherUsageHistory == null)
            {
                return BadRequest();
            }
            var create = await _repository.createVoucherUsageHistory(voucherUsageHistory);
            return StatusCode(201, "Create Successfully!");
        }


        [HttpGet("/getVoucherUsageHistoryByUserId")]
        public async Task<IActionResult> getVoucherUsageHistoryByUserId()
        {
            var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);
            if (userId != null && userId != 0)
            {
                var check = await _repository.getVoucherUsageHistoryByUserId(userId.Value);
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

        [HttpGet("/getVoucherUsageHistoryByUserId/{userId}")]
        public async Task<IActionResult> getVoucherUsageHistoryByUserId(int userId)
        {
            //var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);
            if (userId != null && userId != 0)
            {
                var check = await _repository.getVoucherUsageHistoryByUserId(userId);
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

    }
}
