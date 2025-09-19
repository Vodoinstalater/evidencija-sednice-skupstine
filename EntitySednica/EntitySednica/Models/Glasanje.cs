using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitySednica.Models
{
    [Table("glasanje")]
    public class Glasanje
    {
        [Key]
        [Column("id_glasanja")]
        public int Id { get; set; }

        [Column("id_pitanja")]
        public int PitanjeId { get; set; }
        public Pitanje Pitanje { get; set; }

        [Column("id_lica")]
        public int LiceId { get; set; }
        public Lice Lice { get; set; }

        [Required]
        [MaxLength(10)]
        public string Glas { get; set; }
    }
}
