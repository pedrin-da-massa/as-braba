using MinhaApi.Models;

namespace MinhaApi.Repositories;

public interface IVendaRepository
{
    Venda Add(Venda venda);

    IEnumerable<Venda> GetAll();

    Venda? GetById(int id);
}