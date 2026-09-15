using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Grupo06_Caso01.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Movimientosinventario> Movimientosinventarios { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=ep-autumn-waterfall-a5kuewk8-pooler.us-east-2.aws.neon.tech;Port=5432;Database=neondb;Username=neondb_owner;Password=npg_W2EGpUaRZ7YK;SSL Mode=Require;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Categoriaid).HasName("categorias_pkey");

            entity.ToTable("categorias");

            entity.Property(e => e.Categoriaid).HasColumnName("categoriaid");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Movimientosinventario>(entity =>
        {
            entity.HasKey(e => e.Movimientoid).HasName("movimientosinventario_pkey");

            entity.ToTable("movimientosinventario");

            entity.Property(e => e.Movimientoid).HasColumnName("movimientoid");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha");
            entity.Property(e => e.Productoid).HasColumnName("productoid");
            entity.Property(e => e.Tipomovimiento)
                .HasMaxLength(10)
                .HasColumnName("tipomovimiento");

            entity.HasOne(d => d.Producto).WithMany(p => p.Movimientosinventarios)
                .HasForeignKey(d => d.Productoid)
                .HasConstraintName("fk_movimientos_productos");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Pedidoid).HasName("pedidos_pkey");

            entity.ToTable("pedidos");

            entity.Property(e => e.Pedidoid).HasColumnName("pedidoid");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pendiente'::character varying")
                .HasColumnName("estado");
            entity.Property(e => e.Fechapedido)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fechapedido");
            entity.Property(e => e.Proveedorid).HasColumnName("proveedorid");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.Proveedorid)
                .HasConstraintName("fk_pedidos_proveedores");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Productoid).HasName("productos_pkey");

            entity.ToTable("productos");

            entity.Property(e => e.Productoid).HasColumnName("productoid");
            entity.Property(e => e.Categoriaid).HasColumnName("categoriaid");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");
            entity.Property(e => e.Stockactual).HasColumnName("stockactual");
            entity.Property(e => e.Stockminimo)
                .HasDefaultValue(5)
                .HasColumnName("stockminimo");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.Categoriaid)
                .HasConstraintName("fk_productos_categorias");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.Proveedorid).HasName("proveedores_pkey");

            entity.ToTable("proveedores");

            entity.Property(e => e.Proveedorid).HasColumnName("proveedorid");
            entity.Property(e => e.Contacto)
                .HasMaxLength(100)
                .HasColumnName("contacto");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
