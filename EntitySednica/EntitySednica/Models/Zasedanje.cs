using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitySednica.Models
{
    [Table("zasedanje")]
    public class Zasedanje
    {
        [Key]
        [Column("id_zasedanja")]
        public int Id { get; set; }

        [Column("tip")]
        public int TipZasedanjaId { get; set; }
        public TipZasedanja TipZasedanja { get; set; }

        [Required]
        [Column("naziv_zasedanja")]
        [MaxLength(50)]
        public string Naziv { get; set; }

        [Column("id_saziv")]
        public int SazivId { get; set; }
        public Saziv Saziv { get; set; }
    }
}
