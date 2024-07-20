using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface ICommentRepository
    {
        public Task<Comment> createComment(Comment comment);
        public Task<IEnumerable<Comment>> getCommentByHotelId(int hotelId);

        public Task<IEnumerable<Comment>> getCommentByOrderHotelHeaderId(int OrderHotelHeaderId);
        public Task<IEnumerable<Comment>> getCommentByUserId(int userId);

        public Task<bool> checkFeedBack(int orderHotelHeaderId, int userId);

    }
}
