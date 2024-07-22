using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderTourHeaderAPIController : ControllerBase
    {
        private readonly IOrderTourHeaderRepository _repository;
        private readonly AuthMiddleWare _authMiddleWare;

        public OrderTourHeaderAPIController(IOrderTourHeaderRepository repository, AuthMiddleWare authMiddleWare)
        {
            _repository = repository;
            _authMiddleWare = authMiddleWare;
        }
        [HttpPut("/updateOrderTourHeader")]
        public async Task<IActionResult> updateOrderTourHeader(OrderTourHeader orderTourHeader)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var check = await _repository.getOrderTourHeaderByUserId(orderTourHeader.Id);
            if (check == null)
            {
                return BadRequest("Not found Order Tour Header");
            }
            var update = await _repository.updateOrderTourHeader(orderTourHeader);
            return Ok(new { message = "Update successful", data = update });
        }
         [HttpPut("/toggleOrderTourHeaderStatus")]
        public async Task<IActionResult> toggleOrderTourHeaderStatus([FromBody] ToggleOrderTourHeaderRequest request)
        {
            var result = await _repository.ToggleStatus(request);
            if (result is NotFoundResult)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet("/getOrderTourHeaderBySupplierIdAdmin/{supplierId}")]
        public async Task<IActionResult> getOrderTourHeaderBySupplierId(int supplierId)
        {
            var check = await _repository.getOrderTourHeaderBySupplierId(supplierId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getOrderTourHeaderByUserId")]
        public async Task<IActionResult> getOrderTourHeaderByUserId()
        {
            var userId = _authMiddleWare.GetUserIdFromToken(HttpContext);
            var check = await _repository.getOrderTourHeaderByUserId(userId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getOrderTourHeaderBySupplierId")]
        public async Task<IActionResult> getOrderTourHeaderBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getOrderTourHeaderBySupplierId(supplierId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getRevenueTourBySupplierId")]
        public async Task<IActionResult> getRevenueTourBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getRevenueTourBySupplierId(supplierId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/countTotalOrderTourBySupplierId")]
        public async Task<IActionResult> countTotalOrderTourBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.countTotalOrderTourBySupplierId(supplierId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getPercentChangeTourFromLastWeek")]
        public async Task<IActionResult> getPercentChangeTourFromLastWeek()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getPercentChangeTourFromLastWeek(supplierId.Value, DateTime.Now);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getTotalRevenueTourBySupplierId")]
        public async Task<IActionResult> getTotalRevenueTourBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getTotalRevenueTourBySupplierId(supplierId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getPercentChangeRevenueTourFromLastWeek")]
        public async Task<IActionResult> getPercentChangeRevenueTourFromLastWeek()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getPercentChangeRevenueTourFromLastWeek(supplierId.Value, DateTime.Now);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }


        [HttpGet("/getCurrentWeekRevenueTourBySupplierId")]
        public async Task<IActionResult> getCurrentWeekRevenueTourBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getCurrentWeekRevenueTourBySupplierId(supplierId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getCurrentMonthOfYearRevenueTourBySupplierId")]
        public async Task<IActionResult> getCurrentMonthOfYearRevenueTourBySupplierId()
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getCurrentMonthOfYearRevenueTourBySupplierId(supplierId.Value);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getRevenueQuarterOfYearTourBySupplierId")]
        public async Task<IActionResult> getRevenueQuarterOfYearTourBySupplierId(int year)
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getRevenueQuarterOfYearTourBySupplierId(supplierId.Value, year);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getRevenueTourBySupplierIdAndDateRange")]
        public async Task<IActionResult> getRevenueTourBySupplierIdAndDateRange(DateTime startDate, DateTime endDate)
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);

            if (endDate < startDate || (endDate - startDate).TotalDays > 31)
            {
                return BadRequest("The date range must be within one month.");
            }

            try
            {
                var revenue = await _repository.getRevenueTourBySupplierIdAndDateRange(supplierId.Value, startDate, endDate);
                return Ok(revenue);
            }
            catch (Exception ex)
            {
                return (IActionResult)ex;
            }
        }

        [HttpGet("/getRevenueTourMonthToYearBySupplierId")]
        public async Task<IActionResult> getRevenueTourMonthToYearBySupplierId(int year)
        {
            var supplierId = _authMiddleWare.GetSupplierIdFromToken(HttpContext);
            var check = await _repository.getRevenueTourMonthToYearBySupplierId(supplierId.Value, year);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

    }
}
