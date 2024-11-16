using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackAndPromote.Entities
{
    [Table("LojaImagem", Schema = "venda")]
    public class LojaImagem
    {
        [Key]
        [Column("IdLojaImagem")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLojaImagem { get; set; }

        [Column("IdLoja")]
        public int IdLoja { get; set; }

        [Column("IdImagem")]
        public int IdImagem { get; set; }


        [ForeignKey("IdLoja")]
        public Loja Loja { get; set; }

        [ForeignKey("IdImagem")]
        public Imagem Imagem { get; set; }
    }
}
