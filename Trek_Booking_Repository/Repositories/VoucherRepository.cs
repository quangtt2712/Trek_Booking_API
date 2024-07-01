using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Repository.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {

        private readonly ApplicationDBContext _context;

        public VoucherRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Voucher> createVoucher(Voucher voucher)
        {
            _context.vouchers.Add(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }

        public async Task<int> deleteVoucher(int voucherId)
        {
            var deleteVoucher = await _context.vouchers.FirstOrDefaultAsync(t => t.VoucherId == voucherId);
            if (deleteVoucher != null)
            {
                deleteVoucher.VoucherStatus = false;
                _context.vouchers.Update(deleteVoucher);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<Voucher>> getVoucherByHotelId(int hotelId)
        {
            var vouchers = await _context.vouchers.Where(t => t.HotelId == hotelId).ToListAsync();
            return vouchers;
        }

        public async Task<Voucher> getVoucherById(int voucherId)
        {
            var voucher = await _context.vouchers.FirstOrDefaultAsync(t => t.VoucherId == voucherId);
            return voucher;
        }

        public async Task<bool> checkExitsName(string name)
        {
            var check = await _context.vouchers.AnyAsync(t => t.VoucherCode == name);
            return check;
        }
        public async Task<Voucher> updateVouchher(Voucher voucher)
        {
            var findvoucher = await _context.vouchers.FirstOrDefaultAsync(t => t.VoucherId == voucher.VoucherId);
            if (findvoucher != null)
            {
                findvoucher.VoucherCode = voucher.VoucherCode;
                findvoucher.AvailableDate = voucher.AvailableDate;
                findvoucher.ExpireDate = voucher.ExpireDate;
                findvoucher.VoucherQuantity = voucher.VoucherQuantity;
                findvoucher.DiscountPercent = voucher.DiscountPercent;
                findvoucher.VoucherStatus = voucher.VoucherStatus;

                _context.vouchers.Update(findvoucher);
                await _context.SaveChangesAsync();
                return findvoucher;
            }
            return null;
        }
    }
}

