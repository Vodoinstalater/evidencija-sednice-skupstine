using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitySednica.Models
{
    [Table("saziv")]
    public class Saziv
    {
        [Key]
        [Column("id_saziva")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Ime { get; set; }

        [Required]
        public DateTime Pocetak { get; set; }

        [Required]
        public DateTime Kraj { get; set; }

        [Required]
        public string Opis { get; set; }
    }
}
