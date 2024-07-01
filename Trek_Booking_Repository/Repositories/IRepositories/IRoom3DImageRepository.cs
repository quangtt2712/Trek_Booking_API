using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IRoom3DImageRepository
    {
        public Task<Room3DImage> createRoom3DImage(Room3DImage room3DImage);
        public Task<Room3DImage> updateRoom3DImage(Room3DImage room3DImage);
        public Task<int> deleteRoom3DImage(int roomImage3DId);
        public Task<Room3DImage> getRoom3DImagebyId(int roomImage3DId);
        public Task<IEnumerable<Room3DImage>> getRoom3DImages();
        public Task<IEnumerable<Room3DImage>> getRoom3DImageByRoomId(int roomId);
    }
}
