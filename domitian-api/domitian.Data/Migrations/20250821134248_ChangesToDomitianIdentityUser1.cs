using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace domitian.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangesToDomitianIdentityUser1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09646d88-822c-412c-9310-524d3570fce2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09b2f52d-ab08-4097-b26d-122df4cb34a2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48a5a0b0-dad6-493a-8325-68990cd003b1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5c868b36-f686-41d4-a04b-69253bc49b1f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88af3fb0-ec1b-4cce-b530-73ee9fe78083");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "09646d88-822c-412c-9310-524d3570fce2", null, "tier one user", "TIER ONE USER" },
                    { "09b2f52d-ab08-4097-b26d-122df4cb34a2", null, "tier three user", "TIER THREE USER" },
                    { "48a5a0b0-dad6-493a-8325-68990cd003b1", null, "tier two user", "TIER TWO USER" },
                    { "5c868b36-f686-41d4-a04b-69253bc49b1f", null, "free user", "FREE USER" },
                    { "88af3fb0-ec1b-4cce-b530-73ee9fe78083", null, "admin", "ADMIN" }
                });
        }
    }
}
