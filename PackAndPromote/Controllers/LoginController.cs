using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PackAndPromote.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DbPackAndPromote _dbPackAndPromote;
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration, DbPackAndPromote context)
        {
            _configuration = configuration;
            _dbPackAndPromote = context;
        }

        [Authorize]
        [HttpGet("ListarPerfis")]
        public ActionResult<IEnumerable<Perfil>> ListarPerfis()
        {
            return _dbPackAndPromote.Perfil.ToList();
        }

        [Authorize]
        [HttpGet("PesquisarPerfil/{id}")]
        public ActionResult<Perfil> PesquisarPerfil(int id)
        {
            var perfil = _dbPackAndPromote.Perfil.Find(id);

            if (perfil == null)
            {
                return NotFound();
            }

            return perfil;
        }

        [Authorize]
        [HttpPost("CriarPerfil")]
        public ActionResult<Perfil> CriarPerfil(PerfilDto perfilDto)
        {
            if (string.IsNullOrWhiteSpace(perfilDto.NomePerfil))
                return BadRequest("O Nome do Perfil não pode ser vazio ou nulo.");

            if (string.IsNullOrWhiteSpace(perfilDto.DescricaoPerfil))
                return BadRequest("A descrição do Perfil não pode ser vazio ou nulo.");

            Perfil perfil = new Perfil
            {
                NomePerfil = perfilDto.NomePerfil,
                DescricaoPerfil = perfilDto.DescricaoPerfil
            };

            _dbPackAndPromote.Perfil.Add(perfil);
            _dbPackAndPromote.SaveChanges();

            return CreatedAtAction(nameof(PesquisarPerfil), new { id = perfil.IdPerfil }, perfil);
        }

        [Authorize]
        [HttpPut("AlterarPerfil/{id}")]
        public IActionResult AlterarPerfil(int id, PerfilDto perfilDto)
        {
            var perfil = _dbPackAndPromote.Perfil.Find(id);

            if (perfil == null)
            {  
                return NotFound(); 
            }

            perfil.NomePerfil = perfilDto.NomePerfil;
            perfil.DescricaoPerfil = perfilDto.DescricaoPerfil;

            _dbPackAndPromote.SaveChanges();

            return Ok(perfil);
        }

        [Authorize]
        [HttpDelete("ExcluirPerfil/{id}")]
        public IActionResult ExcluirPerfil(int id)
        {
            var perfil = _dbPackAndPromote.Perfil.Find(id);

            if (perfil == null)
            {
                return NotFound();
            }

            _dbPackAndPromote.Perfil.Remove(perfil);
            _dbPackAndPromote.SaveChanges();

            return Ok("Perfil excluído com sucesso!");
        }

        [Authorize]
        [HttpGet("ListarUsuarios")]
        public ActionResult<IEnumerable<UsuarioSimplesDto>> ListarUsuarios()
        {
            var usuarios = _dbPackAndPromote.Usuario
                                            .Select(xs => new UsuarioSimplesDto
                                            {
                                                IdUsuario = xs.IdUsuario,
                                                Login = xs.Login,
                                                IdLoja = xs.IdLoja,
                                            })
                                            .ToList();

            return usuarios;
        }

        [Authorize]
        [HttpGet("PesquisarUsuario/{id}")]
        public ActionResult<UsuarioSimplesDto> PesquisarUsuario(int id)
        {
            var usuario = _dbPackAndPromote.Usuario.Find(id);

            if (usuario == null)
            {
                return NotFound();
            }

            UsuarioSimplesDto user = new UsuarioSimplesDto
            {
                IdUsuario = usuario.IdUsuario,
                Login = usuario.Login,
                IdLoja = usuario.IdLoja
            };

            return user;
        }

        [Authorize]
        [HttpPost("CriarUsuario")]
        public ActionResult<Usuario> CriarUsuario(UsuarioDto novoUsuarioDto)
        {
            var loja = new Loja()
            {
                NomeLoja = novoUsuarioDto.NomeLoja,
                EnderecoLoja = novoUsuarioDto.EnderecoLoja,
                DescricaoLoja = novoUsuarioDto.DescricaoLoja,
                TelefoneLoja = novoUsuarioDto.TelefoneLoja,
                CNPJLoja = novoUsuarioDto.CNPJLoja,
                EmailLoja = novoUsuarioDto.EmailLoja,
                DataCriacao = DateTime.Now,
                IdCategoria = novoUsuarioDto.IdCategoria,
                IdPublicoAlvo = novoUsuarioDto.IdPublicoAlvo,
                IdFaixaEtaria = novoUsuarioDto.IdFaixaEtaria,
                IdRegiaoAlvo = novoUsuarioDto.IdRegiaoAlvo,
                IdPreferenciaAlvo = novoUsuarioDto.IdPreferenciaAlvo
            };

            _dbPackAndPromote.Loja.Add(loja);
            _dbPackAndPromote.SaveChanges();

            Usuario usuario = new Usuario
            {
                Login = novoUsuarioDto.Login,
                Senha = novoUsuarioDto.Senha, // TODO Criptografar a senha
                IdLoja = loja.IdLoja
            };

            _dbPackAndPromote.Usuario.Add(usuario);
            _dbPackAndPromote.SaveChanges();

            var usuarioPerfil = new UsuarioPerfil()
            {
                IdUsuario = usuario.IdUsuario,
                IdPerfil = 2 // Perfil Cliente
            };

            _dbPackAndPromote.UsuarioPerfil.Add(usuarioPerfil);
            _dbPackAndPromote.SaveChanges();

            return CreatedAtAction(nameof(PesquisarUsuario), new { id = usuario.IdUsuario }, usuario);
        }

        [HttpPost("Entrar")]
        public IActionResult Entrar(LoginDto loginDto)
        {
            var usuario = _dbPackAndPromote.Usuario
                                           .SingleOrDefault(u => u.Login == loginDto.Login &&
                                                                 u.Senha == loginDto.Senha);

            if (usuario == null)
            {
                return Unauthorized("Login e/ou senha inválidos.");
            }

            // Gerar Token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecretKey = _configuration["Jwt:SecretKey"]; // Chave JWT
            var key = Encoding.ASCII.GetBytes(jwtSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Login.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Token válido por 2 horas
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

        [HttpPut("RedefinirSenha/{id}")]
        public IActionResult RedefinirSenha(int id, NovaSenhaDto novaSenhaDto)
        {
            var usuario = _dbPackAndPromote.Usuario.Find(id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            usuario.Senha = novaSenhaDto.NovaSenha; // TODO Criptografar a senha
            _dbPackAndPromote.SaveChanges();

            return Ok("Senha alterada com sucesso.");
        }

        [Authorize]
        [HttpPut("AlterarUsuario/{id}")]
        public IActionResult AlterarUsuario(int id, UsuarioAlteradoDto usuarioAlteradoDto)
        {
            var usuario = _dbPackAndPromote.Usuario.Find(id);

            if (usuario == null)
            {
                return NotFound();
            }

            var loja = _dbPackAndPromote.Loja.Find(usuario.IdLoja);

            if (loja != null)
            {
                loja.NomeLoja = usuarioAlteradoDto.NomeLoja;
                loja.EnderecoLoja = usuarioAlteradoDto.EnderecoLoja;
                loja.DescricaoLoja = usuarioAlteradoDto.DescricaoLoja;
                loja.TelefoneLoja = usuarioAlteradoDto.TelefoneLoja;
                loja.CNPJLoja = usuarioAlteradoDto.CNPJLoja;
                loja.EmailLoja = usuarioAlteradoDto.EmailLoja;

                loja.IdCategoria = usuarioAlteradoDto.IdCategoria;
                loja.IdPublicoAlvo = usuarioAlteradoDto.IdPublicoAlvo;
                loja.IdFaixaEtaria = usuarioAlteradoDto.IdFaixaEtaria;
                loja.IdRegiaoAlvo = usuarioAlteradoDto.IdRegiaoAlvo;
                loja.IdPreferenciaAlvo = usuarioAlteradoDto.IdPreferenciaAlvo;
            }

            _dbPackAndPromote.SaveChanges();

            return Ok("Usuário alterado com sucesso!");
        }

        [Authorize]
        [HttpDelete("ExcluirUsuario/{id}")]
        public IActionResult ExcluirUsuario(int id)
        {
            var usuario = _dbPackAndPromote.Usuario.Find(id);

            if (usuario == null)
            {
                return NotFound();
            }

            var loja = _dbPackAndPromote.Loja.Find(usuario.IdLoja);
            var usuarioPerfil = _dbPackAndPromote.UsuarioPerfil.Find(usuario.IdUsuario);

            if (loja == null || usuarioPerfil == null)
            {
                return NotFound();
            }

            _dbPackAndPromote.Loja.Remove(loja);
            _dbPackAndPromote.UsuarioPerfil.Remove(usuarioPerfil);
            _dbPackAndPromote.Usuario.Remove(usuario);
            _dbPackAndPromote.SaveChanges();

            return Ok("Usuário excluído com sucesso!");
        }
    }
}
