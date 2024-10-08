using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PackAndPromote.Entities
{
    [Table("UsuarioPerfil", Schema = "acesso")]
    public class UsuarioPerfil
    {
        [Key]
        [Column("IdUsuarioPerfil")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuarioPerfil { get; set; }

        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("IdPerfil")]
        public int IdPerfil { get; set; }


        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }

        [ForeignKey("IdPerfil")]
        public Perfil Perfil { get; set; }
    }
}
