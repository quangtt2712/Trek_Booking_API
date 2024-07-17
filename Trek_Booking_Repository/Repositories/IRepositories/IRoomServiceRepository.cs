using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IRoomServiceRepository
    {
        public Task<RoomService> createRoomService(RoomService roomService);
        public Task<RoomService> deleteRoomService(RoomService roomService);
        public Task<IEnumerable<Services>> getServicebyRoomId(int roomId);
        public Task<IEnumerable<Room>> getRoombyServiceId(int serviceId);
        public Task<IEnumerable<RoomService>> getRoomServices();
        public Task<RoomService> GetRoomServiceByRoomIdAndServiceId(int roomId, int serviceId);
        public Task<RoomService> UpdateRoomService(RoomService roomService);
    }
}
