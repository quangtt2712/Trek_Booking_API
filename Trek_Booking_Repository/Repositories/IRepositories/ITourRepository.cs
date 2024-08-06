using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface ITourRepository
    {
        public Task<Tour> createTour(Tour tour);
        public Task<Tour> updateTour(Tour tour);
        public Task<int> deleteTour(int tourId);
        public Task<Tour> getTourById(int tourId);
        public Task<IEnumerable<Tour>> getTours();
        public Task<IEnumerable<Tour>> getToursByAdmin();

        public Task<bool> checkExitsName(string name, int supplierId);
        public Task<IEnumerable<Tour>> getTourBySupplierId(int supplierId);
        Task<IActionResult> ToggleStatus(ToggleTourRequest request);
        Task<IActionResult> LockTour(ToggleTourRequest request);
        Task<IEnumerable<Tour>> searchTourByAddress(string address);
    }
}
