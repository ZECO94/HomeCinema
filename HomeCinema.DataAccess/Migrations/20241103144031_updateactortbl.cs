using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeCinema.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateactortbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Actors");
        }
    }
}
