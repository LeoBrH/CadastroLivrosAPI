using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace LivrosAPI.Models
{
    public class Livro_Autor
    {
        [ForeignKey("Livro")]
        public int Livro_Codl { get; set; }

        [ForeignKey("Autor")]
        public int Autor_CodAu { get; set; }

        [IgnoreDataMember]
        public Livro? Livro { get; set; }

        [IgnoreDataMember]
        public Autor? Autor { get; set; }
    }
}
