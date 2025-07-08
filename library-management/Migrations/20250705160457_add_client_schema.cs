using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class add_client_schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_loan_user_user_id",
                table: "loan");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "loan",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_loan_user_id",
                table: "loan",
                newName: "IX_loan_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "loan",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "client_id",
                table: "loan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_loan_client_id",
                table: "loan",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_client_email",
                table: "client",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_client_phone_number",
                table: "client",
                column: "phone_number",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_client_client_id",
                table: "loan",
                column: "client_id",
                principalTable: "client",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_user_UserId",
                table: "loan",
                column: "UserId",
                principalTable: "user",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_loan_client_client_id",
                table: "loan");

            migrationBuilder.DropForeignKey(
                name: "FK_loan_user_UserId",
                table: "loan");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropIndex(
                name: "IX_loan_client_id",
                table: "loan");

            migrationBuilder.DropColumn(
                name: "client_id",
                table: "loan");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "loan",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_loan_UserId",
                table: "loan",
                newName: "IX_loan_user_id");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "loan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_loan_user_user_id",
                table: "loan",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
