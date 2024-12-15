using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace LivrosAPI.Models
{
    public class Livro_Assunto
    {
        [ForeignKey("Livro")]
        public int Livro_Codl { get; set; }

        [ForeignKey("Assunto")]
        public int Assunto_codAs { get; set; }

        [IgnoreDataMember]
        public Livro? Livro { get; set; }

        [IgnoreDataMember]
        public Assunto? Assunto { get; set; }
    }
}
