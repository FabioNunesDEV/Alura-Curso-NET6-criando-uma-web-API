using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.DTO;
using FilmesApi.Models;
using FilmesApi.Profiles;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController: ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost("adicionar")]
    public IActionResult Adicionar([FromBody] CreateFilmeDTO filmeDTO)
    {
        Filme filme = _mapper.Map<Filme>(filmeDTO);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperarPorId),
            new { id = filme.Id },
            filme);
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
        return _context.Filmes.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperarPorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        return Ok(filme);
    }

    [HttpPut("atualizarFilme/{id}")]
    public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDTO filmeDTO)
    {
        // Verifica se o filme existe
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        // Se não existir, retorna NotFound
        if (filme == null) return NotFound();

        // Atualiza o filme
        _mapper.Map(filmeDTO, filme);
        _context.SaveChanges();

        // Retorna status code 204 - NoContent
        return NoContent();
    }

    [HttpPatch("atualizarFilmeParcial/{id}")]
    public IActionResult AtualizarFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDTO> patch)
    {
        // Verifica se o filme existe
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        // Se não existir, retorna NotFound
        if (filme == null) return NotFound();

        // Converte o filme para um DTO
        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDTO>(filme);

        // Aplica o patch ao filme para atualizar
        patch.ApplyTo(filmeParaAtualizar, ModelState);

        // Verifica se o modelo é válido
        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        // Atualiza o filme
        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();

        // Retorna status code 204 - NoContent
        return NoContent();
    }
}
