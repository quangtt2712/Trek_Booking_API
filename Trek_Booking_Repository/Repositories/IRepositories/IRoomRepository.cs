using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;
using Trek_Booking_DataAccess.Data;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IRoomRepository
    {
        public Task<Room> createRoom(Room room);
        public Task<Room> updateRoom(Room room);
        public Task<int> deleteRoom(int roomId);
        public Task<Room> getRoombyId(int roomId);
        public Task<IEnumerable<Room>> getRooms();
        public Task<IEnumerable<Room>> getRoombyHotelId(int hotelId);
        public Task<bool> checkExitsName(string name, int hotelId);
        public Task<int> recoverRoomDeleted(int roomId);
        public Task<IEnumerable<RoomAvailabilityDto>> SearchRoomSchedule(int hotelId, DateTime checkInDate, DateTime checkOutDate);
    }
}
