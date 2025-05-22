using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestyLogic.Models;

public partial class TestyDBContext : DbContext
{
    public TestyDBContext()
    {
    }

    public TestyDBContext(DbContextOptions<TestyDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kategoria> Kategorie { get; set; }

    public virtual DbSet<Odpowiedz> Odpowiedzi { get; set; }

    public virtual DbSet<Przedmiot> Przedmioty { get; set; }

    public virtual DbSet<PrzynaleznoscPytania> PrzynaleznoscPytan { get; set; }

    public virtual DbSet<PytanieOtwarte> PytaniaOtwarte { get; set; }

    public virtual DbSet<PytanieWZestawie> PytaniaWZestawach { get; set; }

    public virtual DbSet<Pytanie> Pytania { get; set; }

    public virtual DbSet<Zestaw> Zestawy { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("ConnectionstringTODO");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kategoria>(entity =>
        {
            entity.HasKey(e => e.IdKategorii);

            entity.ToTable("KATEGORIE");

            entity.HasIndex(e => e.Nazwa, "INDEX_KATEGORIE_NAZWA");

            entity.Property(e => e.IdKategorii).HasColumnName("idKategorii");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(48)
                .IsUnicode(false)
                .HasColumnName("nazwa");
        });

        modelBuilder.Entity<Odpowiedz>(entity =>
        {
            entity.HasKey(e => e.IdOdpowiedzi);

            entity.ToTable("ODPOWIEDZI");

            entity.HasIndex(e => e.IdPytania, "INDEX_ODPOWIEDZI_PYTANIA");

            entity.Property(e => e.IdOdpowiedzi).HasColumnName("idOdpowiedzi");
            entity.Property(e => e.CzyPoprawna).HasColumnName("czyPoprawna");
            entity.Property(e => e.IdPytania).HasColumnName("idPytania");
            entity.Property(e => e.Tresc)
                .HasMaxLength(2048)
                .IsUnicode(false)
                .HasColumnName("tresc");

            entity.HasOne(d => d.IdPytaniaNavigation).WithMany(p => p.Odpowiedzi)
                .HasForeignKey(d => d.IdPytania)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ODPOWIEDZI_PYTANIA");
        });

        modelBuilder.Entity<Przedmiot>(entity =>
        {
            entity.HasKey(e => e.IdPrzedmiotu).HasName("PK__PRZEDMIO__EED8D5BBB609886C");

            entity.ToTable("PRZEDMIOTY");

            entity.HasIndex(e => e.Nazwa, "INDEX_PRZEDMIOTY_NAZWA");

            entity.Property(e => e.IdPrzedmiotu).HasColumnName("idPrzedmiotu");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("nazwa");
        });

        modelBuilder.Entity<PrzynaleznoscPytania>(entity =>
        {
            entity.HasKey(e => new { e.IdPytania, e.IdPrzedmiotu, e.IdKategorii })
                .HasName("PK_przynaleznosc")
                .IsClustered(false);

            entity.ToTable("PRZYNALEZNOSC_PYTAN");

            entity.Property(e => e.IdPytania).HasColumnName("idPytania");
            entity.Property(e => e.IdPrzedmiotu).HasColumnName("idPrzedmiotu");
            entity.Property(e => e.IdKategorii).HasColumnName("idKategorii");

            entity.HasOne(d => d.IdKategoriiNavigation).WithMany(p => p.PrzynaleznoscPytanNavigation)
                .HasForeignKey(d => d.IdKategorii)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRZYNALEZNOSC_KATEGORIE");

            entity.HasOne(d => d.IdPrzedmiotuNavigation).WithMany(p => p.PrzynaleznoscPytanNavigation)
                .HasForeignKey(d => d.IdPrzedmiotu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRZYNALEZNOSC_PRZEDMIOTY");

            entity.HasOne(d => d.IdPytaniaNavigation).WithMany(p => p.PrzynaleznoscPytanNavigation)
                .HasForeignKey(d => d.IdPytania)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRZYNALEZNOSC_PYTANIA");
        });

        modelBuilder.Entity<PytanieOtwarte>(entity =>
        {
            entity.HasKey(e => e.IdPytania);

            entity.ToTable("PYTANIA_OTWARTE");

            entity.Property(e => e.IdPytania)
                .ValueGeneratedNever()
                .HasColumnName("idPytania");
            entity.Property(e => e.Klucz)
                .HasMaxLength(2048)
                .IsUnicode(false)
                .HasColumnName("klucz");
        });

        modelBuilder.Entity<PytanieWZestawie>(entity =>
        {
            entity.HasKey(e => new { e.IdPytania, e.IdZestawu })
                .HasName("PK_pwz")
                .IsClustered(false);

            entity.ToTable("PYTANIA_W_ZESTAWACH");

            entity.HasIndex(e => e.IdPrzedmiotu, "INDEX_PWZ_PRZEDMIOTY");

            entity.Property(e => e.IdPytania).HasColumnName("idPytania");
            entity.Property(e => e.IdZestawu).HasColumnName("idZestawu");
            entity.Property(e => e.IdPrzedmiotu).HasColumnName("idPrzedmiotu");

            entity.HasOne(d => d.IdPrzedmiotuNavigation).WithMany(p => p.PytaniaWZestawachNavigation)
                .HasForeignKey(d => d.IdPrzedmiotu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PWZ_PRZEDMIOTY");

            entity.HasOne(d => d.IdPytaniaNavigation).WithMany(p => p.PytaniaWZestawachNavigation)
                .HasForeignKey(d => d.IdPytania)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PWZ_PYTANIA");

            entity.HasOne(d => d.IdZestawuNavigation).WithMany(p => p.PytaniaWZestawachNavigation)
                .HasForeignKey(d => d.IdZestawu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PWZ_ZESTAWY");
        });

        modelBuilder.Entity<Pytanie>(entity =>
        {
            entity.HasKey(e => e.IdPytania);

            entity.ToTable("PYTANIA");

            entity.Property(e => e.IdPytania).HasColumnName("idPytania");
            entity.Property(e => e.Punkty).HasColumnName("punkty");
            entity.Property(e => e.Tresc)
                .HasMaxLength(2048)
                .IsUnicode(false)
                .HasColumnName("tresc");
            entity.Property(e => e.TypPytania).HasColumnName("typPytania");
        });

        modelBuilder.Entity<Zestaw>(entity =>
        {
            entity.HasKey(e => e.IdZestawu);

            entity.ToTable("ZESTAWY");

            entity.HasIndex(e => new { e.Nazwa, e.DataUtworzenia }, "INDEX_ZESTAWY_NAZWA_DATAUTW");

            entity.HasIndex(e => e.IdPrzedmiotu, "INDEX_ZESTAWY_PRZEDMIOTY");

            entity.Property(e => e.IdZestawu).HasColumnName("idZestawu");
            entity.Property(e => e.DataUtworzenia)
                .HasPrecision(0)
                .HasColumnName("dataUtworzenia");
            entity.Property(e => e.IdPrzedmiotu).HasColumnName("idPrzedmiotu");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("nazwa");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
