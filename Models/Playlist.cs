namespace SistemaStreaming.Models
{
    public class Playlist
    {
        public int ID { get; set; }
        public required string Nome { get; set; }
        public int UsuarioID { get; set; }
        public Usuario? Usuario { get; set; }

        // Adiciona uma coleção de itens de playlist
        public List<ItemPlaylist> Itens { get; set; } = new();
    }
}
