using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitySednica.Models
{
    [Table("pozicija")]
    public class Pozicija
    {
        [Key]
        [Column("id_pozicije")]
        public int Id { get; set; }

        [Required]
        [Column("naziv_pozicije")]
        [MaxLength(50)]
        public string Naziv { get; set; }
    }
}
