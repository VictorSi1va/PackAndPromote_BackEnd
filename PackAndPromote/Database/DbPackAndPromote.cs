using Microsoft.EntityFrameworkCore;
using PackAndPromote.Entities;

namespace PackAndPromote.Database
{
    public class DbPackAndPromote : DbContext
    {
        public DbPackAndPromote (DbContextOptions<DbPackAndPromote> options) : base(options)
        {

        }

        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<FaixaEtaria> FaixaEtaria { get; set; }
        public DbSet<Loja> Loja { get; set; }
        public DbSet<LojaCategoria> LojaCategoria { get; set; }
        public DbSet<LojaFaixaEtaria> LojaFaixaEtaria { get; set; }
        public DbSet<LojaPlano> LojaPlano { get; set; }
        public DbSet<LojaPreferenciaAlvo> LojaPreferenciaAlvo { get; set; }
        public DbSet<LojaPublicoAlvo> LojaPublicoAlvo { get; set; }
        public DbSet<LojaRegiaoAlvo> LojaRegiaoAlvo { get; set; }
        public DbSet<PedidoEmbalagem> PedidoEmbalagem { get; set; }
        public DbSet<PreferenciaAlvo> PreferenciaAlvo { get; set; }
        public DbSet<PublicoAlvo> PublicoAlvo { get; set; }
        public DbSet<RegiaoAlvo> RegiaoAlvo { get; set; }
        public DbSet<Plano> Plano { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // FKs da Tabela PedidoEmbalagem
            modelBuilder.Entity<PedidoEmbalagem>()
                        .HasOne(u => u.Loja)
                        .WithMany()
                        .HasForeignKey(u => u.IdLoja);

            modelBuilder.Entity<PedidoEmbalagem>()
                        .HasOne(u => u.Loja)
                        .WithMany()
                        .HasForeignKey(u => u.IdLojaDelivery);

            modelBuilder.Entity<PedidoEmbalagem>()
                        .HasOne(u => u.Loja)
                        .WithMany()
                        .HasForeignKey(u => u.IdLojaEmbalagem);

            // FKs da Tabela Loja Categoria
            modelBuilder.Entity<LojaCategoria>()
                        .HasOne(u => u.Loja)
                        .WithMany()
                        .HasForeignKey(u => u.IdLoja);

            modelBuilder.Entity<LojaCategoria>()
                        .HasOne(u => u.Categoria)
                        .WithMany()
                        .HasForeignKey(u => u.IdCategoria);

            // FKs da Tabela Loja Faixa Etaria
            modelBuilder.Entity<LojaFaixaEtaria>()
                        .HasOne(u => u.Loja)
                        .WithMany()
                        .HasForeignKey(u => u.IdLoja);

            modelBuilder.Entity<LojaFaixaEtaria>()
                        .HasOne(u => u.FaixaEtaria)
                        .WithMany()
                        .HasForeignKey(u => u.IdFaixaEtaria);

            // FKs da Tabela Loja Plano
            modelBuilder.Entity<LojaPlano>()
                        .HasOne(u => u.Loja)
                        .WithMany()
                        .HasForeignKey(u => u.IdLoja);

            modelBuilder.Entity<LojaPlano>()
                        .HasOne(u => u.Plano)
                        .WithMany()
                        .HasForeignKey(u => u.IdPlano);

            // FKs da Tabela Loja Preferencia Alvo
            modelBuilder.Entity<LojaPreferenciaAlvo>()
                        .HasOne(u => u.Loja)
                        .WithMany()
                        .HasForeignKey(u => u.IdLoja);

            modelBuilder.Entity<LojaPreferenciaAlvo>()
                        .HasOne(u => u.PreferenciaAlvo)
                        .WithMany()
                        .HasForeignKey(u => u.IdPreferenciaAlvo);

            // FKs da Tabela Loja Publico Alvo
            modelBuilder.Entity<LojaPublicoAlvo>()
                        .HasOne(u => u.Loja)
                        .WithMany()
                        .HasForeignKey(u => u.IdLoja);

            modelBuilder.Entity<LojaPublicoAlvo>()
                        .HasOne(u => u.PublicoAlvo)
                        .WithMany()
                        .HasForeignKey(u => u.IdPublicoAlvo);

            // FKs da Tabela Loja Regiao Alvo
            modelBuilder.Entity<LojaRegiaoAlvo>()
                        .HasOne(u => u.Loja)
                        .WithMany()
                        .HasForeignKey(u => u.IdLoja);

            modelBuilder.Entity<LojaRegiaoAlvo>()
                        .HasOne(u => u.RegiaoAlvo)
                        .WithMany()
                        .HasForeignKey(u => u.IdRegiaoAlvo);

            // FK da Tabela Usuario
            modelBuilder.Entity<Usuario>()
                        .HasOne(u => u.Loja)
                        .WithMany()
                        .HasForeignKey(u => u.IdLoja);

            // FKs da Tabela UsuarioPerfil
            modelBuilder.Entity<UsuarioPerfil>()
                        .HasOne(u => u.Usuario)
                        .WithMany()
                        .HasForeignKey(u => u.IdUsuario);

            modelBuilder.Entity<UsuarioPerfil>()
                        .HasOne(u => u.Perfil)
                        .WithMany()
                        .HasForeignKey(u => u.IdPerfil);
        }
    }
}