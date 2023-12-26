using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Message.Sms.Web.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users_recharge_logs");

            migrationBuilder.AddColumn<Guid>(
                name: "ApiServiceProviderId",
                table: "project",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "ApiServiceProviderType",
                table: "project",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users_balance_bill",
                columns: table => new
                {
                    KeyId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserKeyId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    BeforeBalance = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    AfterBalance = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Remake = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_balance_bill", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_users_balance_bill_users_UserKeyId",
                        column: x => x.UserKeyId,
                        principalTable: "users",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_users_balance_bill_UserKeyId",
                table: "users_balance_bill",
                column: "UserKeyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users_balance_bill");

            migrationBuilder.DropColumn(
                name: "ApiServiceProviderId",
                table: "project");

            migrationBuilder.DropColumn(
                name: "ApiServiceProviderType",
                table: "project");

            migrationBuilder.CreateTable(
                name: "users_recharge_logs",
                columns: table => new
                {
                    KeyId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AfterBalance = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    BeforeBalance = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Code = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UsersRechargeKeyId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_recharge_logs", x => x.KeyId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
