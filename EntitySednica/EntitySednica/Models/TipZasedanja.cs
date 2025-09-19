using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitySednica.Models
{
    [Table("tipovi")]
    public class TipZasedanja
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("tip_zasedanja")]
        [MaxLength(50)]
        public string Tip { get; set; }
    }
}
