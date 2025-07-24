using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace registro_ponto_api.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAtivoToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "AspNetUsers");
        }
    }
}
