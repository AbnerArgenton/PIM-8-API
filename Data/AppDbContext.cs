namespace SistemaStreaming.Data
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using SistemaStreaming.Models;

    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Criador> Criadores { get; set; }
        public DbSet<Conteudo> Conteudos { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<ItemPlaylist> ItemPlaylists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemPlaylist>()
                .HasKey(ip => new { ip.PlaylistID, ip.ConteudoID });

            modelBuilder.Entity<ItemPlaylist>()
                .HasOne(ip => ip.Playlist)
                .WithMany(p => p.Itens)
                .HasForeignKey(ip => ip.PlaylistID);

            modelBuilder.Entity<ItemPlaylist>()
                .HasOne(ip => ip.Conteudo)
                .WithMany()
                .HasForeignKey(ip => ip.ConteudoID);
        }
    }

}
