using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PackAndPromote.Entities
{
    [Table("LojaPreferenciaAlvo", Schema = "venda")]
    public class LojaPreferenciaAlvo
    {
        [Key]
        [Column("IdLojaPreferenciaAlvo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLojaPreferenciaAlvo { get; set; }

        [Column("IdLoja")]
        public int IdLoja { get; set; }

        [Column("IdPreferenciaAlvo")]
        public int IdPreferenciaAlvo { get; set; }


        [ForeignKey("IdLoja")]
        public Loja Loja { get; set; }

        [ForeignKey("IdPreferenciaAlvo")]
        public PreferenciaAlvo PreferenciaAlvo { get; set; }
    }
}