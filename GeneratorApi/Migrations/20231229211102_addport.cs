using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeneratorApi.Migrations
{
    /// <inheritdoc />
    public partial class addport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "MaritalStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Port",
                table: "MaritalStatuses");
        }
    }
}
