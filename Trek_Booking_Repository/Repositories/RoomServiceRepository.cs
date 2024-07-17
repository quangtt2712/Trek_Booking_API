using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Trek_Booking_Repository.Repositories
{
    public class RoomServiceRepository : IRoomServiceRepository
    {
        private readonly ApplicationDBContext _context;

        public RoomServiceRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<RoomService> GetRoomServiceByRoomIdAndServiceId(int roomId, int serviceId)
        {
            return await _context.roomServices
                .FirstOrDefaultAsync(rs => rs.RoomId == roomId && rs.ServiceId == serviceId);
        }

        public async Task<RoomService> UpdateRoomService(RoomService roomService)
        {
            _context.roomServices.Update(roomService);
            await _context.SaveChangesAsync();
            return roomService;
        }

        public async Task<RoomService> createRoomService(RoomService roomService)
        {
            _context.roomServices.Add(roomService);
            await _context.SaveChangesAsync();
            return roomService;
        }

       

        public async Task<RoomService> deleteRoomService(RoomService roomService)
        {
            var deleteRoomService = await _context.roomServices.FirstOrDefaultAsync(t => t.RoomId == roomService.RoomId && t.ServiceId == roomService.ServiceId);
            if (deleteRoomService != null)
            {
                _context.roomServices.Remove(deleteRoomService);
                 await _context.SaveChangesAsync();
            }
            return deleteRoomService;
        }
        public async Task<IEnumerable<RoomService>> getRoomServices()
        {
            return await _context.roomServices
                                 .Include(rs => rs.Room)
                                 .Include(rs => rs.Service)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Room>> getRoombyServiceId(int serviceId)
        {
            var rooms = await _context.roomServices
                                      .Where(rs => rs.ServiceId == serviceId)
                                      .Include(rs => rs.Room)
                                      .Select(rs => rs.Room)
                                      .ToListAsync();

            return rooms;
        }




        public async Task<IEnumerable<Services>> getServicebyRoomId(int roomId)
        {
            var services = await _context.roomServices
                                         .Where(rs => rs.RoomId == roomId)
                                         .Include(rs => rs.Service)
                                         .Select(rs => rs.Service)
                                         .ToListAsync();

            return services;
        }


    }
}
