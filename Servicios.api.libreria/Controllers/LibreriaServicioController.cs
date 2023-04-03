using Microsoft.AspNetCore.Mvc;
using Servicios.api.libreria.Repository;
using Servicios.api.libreria.Core.Entities;


namespace Servicios.api.libreria.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibreriaServicioController : ControllerBase
{

    private readonly IAuthorRepository authorRepository;
    private readonly IMongoRepository<AuthorEntity> authorGenericRepository;
    public LibreriaServicioController(IAuthorRepository repository, IMongoRepository<AuthorEntity> genericrepository)
    {
        authorRepository = repository;
        authorGenericRepository = genericrepository;
    }

    [HttpGet("autores")]
    public async Task<ActionResult<IEnumerable<Author>>> GetAutores()
    {

        var rta = await authorRepository.GetAutores();

        return Ok(rta);
    }


    [HttpGet("autorgenericos")]
    public async Task<ActionResult<IEnumerable<AuthorEntity>>> GetAutoresGenericos()
    {

        var rta = await authorGenericRepository.Getall();
        return Ok(rta);
    }
}
