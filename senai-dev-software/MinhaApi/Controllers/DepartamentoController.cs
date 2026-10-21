using Microsoft.AspNetCore.Mvc;
using MinhaApi.Models;
using MinhaApi.Services;

namespace MinhaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartamentoController : ControllerBase
{
    private readonly IDepartamentoService _service;

    public DepartamentoController(IDepartamentoService service)
    {
        _service = service;
    }

    // GET /api/departamento
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAll());
    }

    // GET /api/departamento/1
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var departamento = _service.GetById(id);

        if (departamento == null)
            return NotFound();

        return Ok(departamento);
    }

    // POST /api/departamento
    [HttpPost]
    public IActionResult Create([FromBody] Departamento departamento)
    {
        try
        {
            var criado = _service.Create(departamento);

            return CreatedAtAction(
                nameof(GetById),
                new { id = criado.Id },
                criado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    // PUT /api/departamento/1
    [HttpPut("{id:int}")]
    public IActionResult Update(
        int id,
        [FromBody] Departamento departamento)
    {
        try
        {
            var atualizado = _service.Update(id, departamento);

            if (atualizado == null)
                return NotFound();

            return Ok(atualizado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    // DELETE /api/departamento/1
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var deletado = _service.Delete(id);

            if (!deletado)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}