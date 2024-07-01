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
    public class RoomImageRepository : IRoomImageRepository
    {

        private readonly ApplicationDBContext _context;

        public RoomImageRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<RoomImage> createRoomImage(RoomImage roomImage)
        {
            _context.roomImages.Add(roomImage);
            await _context.SaveChangesAsync();
            return roomImage;
        }

        public async Task<int> deleteRoomImage(int roomImageId)
        {
            var deleteRoomImage = await _context.roomImages.FirstOrDefaultAsync(t => t.RoomImageId == roomImageId);
            if (deleteRoomImage != null)
            {
                _context.roomImages.Remove(deleteRoomImage);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<RoomImage> getRoomImagebyId(int roomImageId)
        {
            var getRoomImage = await _context.roomImages.FirstOrDefaultAsync(t => t.RoomImageId == roomImageId);
            return getRoomImage;
        }

        public async Task<IEnumerable<RoomImage>> getRoomImageByRoomId(int roomId)
        {
            var check = await _context.roomImages.Where(t => t.RoomId == roomId).ToListAsync();
            return check;
        }

        public async Task<IEnumerable<RoomImage>> getRoomImages()
        {
            var roomImages = await _context.roomImages.Include(r => r.Room).ToListAsync();
            return roomImages;
        }

        public async Task<RoomImage> updateRoomImage(RoomImage roomImage)
        {
            var findRoomImage = await _context.roomImages.FirstOrDefaultAsync(t => t.RoomImageId == roomImage.RoomImageId);
            if (findRoomImage != null)
            {
                findRoomImage.RoomImageURL = roomImage.RoomImageURL;
                findRoomImage.RoomId = roomImage.RoomId;
                _context.roomImages.Update(findRoomImage);
                await _context.SaveChangesAsync();
                return findRoomImage;
            }
            return null;
        }
    }
}
