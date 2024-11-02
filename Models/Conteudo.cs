namespace SistemaStreaming.Models
{
    public class Conteudo
    {
        public int ID { get; set; }
        public required string Titulo { get; set; }
        public required string Descricao { get; set; }
        public required string Link { get; set; }
        public int CriadorID { get; set; }
        public Criador? Criador { get; set; }
    }
}
