﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PortalWeb_API.Models;

namespace PortalWeb_API.Data;

public partial class PortalWebContext : DbContext
{
    public PortalWebContext(DbContextOptions<PortalWebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AsignModUser> AsignModUser { get; set; }

    public virtual DbSet<AsignacionEquipos> AsignacionEquipos { get; set; }

    public virtual DbSet<CentroProceso> CentroProceso { get; set; }

    public virtual DbSet<Clientes> Clientes { get; set; }

    public virtual DbSet<CuentaSignaTienda> CuentaSignaTienda { get; set; }

    public virtual DbSet<CuentasBancarias> CuentasBancarias { get; set; }

    public virtual DbSet<DatosPersonales> DatosPersonales { get; set; }

    public virtual DbSet<Depositos> Depositos { get; set; }

    public virtual DbSet<Equipos> Equipos { get; set; }

    public virtual DbSet<EquiposTemporales> EquiposTemporales { get; set; }

    public virtual DbSet<ErrorAlerts> ErrorAlerts { get; set; }

    public virtual DbSet<ManualDepositos> ManualDepositos { get; set; }

    public virtual DbSet<Marca> Marca { get; set; }

    public virtual DbSet<MasterEquipos> MasterEquipos { get; set; }

    public virtual DbSet<MasterTable> MasterTable { get; set; }

    public virtual DbSet<Modelo> Modelo { get; set; }

    public virtual DbSet<ModulosApp> ModulosApp { get; set; }

    public virtual DbSet<Recolecciones> Recolecciones { get; set; }

    public virtual DbSet<RespuestaSentencia> RespuestaSentencia { get; set; }

    public virtual DbSet<Tiendas> Tiendas { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    public virtual DbSet<UsuariosPortal> UsuariosPortal { get; set; }

    public virtual DbSet<UsuariosTemporales> UsuariosTemporales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CentroProceso>(entity =>
        {
            entity.Property(e => e.Fecrea).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Clientes>(entity =>
        {
            entity.HasKey(e => e.CodigoCliente).HasName("PK_Cliente");

            entity.ToTable(tb => tb.HasComment("Tabla que tiene los clientes que tienen los servicios"));

            entity.Property(e => e.Active).HasDefaultValueSql("('A')");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<CuentasBancarias>(entity =>
        {
            entity.Property(e => e.Fecrea).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<DatosPersonales>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValueSql("('A')");
        });

        modelBuilder.Entity<Depositos>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValueSql("('A')");
            entity.Property(e => e.DepositoBill1).HasDefaultValueSql("((0))");
            entity.Property(e => e.DepositoBill10).HasDefaultValueSql("((0))");
            entity.Property(e => e.DepositoBill100).HasDefaultValueSql("((0))");
            entity.Property(e => e.DepositoBill2).HasDefaultValueSql("((0))");
            entity.Property(e => e.DepositoBill20).HasDefaultValueSql("((0))");
            entity.Property(e => e.DepositoBill5).HasDefaultValueSql("((0))");
            entity.Property(e => e.DepositoBill50).HasDefaultValueSql("((0))");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.TotalDepositoBill1).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill10).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill100).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill2).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill20).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill5).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill50).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill1).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill10).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill100).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill2).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill20).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill5).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill50).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin1).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin10).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin100).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin25).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin5).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin50).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<Equipos>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.SerieEquipo }).HasName("PK_Equipos1_1");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Active).HasDefaultValueSql("('A')");
        });

        modelBuilder.Entity<EquiposTemporales>(entity =>
        {
            entity.HasKey(e => e.SerieEquipo).HasName("PK__EquiposT__7A57188BF2A131DD");

            entity.Property(e => e.Active).HasDefaultValueSql("('A')");
            entity.Property(e => e.Fecrea).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ErrorAlerts>(entity =>
        {
            entity.Property(e => e.Active).IsFixedLength();
        });

        modelBuilder.Entity<ManualDepositos>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValueSql("('A')");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ManualDepositoBill1).HasDefaultValueSql("((0))");
            entity.Property(e => e.ManualDepositoBill10).HasDefaultValueSql("((0))");
            entity.Property(e => e.ManualDepositoBill100).HasDefaultValueSql("((0))");
            entity.Property(e => e.ManualDepositoBill2).HasDefaultValueSql("((0))");
            entity.Property(e => e.ManualDepositoBill20).HasDefaultValueSql("((0))");
            entity.Property(e => e.ManualDepositoBill5).HasDefaultValueSql("((0))");
            entity.Property(e => e.ManualDepositoBill50).HasDefaultValueSql("((0))");
            entity.Property(e => e.ManualDepositoCoin1).HasDefaultValueSql("((0))");
            entity.Property(e => e.ManualDepositoCoin10).HasDefaultValueSql("((0))");
            entity.Property(e => e.ManualDepositoCoin100).HasDefaultValueSql("((0))");
            entity.Property(e => e.ManualDepositoCoin25).HasDefaultValueSql("((0))");
            entity.Property(e => e.ManualDepositoCoin5).HasDefaultValueSql("((0))");
            entity.Property(e => e.ManualDepositoCoin50).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill1).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill10).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill100).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill2).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill20).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill5).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill50).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill1).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill10).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill100).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill2).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill20).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill5).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill50).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin1).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin10).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin100).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin25).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin5).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin50).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => new { e.Codigotipomaq, e.Codmarca }).HasName("PK_modelo1");

            entity.Property(e => e.Codmarca).IsFixedLength();
        });

        modelBuilder.Entity<MasterEquipos>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValueSql("('A')");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<MasterTable>(entity =>
        {
            entity.Property(e => e.Campo1).IsFixedLength();
            entity.Property(e => e.Campo2).IsFixedLength();
            entity.Property(e => e.Codigo).IsFixedLength();
            entity.Property(e => e.Gestion)
                .HasDefaultValueSql("('')")
                .IsFixedLength();
            entity.Property(e => e.Master).IsFixedLength();
            entity.Property(e => e.Nomtag)
                .HasDefaultValueSql("(' ')")
                .IsFixedLength();
            entity.Property(e => e.Sgrupo).IsFixedLength();
            entity.Property(e => e.Valor).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<Modelo>(entity =>
        {
            entity.HasKey(e => new { e.Codigotipomaq, e.Codmodelo, e.Codmarca }).HasName("PK_marca1_1");

            entity.Property(e => e.Codmodelo).IsFixedLength();
            entity.Property(e => e.Codmarca).IsFixedLength();
        });

        modelBuilder.Entity<Recolecciones>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValueSql("('A')");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.TotalDepositoBill1).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill10).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill100).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill2).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill20).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill5).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalDepositoBill50).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill1).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill10).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill100).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill2).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill20).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill5).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoBill50).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin1).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin10).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin100).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin25).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin5).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalManualDepositoCoin50).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<Tiendas>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValueSql("('A')");
            entity.Property(e => e.CodProv).IsFixedLength();
            entity.Property(e => e.Fecreate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValueSql("('A')");
        });

        modelBuilder.Entity<UsuariosTemporales>(entity =>
        {
            entity.Property(e => e.Active).HasDefaultValueSql("('A')");
            entity.Property(e => e.Fecrea).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}