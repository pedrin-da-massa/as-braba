using MinhaApi.Models;
using MinhaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MinhaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendaController : ControllerBase
{
    private readonly IVendaService _service;

    public VendaController(IVendaService service)
    {
        _service = service;
    }

    // POST: api/venda
    [HttpPost]
    public IActionResult Create([FromBody] VendaRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var venda = _service.RealizarVenda(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = venda.Id },
                venda
            );
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                mensagem = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensagem = ex.Message
            });
        }
    }

    // GET: api/venda
    [HttpGet]
    public IActionResult GetAll()
    {
        var vendas = _service.GetAll();

        return Ok(vendas);
    }

    // GET: api/venda/1
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var venda = _service.GetById(id);

        if (venda == null)
        {
            return NotFound(new
            {
                mensagem = "Venda não encontrada."
            });
        }

        return Ok(venda);
    }
}