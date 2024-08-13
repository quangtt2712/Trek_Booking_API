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
    public class DashBoardAdminRepository : IDashBoardAdminRepository
    {
        private readonly ApplicationDBContext _context;
        public DashBoardAdminRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<int> countUserBanned()
        {
            var count = await _context.users.Where(s => s.Status == false).CountAsync();
            return count;
        }

        public async Task<int> countSupplierBanned()
        {
            var count = await _context.suppliers.Where(s => s.Status == false).CountAsync();
            return count;
        }

        public async Task<int> countAllBookingRoom()
        {
            var count = await _context.OrderHotelHeaders.CountAsync();
            return count;
        }

        public async Task<int> countAllBookingTour()
        {
            var count = await _context.OrderTourHeaders.CountAsync();
            return count;
        }


        public async Task<int> countAllSupplier()
        {
            var count = await _context.suppliers.CountAsync();
            return count;
        }

        public async Task<int> countAllUser()
        {
            var count = await _context.users.CountAsync();
            return count;
        }

        public async Task<IEnumerable<ReportSupplier>> getSupplierReportsInCurrentMonth()
        {
            var month = DateTime.Now.Month;
            var supplierStatistics = await _context.suppliers
            .Select(s => new ReportSupplier
            {
                SupplierId = s.SupplierId,
                SupplierName = s.SupplierName,
                Status = s.Status,
                Email = s.Email,
                ActiveTours = s.tours.Count(t => t.Status == true && t.Lock == false),
                ActiveHotels = s.hotels.Count(h => h.IsVerify == true && h.Lock == false),
                ActiveRooms = s.hotels.Where(h => h.IsVerify == true && h.Lock == false).SelectMany(h => h.rooms).Count(r => r.RoomStatus == true),
                TourBookings = _context.OrderTourHeaders.Where(ohh => ohh.Process == "Success" && ohh.Completed == true
                && ohh.TourOrderDate.Value.Month == month)
                .Count(oth => oth.SupplierId == s.SupplierId),

                HotelBookings = _context.OrderHotelHeaders.Where(ohh => ohh.Process == "Success" && ohh.Completed == true
                && ohh.CheckOutDate.Value.Month == month)
                .Count(ohh => ohh.SupplierId == s.SupplierId),

                TourRevenue = _context.OrderTourHeaders.Where(ohh => ohh.Process == "Success"
                && ohh.Completed == true && ohh.TourOrderDate.Value.Month == month)
                    .Where(oth => oth.SupplierId == s.SupplierId)
                    .Sum(oth => oth.TotalPrice ?? 0),

                HotelRevenue = _context.OrderHotelHeaders.Where(ohh => ohh.Process == "Success"
                && ohh.Completed == true && ohh.CheckOutDate.Value.Month == month)
                    .Where(ohh => ohh.SupplierId == s.SupplierId)
                    .Sum(ohh => ohh.TotalPrice ?? 0)
            })
            .ToListAsync();

            return supplierStatistics;
        }


        public async Task<IEnumerable<TopTourAdmin>> getTopTourOfSupplierInWeek(DateTime startDate, DateTime endDate)
        {
            var topTours = await _context.OrderTourHeaders
        .Where(oh => oh.TourOrderDate >= startDate && oh.TourOrderDate <= endDate
        && oh.Process == "Success" && oh.Completed == true)
        .Join(_context.OrderTourDetails,
            oh => oh.Id,
            od => od.OrderTourHeaderlId,
            (oh, od) => new { OrderHeader = oh, OrderDetail = od })
        .GroupBy(x => new
        {
            x.OrderDetail.TourId,
            x.OrderDetail.TourName,
            x.OrderHeader.SupplierId
        })
        .Select(g => new TopTourAdmin
        {
            TourId = g.Key.TourId,
            TourName = g.Key.TourName,
            SupplierId = g.Key.SupplierId ?? 0,
            SupplierName = _context.suppliers
                .Where(s => s.SupplierId == g.Key.SupplierId)
                .Select(s => s.SupplierName)
                .FirstOrDefault(),
            OrderCount = g.Count()
        })
        .OrderByDescending(th => th.OrderCount)
        //.Take(10)
        .ToListAsync();

            return topTours;
        }

        public async Task<IEnumerable<TopTourAdmin>> getTopTourOfSupplierDateRange(DateTime startDate, DateTime endDate)
        {
            var topTours = await _context.OrderTourHeaders
        .Where(oh => oh.TourOrderDate >= startDate && oh.TourOrderDate <= endDate
        && oh.Process == "Success" && oh.Completed == true)
        .Join(_context.OrderTourDetails,
            oh => oh.Id,
            od => od.OrderTourHeaderlId,
            (oh, od) => new { OrderHeader = oh, OrderDetail = od })
        .GroupBy(x => new
        {
            x.OrderDetail.TourId,
            x.OrderDetail.TourName,
            x.OrderHeader.SupplierId
        })
        .Select(g => new TopTourAdmin
        {
            TourId = g.Key.TourId,
            TourName = g.Key.TourName,
            SupplierId = g.Key.SupplierId ?? 0,
            SupplierName = _context.suppliers
                .Where(s => s.SupplierId == g.Key.SupplierId)
                .Select(s => s.SupplierName)
                .FirstOrDefault(),
            OrderCount = g.Count()
        })
        .OrderByDescending(th => th.OrderCount)
        //.Take(10)
        .ToListAsync();

            return topTours;
        }



        public async Task<IEnumerable<NewUserSupplier>> getNewUserRegister(DateTime startDate, DateTime endDate)
        {
            // Tạo danh sách các ngày trong khoảng thời gian
            var allDates = Enumerable.Range(0, 1 + (endDate.Date - startDate.Date).Days)
                                     .Select(offset => startDate.Date.AddDays(offset))
                                     .ToList();

            // Truy vấn dữ liệu từ cơ sở dữ liệu
            var user = await _context.users
.Where(d => d.CreateDate.Date >= startDate.Date && d.CreateDate.Date <= endDate.Date)
                .GroupBy(d => d.CreateDate.Date)
                .Select(g => new NewUserSupplier
                {
                    CreateDate = g.Key,
                    countUser = g.Count()
                }).ToListAsync();

            // Kết hợp dữ liệu để đảm bảo mỗi ngày đều có một giá trị
            var result = allDates.GroupJoin(user,
                                            date => date,
                                            userGroup => userGroup.CreateDate,
                                            (date, userGroup) => new NewUserSupplier
                                            {
                                                CreateDate = date,
                                                countUser = userGroup.Any() ? userGroup.First().countUser : 0
                                            }).ToList();

            return result;
        }



        public async Task<IEnumerable<NewUserSupplier>> getNewSupplierRegister(DateTime startDate, DateTime endDate)
        {

            var allDates = Enumerable.Range(0, 1 + (endDate.Date - startDate.Date).Days)
                                     .Select(offset => startDate.Date.AddDays(offset))
                                     .ToList();

            var user = await _context.suppliers
                .Where(d => d.CreateDate.Date >= startDate.Date && d.CreateDate.Date <= endDate.Date)
                .GroupBy(d => d.CreateDate.Date)
                .Select(g => new NewUserSupplier
                {
                    CreateDate = g.Key,
                    countUser = g.Count()
                }).ToListAsync();

            // Kết hợp dữ liệu để đảm bảo mỗi ngày đều có một giá trị
            var result = allDates.GroupJoin(user,
                                            date => date,
                                            userGroup => userGroup.CreateDate,
                                            (date, userGroup) => new NewUserSupplier
                                            {
                                                CreateDate = date,
                                                countUser = userGroup.Any() ? userGroup.First().countUser : 0
                                            }).ToList();

            return result;
        }

        public async Task<IEnumerable<RevenueTourAndHotel>> getRevenueTourByAdmin(DateTime startDate, DateTime endDate)
        {
            var Revenue = await _context.OrderTourHeaders
                .Where(o => o.Process == "Success" && o.Completed == true && o.TourOrderDate.Value.Date >= startDate
                && o.TourOrderDate.Value.Date <= endDate)
                .GroupBy(o => o.TourOrderDate.Value.Date)
                .Select(g => new RevenueTourAndHotel
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.TotalPrice ?? 0)
                })
