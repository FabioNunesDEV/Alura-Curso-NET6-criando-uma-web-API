using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.DTO;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController: ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context)
    {
        _context = context;
    }

    [HttpPost("adicionar")]
    public IActionResult Adicionar([FromBody] CreateFilmeDTO filmeDTO)
    {
        // Converte o DTO para o modelo
        Filme filme = _mapper.Map<Filme>(filmeDTO);

        _context.Filmes.Add(filme);
        _context.SaveChanges();

        Console.WriteLine($"Id: {filme.Id} - Titulo: {filme.Titulo} - Duração: {filme.Duracao}");

        return CreatedAtAction(nameof(RecuperarPorId), new { id = filme.Id }, filme);
    }

    [HttpPost("adicionarEmLote")]
    public IActionResult AdicionarEmLote([FromBody] List<CreateFilmeDTO> filmesDTO)
    {
        foreach (var filmeDTO in filmesDTO)
        {
            // Converte o DTO para o modelo
            Filme filme = _mapper.Map<Filme>(filmeDTO);

            _context.Filmes.Add(filme);
            _context.SaveChanges();
            Console.WriteLine($"Id: {filme.Id} - Titulo: {filme.Titulo} - Duração: {filme.Duracao}");
        }

        return CreatedAtAction(nameof(RecuperarTodos), new { }, filmesDTO);
    }

    [HttpGet("recuperarTodos")]
    public IEnumerable<Filme> RecuperarTodos()
    {
        return _context.Filmes;
    }

    [HttpGet("paginacao/skip/{skip}/take/{take}")]
    public IEnumerable<Filme> RecuperarPaginacao([FromRoute] int skip=0, [FromRoute] int take=10)
    {
        var fabio = "10";
        return _context.Filmes.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperarPorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        return Ok(filme);
    }
}
