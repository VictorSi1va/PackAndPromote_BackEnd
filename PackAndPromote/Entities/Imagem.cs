using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackAndPromote.Entities
{
    [Table("Imagem", Schema = "acesso")]
    public class Imagem
    {
        [Key]
        [Column("IdImagem")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdImagem { get; set; }

        [Column("DadosImagem")]
        public byte[] DadosImagem { get; set; }

        [Column("TipoExtensao")]
        public string TipoExtensao { get; set; }

        [Column("NomeImagem")]
        public string NomeImagem { get; set; }

        [Column("DataCriacao")]
        public DateTime DataCriacao { get; set; }
    }
}