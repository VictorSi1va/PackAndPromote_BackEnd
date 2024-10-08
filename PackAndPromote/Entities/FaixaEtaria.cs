using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackAndPromote.Entities
{
    [Table("FaixaEtaria", Schema = "venda")]
    public class FaixaEtaria
    {
        [Key]
        [Column("IdFaixaEtaria")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFaixaEtaria { get; set; }

        [Column("DescricaoFaixaEtaria")]
        public string DescricaoFaixaEtaria { get; set; }
    }
}
