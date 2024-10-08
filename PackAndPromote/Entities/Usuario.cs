using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PackAndPromote.Entities
{
    [Table("Usuario", Schema = "acesso")]
    public class Usuario
    {
        [Key]
        [Column("IdUsuario")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Column("Login")]
        public string Login { get; set; }

        [Column("Senha")]
        public string Senha { get; set; }

        [Column("IdLoja")]
        public int IdLoja { get; set; }


        [ForeignKey("IdLoja")]
        public Loja Loja { get; set; }
    }
}
