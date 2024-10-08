using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PackAndPromote.Entities
{
    [Table("Perfil", Schema = "acesso")]
    public class Perfil
    {
        [Key]
        [Column("IdPerfil")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPerfil { get; set; }

        [Column("NomePerfil")]
        public string NomePerfil { get; set; }

        [Column("DescricaoPerfil")]
        public string DescricaoPerfil { get; set; }
    }
}
