using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackAndPromote.Entities
{
    [Table("Plano", Schema = "venda")]
    public class Plano
    {
        [Key]
        [Column("IdPlano")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPlano { get; set; }

        [Column("NomePlano")]
        public string NomePlano { get; set; }

        [Column("DescricaoPlano")]
        public string DescricaoPlano { get; set; }

        [Column("Custo")]
        public decimal Custo { get; set; }
    }
}
