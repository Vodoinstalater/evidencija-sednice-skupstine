using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitySednica.Models
{
    [Table("stranka")]
    public class Stranka
    {
        [Key]
        [Column("id_stranke")]
        public int Id { get; set; }

        [Required]
        [Column("naziv_stranke")]
        [MaxLength(50)]
        public string Naziv { get; set; }
    }
}
