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
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDBContext _context;

        public ServiceRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<bool> checkExitsName(string name)
        {
            var check = await _context.services.AnyAsync(t => t.ServiceName == name);
            return check;
        }

        public async Task<Services> createService(Services service)
        {
            _context.services.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<int> deleteService(int serviceId)
        {
            var deleteService = await _context.services.FirstOrDefaultAsync(t => t.ServiceId == serviceId);
            if (deleteService != null)
            {
                _context.services.Remove(deleteService);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<Services> getServicebyId(int serviceId)
        {
            var get = await _context.services.FirstOrDefaultAsync(t => t.ServiceId == serviceId);
            return get;
        }

        public async Task<IEnumerable<Services>> getServices()
        {
            var services = await _context.services.ToListAsync();
            return services;
        }

        public async Task<Services> updateService(Services service)
        {
            var findService = await _context.services.FirstOrDefaultAsync(t => t.ServiceId == service.ServiceId);
            if (findService != null)
            {
                findService.ServiceName = service.ServiceName;
                findService.ServiceDescription = service.ServiceDescription;
                findService.ServiceImage = service.ServiceImage;
                _context.services.Update(findService);
                await _context.SaveChangesAsync();
                return findService;
            }
            return null;
        }
    }
}
