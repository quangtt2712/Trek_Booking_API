using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class DashBoardAdminAPIController : ControllerBase
    {
        private readonly IDashBoardAdminRepository _repository;

        public DashBoardAdminAPIController(IDashBoardAdminRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("/countAllSupplier")]
        public async Task<IActionResult> countAllSupplier()
        {
            var check = await _repository.countAllSupplier();
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/countAllUser")]
        public async Task<IActionResult> countAllUser()
        {
            var check = await _repository.countAllUser();
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

   



        [HttpGet("/countAllBookingRoom")]
        public async Task<IActionResult> countAllBookingRoom()
        {
            var check = await _repository.countAllBookingRoom();
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/countAllBookingTour")]
        public async Task<IActionResult> countAllBookingTour()
        {
            var check = await _repository.countAllBookingTour();
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }


        [HttpGet("/getSupplierReportsInCurrentMonth")]
        public async Task<IActionResult> getSupplierReportsInCurrentMonth()
        {
            var check = await _repository.getSupplierReportsInCurrentMonth();
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getSupplierReportsInDateRange")]
        public async Task<IActionResult> getSupplierReportsInDateRange(DateTime startDate, DateTime endDate)
        {
            var check = await _repository.getSupplierReportsInDateRange(startDate, endDate);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }


        [HttpGet("/getTopHotelOfSupplierInWeek")]
        public async Task<IActionResult> getTopHotelOfSupplierInWeek()
        {
            var today = DateTime.Today;
            var startDate = today.AddDays(-(int)today.DayOfWeek);
            var endDate = startDate.AddDays(7);
            var check = await _repository.getTopHotelOfSupplierInWeek(startDate, endDate);
            if (!check.Any())
            { 
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getTopHotelOfSupplierDateRange")]
        public async Task<IActionResult> getTopHotelOfSupplierDateRange(DateTime startDate, DateTime endDate)
        {
            var check = await _repository.getTopHotelOfSupplierDateRange(startDate, endDate);
            if (!check.Any())
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }


        [HttpGet("/getTopTourOfSupplierInWeek")]
        public async Task<IActionResult> getTopTourOfSupplierInWeek()
        {
            var today = DateTime.Today;
            var startDate = today.AddDays(-(int)today.DayOfWeek);
            var endDate = startDate.AddDays(7);
            var check = await _repository.getTopTourOfSupplierInWeek(startDate, endDate);
            if (!check.Any())
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getTopTourOfSupplierDateRange")]
        public async Task<IActionResult> getTopTourOfSupplierDateRange(DateTime startDate, DateTime endDate)
        {
            var check = await _repository.getTopTourOfSupplierDateRange(startDate, endDate);
            if (!check.Any())
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }



        [HttpGet("/getNewUserRegister")]
        public async Task<IActionResult> getNewUserRegister(DateTime startDate, DateTime endDate)
        {
            var check = await _repository.getNewUserRegister(startDate, endDate);
            if (!check.Any())
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }


        [HttpGet("/getNewSupplierRegister")]
        public async Task<IActionResult> getNewSupplierRegister(DateTime startDate, DateTime endDate)
        {
            var check = await _repository.getNewSupplierRegister(startDate, endDate);
            if (!check.Any())
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getRevenueTourByAdmin")]
        public async Task<IActionResult> getRevenueTourByAdmin(DateTime startDate, DateTime endDate)
        {
            var check = await _repository.getRevenueTourByAdmin(startDate, endDate);
            if (!check.Any())
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpGet("/getRevenueHotelByAdmin")]
        public async Task<IActionResult> getRevenueHotelByAdmin(DateTime startDate, DateTime endDate)
        {
            var check = await _repository.getRevenueHotelByAdmin(startDate, endDate);
            if (!check.Any())
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
        [HttpGet("/getRevenueAdminInMonth")]
        public async Task<IActionResult> getRevenueAdminInMonth()
        {
            var check = await _repository.getRevenueAdminInMonth();
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }


        [HttpGet("/getRevenueAdminInDateRange")]
        public async Task<IActionResult> getRevenueAdminInDateRange(DateTime startDate, DateTime endDate)
        {
            var check = await _repository.getRevenueAdminInDateRange(startDate, endDate);
            if (!check.Any())
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }
    }
}
