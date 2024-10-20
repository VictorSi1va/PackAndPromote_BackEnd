namespace PackAndPromote.Dtos
{
    public class PedidoEmbalagemDto
    {
        public int IdPedidoEmbalagem { get; set; }

        public int Quantidade { get; set; }

        public string DescricaoPersonalizada { get; set; }

        public int IdLoja { get; set; }

        public string NomeLoja { get; set; }

        public int IdLojaDelivery { get; set; }

        public string NomeLojaDelivery { get; set; }

        public int IdLojaEmbalagem { get; set; }

        public string NomeLojaEmbalagem { get; set; }

        public string StatusPedido { get; set; }

        public DateTime? DataPedido { get; set; }
    }
}
