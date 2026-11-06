using MinhaApi.Models;
using MinhaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MinhaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IClienteService _service;

    public ClienteController(IClienteService service)
    {
        _service = service;
    }

    // GET: api/cliente
    [HttpGet]
    public IActionResult GetAll()
    {
        var clientes = _service.GetAll();

        return Ok(clientes);
    }

    // GET: api/cliente/1
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var cliente = _service.GetById(id);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    // POST: api/cliente
    [HttpPost]
    public IActionResult Create([FromBody] Cliente cliente)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var criado = _service.Create(cliente);

            return CreatedAtAction(
                nameof(GetById),
                new { id = criado.Id },
                criado
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensagem = ex.Message
            });
        }
    }

    // PUT: api/cliente/1
    [HttpPut("{id}")]
    public IActionResult Update(
        int id,
        [FromBody] Cliente cliente)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var atualizado = _service.Update(id, cliente);

            if (atualizado == null)
                return NotFound();

            return Ok(atualizado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensagem = ex.Message
            });
        }
    }

    // DELETE: api/cliente/1
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deletado = _service.Delete(id);

        if (!deletado)
            return NotFound();

        return NoContent();
    }
}