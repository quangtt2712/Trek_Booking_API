using Microsoft.AspNetCore.Mvc;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Hotel_3D_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherAPIController : ControllerBase
    {
        private readonly IVoucherRepository _repository;

        public VoucherAPIController(IVoucherRepository repository)
        {
            _repository = repository;
        }


        [HttpPost("/createVoucher")]
        public async Task<IActionResult> createVoucher([FromBody] Voucher voucher)
        {
            if (voucher == null)
            {
                return BadRequest();
            }
            else if (await _repository.checkExitsName(voucher.VoucherCode))
            {
                return BadRequest("VoucherCode already exits");
            }
            var create = await _repository.createVoucher(voucher);
            return StatusCode(201, "Create Successfully!");
        }


        [HttpGet("/getVouchersByHotelId/{hotelId}")]
        public async Task<IActionResult> getVouchersByHotelId(int hotelId)
        {
            var check = await _repository.getVoucherByHotelId(hotelId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpPut("/updateVoucher")]
        public async Task<IActionResult> updateVoucher([FromBody] Voucher voucher)
        {
            var check = await _repository.getVoucherById(voucher.VoucherId);
            if (check == null)
            {
                return BadRequest("Not found Voucher");
            }
            var update = await _repository.updateVouchher(voucher);
            return Ok(update);
        }

        [HttpGet("/getVoucherById/{voucherId}")]
        public async Task<IActionResult> getVoucherbyId(int voucherId)
        {
            var check = await _repository.getVoucherById(voucherId);
            if (check == null)
            {
                return NotFound("Not Found");
            }
            return Ok(check);
        }

        [HttpPut("/deleteVoucher/{voucherId}")]
        public async Task<IActionResult> deleteVoucher(int voucherId)
        {
            var check = await _repository.getVoucherById(voucherId);
            if (check == null)
            {
                return NotFound("Not found Voucher");
            }
            await _repository.deleteVoucher(voucherId);
            return StatusCode(200, "Delele Successfully!");
        }




    }
}
