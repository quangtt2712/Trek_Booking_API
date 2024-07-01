using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IVoucherRepository
    {
        public Task<Voucher> createVoucher(Voucher voucher);
        public Task<Voucher> updateVouchher(Voucher voucher);
        public Task<int> deleteVoucher(int voucherId);
        public Task<Voucher> getVoucherById(int voucherId);
        public Task<IEnumerable<Voucher>> getVoucherByHotelId(int hotelId);
        public Task<bool> checkExitsName(string name);

    }
}
