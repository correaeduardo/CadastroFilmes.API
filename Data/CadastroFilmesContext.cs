using CadastroFilmes.API.Model;
using Microsoft.EntityFrameworkCore;

namespace CadastroFilmes.API.Data
{
    public class CadastroFilmesContext : DbContext
    {
        public CadastroFilmesContext(DbContextOptions<CadastroFilmesContext> options) : base(options) { }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }
        public DbSet<LocacaoFilme> LocacaoesFilmes { get; set; }
    }
}