using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trek_Booking_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initfixvoucherandpayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInformation_User_UserId",
                table: "PaymentInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherUsageHistory_Booking_BookingId",
                table: "VoucherUsageHistory");

            migrationBuilder.AlterColumn<int>(
                name: "VoucherId",
                table: "VoucherUsageHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "VoucherUsageHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "VoucherUsageHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OrderHotelHeaderId",
                table: "VoucherUsageHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Process",
                table: "VoucherUsageHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PaymentInformation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "CartNumber",
                table: "PaymentInformation",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<string>(
                name: "Process",
                table: "PaymentInformation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoucherUsageHistory_OrderHotelHeaderId",
                table: "VoucherUsageHistory",
                column: "OrderHotelHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInformation_User_UserId",
                table: "PaymentInformation",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherUsageHistory_Booking_BookingId",
                table: "VoucherUsageHistory",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherUsageHistory_OrderHotelHeader_OrderHotelHeaderId",
                table: "VoucherUsageHistory",
                column: "OrderHotelHeaderId",
                principalTable: "OrderHotelHeader",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInformation_User_UserId",
                table: "PaymentInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherUsageHistory_Booking_BookingId",
                table: "VoucherUsageHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherUsageHistory_OrderHotelHeader_OrderHotelHeaderId",
                table: "VoucherUsageHistory");

            migrationBuilder.DropIndex(
                name: "IX_VoucherUsageHistory_OrderHotelHeaderId",
                table: "VoucherUsageHistory");

            migrationBuilder.DropColumn(
                name: "OrderHotelHeaderId",
                table: "VoucherUsageHistory");

            migrationBuilder.DropColumn(
                name: "Process",
                table: "VoucherUsageHistory");

            migrationBuilder.DropColumn(
                name: "Process",
                table: "PaymentInformation");

            migrationBuilder.AlterColumn<int>(
                name: "VoucherId",
                table: "VoucherUsageHistory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "VoucherUsageHistory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "VoucherUsageHistory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PaymentInformation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "CartNumber",
                table: "PaymentInformation",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInformation_User_UserId",
                table: "PaymentInformation",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherUsageHistory_Booking_BookingId",
                table: "VoucherUsageHistory",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
