using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PackAndPromote.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region Variáveis e Construtor

        // Campos privados que armazenam a instância do banco de dados e as configurações
        private readonly DbPackAndPromote _dbPackAndPromote;
        private readonly IConfiguration _configuration;

        // Construtor da controller que injeta as dependências de configuração e contexto do banco
        public LoginController(IConfiguration configuration, DbPackAndPromote context)
        {
            _configuration = configuration;
            _dbPackAndPromote = context;
        }

        #endregion

        #region Login - Métodos

        #region Perfil

        #region Listar Perfis
        // Endpoint para listar todos os perfis armazenados no banco de dados
        [HttpGet("ListarPerfis")]
        public ActionResult<IEnumerable<Perfil>> ListarPerfis()
        {
            // Obtém e retorna todos os perfis armazenados no banco de dados
            return _dbPackAndPromote.Perfil.ToList(); // Retorna uma lista com todos os perfis
        }
        #endregion

        #region Pesquisar Perfil por Id
        // Endpoint para pesquisar um perfil específico pelo ID
        [Authorize] // Requer autorização para acessar este endpoint
        [HttpGet("PesquisarPerfil/{id}")]
        public ActionResult<Perfil> PesquisarPerfil(int id)
        {
            // Busca o perfil pelo ID no banco de dados
            var perfil = _dbPackAndPromote.Perfil.Find(id);

            // Verifica se o perfil foi encontrado
            if (perfil == null)
            {
                return NotFound(); // Retorna 404 se o perfil não for encontrado
            }

            // Retorna o perfil encontrado
            return perfil;
        }
        #endregion

        #region Criar Perfil
        // Endpoint para criar um novo perfil com base nos dados enviados
        [Authorize] // Requer autorização para acessar este endpoint
        [HttpPost("CriarPerfil")]
        public ActionResult<Perfil> CriarPerfil(PerfilDto perfilDto)
        {
            // Verifica se o nome e descrição do perfil são válidos
            if (string.IsNullOrWhiteSpace(perfilDto.NomePerfil) || string.IsNullOrWhiteSpace(perfilDto.DescricaoPerfil))
                return BadRequest("O Nome ou Descrição do Perfil não pode ser vazio."); // Retorna erro 400 se inválido

            // Cria um novo objeto de perfil com os dados fornecidos
            Perfil perfil = new Perfil
            {
                NomePerfil = perfilDto.NomePerfil,
                DescricaoPerfil = perfilDto.DescricaoPerfil
            };

            // Adiciona o novo perfil ao banco de dados e salva as alterações
            _dbPackAndPromote.Perfil.Add(perfil);
            _dbPackAndPromote.SaveChanges();

            // Retorna o perfil criado com a localização (URL) do recurso criado
            return CreatedAtAction(nameof(PesquisarPerfil), new { id = perfil.IdPerfil }, perfil);
        }
        #endregion

        #region Alterar Perfil
        // Endpoint para alterar os dados de um perfil existente pelo ID
        [Authorize] // Requer autorização para acessar este endpoint
        [HttpPut("AlterarPerfil/{id}")]
        public IActionResult AlterarPerfil(int id, PerfilDto perfilDto)
        {
            // Busca o perfil pelo ID no banco de dados
            var perfil = _dbPackAndPromote.Perfil.Find(id);

            // Verifica se o perfil existe
            if (perfil == null)
            {
                return NotFound(); // Retorna 404 se o perfil não for encontrado
            }

            // Atualiza os dados do perfil com os valores fornecidos
            perfil.NomePerfil = perfilDto.NomePerfil;
            perfil.DescricaoPerfil = perfilDto.DescricaoPerfil;

            // Salva as alterações no banco de dados
            _dbPackAndPromote.SaveChanges();

            // Retorna o perfil atualizado como resposta
            return Ok(perfil);
        }
        #endregion

        #region Excluir Perfil
        // Endpoint para excluir um perfil específico pelo ID
        [Authorize] // Requer autorização para acessar este endpoint
        [HttpDelete("ExcluirPerfil/{id}")]
        public IActionResult ExcluirPerfil(int id)
        {
            // Busca o perfil pelo ID no banco de dados
            var perfil = _dbPackAndPromote.Perfil.Find(id);

            // Verifica se o perfil existe
            if (perfil == null)
            {
                return NotFound(); // Retorna 404 se o perfil não for encontrado
            }

            // Remove o perfil do banco de dados e salva as alterações
            _dbPackAndPromote.Perfil.Remove(perfil);
            _dbPackAndPromote.SaveChanges();

            // Retorna uma mensagem de confirmação
            return Ok("Perfil excluído com sucesso!");
        }
        #endregion

        #endregion

        #region Usuário

        #region Listar Usuários
        [Authorize] // Requer autorização para acessar este endpoint
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
        #endregion

        #region Pesquisar Usuário por Id
        [Authorize] // Requer autorização para acessar este endpoint
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
        #endregion

        #region Criar Usuário
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
        #endregion

        #region Entrar
        // Endpoint para login e geração de um token JWT para autenticação
        [HttpPost("Entrar")]
        public IActionResult Entrar(LoginDto loginDto)
        {
            // Verifica se o usuário existe e se a senha está correta
            var usuario = _dbPackAndPromote.Usuario
                                           .SingleOrDefault(u => u.Login == loginDto.Login &&
                                                                 u.Senha == loginDto.Senha);

            // Retorna erro 401 se o login ou senha estiver incorreto
            if (usuario == null)
            {
                return Unauthorized("Login e/ou senha inválidos.");
            }

            // Cria um manipulador de token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            // Lê a chave secreta para o JWT das configurações da aplicação
            var jwtSecretKey = _configuration["Jwt_SecretKey"];
            var key = Encoding.ASCII.GetBytes(jwtSecretKey);

            // Configura os dados do token (claims, expiração e credenciais)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()), // ID do usuário
                    new Claim(ClaimTypes.Name, usuario.Login) // Login do usuário
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Define a expiração do token para 2 horas
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Configura a assinatura com a chave secreta
            };

            // Gera o token JWT
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token); // Converte o token para string

            // Retorna o token JWT gerado como resposta
            return Ok(new { Token = tokenString });
        }
        #endregion

        #region Redefinir Senha
        [HttpPut("RedefinirSenha/{id}")]
        public IActionResult RedefinirSenha(int id, NovaSenhaDto novaSenhaDto)
        {
            // Busca o usuário pelo ID fornecido
            var usuario = _dbPackAndPromote.Usuario.Find(id);

            // Se o usuário não for encontrado, retorna um status 404 NotFound
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Atualiza a senha do usuário com a nova senha fornecida
            usuario.Senha = novaSenhaDto.NovaSenha; // TODO Criptografar a senha

            // Salva as alterações no banco de dados
            _dbPackAndPromote.SaveChanges();

            // Retorna um status 200 OK com uma mensagem de sucesso
            return Ok("Senha alterada com sucesso.");
        }
        #endregion

        #region Alterar Usuário
        [Authorize] // Requer autorização para acessar este endpoint
        [HttpPut("AlterarUsuario/{id}")]
        public IActionResult AlterarUsuario(int id, UsuarioAlteradoDto usuarioAlteradoDto)
        {
            // Busca o usuário pelo ID fornecido
            var usuario = _dbPackAndPromote.Usuario.Find(id);

            // Se o usuário não for encontrado, retorna um status 404 NotFound
            if (usuario == null)
            {
                return NotFound();
            }

            // Busca a loja associada ao usuário
            var loja = _dbPackAndPromote.Loja.Find(usuario.IdLoja);

            // Se a loja for encontrada, atualiza os dados da loja
            if (loja != null)
            {
                loja.NomeLoja = usuarioAlteradoDto.NomeLoja;
                loja.EnderecoLoja = usuarioAlteradoDto.EnderecoLoja;
                loja.DescricaoLoja = usuarioAlteradoDto.DescricaoLoja;
                loja.TelefoneLoja = usuarioAlteradoDto.TelefoneLoja;
                loja.CNPJLoja = usuarioAlteradoDto.CNPJLoja;
                loja.EmailLoja = usuarioAlteradoDto.EmailLoja;

                // Atualiza as informações adicionais da loja
                loja.IdCategoria = usuarioAlteradoDto.IdCategoria;
                loja.IdPublicoAlvo = usuarioAlteradoDto.IdPublicoAlvo;
                loja.IdFaixaEtaria = usuarioAlteradoDto.IdFaixaEtaria;
                loja.IdRegiaoAlvo = usuarioAlteradoDto.IdRegiaoAlvo;
                loja.IdPreferenciaAlvo = usuarioAlteradoDto.IdPreferenciaAlvo;
            }

            // Salva as alterações no banco de dados
            _dbPackAndPromote.SaveChanges();

            // Retorna um status 200 OK com uma mensagem de sucesso
            return Ok("Usuário alterado com sucesso!");
        }
        #endregion

        #region Excluir Usuário
        [Authorize] // Requer autorização para acessar este endpoint
        [HttpDelete("ExcluirUsuario/{id}")]
        public IActionResult ExcluirUsuario(int id)
        {
            // Busca o usuário pelo ID fornecido
            var usuario = _dbPackAndPromote.Usuario.Find(id);

            // Se o usuário não for encontrado, retorna um status 404 NotFound
            if (usuario == null)
            {
                return NotFound();
            }

            // Busca a loja e o perfil de usuário associados
            var loja = _dbPackAndPromote.Loja.Find(usuario.IdLoja);
            var usuarioPerfil = _dbPackAndPromote.UsuarioPerfil.Find(usuario.IdUsuario);

            // Se a loja ou o perfil do usuário não forem encontrados, retorna NotFound
            if (loja == null || usuarioPerfil == null)
            {
                return NotFound();
            }

            // Remove a loja, o perfil do usuário e o usuário do banco de dados
            _dbPackAndPromote.Loja.Remove(loja);
            _dbPackAndPromote.UsuarioPerfil.Remove(usuarioPerfil);
            _dbPackAndPromote.Usuario.Remove(usuario);

            // Salva as alterações no banco de dados
            _dbPackAndPromote.SaveChanges();

            // Retorna um status 200 OK com uma mensagem de sucesso
            return Ok("Usuário excluído com sucesso!");
        }
        #endregion

        #endregion

        #endregion
    }
}