namespace PackAndPromote.Dtos
{
    public class PedidoEmbalagemSimplesDto
    {
        public int Quantidade { get; set; }

        public string DescricaoPersonalizada { get; set; }

        public string StatusPedido { get; set; }

        public DateTime? DataPedido { get; set; }

        public int IdLoja { get; set; }

        public int IdLojaDelivery { get; set; }

        public int IdLojaEmbalagem { get; set; }
    }
}
