using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackAndPromote.Entities
{
    [Table("PedidoEmbalagem", Schema = "venda")]
    public class PedidoEmbalagem
    {
        [Key]
        [Column("IdPedidoEmbalagem")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPedidoEmbalagem { get; set; }

        [Column("Quantidade")]
        public int Quantidade { get; set; }

        [Column("DescricaoPersonalizada")]
        public string DescricaoPersonalizada { get; set; }

        [Column("StatusPedido")]
        public string StatusPedido { get; set; }

        [Column("DataPedido")]
        public DateTime? DataPedido { get; set; }

        [Column("IdLoja")]
        public int IdLoja { get; set; }

        [Column("IdLojaDelivery")]
        public int IdLojaDelivery { get; set; }

        [Column("IdLojaEmbalagem")]
        public int IdLojaEmbalagem { get; set; }


        [ForeignKey("IdLoja")]
        public Loja Loja { get; set; }

        [ForeignKey("IdLojaDelivery")]
        public Loja LojaDelivery { get; set; }

        [ForeignKey("IdLojaEmbalagem")]
        public Loja LojaEmbalagem { get; set; }
    }
}
