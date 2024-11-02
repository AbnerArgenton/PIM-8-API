using Microsoft.AspNetCore.Mvc;
using SistemaStreaming.Data;
using SistemaStreaming.Models;

namespace SistemaStreaming.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // Criar um novo usuário
        [HttpPost("criar")]
        public IActionResult CreateUsuario(string nome, string email, string pais)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Dados do usuário são inválidos.");
            }

            var usuario = new Usuario
            {
                Nome = nome,
                Email = email,
                Pais = pais
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return CreatedAtAction(nameof(CreateUsuario), new { id = usuario.ID }, usuario);
        }

        // Atualizar informações do usuário
        [HttpPut("atualizar")]
        public IActionResult UpdateUsuario(int id, string nome, string email, string pais)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            usuario.Nome = nome;
            usuario.Email = email;
            usuario.Pais = pais;

            _context.SaveChanges();
            return Ok(new { message = "Usuário atualizado com sucesso." });
        }


        // Visualizar usuário por ID
        [HttpGet("{id}")]
        public IActionResult GetUsuarioById(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(usuario);
        }

        // Listar todos os usuários
        [HttpGet("todos")]
        public IActionResult GetAllUsuarios()
        {
            var usuarios = _context.Usuarios.ToList();
            return Ok(usuarios);
        }

        // Deletar usuário
        [HttpDelete("deletar/{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound(new
                {
                    message = "Usuário não encontrado.",
                });
            }

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            // Retornar uma mensagem JSON confirmando a exclusão
            return Ok(new
            {
                message = "Usuário removido com sucesso.",
            });
        }


    }
}
