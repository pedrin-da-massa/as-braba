using MinhaApi.Models;
using MinhaApi.Repositories;

namespace MinhaApi.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repo;

    public ClienteService(IClienteRepository repo)
    {
        _repo = repo;
    }

    // GET ALL
    public IEnumerable<Cliente> GetAll()
    {
        return _repo.GetAll();
    }

    // GET BY ID
    public Cliente? GetById(int id)
    {
        return _repo.GetById(id);
    }

    // POST
    public Cliente Create(Cliente cliente)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));

        if (string.IsNullOrWhiteSpace(cliente.Nome))
            throw new ArgumentException(
                "Nome do cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(cliente.Email))
            throw new ArgumentException(
                "Email do cliente é obrigatório.");

        cliente.Ativo = true;

        _repo.Add(cliente);

        return cliente;
    }

    // PUT
    public Cliente? Update(int id, Cliente cliente)
    {
        if (cliente == null)
            return null;

        var existente = _repo.GetById(id);

        if (existente == null)
            return null;

        if (string.IsNullOrWhiteSpace(cliente.Nome))
            throw new ArgumentException(
                "Nome do cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(cliente.Email))
            throw new ArgumentException(
                "Email do cliente é obrigatório.");

        cliente.Id = id;

        _repo.Update(cliente);

        return cliente;
    }

    // DELETE - SOFT DELETE
    public bool Delete(int id)
    {
        var cliente = _repo.GetById(id);

        if (cliente == null)
            return false;

        _repo.Delete(id);

        return true;
    }
}