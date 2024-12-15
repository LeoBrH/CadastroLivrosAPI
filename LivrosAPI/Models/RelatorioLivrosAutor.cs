namespace LivrosAPI.Models
{
    public class RelatorioLivrosAutor
    {
        public required string Autor { get; set; }
        public string? Titulo { get; set; }
        public string? Editora { get; set; }
        public int Edicao { get; set; }
        public string? AnoPublicacao { get; set; }
        public string? Assunto { get; set; }
        public string? FormaCompra { get; set; }
        public double Valor { get; set; }
    }
}
