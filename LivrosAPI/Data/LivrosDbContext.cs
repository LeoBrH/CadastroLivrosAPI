using LivrosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LivrosAPI.Data
{
    public class LivrosDbContext : DbContext
    {
        public LivrosDbContext(DbContextOptions<LivrosDbContext> options) : base(options)
        {
        }

        public DbSet<Livro> Livro { get; set; }
        public DbSet<Autor> Autor { get; set; }
        public DbSet<Assunto> Assunto { get; set; }
        public DbSet<Forma_Compra> Forma_Compra { get; set; }
        public DbSet<Livro_Autor> Livro_Autor { get; set; }
        public DbSet<Livro_Assunto> Livro_Assunto { get; set; }
        public DbSet<Livro_Valor> Livro_Valor { get; set; }
        public DbSet<RelatorioLivrosAutor> View_LivrosPorAutor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Index keys
            modelBuilder.Entity<Livro>(b =>
            {
                b.HasIndex(i => new { i.Titulo, i.Editora, i.Edicao, i.AnoPublicacao }).IsUnique();
            });

            modelBuilder.Entity<Autor>(b =>
            {
                b.HasIndex(i => i.Nome ).IsUnique();
            });

            modelBuilder.Entity<Assunto>(b =>
            {
                b.HasIndex(i => new { i.Descricao }).IsUnique();
            });

            modelBuilder.Entity<Forma_Compra>(b =>
            {
                b.HasIndex(i => new { i.Descricao }).IsUnique();
            });

            modelBuilder.Entity<Livro_Valor>(b =>
            {
                b.HasIndex(i => new { i.Livro_Codl, i.Forma_Compra_CodFC }).IsUnique();
            });
            #endregion

            #region Livro_Autor
            modelBuilder.Entity<Livro_Autor>()
                .HasKey(la => new { la.Livro_Codl, la.Autor_CodAu });

            modelBuilder.Entity<Livro_Autor>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.Livro_Autor)
                .HasForeignKey(la => la.Livro_Codl);

            modelBuilder.Entity<Livro_Autor>()
                .HasOne(la => la.Autor)
                .WithMany(a => a.Livro_Autor)
                .HasForeignKey(la => la.Autor_CodAu);
            #endregion

            #region Livro_Assunto
            modelBuilder.Entity<Livro_Assunto>()
                .HasKey(ls => new { ls.Livro_Codl, ls.Assunto_codAs });

            modelBuilder.Entity<Livro_Assunto>()
                .HasOne(ls => ls.Livro)
                .WithMany(l => l.Livro_Assunto)
                .HasForeignKey(ls => ls.Livro_Codl);

            modelBuilder.Entity<Livro_Assunto>()
                .HasOne(ls => ls.Assunto)
                .WithMany(a => a.Livro_Assunto)
                .HasForeignKey(ls => ls.Assunto_codAs);
            #endregion

            #region Livro_Valor
            modelBuilder.Entity<Livro_Valor>()
                .HasKey(lv => new { lv.Livro_Codl, lv.Forma_Compra_CodFC });

            modelBuilder.Entity<Livro_Valor>()
                .HasOne(lv => lv.Livro)
                .WithMany(l => l.Livro_Valor)
                .HasForeignKey(lv => lv.Livro_Codl);

            modelBuilder.Entity<Livro_Valor>()
                .HasOne(lv => lv.Forma_Compra)
                .WithMany(a => a.Livro_Valor)
                .HasForeignKey(lv => lv.Forma_Compra_CodFC);
            #endregion

            #region Relatorio
            modelBuilder.Entity<RelatorioLivrosAutor>()
                .HasNoKey()
                .ToView("View_AutorLivros");
            #endregion
        }

    }
}
