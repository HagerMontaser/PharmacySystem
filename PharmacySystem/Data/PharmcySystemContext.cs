// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PharmacySystem.Models1;

namespace PharmacySystem.Data
{
    public partial class PharmcySystemContext : DbContext
    {
        public PharmcySystemContext()
        {
        }

        public PharmcySystemContext(DbContextOptions<PharmcySystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Item_Invoice> Item_Invoices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=PharmcySystem;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.CustName).IsFixedLength();

                entity.Property(e => e.Type).IsFixedLength();

                entity.HasOne(d => d.Employee_UsernameNavigation)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.Employee_Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_AspNetUsers");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.ImageName).IsFixedLength();

                entity.Property(e => e.Name).IsFixedLength();

                entity.Property(e => e.Quantity).IsFixedLength();
            });

            modelBuilder.Entity<Item_Invoice>(entity =>
            {
                entity.HasKey(e => new { e.InvoiceID, e.ItemID });

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.Item_Invoices)
                    .HasForeignKey(d => d.InvoiceID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Item_Invoice_Invoice");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Item_Invoices)
                    .HasForeignKey(d => d.ItemID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Item_Invoice_Item");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}