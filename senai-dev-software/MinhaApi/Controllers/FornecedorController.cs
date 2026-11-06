
using Microsoft.AspNetCore.Mvc;
using MinhaApi.Models;
using MinhaApi.Services;

[ApiController]
[Route("api/[controller]")]
public class FornecedorController : ControllerBase
{
    private readonly IFornecedorService _service;

    public FornecedorController(IFornecedorService service)
    {
        _service = service;
    }

    // GET /api/fornecedor
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAll());
    }

    // GET /api/fornecedor/1
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        if (id <= 0)
            return BadRequest("O ID deve ser maior que zero.");

        var fornecedor = _service.GetById(id);

        if (fornecedor == null)
            return NotFound();

        return Ok(fornecedor);
    }

    // POST /api/fornecedor
    [HttpPost]
    public IActionResult Create([FromBody] Fornecedor fornecedor)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var criado = _service.Create(fornecedor);

        return CreatedAtAction(
            nameof(GetById),
            new { id = criado!.Id },
            criado);
    }

    // PUT /api/fornecedor/1
    [HttpPut("{id:int}")]
    public IActionResult Update(
        int id,
        [FromBody] Fornecedor fornecedor)
    {
        if (id <= 0)
            return BadRequest("O ID deve ser maior que zero.");

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var atualizado = _service.Update(id, fornecedor);

        if (atualizado == null)
            return NotFound();

        return Ok(atualizado);
    }

    // DELETE /api/fornecedor/1
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (id <= 0)
            return BadRequest("O ID deve ser maior que zero.");

        var deletado = _service.Delete(id);

        if (!deletado)
            return NotFound();

        return NoContent();
    }
}
