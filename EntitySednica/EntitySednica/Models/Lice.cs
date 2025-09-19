using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitySednica.Models
{
    [Table("lica")]
    public class Lice
    {
        [Key]
        [Column("id_lica")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Ime { get; set; }

        [Required]
        [MaxLength(50)]
        public string Prezime { get; set; }

        [Column("pozicija")]
        public int PozicijaId { get; set; }
        public Pozicija Pozicija { get; set; }

        [Column("stranka")]
        public int StrankaId { get; set; }
        public Stranka Stranka { get; set; }

        [Required]
        [MaxLength(1)]
        public string Pol { get; set; }

        [Required]
        [Column("datumr")]
        public DateTime DatumRodjenja { get; set; }

        [Required]
        public string Bio { get; set; }

        [Required]
        [Column("korisnicko_ime")]
        [MaxLength(50)]
        public string KorisnickoIme { get; set; }

        [Required]
        [MaxLength(50)]
        public string Lozinka { get; set; }
    }
}
