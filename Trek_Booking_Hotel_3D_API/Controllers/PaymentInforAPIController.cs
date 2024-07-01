using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentInforAPIController : ControllerBase
    {
        private readonly IPaymentInforRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;

        public PaymentInforAPIController(IPaymentInforRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;
        }
        [HttpGet("/getPaymentInfors")]
        public async Task<IActionResult> getPaymentInfors()
        {
            var c = await _repository.getPaymentInfors();
            if (c == null)
            {
                return NotFound("Not Found");
            }
            return Ok(c);
        }
        [HttpGet("/getPaymentInforById/{paymentInforId}")]
        public async Task<IActionResult> getPaymentInforById(int paymentInforId)
        {
            var check = await _repository.getPaymentInforById(paymentInforId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpPost("/createPaymentInfor")]
        public async Task<IActionResult> createPaymentInfor([FromBody] PaymentInformation paymentInformation)
        {
            if (paymentInformation == null)
            {
                return BadRequest();
            }            
            var create = await _repository.createPaymentInfor(paymentInformation);
            return StatusCode(201, "Create Successfully!");
        }
        [HttpPut("/updatePaymentInfor")]
        public async Task<IActionResult> updatePaymentInfor([FromBody] PaymentInformation paymentInformation)
        {
            var check = await _repository.getPaymentInforById(paymentInformation.PaymentInforId);
            if (check == null)
            {
                return BadRequest("Not found Payment Information");
            }
            var update = await _repository.updatePaymentInfor(paymentInformation);
            return Ok(update);
        }

        [HttpGet("/getPaymentInforByUserId")]
        public async Task<IActionResult> getPaymentInforByUserId()
        {
            var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);
            var check = await _repository.getPaymentInforByUserId(userId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
    }
}
