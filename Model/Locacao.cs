using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CadastroFilmes.API.Model
{
    public class Locacao
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(14, ErrorMessage = "O campo {0} precisa ter {1} caracteres. ")]
        public string CPF { get; set; }
        public DateTime DataLocacao { get; set; } = DateTime.Now;
        public virtual ICollection<LocacaoFilme> LocacaoFilmes { get; set; }
    }
}