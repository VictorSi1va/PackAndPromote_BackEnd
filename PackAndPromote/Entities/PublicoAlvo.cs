using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackAndPromote.Entities
{
    [Table("PublicoAlvo", Schema = "venda")]
    public class PublicoAlvo
    {
        [Key]
        [Column("IdPublicoAlvo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPublicoAlvo { get; set; }

        [Column("DescricaoPublicoAlvo")]
        public string DescricaoPublicoAlvo { get; set; }
    }
}
