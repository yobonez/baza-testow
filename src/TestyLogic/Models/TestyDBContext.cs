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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Kategoria>(entity =>
        {
            entity.HasKey(e => e.IdKategorii);

            entity.ToTable("KATEGORIE", tb =>
            {
                tb.HasTrigger("sprawdz_dodawanie_kategorii_k");
                tb.HasTrigger("sprawdz_usuwanie_kategorii_k");
            });

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

            entity.ToTable("ODPOWIEDZI", tb => tb.HasTrigger("sprawdz_pytanie_istnieje_o"));

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
                .HasConstraintName("FK_ODPOWIEDZI_PYTANIA");
        });

        modelBuilder.Entity<OdpowiedziNaPytaniaBezPoprawnosci>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ODPOWIEDZI_NA_PYTANIA_BEZ_POPRAWNOSCI");

            entity.Property(e => e.IdOdpowiedzi).HasColumnName("idOdpowiedzi");
            entity.Property(e => e.IdPytania).HasColumnName("idPytania");
            entity.Property(e => e.Odpowiedz)
                .HasMaxLength(2048)
                .IsUnicode(false);
            entity.Property(e => e.Punkty).HasColumnName("punkty");
            entity.Property(e => e.Pytanie)
                .HasMaxLength(2048)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Przedmiot>(entity =>
        {
            entity.HasKey(e => e.IdPrzedmiotu).HasName("PK__PRZEDMIO__EED8D5BB4846763D");

            entity.ToTable("PRZEDMIOTY", tb =>
            {
                tb.HasTrigger("sprawdz_dodawanie_przedmiotu_k");
                tb.HasTrigger("sprawdz_usuwanie_przedmiotu_prz");
            });

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
                .HasConstraintName("FK_PRZYNALEZNOSC_PYTANIA");
        });

        modelBuilder.Entity<PytanieOtwarte>(entity =>
        {
            entity.HasKey(e => e.IdPytania);

            entity.ToTable("PYTANIA_OTWARTE", tb =>
            {
                tb.HasTrigger("sprawdz_pytanie_istnieje_po");
                tb.HasTrigger("usun_wskazowka_po");
            });

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
                .HasConstraintName("FK_PWZ_PYTANIA");

            entity.HasOne(d => d.IdZestawuNavigation).WithMany(p => p.PytaniaWZestawachNavigation)
                .HasForeignKey(d => d.IdZestawu)
                .HasConstraintName("FK_PWZ_ZESTAWY");
        });

        modelBuilder.Entity<Pytanie>(entity =>
        {
            entity.HasKey(e => e.IdPytania);

            entity.ToTable("PYTANIA", tb =>
            {
                tb.HasTrigger("sprawdz_istnienie_otwartego_pytania_p");
                tb.HasTrigger("usun_pozostalosci_p");
            });

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

            entity.ToTable("ZESTAWY", tb =>
            {
                tb.HasTrigger("sprawdz_data_edytowana_z");
                tb.HasTrigger("sprawdz_dodawanie_zestawu");
            });

            entity.HasIndex(e => new { e.Nazwa, e.DataUtworzenia }, "INDEX_ZESTAWY_NAZWA_DATAUTW");

            entity.HasIndex(e => e.IdPrzedmiotu, "INDEX_ZESTAWY_PRZEDMIOTY");

            entity.Property(e => e.IdZestawu).HasColumnName("idZestawu");
            entity.Property(e => e.DataUtworzenia)
                .HasPrecision(0)
                .HasDefaultValueSql("(getdate())")
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
