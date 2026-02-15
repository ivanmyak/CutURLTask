using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace CutURLTask.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrlRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LongUrl = table.Column<string>(type: "varchar(255)", nullable: false),
                    Code = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    UsedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlRecords", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UrlRecords_Code",
                table: "UrlRecords",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UrlRecords_LongUrl",
                table: "UrlRecords",
                column: "LongUrl",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlRecords");
        }
    }
}
