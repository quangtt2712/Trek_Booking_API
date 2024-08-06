using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Trek_Booking_Repository.Repositories
{
    public class TourRepository : ITourRepository
    {
        private readonly ApplicationDBContext _context;
        public TourRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> checkExitsName(string name, int supplierId)
        {
            var check = await _context.tours.Where(x => x.SupplierId == supplierId && x.TourName == name).AnyAsync();
            return check;
        }

        public async Task<Tour> createTour(Tour tour)
        {
            tour.Status = true;
            _context.tours.Add(tour);
            await _context.SaveChangesAsync();
            return tour;
        }

        public async Task<int> deleteTour(int tourId)
        {
            var deleteTour = await _context.tours.FirstOrDefaultAsync(t => t.TourId == tourId);
            if (deleteTour != null)
            {
                _context.tours.Remove(deleteTour);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<Tour> getTourById(int tourId)
        {
            var getTour = await _context.tours.FirstOrDefaultAsync(t => t.TourId == tourId);
            return getTour;
        }

        public async Task<IEnumerable<Tour>> getTours()
        {
            var tours = await _context.tours.Where(t => t.Status == true && t.Lock == false).ToListAsync();
            return tours;
        }

        public async Task<Tour> updateTour(Tour tour)
        {
            var findTour = await _context.tours.FirstOrDefaultAsync(t => t.TourId == tour.TourId);
            if (findTour != null)
            {
                findTour.TourName = tour.TourName;
                findTour.TourDescription = tour.TourDescription;
                findTour.TourPrice = tour.TourPrice;
                findTour.TourAddress = tour.TourAddress;
                findTour.TourTime = tour.TourTime;
                findTour.TourTransportation = tour.TourTransportation;
                findTour.TourDiscount = tour.TourDiscount;
                findTour.TourCapacity = tour.TourCapacity;
                findTour.TourDay = tour.TourDay;
                _context.tours.Update(findTour);
                await _context.SaveChangesAsync();
                return findTour;
            }
            return null;
        }

        public async Task<IEnumerable<Tour>> getTourBySupplierId(int supplierId)
        {
            var tourBySup = await _context.tours.Where(s => s.SupplierId == supplierId).ToListAsync();
            return tourBySup;
        }
        public async Task<IActionResult> ToggleStatus(ToggleTourRequest request)
        {
            var tour = await _context.tours.FindAsync(request.TourId);
            if (tour == null)
            {
                return new NotFoundResult();
            }

            tour.Status = !tour.Status;
            _context.Entry(tour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            return new NoContentResult();
        }

        public async Task<IEnumerable<Tour>> searchTourByAddress(string address)
        {
            var tours = await _context.tours
                .Where(h => EF.Functions.Like(h.TourAddress, $"%{address}%"))
                .ToListAsync();

            return tours;
        }

        public async Task<IActionResult> LockTour(ToggleTourRequest request)
        {
            var tour = await _context.tours.FindAsync(request.TourId);
            if (tour == null)
            {
                return new NotFoundResult();
            }

            tour.Lock = !tour.Lock;
            _context.Entry(tour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            return new NoContentResult();
        }

        public async Task<IEnumerable<Tour>> getToursByAdmin()
        {
            var tours = await _context.tours.ToListAsync();
            return tours;
        }
    }
}
