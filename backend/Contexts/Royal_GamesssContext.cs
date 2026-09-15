using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoyalGamess.Domains;

namespace RoyalGamess.Contexts;

public partial class Royal_GamesssContext : DbContext
{
    public Royal_GamesssContext()
    { //1236757
      //1236805
    }

    public Royal_GamesssContext(DbContextOptions<Royal_GamesssContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClassificacaoIndicativa> ClassificacaoIndicativa { get; set; }

    public virtual DbSet<Genero> Genero { get; set; }

    public virtual DbSet<Jogo> Jogo { get; set; }

    public virtual DbSet<JogoPromocao> JogoPromocao { get; set; }

    public virtual DbSet<Log_Alteracao_Jogo> Log_Alteracao_Jogo { get; set; }

    public virtual DbSet<Plataforma> Plataforma { get; set; }

    public virtual DbSet<Promocao> Promocao { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=royal_games;Username=postgres;Password=postgres");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClassificacaoIndicativa>(entity =>
        {
            entity.HasKey(e => e.ClassificacaoIndicativaId).HasName("PK__Classifi__892DEC0FC2472ACC");

            entity.Property(e => e.Classificacao)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.GeneroId).HasName("PK__Genero__A99D02480A23E34E");

            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Jogo>(entity =>
        {
            entity.HasKey(e => e.JogoId).HasName("PK__Jogo__59196835D39D3B47");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("trg_AlteracaoJogo");
                    tb.HasTrigger("trg_excluirJogo");
                });

            entity.Property(e => e.Descricao).HasMaxLength(255);
            entity.Property(e => e.Nome)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Preco).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StatusJogo).HasDefaultValue(true);

            entity.HasOne(d => d.ClassificaçãoIdFKNavigation).WithMany(p => p.Jogo)
                .HasForeignKey(d => d.ClassificaçãoIdFK)
                .HasConstraintName("FK__Jogo__Classifica__5812160E");

            entity.HasOne(d => d.UsuarioIdFKNavigation).WithMany(p => p.Jogo)
                .HasForeignKey(d => d.UsuarioIdFK)
                .HasConstraintName("FK__Jogo__UsuarioIdF__571DF1D5");

            entity.HasMany(d => d.GeneroIdFK).WithMany(p => p.JogoIdFK)
                .UsingEntity<Dictionary<string, object>>(
                    "JogoGenero",
                    r => r.HasOne<Genero>().WithMany()
                        .HasForeignKey("GeneroIdFK")
                        .HasConstraintName("FK_JogoGenero_Genero"),
                    l => l.HasOne<Jogo>().WithMany()
                        .HasForeignKey("JogoIdFK")
                        .HasConstraintName("FK_JogoGenero_Jogo"),
                    j =>
                    {
                        j.HasKey("JogoIdFK", "GeneroIdFK").HasName("Jogo_Genero_Id_FK");
                    });
        });

        modelBuilder.Entity<JogoPromocao>(entity =>
        {
            entity.HasKey(e => new { e.JogoIdFK, e.PromocaoIdFK }).HasName("Jogo_Promocao_Id_FK");

            entity.Property(e => e.PrecoAtual).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.JogoIdFKNavigation).WithMany(p => p.JogoPromocao)
                .HasForeignKey(d => d.JogoIdFK)
                .HasConstraintName("FK_JogoPromocao_Jogo");

            entity.HasOne(d => d.PromocaoIdFKNavigation).WithMany(p => p.JogoPromocao)
                .HasForeignKey(d => d.PromocaoIdFK)
                .HasConstraintName("FK_JogoPromocao_Promocao");
        });

        modelBuilder.Entity<Log_Alteracao_Jogo>(entity =>
        {
            entity.HasKey(e => e.Log_Alteracao_Jogo_Id).HasName("PK__Log_Alte__B35E86FA67A5A1FA");

            entity.Property(e => e.DataAlteracao).HasPrecision(0);
            entity.Property(e => e.NomeAnterior)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrecoAnterior).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Jogo).WithMany(p => p.Log_Alteracao_Jogo)
                .HasForeignKey(d => d.JogoId)
                .HasConstraintName("FK__Log_Alter__JogoI__5AEE82B9");
        });

        modelBuilder.Entity<Plataforma>(entity =>
        {
            entity.HasKey(e => e.PlataformaId).HasName("PK__Platafor__B83567EDE3A08254");

            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(d => d.JogoIdFK).WithMany(p => p.PlataformaIdFK)
                .UsingEntity<Dictionary<string, object>>(
                    "JogoPlataforma",
                    r => r.HasOne<Jogo>().WithMany()
                        .HasForeignKey("JogoIdFK")
                        .HasConstraintName("FK_JogoPlataforma_Jogo"),
                    l => l.HasOne<Plataforma>().WithMany()
                        .HasForeignKey("PlataformaIdFK")
                        .HasConstraintName("FK_JogoPlataforma_Plataforma"),
                    j =>
                    {
                        j.HasKey("PlataformaIdFK", "JogoIdFK").HasName("Jogo_Plataforma_Id_FK");
                    });
        });

        modelBuilder.Entity<Promocao>(entity =>
        {
            entity.HasKey(e => e.PromocaoId).HasName("PK__Promocao__254B581D2A4149D6");

            entity.Property(e => e.DataExpiração).HasPrecision(0);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuario__2B3DE7B84DAB1A5D");

            entity.ToTable(tb => tb.HasTrigger("trg_ExclusaoUsuario"));

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D10534645E0462").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Nome).HasMaxLength(60);
            entity.Property(e => e.Senha).HasMaxLength(32);
            entity.Property(e => e.StatusUsuario).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
