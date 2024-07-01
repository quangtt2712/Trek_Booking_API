using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IServiceRepository
    {
        public Task<Services> createService(Services service);
        public Task<Services> updateService(Services service);
        public Task<int> deleteService(int serviceId);
        public Task<Services> getServicebyId(int serviceId);
        public Task<IEnumerable<Services>> getServices();

        public Task<bool> checkExitsName(string name);
    }
}
