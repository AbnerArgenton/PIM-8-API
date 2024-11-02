using Microsoft.AspNetCore.Mvc;
using SistemaStreaming.Data;
using SistemaStreaming.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;

namespace SistemaStreaming.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConteudoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConteudoController(AppDbContext context)
        {
            _context = context;
        }

        // Postar conteúdo
        [HttpPost("postar")]
        [SwaggerOperation(Summary = "Cria um novo conteúdo", Description = "Adiciona um conteúdo ao banco de dados.")]
        [SwaggerResponse(201, "Conteúdo criado com sucesso.", typeof(ConteudoDto))]
        [SwaggerResponse(400, "Erro ao criar conteúdo.")]
        public IActionResult PostConteudo(string titulo, string descricao, string link, int criadorID)
        {
            if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(descricao) || string.IsNullOrWhiteSpace(link) || criadorID <= 0)
            {
                return BadRequest("Dados do conteúdo são inválidos.");
            }

            var criador = _context.Criadores.Find(criadorID);
            if (criador == null)
            {
                return BadRequest("Criador não encontrado.");
            }

            var conteudo = new Conteudo
            {
                Titulo = titulo,
                Descricao = descricao,
                Link = link, // Usando o link do vídeo ou áudio
                CriadorID = criadorID
            };

            _context.Conteudos.Add(conteudo);
            _context.SaveChanges();

            var conteudoDto = new ConteudoDto(conteudo);

            return CreatedAtAction(nameof(PostConteudo), new { id = conteudoDto.ConteudoID }, conteudoDto);
        }

        // Editar conteúdo
        [HttpPut("{conteudoId}/editar")]
        public IActionResult UpdateConteudo(int conteudoId, string novoTitulo, string novaDescricao, string novoLink)
        {
            var conteudo = _context.Conteudos.Find(conteudoId);
            if (conteudo == null)
            {
                return NotFound("Conteúdo não encontrado.");
            }

            conteudo.Titulo = novoTitulo;
            conteudo.Descricao = novaDescricao;
            conteudo.Link = novoLink;

            _context.SaveChanges();
            return Ok("Conteúdo atualizado com sucesso.");
        }

        // Apagar conteúdo
        [HttpDelete("{conteudoId}/deletar")]
        public IActionResult DeleteConteudo(int conteudoId)
        {
            var conteudo = _context.Conteudos.Find(conteudoId);
            if (conteudo == null)
            {
                return NotFound("Conteúdo não encontrado.");
            }

            _context.Conteudos.Remove(conteudo);
            _context.SaveChanges();
            return Ok("Conteúdo removido com sucesso.");
        }

        // Visualizar conteúdo pelo ID
        [HttpGet("{conteudoId}")]
        public IActionResult GetConteudoById(int conteudoId)
        {
            var conteudo = _context.Conteudos
                .Include(c => c.Criador)
                .FirstOrDefault(c => c.ID == conteudoId);

            if (conteudo == null)
            {
                return NotFound("Conteúdo não encontrado.");
            }

            var conteudoDto = new ConteudoDto(conteudo);

            return Ok(conteudoDto);
        }

        // Visualizar todos os conteúdos de um criador
        [HttpGet("criador/{criadorId}")]
        public IActionResult GetConteudosByCriador(int criadorId)
        {
            var conteudos = _context.Conteudos
                .Where(c => c.CriadorID == criadorId)
                .Select(conteudo => new ConteudoDto(conteudo))
                .ToList();

            if (!conteudos.Any())
            {
                return NotFound("Nenhum conteúdo encontrado para este criador.");
            }

            return Ok(conteudos);
        }


        // Visualizar todos os conteúdos gerais
        [HttpGet("todos")]
        public IActionResult GetAllConteudos()
        {
            // Busca todos os conteúdos, tratando valores nulos
            var conteudos = _context.Conteudos
                .Select(conteudo => new ConteudoDto(conteudo)).ToList();

            // Verifica se há conteúdos
            if (!conteudos.Any())
            {
                return NotFound("Nenhum conteúdo encontrado.");
            }

            return Ok(conteudos);
        }
    }
}
