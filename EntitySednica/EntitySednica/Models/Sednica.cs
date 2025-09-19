using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitySednica.Models
{
    [Table("sednica")]
    public class Sednica
    {
        [Key]
        [Column("id_sednice")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Naziv { get; set; }

        [Required]
        public DateTime Datum { get; set; }

        [Required]
        public string Opis { get; set; }

        [Column("zasedanje_id")]
        public int ZasedanjeId { get; set; }
        public Zasedanje Zasedanje { get; set; }
    }
}
