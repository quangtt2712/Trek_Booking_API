using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trek_Booking_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatemodelhotelandtour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TourView",
                table: "Tour",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HotelView",
                table: "Hotel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TourView",
                table: "Tour");

            migrationBuilder.DropColumn(
                name: "HotelView",
                table: "Hotel");
        }
    }
}
