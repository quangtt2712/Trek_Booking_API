using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Trek_Booking_Repository.Repositories
{
    public class VoucherUsageHistoryRepository : IVoucherUsageHistoryRepository
    {
        private readonly ApplicationDBContext _context;
        public VoucherUsageHistoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }       

        public async Task<VoucherUsageHistory> createVoucherUsageHistory(VoucherUsageHistory voucherUsageHistory)
        {
            _context.voucherUsageHistories.Add(voucherUsageHistory);
            await _context.SaveChangesAsync();
            return voucherUsageHistory;
        }        

        public async Task<VoucherUsageHistory> getVoucherUsageHistoryById(int UserVoucherId)
        {
            var getVoucherUsageHistory = await _context.voucherUsageHistories.FirstOrDefaultAsync(t => t.UserVoucherId == UserVoucherId);
            return getVoucherUsageHistory;
        }

        public async Task<IEnumerable<VoucherUsageHistory>> getVoucherUsageHistories()
        {
            var voucherUsageHistories = await _context.voucherUsageHistories.ToListAsync();
            return voucherUsageHistories;
        }

        public async Task<IEnumerable<VoucherUsageHistory>> getVoucherUsageHistoryByUserId(int userId)
        {
            var result = await _context.voucherUsageHistories
                                       .Include(t => t.OrderHotelHeader)
                                       .Include(v => v.Voucher)
                                       .Where(t => t.UserId == userId && t.OrderHotelHeader != null)
                                       .ToListAsync();
            return result;
        }

    }
}
