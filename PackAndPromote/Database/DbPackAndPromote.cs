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

            // FKs da Tabela Loja
            modelBuilder.Entity<Loja>()
                        .HasOne(u => u.Categoria)
                        .WithMany()
                        .HasForeignKey(u => u.IdCategoria);

            modelBuilder.Entity<Loja>()
                        .HasOne(u => u.PublicoAlvo)
                        .WithMany()
                        .HasForeignKey(u => u.IdPublicoAlvo);

            modelBuilder.Entity<Loja>()
                        .HasOne(u => u.FaixaEtaria)
                        .WithMany()
                        .HasForeignKey(u => u.IdFaixaEtaria);

            modelBuilder.Entity<Loja>()
                        .HasOne(u => u.RegiaoAlvo)
                        .WithMany()
                        .HasForeignKey(u => u.IdRegiaoAlvo);

            modelBuilder.Entity<Loja>()
                        .HasOne(u => u.PreferenciaAlvo)
                        .WithMany()
                        .HasForeignKey(u => u.IdPreferenciaAlvo);

            modelBuilder.Entity<Loja>()
                        .HasOne(u => u.Plano)
                        .WithMany()
                        .HasForeignKey(u => u.IdPlano);

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
