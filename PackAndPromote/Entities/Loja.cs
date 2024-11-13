using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackAndPromote.Entities
{
    [Table("Loja", Schema = "venda")]
    public class Loja
    {
        [Key]
        [Column("IdLoja")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLoja { get; set; }

        [Column("NomeLoja")]
        public string NomeLoja { get; set; }

        [Column("EnderecoLoja")]
        public string EnderecoLoja { get; set; }

        [Column("DescricaoLoja")]
        public string DescricaoLoja { get; set; }

        [Column("TelefoneLoja")]
        public string TelefoneLoja { get; set; }

        [Column("CNPJLoja")]
        public string CNPJLoja { get; set; }

        [Column("EmailLoja")]
        public string EmailLoja { get; set; }

        [Column("DataCriacao")]
        public DateTime DataCriacao { get; set; }
    }
}