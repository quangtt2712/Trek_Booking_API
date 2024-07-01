using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Trek_Booking_Repository.Repositories
{
    public class TourImageRepository : ITourImageRepository
    {
        private readonly ApplicationDBContext _context;
        public TourImageRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<TourImage> createTourImage(TourImage tourImage)
        {
            _context.tourImages.Add(tourImage);
            await _context.SaveChangesAsync();
            return tourImage;
        }

        public async Task<int> deleteTourImage(int tourImageId)
        {
            var deleteTourImage = await _context.tourImages.FirstOrDefaultAsync(t => t.TourImageId == tourImageId);
            if (deleteTourImage != null)
            {
                _context.tourImages.Remove(deleteTourImage);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<TourImage> getTourImageById(int tourImageId)
        {
            var getTourImage = await _context.tourImages.FirstOrDefaultAsync(t => t.TourImageId == tourImageId);
            return getTourImage;
        }

        public async Task<IEnumerable<TourImage>> getTourImageByTourId(int tourId)
        {
            var getTourImage = await _context.tourImages.Where(t => t.TourId == tourId).ToListAsync();
            return getTourImage;
        }

        public async Task<IEnumerable<TourImage>> getTourImages()
        {
            var tours = await _context.tourImages.ToListAsync();
            return tours;
        }

        public async Task<TourImage> updateTourImage(TourImage tourImage)
        {
            var findTourImages = await _context.tourImages.FirstOrDefaultAsync(t => t.TourImageId == tourImage.TourImageId);
            if (findTourImages != null)
            {
                findTourImages.TourImageURL = tourImage.TourImageURL;
                _context.tourImages.Update(findTourImages);
                await _context.SaveChangesAsync();
                return findTourImages;
            }
            return null;
        }
    }
}
