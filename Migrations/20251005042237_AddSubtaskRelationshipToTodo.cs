using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskly.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSubtaskRelationshipToTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Todos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Todos_ParentId",
                table: "Todos",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Todos_ParentId",
                table: "Todos",
                column: "ParentId",
                principalTable: "Todos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Todos_ParentId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_ParentId",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Todos");
        }
    }
}
