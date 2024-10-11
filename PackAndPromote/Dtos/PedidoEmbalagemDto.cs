namespace PackAndPromote.Dtos
{
    public class PedidoEmbalagemDto
    {
        public int Quantidade { get; set; }

        public string DescricaoPersonalizada { get; set; }

        public string StatusPedido { get; set; }

        public DateTime? DataPedido { get; set; }
    }
}
