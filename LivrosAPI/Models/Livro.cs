using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LivrosAPI.Models
{
    public class Livro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Codl { get; set; }

        [MaxLength(40)]
        public required string Titulo { get; set; }

        [MaxLength(40)]
        public required string Editora { get; set; }

        public int Edicao { get; set; }

        [MaxLength(4)]
        public required string AnoPublicacao { get; set; }

        [IgnoreDataMember]
        public ICollection<Livro_Autor>? Livro_Autor { get; set; }

        [IgnoreDataMember]
        public ICollection<Livro_Assunto>? Livro_Assunto { get; set; }

        [IgnoreDataMember]
        public ICollection<Livro_Valor>? Livro_Valor { get; set; }
    }
}
