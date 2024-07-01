using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IRoomImageRepository
    {
        public Task<RoomImage> createRoomImage(RoomImage roomImage);
        public Task<RoomImage> updateRoomImage(RoomImage roomImage);
        public Task<int> deleteRoomImage(int roomImageId);
        public Task<RoomImage> getRoomImagebyId(int roomImageId);
        public Task<IEnumerable<RoomImage>> getRoomImages();
        public Task<IEnumerable<RoomImage>> getRoomImageByRoomId(int roomId);
    }
}
