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
    public class Room3DImageRepository : IRoom3DImageRepository
    {
        private readonly ApplicationDBContext _context;

        public Room3DImageRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Room3DImage> createRoom3DImage(Room3DImage room3DImage)
        {
            _context.room3DImages.Add(room3DImage);
            await _context.SaveChangesAsync();
            return room3DImage;
        }

        public async Task<int> deleteRoom3DImage(int roomImage3DId)
        {
            var deleteRoom3DImage = await _context.room3DImages.FirstOrDefaultAsync(t => t.RoomImage3DId == roomImage3DId);
            if (deleteRoom3DImage != null)
            {
                _context.room3DImages.Remove(deleteRoom3DImage);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<Room3DImage> getRoom3DImagebyId(int roomImage3DId)
        {
            var getRoomImage = await _context.room3DImages.FirstOrDefaultAsync(t => t.RoomImage3DId == roomImage3DId);
            return getRoomImage;
        }

        public async Task<IEnumerable<Room3DImage>> getRoom3DImageByRoomId(int roomId)
        {
            var check = await _context.room3DImages.Where(t => t.RoomId == roomId).ToListAsync();
            if (!check.Any())
            {
                throw new Exception("Room not found");
            }
            return check;
        }

        public async Task<IEnumerable<Room3DImage>> getRoom3DImages()
        {
            var room3DImages = await _context.room3DImages.Include(r => r.Room).ToListAsync();
            return room3DImages;
        }

        public async Task<Room3DImage> updateRoom3DImage(Room3DImage room3DImage)
        {
            var findRoom3DImage = await _context.room3DImages.FirstOrDefaultAsync(t => t.RoomImage3DId == room3DImage.RoomImage3DId);
            if (findRoom3DImage != null)
            {
                findRoom3DImage.RoomImage3DURL = room3DImage.RoomImage3DURL;
                findRoom3DImage.RoomId = room3DImage.RoomId;
                _context.room3DImages.Update(findRoom3DImage);
                await _context.SaveChangesAsync();
                return findRoom3DImage;
            }
            return null;
        }


    }
}
