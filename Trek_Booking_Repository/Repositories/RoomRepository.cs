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
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDBContext _context;

        public RoomRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> checkExitsName(string name)
        {
            var check = await _context.rooms.AnyAsync(t => t.RoomName == name);
            return check;
        }

        public async Task<Room> createRoom(Room room)
        {
            room.RoomStatus = true;
            _context.rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<int> deleteRoom(int roomId)
        {
            var deleteRoom = await _context.rooms.FirstOrDefaultAsync(t => t.RoomId == roomId);
            if (deleteRoom != null)
            {
                deleteRoom.RoomStatus = false;
                _context.rooms.Update(deleteRoom);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
        public async Task<int> recoverRoomDeleted(int roomId)
        {
            var recoverDele = await _context.rooms.FirstOrDefaultAsync(t => t.RoomId == roomId);
            if (recoverDele != null)
            {
                recoverDele.RoomStatus = true;
                _context.rooms.Update(recoverDele);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
        public async Task<IEnumerable<Room>> getRoombyHotelId(int hotelId)
        {
            var check = await _context.rooms.Where(t => t.HotelId == hotelId).ToListAsync();
            return check;
        }

        public async Task<Room> getRoombyId(int roomId)
        {
            var getHotel = await _context.rooms.FirstOrDefaultAsync(t => t.RoomId == roomId);
            return getHotel;
        }

        public async Task<IEnumerable<Room>> getRooms()
        {
            var c = await _context.rooms.Include(t => t.Hotel).ToListAsync();
            return c;
        }

        public async Task<Room> updateRoom(Room room)
        {
            var findRoom = await _context.rooms.FirstOrDefaultAsync(t => t.RoomId == room.RoomId);
            if (findRoom != null)
            {
                findRoom.RoomName = room.RoomName;
                findRoom.RoomDescription = room.RoomDescription;
                findRoom.RoomNote = room.RoomNote;
                findRoom.RoomAvailable = room.RoomAvailable;
                findRoom.RoomPrice = room.RoomPrice;
                findRoom.RoomCapacity = room.RoomCapacity;
                findRoom.DiscountPercent = room.DiscountPercent;
                findRoom.HotelId = room.HotelId;
                _context.rooms.Update(findRoom);
                await _context.SaveChangesAsync();
                return findRoom;
            }
            return null;
        }
    }
}
