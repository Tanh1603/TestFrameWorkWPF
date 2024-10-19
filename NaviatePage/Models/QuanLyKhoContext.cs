﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NaviatePage.Models.Data;

namespace NaviatePage.Models;

public partial class QuanLyKhoContext : DbContext
{
    public QuanLyKhoContext(DbContextOptions<QuanLyKhoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Input> Inputs { get; set; }

    public virtual DbSet<Inputinfo> Inputinfos { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Output> Outputs { get; set; }

    public virtual DbSet<Outputinfo> Outputinfos { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Idcustomer).HasName("pk_customer");

            entity.ToTable("customer");

            entity.Property(e => e.Idcustomer).HasColumnName("idcustomer");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Contractdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractdate");
            entity.Property(e => e.Displayname).HasColumnName("displayname");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.Moreinfo).HasColumnName("moreinfo");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Input>(entity =>
        {
            entity.HasKey(e => e.Idinput).HasName("pk_input");

            entity.ToTable("input");

            entity.Property(e => e.Idinput)
                .HasMaxLength(128)
                .HasColumnName("idinput");
            entity.Property(e => e.Dateinput)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateinput");
        });

        modelBuilder.Entity<Inputinfo>(entity =>
        {
            entity.HasKey(e => e.Idinputinfo).HasName("pk_inputinfo");

            entity.ToTable("inputinfo");

            entity.Property(e => e.Idinputinfo)
                .HasMaxLength(128)
                .HasColumnName("idinputinfo");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Idinput)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("idinput");
            entity.Property(e => e.Idmaterial)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("idmaterial");
            entity.Property(e => e.Inputprice)
                .HasDefaultValueSql("0")
                .HasColumnName("inputprice");
            entity.Property(e => e.Outputprice)
                .HasDefaultValueSql("0")
                .HasColumnName("outputprice");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdinputNavigation).WithMany(p => p.Inputinfos)
                .HasForeignKey(d => d.Idinput)
                .HasConstraintName("inputinfo_idinput_fkey");

            entity.HasOne(d => d.IdmaterialNavigation).WithMany(p => p.Inputinfos)
                .HasForeignKey(d => d.Idmaterial)
                .HasConstraintName("inputinfo_idmaterial_fkey");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Idmaterial).HasName("pk_material");

            entity.ToTable("material");

            entity.Property(e => e.Idmaterial)
                .HasMaxLength(128)
                .HasColumnName("idmaterial");
            entity.Property(e => e.Barcode).HasColumnName("barcode");
            entity.Property(e => e.Displayname).HasColumnName("displayname");
            entity.Property(e => e.Idsupplier).HasColumnName("idsupplier");
            entity.Property(e => e.Idunit).HasColumnName("idunit");
            entity.Property(e => e.Qrcode).HasColumnName("qrcode");

            entity.HasOne(d => d.IdsupplierNavigation).WithMany(p => p.Materials)
                .HasForeignKey(d => d.Idsupplier)
                .HasConstraintName("material_idsupplier_fkey");

            entity.HasOne(d => d.IdunitNavigation).WithMany(p => p.Materials)
                .HasForeignKey(d => d.Idunit)
                .HasConstraintName("material_idunit_fkey");
        });

        modelBuilder.Entity<Output>(entity =>
        {
            entity.HasKey(e => e.Idoutput).HasName("pk_output");

            entity.ToTable("output");

            entity.Property(e => e.Idoutput)
                .HasMaxLength(128)
                .HasColumnName("idoutput");
            entity.Property(e => e.Dateoutput)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateoutput");
        });

        modelBuilder.Entity<Outputinfo>(entity =>
        {
            entity.HasKey(e => e.Idoutputinfo).HasName("pk_outputinfo");

            entity.ToTable("outputinfo");

            entity.Property(e => e.Idoutputinfo)
                .HasMaxLength(128)
                .HasColumnName("idoutputinfo");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Idcustomer).HasColumnName("idcustomer");
            entity.Property(e => e.Idmaterial)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("idmaterial");
            entity.Property(e => e.Idoutput)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("idoutput");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdcustomerNavigation).WithMany(p => p.Outputinfos)
                .HasForeignKey(d => d.Idcustomer)
                .HasConstraintName("outputinfo_idcustomer_fkey");

            entity.HasOne(d => d.IdmaterialNavigation).WithMany(p => p.Outputinfos)
                .HasForeignKey(d => d.Idmaterial)
                .HasConstraintName("outputinfo_idmaterial_fkey");

            entity.HasOne(d => d.IdoutputNavigation).WithMany(p => p.Outputinfos)
                .HasForeignKey(d => d.Idoutput)
                .HasConstraintName("outputinfo_idoutput_fkey");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Idsupplier).HasName("pk_supplier");

            entity.ToTable("supplier");

            entity.Property(e => e.Idsupplier).HasColumnName("idsupplier");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Contractdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractdate");
            entity.Property(e => e.Displayname).HasColumnName("displayname");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.Moreinfo).HasColumnName("moreinfo");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.Idunit).HasName("pk_unit");

            entity.ToTable("unit");

            entity.Property(e => e.Idunit).HasColumnName("idunit");
            entity.Property(e => e.Displayname).HasColumnName("displayname");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("pk_users");

            entity.ToTable("users");

            entity.Property(e => e.Iduser).HasColumnName("iduser");
            entity.Property(e => e.Displayname).HasColumnName("displayname");
            entity.Property(e => e.Iduserrole).HasColumnName("iduserrole");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.Email)
                    .IsRequired() // Có thể thiết lập là bắt buộc hay không
                    .HasMaxLength(255) // Thiết lập độ dài tối đa
                    .HasColumnName("email"); // Tên cột trong cơ sở dữ liệu

            entity.HasOne(d => d.IduserroleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Iduserrole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_iduserrole_fkey");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.Iduserrole).HasName("pk_userrole");

            entity.ToTable("userrole");

            entity.Property(e => e.Iduserrole).HasColumnName("iduserrole");
            entity.Property(e => e.Displayname).HasColumnName("displayname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}