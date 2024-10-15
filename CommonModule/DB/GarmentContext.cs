using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.DB;

public partial class GarmentContext : DbContext
{
    public GarmentContext()
    {
    }

    public GarmentContext(DbContextOptions<GarmentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleDetail> SaleDetails { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=INTELPOINT\\SQLEXPRESS;Database=BritishGarments;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED8A743232");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Products");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.ToTable("ProductCategory");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK__Purchase__6B0A6BBEC88D0E1A");

            entity.Property(e => e.PurchaseDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.VendorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Purchases__Vendo__03F0984C");
        });

        modelBuilder.Entity<PurchaseDetail>(entity =>
        {
            entity.HasKey(e => e.PurchaseDetailId).HasName("PK__Purchase__88C328B5B302ACA6");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseD__Produ__08B54D69");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseDetails)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseD__Purch__07C12930");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__Sales__1EE3C41F54EB41FF");

            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.SaleDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<SaleDetail>(entity =>
        {
            entity.HasKey(e => e.SaleDetailId).HasName("PK__SaleDeta__70DB141E015A4B10");

            entity.Property(e => e.SaleDetailId).HasColumnName("SaleDetailID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.TotalPrice)
                .HasComputedColumnSql("([Quantity]*[UnitPrice])", false)
                .HasColumnType("decimal(29, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SaleDetai__Produ__7B5B524B");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SaleDetai__SaleI__7A672E12");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("PK__Stock__2C83A9E2EEC1DA02");

            entity.ToTable("Stock");

            entity.Property(e => e.StockId).HasColumnName("StockID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stock__ProductID__7E37BEF6");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Stock__UpdatedBy__00200768");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_User");

            entity.HasIndex(e => e.RoleId, "IX_User_RoleId");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(40);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_User_Roles_RoleId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
