using System.Collections.Generic;
using System.Threading.Tasks;
using CadastroFilmes.API.Data;
using CadastroFilmes.API.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadastroFilmes.API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class LocacaoController : ControllerBase
    {
        private readonly CadastroFilmesContext _context;

        public LocacaoController(CadastroFilmesContext context)
        {
            _context = context;
        }

        //Metodo que lista todas as locações, trazendo o CPF do cliente e a lista de filme que ele locou no mesmo momento.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Locacao>>> GetLocacoes()
        {
            return await _context.Locacoes.Include(q => q.LocacaoFilmes).ThenInclude(q => q.Filme).ToListAsync();
        }

        //Lista somente uma locação, utilizada no retorno do cadastro da locação
        [HttpGet("{id}")]
        public async Task<ActionResult<Locacao>> GetLocacao(int id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);

            if (locacao == null)
            {
                return NotFound();
            }

            return locacao;
        }

        //Cadastro da locação
        //Criado um View Model, para poder realizar o cadastro da locação, e uma lista de id referente aos filmes locados
        [HttpPost]
        public async Task<ActionResult<Locacao>> PostLocacao(LocacaoVM locacaoVM)
        {

            _context.Locacoes.Add(locacaoVM.Locacao);
            await _context.SaveChangesAsync();

            //percorre a lista de filmes que chegou no array da viewmodel e cadastra como filme locado
            foreach (var item in locacaoVM.FilmeId)
            {
                LocacaoFilme locacaoFilme = new LocacaoFilme(locacaoVM.Locacao.Id, item);
                _context.Add(locacaoFilme);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocacao", new { id = locacaoVM.Locacao.Id }, locacaoVM.Locacao);
        }
    }
}
