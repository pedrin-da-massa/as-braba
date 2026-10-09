using MinhaApi.Models;

namespace MinhaApi.Services;

public interface IVendaService
{
    Venda RealizarVenda(VendaRequest request);

    IEnumerable<Venda> GetAll();

    Venda? GetById(int id);
}