using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CadastroFilmes.API.Model
{
    public class Genero
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public string Nome { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public bool Ativo { get; set; }
    }
}