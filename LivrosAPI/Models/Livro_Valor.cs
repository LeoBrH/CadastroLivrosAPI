using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LivrosAPI.Models
{
    public class Livro_Valor
    {
        [ForeignKey("Livro")]
        public int Livro_Codl { get; set; }

        [ForeignKey("Forma_Compra")]
        public int Forma_Compra_CodFC { get; set; }

        public required double Valor { get; set; }

        [IgnoreDataMember]
        public Livro? Livro { get; set; }

        [IgnoreDataMember]
        public Forma_Compra? Forma_Compra { get; set; }
    }
}
