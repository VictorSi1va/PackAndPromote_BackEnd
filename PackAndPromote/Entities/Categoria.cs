using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PackAndPromote.Entities
{
    [Table("Categoria", Schema = "venda")]
    public class Categoria
    {
        [Key]
        [Column("IdCategoria")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCategoria { get; set; }

        [Column("NomeCategoria")]
        public string NomeCategoria { get; set; }
    }
}