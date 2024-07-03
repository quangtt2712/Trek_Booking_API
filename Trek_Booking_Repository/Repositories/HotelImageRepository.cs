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
    public class HotelImageRepository: IHotelImageRepository
    {
        private readonly ApplicationDBContext _context;
        public HotelImageRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<HotelImage> createHotelImage(HotelImage hotelImage)
        {
            _context.hotelImages.Add(hotelImage);
            await _context.SaveChangesAsync();
            return hotelImage;
        }

        public async Task<int> deleteHotelImage(int hotelImageId)
        {
            var deleteHotelImage = await _context.hotelImages.FirstOrDefaultAsync(t => t.HotelImageId == hotelImageId);
            if (deleteHotelImage != null)
            {
                _context.hotelImages.Remove(deleteHotelImage);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<HotelImage>> getHotelImageByHotelId(int hotelId)
        {
            var check = await _context.hotelImages.Where(t => t.HotelId == hotelId).ToListAsync();
            return check;
        }

        public async Task<HotelImage> getHotelImagebyId(int hotelImageId)
        {
            var getHotelImage = await _context.hotelImages.FirstOrDefaultAsync(t => t.HotelImageId == hotelImageId);
            return getHotelImage;
        }

        public async Task<IEnumerable<HotelImage>> getHotelImages()
        {
            var hotelImages = await _context.hotelImages.Include(r => r.Hotel).ToListAsync();
            return hotelImages;
        }

        public async Task<HotelImage> updateHotelImage(HotelImage hotelImage)
        {
            var findHotelImage = await _context.hotelImages.FirstOrDefaultAsync(t => t.HotelImageId == hotelImage.HotelImageId);
            if (findHotelImage != null)
            {
                findHotelImage.HotelImageURL = hotelImage.HotelImageURL;
                findHotelImage.HotelId = hotelImage.HotelId;
                _context.hotelImages.Update(findHotelImage);
                await _context.SaveChangesAsync();
                return findHotelImage;
            }
            return null;
        }
    }
}
