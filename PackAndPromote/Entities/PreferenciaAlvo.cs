using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackAndPromote.Entities
{
    [Table("PreferenciaAlvo", Schema = "venda")]
    public class PreferenciaAlvo
    {
        [Key]
        [Column("IdPreferenciaAlvo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPreferenciaAlvo { get; set; }

        [Column("DescricaoPreferenciaAlvo")]
        public string DescricaoPreferenciaAlvo { get; set; }
    }
}
