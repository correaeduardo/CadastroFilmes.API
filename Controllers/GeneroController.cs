using System.Collections.Generic;
using System.Linq;
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
    public class GeneroController : ControllerBase
    {
        private readonly CadastroFilmesContext _context;

        public GeneroController(CadastroFilmesContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genero>>> GetGeneros()
        {
            //Busca de todos os generos cadastrados
            return await _context.Generos.ToListAsync();
        }

        //Busca um genero especifico por ID, utilizado no retorno do cadastro de genero
        [HttpGet("{id}")]
        public async Task<ActionResult<Genero>> GetGenero(int id)
        {
            var genero = await _context.Generos.FindAsync(id);

            if (genero == null)
            {
                return NotFound();
            }

            return genero;
        }

        //Edição do genero, necessario enviar o objeto completo para que funcione corretamente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenero(int id, Genero genero)
        {
            if (id != genero.Id)
            {
                return BadRequest();
            }

            _context.Entry(genero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //Metodo de cadastro de genero
        [HttpPost]
        public async Task<ActionResult<Genero>> PostGenero(Genero genero)
        {
            _context.Generos.Add(genero);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenero", new { id = genero.Id }, genero);
        }

        //Metodo de delete de genero
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenero(int id)
        {
            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
            {
                return NotFound();
            }

            //Verifica se o genero já esta sendo utilizado em alguma outra tabela.
            if(GeneroUsed(id)){
                return NotFound(new { message = "Esse genero já esta vinculado a um filme, impossivel apaga-lo." });
            }

            _context.Generos.Remove(genero);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GeneroExists(int id)
        {
            return _context.Generos.Any(e => e.Id == id);
        }
        //Verificação se o genero é usado para poder excluir
        private bool GeneroUsed(int id)
        {
            return _context.Filmes.Any(e => e.GeneroId == id);
        }
    }
}
