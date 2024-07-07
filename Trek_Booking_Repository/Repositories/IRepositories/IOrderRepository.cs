using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Task<OrderDTO> Create(OrderDTO objDTO);
        Task<OrderTourDTO> CreateTour(OrderTourDTO objDTO);

        Task<OrderHotelHeader> GetOrderBySessionId(string sessionId);
        Task Update(OrderHotelHeader order);

        Task<OrderTourHeader> GetOrderTourBySessionId(string sessionId);
        Task UpdateTour(OrderTourHeader orderTour);

    }
}
