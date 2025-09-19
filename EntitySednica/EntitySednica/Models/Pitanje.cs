using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitySednica.Models
{
    [Table("pitanja")]
    public class Pitanje
    {
        [Key]
        [Column("id_pitanja")]
        public int Id { get; set; }

        [Column("id_dnevni_red")]
        public int DnevniRedId { get; set; }
        public DnevniRed DnevniRed { get; set; }

        [Required]
        [Column("redni_broj")]
        public int RedniBroj { get; set; }

        [Required]
        public string Tekst { get; set; }
    }
}
