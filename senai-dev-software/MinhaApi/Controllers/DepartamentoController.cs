using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MinhaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentoController : ControllerBase
    {
        private readonly IDepartamentoRepository _departamentoRepository;

        public DepartamentoController(IDepartamentoRepository departamentoRepository)
        {
            _departamentoRepository = departamentoRepository;
        }

        [HttpPost]
        public IActionResult CreateDepartamento(Departamento departamento)
        {
            if (string.IsNullOrEmpty(departamento.Nome))
            {
                return BadRequest("O campo nome é obrigatório.");
            }

            if (_departamentoRepository.Exists(departamento.Nome))
            {
                return Conflict("Já existe um departamento com este nome.");
            }

            _departamentoRepository.Add(departamento);
            return CreatedAtAction(nameof(GetDepartamento), new { id = departamento.Id }, departamento);
        }

        [HttpGet("{id}")]
        public ActionResult<Departamento> GetDepartamento(int id)
        {
            var departamento = _departamentoRepository.GetById(id);
            if (departamento == null || departamento.Ativo == false)
            {
                return NotFound();
            }
            return departamento;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartamento(int id)
        {
            var departamento = _departamentoRepository.GetById(id);
            if (departamento == null || departamento.Ativo == false)
            {
                return NotFound();
            }

            if (_departamentoRepository.HasActiveFornecedores(id))
            {
                return Conflict("Não é permitido desativar um departamento com fornecedores ativos.");
            }

            departamento.Ativo = false; // Soft delete
            _departamentoRepository.Update(departamento);
            return NoContent();
        }
    }

    public class Departamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; } = true;
    }

    public interface IDepartamentoRepository
    {
        void Add(Departamento departamento);
        Departamento GetById(int id);
        void Update(Departamento departamento);
        bool Exists(string nome);
        bool HasActiveFornecedores(int departamentoId);
    }
}