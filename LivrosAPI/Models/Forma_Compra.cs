using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LivrosAPI.Models
{
    public class Forma_Compra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodFC { get; set; }

        [MaxLength(40)]
        public required string Descricao { get; set; }

        [IgnoreDataMember]
        public ICollection<Livro_Valor>? Livro_Valor { get; set; }
    }
}
