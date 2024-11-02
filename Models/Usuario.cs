namespace SistemaStreaming.Models
{
    public class Usuario
    {
        public int ID { get; set; }
        public string? Nome { get; set; }
        public required string Email { get; set; }
        public string? Pais { get; set; }
    }
}
