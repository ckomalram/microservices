using Microsoft.AspNetCore.Mvc;
using Servicios.api.libreria.Repository;
using Servicios.api.libreria.Core.Entities;


namespace Servicios.api.libreria.Controllers;

[ApiController]
[Route("[controller]")]
public class LibreriaServicioController : ControllerBase
{

    private readonly IAuthorRepository authorRepository;
    public LibreriaServicioController(IAuthorRepository repository)
    {
        authorRepository = repository;
    }

    [HttpGet("autores")]
    public async Task<ActionResult<IEnumerable<Author>>> GetAutores()
    {

        var rta = await authorRepository.GetAutores();

        return Ok(rta);
    }
}
