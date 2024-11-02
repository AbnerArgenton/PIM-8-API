using Microsoft.AspNetCore.Mvc;
using SistemaStreaming.Data;
using SistemaStreaming.Models;

namespace SistemaStreaming.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CriadorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CriadorController(AppDbContext context)
        {
            _context = context;
        }

        // Criar um novo criador
        [HttpPost("criar")]
        public IActionResult CreateCriador(string nome, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Dados do criador são inválidos.");
            }

            var criador = new Criador
            {
                Nome = nome,
                Email = email
            };

            _context.Criadores.Add(criador);
            _context.SaveChanges();
            return CreatedAtAction(nameof(CreateCriador), new { id = criador.ID }, criador);
        }

        // Atualizar informações do criador
        [HttpPut("atualizar")]
        public IActionResult UpdateCriador(int id, string nome, string email)
        {
            var criador = _context.Criadores.Find(id);
            if (criador == null)
            {
                return NotFound("Criador não encontrado.");
            }

            criador.Nome = nome;
            criador.Email = email;

            _context.SaveChanges();
            return Ok("Criador atualizado com sucesso.");
        }

        // Visualizar criador por ID
        [HttpGet("{id}")]
        public IActionResult GetCriadorById(int id)
        {
            var criador = _context.Criadores.Find(id);
            if (criador == null)
            {
                return NotFound("Criador não encontrado.");
            }

            return Ok(criador);
        }

        // Listar todos os criadores
        [HttpGet("todos")]
        public IActionResult GetAllCriadores()
        {
            var criadores = _context.Criadores.ToList();
            return Ok(criadores);
        }

        // Deletar criador
        [HttpDelete("deletar")]
        public IActionResult DeleteCriador(int id)
        {
            var criador = _context.Criadores.Find(id);
            if (criador == null)
            {
                return NotFound("Criador não encontrado.");
            }

            _context.Criadores.Remove(criador);
            _context.SaveChanges();
            return Ok("Criador removido com sucesso.");
        }
    }
}
