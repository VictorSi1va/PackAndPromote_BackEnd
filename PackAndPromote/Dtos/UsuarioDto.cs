﻿namespace PackAndPromote.Dtos
{
    public class UsuarioDto
    {
        public string Login { get; set; }
        public string Senha { get; set; }

        public string NomeLoja { get; set; }
        public string EnderecoLoja { get; set; }
        public string DescricaoLoja { get; set; }
        public string TelefoneLoja { get; set; }
        public string CNPJLoja { get; set; }
        public string EmailLoja { get; set; }
        public int IdCategoria { get; set; }
        public int IdPublicoAlvo { get; set; }
        public int IdFaixaEtaria { get; set; }
        public int IdRegiaoAlvo { get; set; }
        public int IdPreferenciaAlvo { get; set; }
    }
}
