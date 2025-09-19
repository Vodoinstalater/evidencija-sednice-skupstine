using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitySednica.Models
{
    [Table("mandat")]
    public class Mandat
    {
        [Key]
        [Column("id_mandata")]
        public int Id { get; set; }

        [Column("id_lica")]
        public int LiceId { get; set; }
        public Lice Lice { get; set; }

        [Column("id_saziva")]
        public int SazivId { get; set; }
        public Saziv Saziv { get; set; }

        [Column("id_stranke")]
        public int StrankaId { get; set; }
        public Stranka Stranka { get; set; }
    }
}
