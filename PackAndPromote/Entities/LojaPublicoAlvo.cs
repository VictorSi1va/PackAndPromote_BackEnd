using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PackAndPromote.Entities
{
    [Table("LojaPublicoAlvo", Schema = "venda")]
    public class LojaPublicoAlvo
    {
        [Key]
        [Column("IdLojaPublicoAlvo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLojaPublicoAlvo { get; set; }

        [Column("IdLoja")]
        public int IdLoja { get; set; }

        [Column("IdPublicoAlvo")]
        public int IdPublicoAlvo { get; set; }


        [ForeignKey("IdLoja")]
        public Loja Loja { get; set; }

        [ForeignKey("IdPublicoAlvo")]
        public PublicoAlvo PublicoAlvo { get; set; }
    }
}