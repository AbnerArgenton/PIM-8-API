using Microsoft.AspNetCore.Mvc;
using SistemaStreaming.Data;
using SistemaStreaming.Models;
using Microsoft.EntityFrameworkCore;

namespace SistemaStreaming.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlaylistController(AppDbContext context)
        {
            _context = context;
        }

        // Criar playlist
        [HttpPost("criar")]
        public IActionResult CreatePlaylist(string nome, int usuarioID)
        {
            if (string.IsNullOrWhiteSpace(nome) || usuarioID <= 0)
            {
                return BadRequest("Os dados da playlist são inválidos.");
            }

            var playlist = new Playlist
            {
                Nome = nome,
                UsuarioID = usuarioID
            };

            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            return CreatedAtAction(nameof(CreatePlaylist), new { id = playlist.ID }, playlist);
        }
        // Editar playlist
        [HttpPut("{playlistId}/editar")]
        public IActionResult UpdatePlaylistName(int playlistId, string novoNome)
        {
            if (string.IsNullOrWhiteSpace(novoNome))
            {
                return BadRequest("O nome da playlist não pode ser vazio.");
            }

            var playlist = _context.Playlists.Find(playlistId);
            if (playlist == null)
            {
                return NotFound("Playlist não encontrada.");
            }

            playlist.Nome = novoNome;
            _context.SaveChanges();

            return Ok("Nome da playlist atualizado com sucesso.");
        }


        // Apagar playlist
        [HttpDelete("{playlistId}/apagar")]
        public IActionResult DeletePlaylist(int playlistId)
        {
            var playlist = _context.Playlists.Find(playlistId);
            if (playlist == null)
            {
                return NotFound("Playlist não encontrada.");
            }

            _context.Playlists.Remove(playlist);
            _context.SaveChanges();
            return Ok("Playlist apagada com sucesso.");
        }

        // Adicionar conteúdo na playlist
        [HttpPost("adicionar")]
        public IActionResult AddConteudoToPlaylist(int playlistId, int conteudoId)
        {
            var playlist = _context.Playlists.Find(playlistId);
            var conteudo = _context.Conteudos.Find(conteudoId);

            if (playlist == null || conteudo == null)
            {
                return NotFound("Playlist ou conteúdo não encontrado.");
            }

            var itemPlaylist = new ItemPlaylist { PlaylistID = playlistId, ConteudoID = conteudoId };
            _context.ItemPlaylists.Add(itemPlaylist);
            _context.SaveChanges();
            return Ok("Conteúdo adicionado à playlist.");
        }


        // Listar todas as playlists do usuário
        [HttpGet("{usuarioId}/listarPlaylists")]
        public IActionResult GetUserPlaylists(int usuarioId)
        {
            var playlists = _context.Playlists
                .Where(p => p.UsuarioID == usuarioId)
                .Select(p => new
                {
                    p.ID,
                    p.Nome,
                    UsuarioID = p.UsuarioID
                }).ToList();

            if (!playlists.Any())
            {
                return NotFound("Nenhuma playlist encontrada para o usuário.");
            }

            return Ok(playlists);
        }

        // Listar todos os conteúdos da playlist
        [HttpGet("{playlistId}/listarConteudos")]
        public IActionResult GetPlaylistConteudos(int playlistId)
        {
            var playlist = _context.Playlists
                .Include(p => p.Itens)
                .ThenInclude(ip => ip.Conteudo)
                .FirstOrDefault(p => p.ID == playlistId);

            if (playlist == null)
            {
                return NotFound("Playlist não encontrada.");
            }

            var conteudos = playlist.Itens
                .Where(ip => ip.Conteudo != null)
                .Select(ip => new
                {
                    ConteudoID = ip.Conteudo!.ID,
                    Titulo = ip.Conteudo.Titulo,
                    Descricao = ip.Conteudo.Descricao,
                    Tipo = ip.Conteudo.Link
                });

            return Ok(conteudos);
        }
    }
}
