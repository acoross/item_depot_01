using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace bank.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_nonstackable_items",
                columns: table => new
                {
                    dbid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    owner_id = table.Column<long>(nullable: false),
                    type = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_nonstackable_items", x => x.dbid);
                });

            migrationBuilder.CreateTable(
                name: "tb_stackable_items",
                columns: table => new
                {
                    owner_id = table.Column<long>(nullable: false),
                    type = table.Column<long>(nullable: false),
                    amount = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_stackable_items", x => new { x.owner_id, x.type });
                });

            migrationBuilder.CreateTable(
                name: "tb_item_transactions",
                columns: table => new
                {
                    transaction_id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    owner_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_item_transactions", x => x.transaction_id);
                });

            migrationBuilder.CreateTable(
                name: "tb_item_transaction_data",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    amount_diff = table.Column<long>(nullable: false),
                    dbid = table.Column<long>(nullable: false),
                    item_type = table.Column<long>(nullable: false),
                    job_type = table.Column<int>(nullable: false),
                    transaction_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_item_transaction_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_item_transaction_data_tb_item_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "tb_item_transactions",
                        principalColumn: "transaction_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_item_transaction_data_transaction_id",
                table: "tb_item_transaction_data",
                column: "transaction_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_nonstackable_items");

            migrationBuilder.DropTable(
                name: "tb_stackable_items");

            migrationBuilder.DropTable(
                name: "tb_item_transaction_data");

            migrationBuilder.DropTable(
                name: "tb_item_transactions");
        }
    }
}
