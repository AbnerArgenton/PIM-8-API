using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaStreaming.Models
{
    public class ItemPlaylist
    {
        [Key, Column(Order = 0)]
        public int PlaylistID { get; set; }

        [Key, Column(Order = 1)]
        public int ConteudoID { get; set; }

        // Navegação para Playlist e Conteúdo
        public virtual Playlist? Playlist { get; set; }
        public virtual Conteudo? Conteudo { get; set; }
    }
}
