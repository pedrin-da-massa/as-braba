
using MinhaApi.Models;
using MinhaApi.Repositories;
using MinhaApi.Services;

public class FornecedorService : IFornecedorService
{
    private readonly IFornecedorRepository _repo;

    public FornecedorService(IFornecedorRepository repo)
    {
        _repo = repo;
    }

    public IEnumerable<Fornecedor> GetAll()
    {
        return _repo.GetAll();
    }

    public Fornecedor? GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.");

        return _repo.GetById(id);
    }

    public Fornecedor Create(Fornecedor fornecedor)
    {
        ValidarFornecedor(fornecedor);

        fornecedor.Id = 0;
        fornecedor.Ativo = true;

        _repo.Add(fornecedor);

        return fornecedor;
    }

    public Fornecedor? Update(int id, Fornecedor fornecedor)
    {
        if (id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.");

        ValidarFornecedor(fornecedor);

        var existente = _repo.GetById(id);

        if (existente == null)
            return null;

        fornecedor.Id = id;
        fornecedor.Ativo = existente.Ativo;

        _repo.Update(fornecedor);

        return fornecedor;
    }

    public bool Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID deve ser maior que zero.");

        var existente = _repo.GetById(id);

        if (existente == null)
            return false;

        if (!existente.Ativo)
            return false;

        _repo.Delete(id);

        return true;
    }

    private static void ValidarFornecedor(Fornecedor fornecedor)
    {
        ArgumentNullException.ThrowIfNull(fornecedor);

        if (string.IsNullOrWhiteSpace(fornecedor.Nome))
            throw new ArgumentException("O nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(fornecedor.Email))
            throw new ArgumentException("O e-mail é obrigatório.");

        if (string.IsNullOrWhiteSpace(fornecedor.Cnpj))
            throw new ArgumentException("O CNPJ é obrigatório.");

        if (string.IsNullOrWhiteSpace(fornecedor.Telefone))
            throw new ArgumentException("O telefone é obrigatório.");
    }
}
