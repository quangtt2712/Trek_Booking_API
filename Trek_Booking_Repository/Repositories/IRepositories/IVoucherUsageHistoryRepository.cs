using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IVoucherUsageHistoryRepository
    {
        public Task<VoucherUsageHistory> createVoucherUsageHistory(VoucherUsageHistory voucherUsageHistory);        
        public Task<VoucherUsageHistory> getVoucherUsageHistoryById(int UserVoucherId);
        public Task<IEnumerable<VoucherUsageHistory>> getVoucherUsageHistories();

        public Task<IEnumerable<VoucherUsageHistory>> getVoucherUsageHistoryByUserId(int userId);
    }
}
