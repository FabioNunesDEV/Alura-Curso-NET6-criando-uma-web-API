using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController: ControllerBase
{
    private static List<Filme> filmes = new List<Filme>();
    private static int id = 0;

    [HttpPost]
    public void AdicionarFilme([FromBody] Filme filme)
    {
        filme.Id = id++;
        filmes.Add(filme);

        Console.WriteLine($"Id: {filme.Id} - Titulo: {filme.Titulo} - Duração: {filme.Duracao}");

    }

    [HttpGet]
    public IEnumerable<Filme> RecuperarFilmes()
    {
        return filmes;
    }

    [HttpGet("{id}")]
    public Filme? RecuperarFilmePorId(int id)
    {
        var filme = filmes.FirstOrDefault(filme => filme.Id == id);
        return filme;
    }
}
