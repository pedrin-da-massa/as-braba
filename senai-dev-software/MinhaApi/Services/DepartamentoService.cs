using MinhaApi.Models;
using MinhaApi.Repositories;

namespace MinhaApi.Services;

public class DepartamentoService : IDepartamentoService
{
    private readonly IDepartamentoRepository _repository;

    public DepartamentoService(IDepartamentoRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Departamento> GetAll()
    {
        return _repository.GetAll();
    }

    public Departamento? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public Departamento Create(Departamento departamento)
    {
        if (string.IsNullOrWhiteSpace(departamento.Nome))
        {
            throw new ArgumentException(
                "O campo nome é obrigatório.");
        }

        departamento.Nome = departamento.Nome.Trim();

        if (_repository.Exists(departamento.Nome))
        {
            throw new InvalidOperationException(
                "Já existe um departamento com este nome.");
        }

        departamento.Ativo = true;

        _repository.Add(departamento);

        return departamento;
    }

    public Departamento? Update(int id, Departamento departamento)
    {
        var existente = _repository.GetById(id);

        if (existente == null)
            return null;

        if (string.IsNullOrWhiteSpace(departamento.Nome))
        {
            throw new ArgumentException(
                "O campo nome é obrigatório.");
        }

        departamento.Nome = departamento.Nome.Trim();

        if (!existente.Nome.Equals(
                departamento.Nome,
                StringComparison.OrdinalIgnoreCase)
            && _repository.Exists(departamento.Nome))
        {
            throw new InvalidOperationException(
                "Já existe um departamento com este nome.");
        }

        existente.Nome = departamento.Nome;
        existente.Ativo = departamento.Ativo;

        _repository.Update(existente);

        return existente;
    }

    public bool Delete(int id)
    {
        var departamento = _repository.GetById(id);

        if (departamento == null || !departamento.Ativo)
            return false;

        if (_repository.HasActiveFornecedores(id))
        {
            throw new InvalidOperationException(
                "Não é permitido desativar um departamento " +
                "com fornecedores ativos.");
        }

        // Exclusão lógica: o registro continua no banco.
        departamento.Ativo = false;

        _repository.Update(departamento);

        return true;
    }
}