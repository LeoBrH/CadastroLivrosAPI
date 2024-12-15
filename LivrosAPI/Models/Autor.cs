using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;

namespace LivrosAPI.Models
{
    public class Autor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodAu { get; set; }

        [MaxLength(40)]
        public required string Nome { get; set; }

        [IgnoreDataMember]
        public ICollection<Livro_Autor>? Livro_Autor { get; set; }
    }
}
