using Trek_Booking_Hotel_3D_API.Service;

namespace Trek_Booking_Hotel_3D_API.Helper
{
    public class AuthMiddleWare
    {
        private readonly IJwtUtils _jwtUtils;

        public AuthMiddleWare(IJwtUtils jwtUtils)
        {
            _jwtUtils = jwtUtils;
        }

        public int? GetUserIdFromToken(HttpContext context)
        {
            // Lấy giá trị của header "Authorization"
            string authHeader = context.Request.Headers["Authorization"];

            // Kiểm tra xem header Authorization có tồn tại và có chứa token không
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                // Lấy token từ header
                string token = authHeader.Substring("Bearer ".Length).Trim();

                // Sử dụng jwtUtils để xác thực token
                var userId = _jwtUtils.ValidateUserToken(token);
                Console.WriteLine("Token: " + token);
                Console.WriteLine("UserId: " + userId);
                return userId;
            }

            return null; // Trả về null nếu không tìm thấy hoặc không có token hợp lệ trong header
        }
        //authen
        public int? GetSupplierIdFromToken(HttpContext context)
        {
            // Lấy giá trị của header "Authorization"
            string authHeader = context.Request.Headers["Authorization"];

            // Kiểm tra xem header Authorization có tồn tại và có chứa token không
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                // Lấy token từ header
                string token = authHeader.Substring("Bearer ".Length).Trim();

                // Sử dụng jwtUtils để xác thực token
                var supplierId = _jwtUtils.ValidateSupplierToken(token);
                Console.WriteLine("Token: " + token);
                Console.WriteLine("SupplierId: " + supplierId);
                return supplierId;
            }
            return null; // Trả về null nếu không tìm thấy hoặc không có token hợp lệ trong header
        }

        public int? GetSupplierStaffIdFromToken(HttpContext context)
        {
            // Lấy giá trị của header "Authorization"
            string authHeader = context.Request.Headers["Authorization"];

            // Kiểm tra xem header Authorization có tồn tại và có chứa token không
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                // Lấy token từ header
                string token = authHeader.Substring("Bearer ".Length).Trim();

                // Sử dụng jwtUtils để xác thực token
                var supplierStaffId = _jwtUtils.ValidateSupplierStaffToken(token);
                Console.WriteLine("Token: " + token);
                Console.WriteLine("SupplierStaffId: " + supplierStaffId);
                return supplierStaffId;
            }
            return null; // Trả về null nếu không tìm thấy hoặc không có token hợp lệ trong header
        }
    }
}