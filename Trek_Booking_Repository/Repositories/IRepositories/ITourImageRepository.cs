using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface ITourImageRepository
    {
        public Task<TourImage> createTourImage(TourImage tourImage);
        public Task<TourImage> updateTourImage(TourImage tourImage);
        public Task<int> deleteTourImage(int tourImageId);
        public Task<TourImage> getTourImageById(int tourImageId);
        public Task<IEnumerable<TourImage>> getTourImageByTourId(int tourId);
        public Task<IEnumerable<TourImage>> getTourImages();
    }
}