.OrderBy(x => x.Date)
                .ToListAsync();

            var fullWeekRevenue = new List<RevenueTourAndHotel>();
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var revenueForDate = Revenue.FirstOrDefault(r => r.Date == date);
                if (revenueForDate != null)
                {
                    fullWeekRevenue.Add(revenueForDate);
                }
                else
                {
                    fullWeekRevenue.Add(new RevenueTourAndHotel { Date = date, Revenue = 0 });
                }
            }

            return fullWeekRevenue.OrderBy(x => x.Date);
        }

        public async Task<IEnumerable<RevenueTourAndHotel>> getRevenueHotelByAdmin(DateTime startDate, DateTime endDate)
        {
            var Revenue = await _context.OrderHotelHeaders
                .Where(o => o.Process == "Success" && o.Completed == true && o.CheckOutDate.Value.Date >= startDate
                && o.CheckOutDate.Value.Date <= endDate)
                .GroupBy(o => o.CheckOutDate.Value.Date)
                .Select(g => new RevenueTourAndHotel
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.TotalPrice ?? 0)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            var fullWeekRevenue = new List<RevenueTourAndHotel>();
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var revenueForDate = Revenue.FirstOrDefault(r => r.Date == date);
                if (revenueForDate != null)
                {
                    fullWeekRevenue.Add(revenueForDate);
                }
                else
                {
                    fullWeekRevenue.Add(new RevenueTourAndHotel { Date = date, Revenue = 0 });
                }
            }

            return fullWeekRevenue.OrderBy(x => x.Date);
        }

        public async Task<IEnumerable<ReportSupplier>> getSupplierReportsInDateRange(DateTime startDate, DateTime endDate)
        {
            var supplierStatistics = await _context.suppliers
            .Select(s => new ReportSupplier
            {
                SupplierId = s.SupplierId,
                SupplierName = s.SupplierName,
                Status = s.Status,
                Email = s.Email,
                ActiveTours = s.tours.Count(t => t.Status == true && t.Lock == false),
                ActiveHotels = s.hotels.Count(h => h.IsVerify == true && h.Lock == false),
                ActiveRooms = s.hotels.Where(h => h.IsVerify == true && h.Lock == false).SelectMany(h => h.rooms).Count(r => r.RoomStatus == true),
                TourBookings = _context.OrderTourHeaders.Where(ohh => ohh.Process == "Success" && ohh.Completed == true
                && ohh.TourOrderDate.Value.Date >= startDate && ohh.TourOrderDate.Value.Date <= endDate)
                .Count(oth => oth.SupplierId == s.SupplierId),

                HotelBookings = _context.OrderHotelHeaders.Where(ohh => ohh.Process == "Success" && ohh.Completed == true
                            && ohh.CheckOutDate.Value.Date >= startDate && ohh.CheckOutDate.Value.Date <= endDate)
                .Count(ohh => ohh.SupplierId == s.SupplierId),

                TourRevenue = _context.OrderTourHeaders.Where(ohh => ohh.Process == "Success"
                && ohh.Completed == true && ohh.TourOrderDate.Value.Date >= startDate && ohh.TourOrderDate.Value.Date <= endDate)
                    .Where(oth => oth.SupplierId == s.SupplierId)
                    .Sum(oth => oth.TotalPrice ?? 0),

                HotelRevenue = _context.OrderHotelHeaders.Where(ohh => ohh.Process == "Success"
                && ohh.Completed == true && ohh.CheckOutDate.Value.Date >= startDate && ohh.CheckOutDate.Value.Date <= endDate)
                    .Where(ohh => ohh.SupplierId == s.SupplierId)
                    .Sum(ohh => ohh.TotalPrice ?? 0)
            })
            .ToListAsync();

            return supplierStatistics;
        }

        public async Task<IEnumerable<ReportRevenueAdmin>> getRevenueAdminInMonth()
        {
            var month = DateTime.Now.Month;
            var supplierStatistics = await _context.suppliers.Where(s => s.Status == true)
                .Select(s => new
                {
                    s.SupplierId,
                    s.SupplierName,
                    s.Status,
                    s.Commission,
                    TourRevenueSum = _context.OrderTourHeaders.Where(oth => oth.Process == "Success" && oth.Completed == true && oth.TourOrderDate.Value.Month == month && oth.SupplierId == s.SupplierId)
                        .Sum(oth => oth.TotalPrice ?? 0),
                    HotelRevenueSum = _context.OrderHotelHeaders.Where(ohh => ohh.Process == "Success" && ohh.Completed == true && ohh.CheckOutDate.Value.Month == month && ohh.SupplierId == s.SupplierId)
                        .Sum(ohh => ohh.TotalPrice ?? 0),

                    TourRevenueFee = _context.OrderTourHeaders.Where(oth => oth.Process == "Success" && oth.Completed == true && oth.TourOrderDate.Value.Month == month && oth.SupplierId == s.SupplierId)
                        .Sum(oth => (oth.TotalPrice ?? 0) * 0.995m),
                    HotelRevenueFee = _context.OrderHotelHeaders.Where(ohh => ohh.Process == "Success" && ohh.Completed == true && ohh.CheckOutDate.Value.Month == month && ohh.SupplierId == s.SupplierId)
                        .Sum(ohh => (ohh.TotalPrice ?? 0) * 0.995m),

                    TourRevenueFeeAdmin = _context.OrderTourHeaders.Where(oth => oth.Process == "Success" && oth.Completed == true && oth.TourOrderDate.Value.Month == month && oth.SupplierId == s.SupplierId)
                        .Sum(oth => (oth.TotalPrice ?? 0) * 0.005m),
                    HotelRevenueFeeAdmin = _context.OrderHotelHeaders.Where(ohh => ohh.Process == "Success" && ohh.Completed == true && ohh.CheckOutDate.Value.Month == month && ohh.SupplierId == s.SupplierId)
                        .Sum(ohh => (ohh.TotalPrice ?? 0) * 0.005m),
                })
                .ToListAsync();

            var reportRevenueAdminList = supplierStatistics
                .Select(s => new ReportRevenueAdmin
                {
                    SupplierId = s.SupplierId,
                    SupplierName = s.SupplierName,
                    Status = s.Status,
                    Commission = s.Commission,
                    TotalRevenue = s.TourRevenueSum + s.HotelRevenueSum,
                    TotalRevenueAfterFee = s.TourRevenueFee + s.HotelRevenueFee,
                    CommissionFeeReceived = s.TourRevenueFeeAdmin + s.HotelRevenueFeeAdmin,
                })
                .ToList();

            return reportRevenueAdminList;
        }

        public async Task<IEnumerable<ReportRevenueAdmin>> getRevenueAdminInDateRange(DateTime startDate, DateTime endDate)
        {
            var supplierStatistics = await _context.suppliers.Where(s => s.Status == true)
                .Select(s => new
                {
                    s.SupplierId,
                    s.SupplierName,
                    s.Status,
                    s.Commission,
                    TourRevenueSum = _context.OrderTourHeaders.Where(oth => oth.Process == "Success" && oth.Completed == true
                    && oth.TourOrderDate.Value.Date >= startDate && oth.TourOrderDate.Value.Date <= endDate &&
                    oth.SupplierId == s.SupplierId)
                        .Sum(oth => oth.TotalPrice ?? 0),
                    HotelRevenueSum = _context.OrderHotelHeaders.Where(ohh => ohh.Process == "Success" && ohh.Completed == true
                    && ohh.CheckOutDate.Value.Date >= startDate && ohh.CheckOutDate.Value.Date <= endDate && ohh.SupplierId == s.SupplierId)
                        .Sum(ohh => ohh.TotalPrice ?? 0),

                    TourRevenueFee = _context.OrderTourHeaders.Where(oth => oth.Process == "Success" && oth.Completed == true && oth.TourOrderDate.Value.Date >= startDate && oth.TourOrderDate.Value.Date <= endDate && oth.SupplierId == s.SupplierId)
                        .Sum(oth => (oth.TotalPrice ?? 0) * 0.995m),
                    HotelRevenueFee = _context.OrderHotelHeaders.Where(ohh => ohh.Process == "Success" && ohh.Completed == true && ohh.CheckOutDate.Value.Date >= startDate && ohh.CheckOutDate.Value.Date <= endDate && ohh.SupplierId == s.SupplierId)
                        .Sum(ohh => (ohh.TotalPrice ?? 0) * 0.995m),

                    TourRevenueFeeAdmin = _context.OrderTourHeaders.Where(oth => oth.Process == "Success" && oth.Completed == true && oth.TourOrderDate.Value.Date >= startDate && oth.TourOrderDate.Value.Date <= endDate && oth.SupplierId == s.SupplierId)
                        .Sum(oth => (oth.TotalPrice ?? 0) * 0.005m),
                    HotelRevenueFeeAdmin = _context.OrderHotelHeaders.Where(ohh => ohh.Process == "Success" && ohh.Completed == true && ohh.CheckOutDate.Value.Date >= startDate && ohh.CheckOutDate.Value.Date <= endDate && ohh.SupplierId == s.SupplierId)
                        .Sum(ohh => (ohh.TotalPrice ?? 0) * 0.005m),
                })
                .ToListAsync();

            var reportRevenueAdminList = supplierStatistics
                .Select(s => new ReportRevenueAdmin
                {
                    SupplierId = s.SupplierId,
                    SupplierName = s.SupplierName,
                    Status = s.Status,
                    Commission = s.Commission,
                    TotalRevenue = s.TourRevenueSum + s.HotelRevenueSum,
                    TotalRevenueAfterFee = s.TourRevenueFee + s.HotelRevenueFee,
                    CommissionFeeReceived = s.TourRevenueFeeAdmin + s.HotelRevenueFeeAdmin,
                })
                .ToList();

            return reportRevenueAdminList;
        }

        public async Task<IEnumerable<TopHotel>> getTopHotelOfSupplierInWeek(DateTime startDate, DateTime endDate)
        {
            var topHotels = await _context.OrderHotelHeaders
        .Where(oh => oh.CheckOutDate >= startDate && oh.CheckOutDate <= endDate
        && oh.Process == "Success" && oh.Completed == true)
        .Join(_context.OrderHotelDetails,
            oh => oh.Id,
            od => od.OrderHotelHeaderlId,
            (oh, od) => new { OrderHeader = oh, OrderDetail = od })
        .GroupBy(x => new
        {
            x.OrderDetail.HotelId,
            x.OrderDetail.HotelName,
            x.OrderHeader.SupplierId
        })
        .Select(g => new TopHotel
        {
            HotelId = g.Key.HotelId ?? 0,
            HotelName = g.Key.HotelName,
            SupplierId = g.Key.SupplierId ?? 0,
            SupplierName = _context.suppliers
                .Where(s => s.SupplierId == g.Key.SupplierId)
                .Select(s => s.SupplierName)
                .FirstOrDefault(),
            OrderCount = g.Count()
        })
        .OrderByDescending(th => th.OrderCount)
        //.Take(10)
        .ToListAsync();

            return topHotels;
        }
        public async Task<IEnumerable<TopHotel>> getTopHotelOfSupplierDateRange(DateTime startDate, DateTime endDate)
        {
            var topHotels = await _context.OrderHotelHeaders
        .Where(oh => oh.CheckOutDate >= startDate && oh.CheckOutDate <= endDate
        && oh.Process == "Success" && oh.Completed == true)
        .Join(_context.OrderHotelDetails,
            oh => oh.Id,
            od => od.OrderHotelHeaderlId,
            (oh, od) => new { OrderHeader = oh, OrderDetail = od })
        .GroupBy(x => new
        {
            x.OrderDetail.HotelId,
            x.OrderDetail.HotelName,
            x.OrderHeader.SupplierId
        })
        .Select(g => new TopHotel
        {
            HotelId = g.Key.HotelId ?? 0,
            HotelName = g.Key.HotelName,
            SupplierId = g.Key.SupplierId ?? 0,
            SupplierName = _context.suppliers
                .Where(s => s.SupplierId == g.Key.SupplierId)
                .Select(s => s.SupplierName)
                .FirstOrDefault(),
            OrderCount = g.Count()
        })
        .OrderByDescending(th => th.OrderCount)
        //.Take(10)
        .ToListAsync();

            return topHotels;
        }
    }
}