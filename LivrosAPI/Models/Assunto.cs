using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace LivrosAPI.Models
{
    public class Assunto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodAs { get; set; }

        [MaxLength(20)]
        public required string Descricao { get; set; }

        [IgnoreDataMember]
        public ICollection<Livro_Assunto>? Livro_Assunto { get; set; }
    }
}
