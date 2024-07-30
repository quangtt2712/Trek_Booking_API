using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IDashBoardAdminRepository
    {
        public Task<int> countAllUser();
    
        public Task<int> countAllSupplier();
     
        public Task<int> countAllBookingRoom();
        public Task<int> countAllBookingTour();
        public Task<IEnumerable<ReportSupplier>> getSupplierReportsInCurrentMonth();
        public Task<IEnumerable<ReportSupplier>> getSupplierReportsInDateRange(DateTime startDate, DateTime endDate);
        public Task<IEnumerable<TopHotel>> getTopHotelOfSupplierInWeek(DateTime startDate, DateTime endDate);
        public Task<IEnumerable<TopHotel>> getTopHotelOfSupplierDateRange(DateTime startDate, DateTime endDate);
        public Task<IEnumerable<TopTourAdmin>> getTopTourOfSupplierInWeek(DateTime startDate, DateTime endDate);
        public Task<IEnumerable<TopTourAdmin>> getTopTourOfSupplierDateRange(DateTime startDate, DateTime endDate);
        public Task<IEnumerable<NewUserSupplier>> getNewUserRegister(DateTime startDate, DateTime endDate);
        public Task<IEnumerable<NewUserSupplier>> getNewSupplierRegister(DateTime startDate, DateTime endDate);
        public Task<IEnumerable<RevenueTourAndHotel>> getRevenueTourByAdmin(DateTime startDate, DateTime endDate);
        public Task<IEnumerable<RevenueTourAndHotel>> getRevenueHotelByAdmin(DateTime startDate, DateTime endDate);
        public Task<IEnumerable<ReportRevenueAdmin>> getRevenueAdminInMonth();
        public Task<IEnumerable<ReportRevenueAdmin>> getRevenueAdminInDateRange(DateTime startDate, DateTime endDate);
    }
}
