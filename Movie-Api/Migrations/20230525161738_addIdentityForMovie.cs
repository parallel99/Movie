using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie_Api.Migrations
{
    /// <inheritdoc />
    public partial class addIdentityForMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImdbId",
                table: "movie",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_movie_ImdbId",
                table: "movie",
                column: "ImdbId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_movie_ImdbId",
                table: "movie");

            migrationBuilder.DropColumn(
                name: "ImdbId",
                table: "movie");
        }
    }
}
