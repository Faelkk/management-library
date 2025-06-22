using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class add_client_schemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordResetTokens_user_UserId",
                table: "PasswordResetTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PasswordResetTokens",
                table: "PasswordResetTokens");

            migrationBuilder.RenameTable(
                name: "PasswordResetTokens",
                newName: "password_reset_token");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "password_reset_token",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "password_reset_token",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "password_reset_token",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ExpirationDate",
                table: "password_reset_token",
                newName: "expiration_date");

            migrationBuilder.RenameIndex(
                name: "IX_PasswordResetTokens_UserId",
                table: "password_reset_token",
                newName: "IX_password_reset_token_user_id");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "user",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "user",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_password_reset_token",
                table: "password_reset_token",
                column: "id");

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
                name: "IX_user_email",
                table: "user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_phone_number",
                table: "user",
                column: "phone_number",
                unique: true);

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
                name: "FK_password_reset_token_user_user_id",
                table: "password_reset_token",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_password_reset_token_user_user_id",
                table: "password_reset_token");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropIndex(
                name: "IX_user_email",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_phone_number",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_password_reset_token",
                table: "password_reset_token");

            migrationBuilder.RenameTable(
                name: "password_reset_token",
                newName: "PasswordResetTokens");

            migrationBuilder.RenameColumn(
                name: "token",
                table: "PasswordResetTokens",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PasswordResetTokens",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "PasswordResetTokens",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "expiration_date",
                table: "PasswordResetTokens",
                newName: "ExpirationDate");

            migrationBuilder.RenameIndex(
                name: "IX_password_reset_token_user_id",
                table: "PasswordResetTokens",
                newName: "IX_PasswordResetTokens_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "user",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "user",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PasswordResetTokens",
                table: "PasswordResetTokens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResetTokens_user_UserId",
                table: "PasswordResetTokens",
                column: "UserId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
