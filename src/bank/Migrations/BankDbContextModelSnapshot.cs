using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using bank.DbModel;

namespace bank.Migrations
{
    [DbContext(typeof(BankDbContext))]
    partial class BankDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("bank.DbModel.NonStackable", b =>
                {
                    b.Property<long>("dbid")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("owner_id");

                    b.Property<long>("type");

                    b.HasKey("dbid");

                    b.ToTable("tb_nonstackable_items");
                });

            modelBuilder.Entity("bank.DbModel.Stackable", b =>
                {
                    b.Property<long>("owner_id");

                    b.Property<long>("type");

                    b.Property<long>("amount");

                    b.HasKey("owner_id", "type");

                    b.ToTable("tb_stackable_items");
                });

            modelBuilder.Entity("bank.DbModel.Transaction", b =>
                {
                    b.Property<long>("transaction_id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("owner_id");

                    b.HasKey("transaction_id");

                    b.ToTable("tb_item_transactions");
                });

            modelBuilder.Entity("bank.DbModel.TransactionData", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("amount_diff");

                    b.Property<long>("dbid");

                    b.Property<long>("item_type");

                    b.Property<int>("job_type");

                    b.Property<long>("transaction_id");

                    b.HasKey("id");

                    b.HasIndex("transaction_id");

                    b.ToTable("tb_item_transaction_data");
                });

            modelBuilder.Entity("bank.DbModel.TransactionData", b =>
                {
                    b.HasOne("bank.DbModel.Transaction", "tran")
                        .WithMany("tran_list")
                        .HasForeignKey("transaction_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
