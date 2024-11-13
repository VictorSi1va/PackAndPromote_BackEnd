using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PackAndPromote.Entities
{
    [Table("LojaRegiaoAlvo", Schema = "venda")]
    public class LojaRegiaoAlvo
    {
        [Key]
        [Column("IdLojaRegiaoAlvo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLojaRegiaoAlvo { get; set; }

        [Column("IdLoja")]
        public int IdLoja { get; set; }

        [Column("IdRegiaoAlvo")]
        public int IdRegiaoAlvo { get; set; }


        [ForeignKey("IdLoja")]
        public Loja Loja { get; set; }

        [ForeignKey("IdRegiaoAlvo")]
        public RegiaoAlvo RegiaoAlvo { get; set; }
    }
}