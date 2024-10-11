using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PackAndPromote.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DbPackAndPromote _dbPackAndPromote;

        public LoginController(DbPackAndPromote context)
        {
            _dbPackAndPromote = context;
        }

        [HttpGet("ListarPerfis")]
        public ActionResult<IEnumerable<Perfil>> ListarPerfis()
        {
            return _dbPackAndPromote.Perfil.ToList();
        }

        [HttpGet("PesquisarPerfil/{id}")]
        public ActionResult<Perfil> PesquisarPerfil(int id)
        {
            var perfil = _dbPackAndPromote.Perfil.Find(id);

            if (perfil == null)
                return NotFound();

            return perfil;
        }

        [HttpPost("CriarPerfil")]
        public ActionResult<Perfil> CriarPerfil(Perfil perfil)
        {
            _dbPackAndPromote.Perfil.Add(perfil);
            _dbPackAndPromote.SaveChanges();

            return CreatedAtAction(nameof(PesquisarPerfil), new { id = perfil.IdPerfil }, perfil);
        }

        [HttpPut("AlterarPerfil/{id}")]
        public IActionResult AlterarPerfil(int id, PerfilDto perfilDto)
        {
            var perfil = _dbPackAndPromote.Perfil.Find(id);

            if (perfil == null)
                return NotFound();

            perfil.NomePerfil = perfilDto.NomePerfil;
            perfil.DescricaoPerfil = perfilDto.DescricaoPerfil;

            _dbPackAndPromote.SaveChanges();

            return Ok(perfil);
        }

        [HttpDelete("ExcluirPerfil/{id}")]
        public IActionResult ExcluirPerfil(int id)
        {
            var perfil = _dbPackAndPromote.Perfil.Find(id);

            if (perfil == null)
                return NotFound();

            _dbPackAndPromote.Perfil.Remove(perfil);
            _dbPackAndPromote.SaveChanges();

            return Ok();
        }

        [HttpGet("ListarUsuarios")]
        public ActionResult<IEnumerable<Usuario>> ListarUsuarios()
        {
            var usuarios = _dbPackAndPromote.Usuario
                                            .ToList();

            return usuarios;
        }

        [HttpGet("PesquisarUsuario/{id}")]
        public ActionResult<Usuario> PesquisarUsuario(int id)
        {
            var usuario = _dbPackAndPromote.Usuario.Find(id);

            if (usuario == null)
                return NotFound();

            return usuario;
        }

        // TODO Linkar com Perfil e com uma nova loja criada
        // Método para criar usuário
        [HttpPost("CriarUsuario")]
        public ActionResult<Usuario> CriarUsuario(UsuarioDto usuarioDto)
        {
            Usuario usuario = new Usuario
            {
                Login = usuarioDto.Login,
                Senha = usuarioDto.Senha
            };

            _dbPackAndPromote.Usuario.Add(usuario);
            _dbPackAndPromote.SaveChanges();

            return CreatedAtAction(nameof(PesquisarUsuario), new { id = usuario.IdUsuario }, usuario);
        }

        [HttpPut("AlterarUsuario/{id}")]
        public IActionResult AlterarUsuario(int id, UsuarioDto usuarioDto)
        {
            var usuario = _dbPackAndPromote.Usuario.Find(id);

            if (usuario == null)
                return NotFound();

            usuario.Login = usuarioDto.Login;
            usuario.Senha = usuarioDto.Senha;

            _dbPackAndPromote.SaveChanges();

            return Ok(usuario);
        }

        [HttpDelete("ExcluirUsuario/{id}")]
        public IActionResult ExcluirUsuario(int id)
        {
            var usuario = _dbPackAndPromote.Usuario.Find(id);

            if (usuario == null)
                return NotFound();

            _dbPackAndPromote.Usuario.Remove(usuario);
            _dbPackAndPromote.SaveChanges();

            return Ok();
        }
    }
}
