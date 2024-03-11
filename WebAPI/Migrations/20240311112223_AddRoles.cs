using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f9ba86a-8a9b-41af-b7a3-fef51fa54534", null, "Admin", "ADMIN" },
                    { "55e613ee-0612-4286-8a7a-d1bb5509a32f", null, "User", "USER" },
                    { "faf81ac2-0f1f-4fd2-b085-85e2d38a64c1", null, "Editor", "EDITOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f9ba86a-8a9b-41af-b7a3-fef51fa54534");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "55e613ee-0612-4286-8a7a-d1bb5509a32f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "faf81ac2-0f1f-4fd2-b085-85e2d38a64c1");
        }
    }
}
