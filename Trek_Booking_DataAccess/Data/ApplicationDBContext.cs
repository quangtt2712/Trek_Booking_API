using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext()
        {

        }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Booking> bookings { get; set; }
        public DbSet<BookingCart> bookingCarts { get; set; }
        public DbSet<CartTour> cartTours { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Hotel> hotels { get; set; }
        public DbSet<PaymentInformation> paymentInformations { get; set; }
        public DbSet<Rate> rates { get; set; }
        public DbSet<Room> rooms { get; set; }
        public DbSet<Room3DImage> room3DImages { get; set; }
        public DbSet<RoomImage> roomImages { get; set; }
        public DbSet<Services> services { get; set; }
        public DbSet<RoomService> roomServices { get; set; }
        public DbSet<Supplier> suppliers { get; set; }
        public DbSet<SupplierStaff> supplierStaff { get; set; }
        public DbSet<Tour> tours { get; set; }
        public DbSet<TourImage> tourImages { get; set; }
        public DbSet<TourOrder> tourOrders { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Voucher> vouchers { get; set; }
        public DbSet<VoucherUsageHistory> voucherUsageHistories { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<OrderHotelDetail> OrderHotelDetails { get; set; }
        public DbSet<OrderHotelHeader> OrderHotelHeaders { get; set; }
        public DbSet<HotelImage> hotelImages { get; set; }
        public DbSet<OrderTourDetail> OrderTourDetails { get; set; }
        public DbSet<OrderTourHeader> OrderTourHeaders { get; set; }
        public DbSet<dataUser> dataUsers { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomService>().HasKey(x => new { x.ServiceId, x.RoomId });
            //Tour Order
            modelBuilder.Entity<TourOrder>()
            .HasOne(t => t.Supplier)
            .WithMany(c => c.tourOrders)
            .HasForeignKey(t => t.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TourOrder>()
                .HasOne(t => t.Tour)
                .WithMany(c => c.tourOrders)
                .HasForeignKey(t => t.TourId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TourOrder>()
                .HasOne(t => t.User)
                .WithMany(c => c.tourOrders)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking Cart
            modelBuilder.Entity<BookingCart>()
                .HasOne(b => b.Hotel)
                .WithMany(c => c.bookingCarts)
                .HasForeignKey(b => b.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingCart>()
                .HasOne(b => b.Room)
                .WithMany(c => c.bookingCarts)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingCart>()
                .HasOne(b => b.User)
                .WithMany(c => c.bookingCarts)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking 

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Hotel)
                .WithMany(c => c.bookings)
                .HasForeignKey(b => b.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(c => c.bookings)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(c => c.bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // Comment
            modelBuilder.Entity<Comment>()
                .HasOne(b => b.Hotel)
                .WithMany(c => c.comments)
                .HasForeignKey(b => b.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(b => b.Booking)
                .WithMany(c => c.comments)
                .HasForeignKey(b => b.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(b => b.User)
                .WithMany(c => c.comments)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Rate
            modelBuilder.Entity<Rate>()
                .HasOne(b => b.Hotel)
                .WithMany(c => c.rates)
                .HasForeignKey(b => b.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rate>()
                .HasOne(b => b.Booking)
                .WithMany(c => c.rates)
                .HasForeignKey(b => b.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rate>()
                .HasOne(b => b.User)
                .WithMany(c => c.rates)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //VoucherUsageHistory

            modelBuilder.Entity<VoucherUsageHistory>()
                .HasOne(b => b.Voucher)
                .WithMany(c => c.voucherUsageHistories)
                .HasForeignKey(b => b.VoucherId)
                .OnDelete(DeleteBehavior.Restrict);

    
            modelBuilder.Entity<VoucherUsageHistory>()
                .HasOne(b => b.User)
                .WithMany(c => c.voucherUsageHistories)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // RoomService
            modelBuilder.Entity<RoomService>()
                .HasOne(i => i.Service)
                .WithMany(c => c.roomServices)
                .HasForeignKey(f => f.ServiceId);

            modelBuilder.Entity<RoomService>()
                .HasOne(i => i.Room)
                .WithMany(c => c.roomServices)
                .HasForeignKey(f => f.RoomId);

            // supplierstaff
            modelBuilder.Entity<SupplierStaff>()
                .HasOne(t => t.Supplier)
                .WithMany(c => c.supplierStaffs)
                .HasForeignKey(t => t.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SupplierStaff>()
                .HasOne(t => t.Role)
                .WithMany(c => c.supplierStaffs)
                .HasForeignKey(t => t.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            // cartour
            modelBuilder.Entity<CartTour>()
               .HasOne(t => t.Tour)
               .WithMany(c => c.cartTours)
               .HasForeignKey(t => t.TourId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartTour>()
                .HasOne(t => t.User)
                .WithMany(c => c.cartTours)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // 
            modelBuilder.Entity<OrderHotelDetail>()
                .HasOne(t => t.Hotel)
                .WithMany(c => c.OrderHotelDetails)
                .HasForeignKey(t => t.HotelId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<OrderHotelDetail>()
             .HasOne(t => t.Room)
             .WithMany(c => c.OrderHotelDetails)
             .HasForeignKey(t => t.RoomId)
             .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
