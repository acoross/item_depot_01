using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace bank.DbModel
{
    public interface ITradable
    {
        long owner_id { get; set; }
        long type { get; set; }
    }

    public class Stackable : ITradable
    {
        public long owner_id { get; set; }
        public long type { get; set; }
        public long amount { get; set; }
    }

    public class NonStackable : ITradable
    {
        [Key]
        public long dbid { get; set; }

        public long owner_id { get; set; }
        public long type { get; set; }
    }

    public interface ITransactionalJob
    {
        bool Do();
        bool Rollback();
    }

    public enum TransactionJobType
    {
        Stackable,
        NonStackable,
        Job
    }

    public class TransactionData
    {
        [Key]
        public long id { get; set; }
        public long transaction_id { get; set; }
        public TransactionJobType job_type { get; set; }
        public long item_type { get; set; }
        public long amount_diff { get; set; }   // ns 이면 음수인지 양수인지만 고려함 (수량은 따지지 않음)
        public long dbid { get; set; }  // ns 이고 amount_diff < 0 일 때만 사용됨

        [ForeignKey("transaction_id")]
        public Transaction tran { get; set; }
    }

    public class Transaction
    {
        [Key]
        public long transaction_id { get; set; }
        public long owner_id { get; set; }
        
        public List<TransactionData> tran_list { get; set; }

        [NotMapped]
        public ICollection<ITransactionalJob> job_list;
    }

    public class BankDbContext : DbContext
    {
        public DbSet<Stackable> tb_stackable_items { get; set; }
        public DbSet<NonStackable> tb_nonstackable_items { get; set; }
        public DbSet<Transaction> tb_item_transactions { get; set; }
        public DbSet<TransactionData> tb_item_transaction_data { get; set; }

        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
        }

        //public new int SaveChanges()
        //{
        //    Database.BeginTransaction();
        //    try
        //    {
        //        base.SaveChanges();
        //        Database.CommitTransaction();
        //    }
        //    catch
        //    {
        //        Database.RollbackTransaction();
        //    }

        //    return 0;
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stackable>()
                .HasKey(s => new { s.owner_id, s.type });

            modelBuilder.Entity<NonStackable>()
                .Property(ns => ns.dbid)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Transaction>()
                .Property(t => t.transaction_id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<TransactionData>()
                .Property(t => t.id)
                .ValueGeneratedOnAdd();
        }
    }
}
