namespace SistemaStreaming.Models
{
    public class ConteudoDto
    {
        public int ConteudoID { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? Link { get; set; } = "link não disponível";
        public int CriadorID { get; set; }
        public string? NomeCriador { get; set; } = "Criador desconhecido";

        public ConteudoDto(Conteudo conteudo)
        {
            this.ConteudoID = conteudo.ID;
            this.Titulo = conteudo.Titulo;
            this.Descricao = conteudo.Descricao;
            this.CriadorID = conteudo.CriadorID;

            if (!string.IsNullOrEmpty(conteudo.Link)) this.Link = conteudo.Link;
            if (!string.IsNullOrEmpty(conteudo.Criador?.Nome)) this.NomeCriador = conteudo.Criador.Nome;
        }
    }
}

//Isso eh o que retorna a api quando eh postado um conteudo.
