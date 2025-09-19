using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitySednica.Models
{
    [Table("dnevni_red")]
    public class DnevniRed
    {
        [Key]
        [Column("id_dnevni_red")]
        public int Id { get; set; }

        [Column("id_sednice")]
        public int SednicaId { get; set; }
        public Sednica Sednica { get; set; }
    }
}
