using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PackAndPromote.Entities
{
    [Table("LojaCategoria", Schema = "venda")]
    public class LojaCategoria
    {
        [Key]
        [Column("IdLojaCategoria")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLojaCategoria { get; set; }

        [Column("IdLoja")]
        public int IdLoja { get; set; }

        [Column("IdCategoria")]
        public int IdCategoria { get; set; }


        [ForeignKey("IdLoja")]
        public Loja Loja { get; set; }

        [ForeignKey("IdCategoria")]
        public Categoria Categoria { get; set; }
    }
}