using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IPaymentInforRepository
    {
        public Task<PaymentInformation> createPaymentInfor(PaymentInformation paymentInformation);
        public Task<PaymentInformation> updatePaymentInfor(PaymentInformation paymentInformation);        
        public Task<PaymentInformation> getPaymentInforById(int paymentInforId);
        public Task<IEnumerable<PaymentInformation>> getPaymentInfors();

        public Task<IEnumerable<PaymentInformation>> getPaymentInforByUserId(int userId);
    }
}
