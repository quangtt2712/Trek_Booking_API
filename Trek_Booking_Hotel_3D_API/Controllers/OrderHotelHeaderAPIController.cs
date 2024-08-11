using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHotelHeaderAPIController : ControllerBase
    {
        private readonly IOrderHotelHeaderRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;

        public OrderHotelHeaderAPIController(IOrderHotelHeaderRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;
        }
        [HttpPut("/updateOrderHotelHeader")]
        public async Task<IActionResult> updateOrderHotelHeader(OrderHotelHeader orderHotelHeader)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var check = await _repository.getOrderHotelHeaderByUserId(orderHotelHeader.Id);
            if (check == null)
            {
                return BadRequest("Not found Order Hotel Header");
            }
            var update = await _repository.updateOrderHotelHeader(orderHotelHeader);
            return Ok(new { message = "Update successful", data = update });
        }

        [HttpPut("/toggleOrderHotelHeaderStatus")]
        public async Task<IActionResult> toggleOrderHotelHeaderStatus([FromBody] ToggleOrderHotelHeaderRequest request)
        {
            var result = await _repository.ToggleStatus(request);
            if (result is NotFoundResult)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet("/getOrderHotelHeaderBySupplierIdAdmin/{supplierId}")]
        public async Task<IActionResult> getOrderHotelHeaderBySupplierId(int supplierId)
        {
            var check = await _repository.getOrderHotelHeaderBySupplierId(supplierId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getOrderHotelHeaderByUserId")]
        public async Task<IActionResult> getOrderHotelHeaderByUserId()
        {
            var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);
            var check = await _repository.getOrderHotelHeaderByUserId(userId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getOrderHotelHeaderBySupplierId")]
        public async Task<IActionResult> getOrderHotelHeaderBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getOrderHotelHeaderBySupplierId(supplierId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        //[HttpGet("/getRevenueYearBySupplierId")]
        //public async Task<IActionResult> getRevenueYearBySupplierId()
        //{
        //    var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
        //    var check = await _repository.getRevenueYearBySupplierId(supplierId.Value);
        //    if (check == null)
        //    {
        //        return NotFound("Not Found");
        //    }
        //    return Ok(check);
        //}

        [HttpGet("/countTotalOrderHotelBySupplierId")]
        public async Task<int> countTotalOrderHotelBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.countTotalOrderHotelBySupplierId(supplierId.Value);
            return check;
        }


        [HttpGet("/getPercentChangeFromLastWeek")]
        public async Task<double> getPercentChangeFromLastWeek()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getPercentChangeFromLastWeek(supplierId.Value);
            return check;
        }

        [HttpGet("/getTotalRevenueHotelBySupplierId")]
        public async Task<decimal> getTotalRevenueHotelBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getTotalRevenueHotelBySupplierId(supplierId.Value);
            return check;
        }

        [HttpGet("/getPercentChangeRevenueFromLastWeek")]
        public async Task<decimal> getPercentChangeRevenueFromLastWeek()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getPercentChangeRevenueFromLastWeek(supplierId.Value);
            return check;
        }
        [HttpGet("/getCurrentWeekRevenueHotelBySupplierId")]
        public async Task<IActionResult> getCurrentWeekRevenueHotelBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getCurrentWeekRevenueHotelBySupplierId(supplierId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getCurrentMonthOfYearRevenueHotelBySupplierId")]
        public async Task<IActionResult> getCurrentMonthOfYearRevenueHotelBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getCurrentMonthOfYearRevenueHotelBySupplierId(supplierId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getRevenueQuarterOfYearHotelBySupplierId")]
        public async Task<IActionResult> getRevenueQuarterOfYearHotelBySupplierId(int year)
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getRevenueQuarterOfYearHotelBySupplierId(supplierId.Value, year);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getRevenueHotelBySupplierIdAndDateRange")]
        public async Task<IActionResult> getRevenueHotelBySupplierIdAndDateRange(DateTime startDate, DateTime endDate)
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);

            if (endDate < startDate || (endDate - startDate).TotalDays > 31)
            {
                return BadRequest("The date range must be within one month.");
            }

            try
            {
                var revenue = await _repository.getRevenueHotelBySupplierIdAndDateRange(supplierId.Value, startDate, endDate);
                return Ok(revenue);
            }
            catch (Exception ex)
            {
                return (IActionResult)ex;
            }
        }

     
        [HttpGet("/getRevenueHotelMonthToYearBySupplierId")]
        public async Task<IActionResult> getRevenueHotelMonthToYearBySupplierId(int year)
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getRevenueHotelMonthToYearBySupplierId(supplierId.Value, year);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
    }
}
