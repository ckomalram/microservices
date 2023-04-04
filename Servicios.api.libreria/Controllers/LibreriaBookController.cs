using Microsoft.AspNetCore.Mvc;
using Servicios.api.libreria.Repository;
using Servicios.api.libreria.Core.Entities;


namespace Servicios.api.libreria.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibreriaBookController : ControllerBase
{
    private readonly IMongoRepository<BookEntity> bookGenericRepository;
    public LibreriaBookController(IMongoRepository<BookEntity> genericrepository)
    {
        bookGenericRepository = genericrepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookEntity>>> Get()
    {
        var rta = await bookGenericRepository.Getall();
        return Ok(rta);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookEntity>> GetById(string id)
    {
        var rta = await bookGenericRepository.GetById(id);
        return Ok(rta);
    }


    [HttpPost]
    public async Task Create(BookEntity book)
    {
        await bookGenericRepository.InsertDocument(book);

    }

    [HttpPost("pagination")]
    public async Task<ActionResult<PaginationEntity<BookEntity>>> PostPagination(PaginationEntity<BookEntity> pagination)
    {

        var rta = await bookGenericRepository.PaginationByFilter(
        pagination
        );

        return Ok(rta);

    }

    [HttpPut("{id}")]
    public async Task Update(string id, BookEntity book)
    {
        book.Id = id;
        await bookGenericRepository.UpdateDocument(book);
        // return Ok();
    }

    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        await bookGenericRepository.DeleteDocument(id);
    }
}
