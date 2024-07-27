using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trek_Booking_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateVoucherUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherUsageHistory_Booking_BookingId",
                table: "VoucherUsageHistory");

            migrationBuilder.DropIndex(
                name: "IX_VoucherUsageHistory_BookingId",
                table: "VoucherUsageHistory");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "VoucherUsageHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "VoucherUsageHistory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoucherUsageHistory_BookingId",
                table: "VoucherUsageHistory",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherUsageHistory_Booking_BookingId",
                table: "VoucherUsageHistory",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingId");
        }
    }
}
