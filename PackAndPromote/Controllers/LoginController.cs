using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

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

        #region Pesquisar Usuário por Id
        [Authorize] // Requer autorização para acessar este endpoint
        [HttpGet("PesquisarDadosMinhaConta/{id}")]
        public ActionResult<UsuarioMinhaContaDto> PesquisarDadosMinhaConta(int id)
        {
            var usuario = _dbPackAndPromote.Usuario.Find(id);

            if (usuario == null)
                return NotFound();

            var loja = _dbPackAndPromote.Loja.Find(usuario.IdLoja);

            if (loja == null)
                return NotFound();

            var lojaCategoria = _dbPackAndPromote.LojaCategoria
                                                 .Where(xs => xs.IdLoja == usuario.IdLoja)
                                                 .FirstOrDefault();

            if (lojaCategoria == null)
                return NotFound();

            var lojaPlano = _dbPackAndPromote.LojaPlano
                                             .Include(xs => xs.Plano)
                                             .Where(xs => xs.IdLoja == usuario.IdLoja)
                                             .FirstOrDefault();

            if (lojaPlano == null)
                return NotFound();

            List<int> lojaPublicoAlvo = _dbPackAndPromote.LojaPublicoAlvo
                                                   .Where(xs => xs.IdLoja == usuario.IdLoja)
                                                   .Select(x => x.IdPublicoAlvo)
                                                   .ToList();

            if (lojaPublicoAlvo == null)
                return NotFound();

            List<int> lojaFaixaEtaria = _dbPackAndPromote.LojaFaixaEtaria
                                                   .Where(xs => xs.IdLoja == usuario.IdLoja)
                                                   .Select(x => x.IdFaixaEtaria)
                                                   .ToList();

            if (lojaFaixaEtaria == null)
                return NotFound();

            List<int> lojaRegiaoAlvo = _dbPackAndPromote.LojaRegiaoAlvo
                                                  .Where(xs => xs.IdLoja == usuario.IdLoja)
                                                  .Select(x => x.IdRegiaoAlvo)
                                                  .ToList();

            if (lojaRegiaoAlvo == null)
                return NotFound();

            List<int> lojaPreferenciaAlvo = _dbPackAndPromote.LojaPreferenciaAlvo
                                                       .Where(xs => xs.IdLoja == usuario.IdLoja)
                                                       .Select(x => x.IdPreferenciaAlvo)
                                                       .ToList();

            if (lojaPreferenciaAlvo == null)
                return NotFound();

            int IdLojaImagem = _dbPackAndPromote.LojaImagem
                                                .Where(xs => xs.IdLoja == usuario.IdLoja)
                                                .Select(xs => xs.IdImagem)
                                                .FirstOrDefault();

            if (IdLojaImagem <= 0)
                return NotFound();

            UsuarioMinhaContaDto retorno = new UsuarioMinhaContaDto
            {
                NomeLoja = loja.NomeLoja,
                CNPJLoja = loja.CNPJLoja,
                EnderecoLoja = loja.EnderecoLoja,
                TelefoneLoja = loja.TelefoneLoja,
                EmailLoja = loja.EmailLoja,
                DescricaoLoja = loja.DescricaoLoja,

                IdCategoria = lojaCategoria.IdCategoria,
                PublicoAlvo = lojaPublicoAlvo,
                FaixaEtaria = lojaFaixaEtaria,
                RegiaoAlvo = lojaRegiaoAlvo,
                PreferenciaAlvo = lojaPreferenciaAlvo,

                IdPlano = lojaPlano.IdPlano,
                NomePlano = lojaPlano.Plano.NomePlano,
                MediaMensalEmbalagensEntreguesPlano = 8750, // TODO Buscar da tabela
                MediaDiariaEmbalagensEntreguesPlano = 25, // TODO Buscar da tabela
                ProximaRenovacaoPlano = "19/12/2024", // TODO Buscar da tabela
                PeriodoPlano = "Mensal", // TODO Buscar da tabela

                IdLojaImagem = IdLojaImagem
            };

            return retorno;
        }
        #endregion

        #region Criar Usuário
        [HttpPost("CriarUsuario")]
        public ActionResult<Usuario> CriarUsuario(UsuarioDto novoUsuarioDto)
        {
            // Validação dos dados de entrada
            if (novoUsuarioDto == null)
                return BadRequest("Os dados do usuário não podem ser nulos.");

            // Validações de campos obrigatórios
            if (string.IsNullOrEmpty(novoUsuarioDto.NomeLoja))
                return BadRequest("O nome da loja é obrigatório.");

            if (string.IsNullOrEmpty(novoUsuarioDto.EnderecoLoja))
                return BadRequest("O endereço da loja é obrigatório.");

            if (string.IsNullOrEmpty(novoUsuarioDto.TelefoneLoja))
                return BadRequest("O telefone da loja é obrigatório.");

            if (string.IsNullOrEmpty(novoUsuarioDto.CNPJLoja))
                return BadRequest("O CNPJ da loja é obrigatório.");

            // Validação de formato do CNPJ (apenas um exemplo simples)
            if (!IsValidCNPJ(novoUsuarioDto.CNPJLoja))
                return BadRequest("O CNPJ da loja é inválido.");

            if (string.IsNullOrEmpty(novoUsuarioDto.EmailLoja) || !IsValidEmail(novoUsuarioDto.EmailLoja))
                return BadRequest("O email da loja é inválido.");

            if (novoUsuarioDto.IdCategoria == 0)
                return BadRequest("A categoria da loja deve ser informada.");

            if (novoUsuarioDto.IdPlano == 0)
                return BadRequest("O plano da loja deve ser informado.");

            // Validação de listas: FaixaEtaria, PreferenciaAlvo, PublicoAlvo, RegiaoAlvo
            if (novoUsuarioDto.FaixaEtaria == null || !novoUsuarioDto.FaixaEtaria.Any())
                return BadRequest("É necessário selecionar pelo menos uma faixa etária.");

            if (novoUsuarioDto.PreferenciaAlvo == null || !novoUsuarioDto.PreferenciaAlvo.Any())
                return BadRequest("É necessário selecionar pelo menos uma preferencia alvo.");

            if (novoUsuarioDto.PublicoAlvo == null || !novoUsuarioDto.PublicoAlvo.Any())
                return BadRequest("É necessário selecionar pelo menos um público alvo.");

            if (novoUsuarioDto.RegiaoAlvo == null || !novoUsuarioDto.RegiaoAlvo.Any())
                return BadRequest("É necessário selecionar pelo menos uma região alvo.");

            // Verifica se já existe um usuário com o mesmo Login
            if (_dbPackAndPromote.Usuario.Any(u => u.Login.ToUpper() == novoUsuarioDto.Login.ToUpper()))
                return BadRequest("Já existe um usuário com esse login.");

            // Criação da loja
            var loja = new Loja()
            {
                NomeLoja = novoUsuarioDto.NomeLoja,
                EnderecoLoja = novoUsuarioDto.EnderecoLoja,
                DescricaoLoja = novoUsuarioDto.DescricaoLoja,
                TelefoneLoja = novoUsuarioDto.TelefoneLoja,
                CNPJLoja = novoUsuarioDto.CNPJLoja,
                EmailLoja = novoUsuarioDto.EmailLoja,
                DataCriacao = DateTime.Now,
            };

            // Verifica se a loja com o mesmo CNPJ já existe
            if (_dbPackAndPromote.Loja.Any(l => l.CNPJLoja == novoUsuarioDto.CNPJLoja))
                return BadRequest("Já existe uma loja com este CNPJ.");

            _dbPackAndPromote.Loja.Add(loja);
            _dbPackAndPromote.SaveChanges();

            // Criação das associações
            var lojaImagem = new LojaImagem()
            {
                IdLoja = loja.IdLoja,
                IdImagem = 3, // Imagem Default
            };
            _dbPackAndPromote.LojaImagem.Add(lojaImagem);

            var lojaCategoria = new LojaCategoria()
            {
                IdLoja = loja.IdLoja,
                IdCategoria = novoUsuarioDto.IdCategoria,
            };
            _dbPackAndPromote.LojaCategoria.Add(lojaCategoria);

            var lojaPlano = new LojaPlano()
            {
                IdLoja = loja.IdLoja,
                IdPlano = novoUsuarioDto.IdPlano,
            };
            _dbPackAndPromote.LojaPlano.Add(lojaPlano);

            // Adiciona faixas etárias
            foreach (int itemFaixaEtaria in novoUsuarioDto.FaixaEtaria)
            {
                var lojaFaixaEtaria = new LojaFaixaEtaria()
                {
                    IdLoja = loja.IdLoja,
                    IdFaixaEtaria = itemFaixaEtaria,
                };
                _dbPackAndPromote.LojaFaixaEtaria.Add(lojaFaixaEtaria);
            }

            // Adiciona preferências alvo
            foreach (int itemPreferenciaAlvo in novoUsuarioDto.PreferenciaAlvo)
            {
                var lojaPreferenciaAlvo = new LojaPreferenciaAlvo()
                {
                    IdLoja = loja.IdLoja,
                    IdPreferenciaAlvo = itemPreferenciaAlvo,
                };
                _dbPackAndPromote.LojaPreferenciaAlvo.Add(lojaPreferenciaAlvo);
            }

            // Adiciona públicos alvo
            foreach (int itemPublicoAlvo in novoUsuarioDto.PublicoAlvo)
            {
                var lojaPublicoAlvo = new LojaPublicoAlvo()
                {
                    IdLoja = loja.IdLoja,
                    IdPublicoAlvo = itemPublicoAlvo,
                };
                _dbPackAndPromote.LojaPublicoAlvo.Add(lojaPublicoAlvo);
            }

            // Adiciona regiões alvo
            foreach (int itemRegiaoAlvo in novoUsuarioDto.RegiaoAlvo)
            {
                var lojaRegiaoAlvo = new LojaRegiaoAlvo()
                {
                    IdLoja = loja.IdLoja,
                    IdRegiaoAlvo = itemRegiaoAlvo,
                };
                _dbPackAndPromote.LojaRegiaoAlvo.Add(lojaRegiaoAlvo);
            }

            _dbPackAndPromote.SaveChanges();

            // Criação do usuário
            if (string.IsNullOrEmpty(novoUsuarioDto.Login))
                return BadRequest("O login do usuário é obrigatório.");

            if (string.IsNullOrEmpty(novoUsuarioDto.Senha))
                return BadRequest("A senha do usuário é obrigatória.");

            Usuario usuario = new Usuario
            {
                Login = novoUsuarioDto.Login,
                Senha = BCrypt.Net.BCrypt.HashPassword(novoUsuarioDto.Senha),
                IdLoja = loja.IdLoja
            };

            _dbPackAndPromote.Usuario.Add(usuario);
            _dbPackAndPromote.SaveChanges();

            // Adiciona perfil padrão para o usuário
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
            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Login) ||
                string.IsNullOrWhiteSpace(loginDto.Senha))
                return BadRequest("Digite o Login e senha corretamente!");

            // Verifica se o usuário existe
            var usuario = _dbPackAndPromote.Usuario
                                           .SingleOrDefault(u => u.Login == loginDto.Login);

            // Retorna erro 401 se o login ou senha estiver incorreto
            if (usuario == null)
                return Unauthorized("Login e/ou senha inválidos.");

            // Verifica se a senha digitada é igual à senha criptografada no banco
            bool senhaValidada = BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.Senha);

            // Retorna erro 401 se a senha for diferente da senha salva no banco
            if (!senhaValidada)
                return Unauthorized("Login e/ou senha inválidos.");

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

            // Retorna o Id do Usuário e o token JWT gerado como resposta
            return Ok(new { Token = tokenString, IdUser = usuario.IdUsuario });
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

                // Atualizar as informações adicionais da loja
                //loja.IdCategoria = usuarioAlteradoDto.IdCategoria;
                //loja.IdCategoria = usuarioAlteradoDto.IdPlano;
                //loja.IdPublicoAlvo = usuarioAlteradoDto.IdPublicoAlvo;
                //loja.IdFaixaEtaria = usuarioAlteradoDto.IdFaixaEtaria;
                //loja.IdRegiaoAlvo = usuarioAlteradoDto.IdRegiaoAlvo;
                //loja.IdPreferenciaAlvo = usuarioAlteradoDto.IdPreferenciaAlvo;
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

        #region Utils
        // Funções auxiliares para validações de CNPJ e Email
        private bool IsValidCNPJ(string cnpj)
        {
            return Regex.IsMatch(cnpj, @"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$");
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");
        }
        #endregion
    }
}