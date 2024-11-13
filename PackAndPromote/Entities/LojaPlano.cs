using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PackAndPromote.Entities
{
    [Table("LojaPlano", Schema = "venda")]
    public class LojaPlano
    {
        [Key]
        [Column("IdLojaPlano")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLojaPlano { get; set; }

        [Column("IdLoja")]
        public int IdLoja { get; set; }

        [Column("IdPlano")]
        public int IdPlano { get; set; }


        [ForeignKey("IdLoja")]
        public Loja Loja { get; set; }

        [ForeignKey("IdPlano")]
        public Plano Plano { get; set; }
    }
}