using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackAndPromote.Entities
{
    [Table("RegiaoAlvo", Schema = "venda")]
    public class RegiaoAlvo
    {
        [Key]
        [Column("IdRegiaoAlvo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRegiaoAlvo { get; set; }

        [Column("DescricaoRegiaoAlvo")]
        public string DescricaoRegiaoAlvo { get; set; }
    }
}
