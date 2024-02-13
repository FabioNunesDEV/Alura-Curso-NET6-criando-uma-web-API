using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController: ControllerBase
{
    private static List<Filme> filmes = new List<Filme>();
    private static int id = 1;

    [HttpPost("adicionar")]
    public IActionResult Adicionar([FromBody] Filme filme)
    {
        filme.Id = id++;
        filmes.Add(filme);

        Console.WriteLine($"Id: {filme.Id} - Titulo: {filme.Titulo} - Duração: {filme.Duracao}");

        return CreatedAtAction(nameof(RecuperarPorId), new { id = filme.Id }, filme);

    }

    [HttpPost("adicionarEmLote")]
    public IActionResult AdicionarEmLote([FromBody] List<Filme> filmes)
    {
        foreach (var filme in filmes)
        {
            filme.Id = id++;
            FilmeController.filmes.Add(filme);
            Console.WriteLine($"Id: {filme.Id} - Titulo: {filme.Titulo} - Duração: {filme.Duracao}");
        }

        return CreatedAtAction(nameof(RecuperarTodos), new { }, filmes);
    }

    [HttpGet("recuperarTodos")]
    public IEnumerable<Filme> RecuperarTodos()
    {
        return filmes;
    }

    [HttpGet("paginacao/skip/{skip}/take/{take}")]
    public IEnumerable<Filme> RecuperarPaginacao([FromRoute] int skip=0, [FromRoute] int take=10)
    {
        return filmes.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperarPorId(int id)
    {
        var filme = filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        return Ok(filme);
    }
}
