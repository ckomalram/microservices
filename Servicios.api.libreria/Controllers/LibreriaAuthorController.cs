using Microsoft.AspNetCore.Mvc;
using Servicios.api.libreria.Repository;
using Servicios.api.libreria.Core.Entities;


namespace Servicios.api.libreria.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibreriaAuthorController : ControllerBase
{
    private readonly IMongoRepository<AuthorEntity> authorGenericRepository;
    public LibreriaAuthorController(IMongoRepository<AuthorEntity> genericrepository)
    {
        authorGenericRepository = genericrepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorEntity>>> Get()
    {
        var rta = await authorGenericRepository.Getall();
        return Ok(rta);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorEntity>> GetById(string id)
    {
        var rta = await authorGenericRepository.GetById(id);
        return Ok(rta);
    }


    [HttpPost]
    public async Task Create(AuthorEntity autor)
    {
        await authorGenericRepository.InsertDocument(autor);

    }

    [HttpPost("pagination")]
    public async Task<ActionResult<PaginationEntity<AuthorEntity>>> PostPagination(PaginationEntity<AuthorEntity> pagination)
    {

        // Console.WriteLine(pagination.Filter);
        var rta = await authorGenericRepository.PaginationBy(
         filter => filter.Nombre == pagination.Filter,
         pagination
        );

        return Ok(rta);

    }

    [HttpPut("{id}")]
    public async Task Update(string id, AuthorEntity autor)
    {
        autor.Id = id;
        await authorGenericRepository.UpdateDocument(autor);
        // return Ok();
    }

    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        await authorGenericRepository.DeleteDocument(id);
    }
}
