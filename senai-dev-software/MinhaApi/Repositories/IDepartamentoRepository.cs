using MinhaApi.Models;

namespace MinhaApi.Repositories;

public interface IDepartamentoRepository
{
    IEnumerable<Departamento> GetAll();

    Departamento? GetById(int id);

    void Add(Departamento departamento);

    void Update(Departamento departamento);

    bool Exists(string nome);

    bool HasActiveFornecedores(int departamentoId);
}