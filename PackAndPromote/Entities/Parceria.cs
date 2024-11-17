using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackAndPromote.Entities
{
    [Table("Parceria", Schema = "venda")]
    public class Parceria
    {
        [Key]
        [Column("IdParceria")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdParceria { get; set; }

        [Column("IdLojaPioneer")]
        public int IdLojaPioneer { get; set; }

        [Column("IdLojaPacker")]
        public int? IdLojaPacker { get; set; }

        [Column("IdLojaPromoter")]
        public int IdLojaPromoter { get; set; }

        [Column("StatusAtual")]
        public string StatusAtual { get; set; }


        [ForeignKey("IdLojaPioneer")]
        public Loja LojaPioneer { get; set; }

        [ForeignKey("IdLojaPromoter")]
        public Loja LojaPromoter { get; set; }
    }
}