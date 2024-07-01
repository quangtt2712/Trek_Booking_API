using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_DataAccess;
using Microsoft.EntityFrameworkCore;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Repository.Repositories
{
    public class PaymentInfoRepository : IPaymentInforRepository
    {
        private readonly ApplicationDBContext _context;
        public PaymentInfoRepository(ApplicationDBContext context)
        {
            _context = context;
        }        

        public async Task<PaymentInformation> createPaymentInfor(PaymentInformation paymentInformation)
        {
            _context.paymentInformations.Add(paymentInformation);
            await _context.SaveChangesAsync();
            return paymentInformation;
        }       

        public async Task<PaymentInformation> getPaymentInforById(int paymentInforId)
        {
            var getPaymentInfor = await _context.paymentInformations.FirstOrDefaultAsync(t => t.PaymentInforId == paymentInforId);
            return getPaymentInfor;
        }

        public async Task<IEnumerable<PaymentInformation>> getPaymentInfors()
        {
            var paymentinfors = await _context.paymentInformations.ToListAsync();
            return paymentinfors;
        }

        public async Task<PaymentInformation> updatePaymentInfor(PaymentInformation paymentInformation)
        {
            var findPaymentInfor = await _context.paymentInformations.FirstOrDefaultAsync(t => t.PaymentInforId == paymentInformation.PaymentInforId);
            if (findPaymentInfor != null)
            {
                findPaymentInfor.PaymentMethod = paymentInformation.PaymentMethod;
                findPaymentInfor.CartNumber = paymentInformation.CartNumber;
                findPaymentInfor.TotalPrice = paymentInformation.TotalPrice;
                findPaymentInfor.PaymentFee = paymentInformation.PaymentFee;
                findPaymentInfor.PaidDate = paymentInformation.PaidDate;                
                _context.paymentInformations.Update(findPaymentInfor);
                await _context.SaveChangesAsync();
                return findPaymentInfor;
            }
            return null;
        }

        public async Task<IEnumerable<PaymentInformation>> getPaymentInforByUserId(int userId)
        {
            var check = await _context.paymentInformations.Where(t => t.UserId == userId).ToListAsync();
            return check;
        }
    }
}
