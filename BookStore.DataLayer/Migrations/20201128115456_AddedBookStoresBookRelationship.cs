using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.DataLayer.Migrations
{
    public partial class AddedBookStoresBookRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookStoresBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BookStoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookStoresBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookStoresBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookStoresBooks_BookStores_BookStoreId",
                        column: x => x.BookStoreId,
                        principalTable: "BookStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookStoresBooks_BookId",
                table: "BookStoresBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookStoresBooks_BookStoreId",
                table: "BookStoresBooks",
                column: "BookStoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookStoresBooks");
        }
    }
}
