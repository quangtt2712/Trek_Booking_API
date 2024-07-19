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
    public class CommentRepository :ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }




        public async Task<bool> checkFeedBack(int orderHotelHeaderId, int userId)
        {
            var hasFeedback = await _context.comments
                .AnyAsync(comment => comment.OrderHotelHeaderId == orderHotelHeaderId && comment.UserId == userId);
            return hasFeedback;
        }

        public async Task<Comment> createComment(Comment comment)
        {
            var bookingId = comment.BookingId;
            var userId = comment.UserId;
            var hotelId = comment.HotelId;
            var orderHotelHeaderId = comment.OrderHotelHeaderId;
            // Kiểm tra xem người dùng đã đặt phòng chưa
            var checkBooked = await _context.OrderHotelHeaders.FirstOrDefaultAsync(b => b.Id == orderHotelHeaderId && b.UserId == userId);
            if (checkBooked == null)
            {
                // Nếu người dùng chưa đặt phòng, bạn có thể throw một Exception hoặc xử lý theo ý của bạn
                throw new Exception("User has not booked this room.");
            }

            // Người dùng đã đặt phòng, tiến hành tạo bình luận
            var newComment = new Comment
            {
                BookingId = comment.BookingId,
                OrderHotelHeaderId = comment.OrderHotelHeaderId,
                Message = comment.Message,
                DateSubmitted = DateTime.Now,
                HotelId = comment.HotelId,
                UserId = comment.UserId
            };

            _context.comments.Add(newComment);
            await _context.SaveChangesAsync();

            return newComment;
        }


        public async Task<IEnumerable<Comment>> getCommentByHotelId(int hotelId)
        {
            //var getHotel = await _context.hotels.FirstOrDefaultAsync(t => t.HotelId == hotelId);
            //var comments = await _context.comments.Where(c => c.Hotel == getHotel).ToListAsync();
            var comments = await _context.comments.Include(t => t.User).Include(t => t.Booking).Include(t => t.Hotel).Where(t => t.HotelId == hotelId).ToListAsync();

            return comments;
        }

        public async Task<IEnumerable<Comment>> getCommentByOrderHotelHeaderId(int OrderHotelHeaderId)
        {
            var comments = await _context.comments.Where(t => t.OrderHotelHeaderId == OrderHotelHeaderId).ToListAsync();
            return comments;
        }


        public async Task<IEnumerable<Comment>> getCommentByUserId(int userId)
        {
            var comments = await _context.comments.Where(t => t.UserId == userId).ToListAsync();
            return comments;
        }

    }
}
