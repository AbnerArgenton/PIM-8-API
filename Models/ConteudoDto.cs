namespace SistemaStreaming.Models
{
    public class ConteudoDto
    {
        public int ConteudoID { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? Link { get; set; }
        public int CriadorID { get; set; }
        public string? NomeCriador { get; set; }
    }
}

//Isso eh o que retorna a api quando eh postado um conteudo.
