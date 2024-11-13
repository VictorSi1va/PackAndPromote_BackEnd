using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PackAndPromote.Entities
{
    [Table("LojaFaixaEtaria", Schema = "venda")]
    public class LojaFaixaEtaria
    {
        [Key]
        [Column("IdLojaFaixaEtaria")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLojaFaixaEtaria { get; set; }

        [Column("IdLoja")]
        public int IdLoja { get; set; }

        [Column("IdFaixaEtaria")]
        public int IdFaixaEtaria { get; set; }


        [ForeignKey("IdLoja")]
        public Loja Loja { get; set; }

        [ForeignKey("IdFaixaEtaria")]
        public FaixaEtaria FaixaEtaria { get; set; }
    }
}