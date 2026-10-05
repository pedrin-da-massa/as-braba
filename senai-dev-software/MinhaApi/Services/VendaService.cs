using MinhaApi.Models;
using MinhaApi.Repositories;

namespace MinhaApi.Services;

public class VendaService : IVendaService
{
    private readonly IVendaRepository _vendaRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IClienteRepository _clienteRepository;

    public VendaService(
        IVendaRepository vendaRepository,
        IProdutoRepository produtoRepository,
        IClienteRepository clienteRepository)
    {
        _vendaRepository = vendaRepository;
        _produtoRepository = produtoRepository;
        _clienteRepository = clienteRepository;
    }

    public Venda RealizarVenda(VendaRequest request)
    {
        // 1. Validar quantidade
        if (request.Quantidade <= 0)
        {
            throw new ArgumentException(
                "A quantidade deve ser maior que zero.");
        }

        // 2. Buscar cliente
        var cliente = _clienteRepository.GetById(request.ClienteId);

        if (cliente == null)
        {
            throw new KeyNotFoundException(
                "Cliente não encontrado.");
        }

        // 3. Verificar se cliente está ativo
        if (!cliente.Ativo)
        {
            throw new ArgumentException(
                "O cliente está inativo.");
        }

        // 4. Buscar produto
        var produto = _produtoRepository.GetById(request.ProdutoId);

        if (produto == null)
        {
            throw new KeyNotFoundException(
                "Produto não encontrado.");
        }

        // 5. Verificar se produto está ativo
        if (!produto.Ativo)
        {
            throw new ArgumentException(
                "O produto está inativo.");
        }

        // 6. Verificar estoque
        if (produto.Estoque < request.Quantidade)
        {
            throw new ArgumentException(
                "Estoque insuficiente para o produto informado.");
        }

        // 7. Calcular valor total
        decimal valorTotal =
            produto.Preco * request.Quantidade;

        // 8. Criar venda
        var venda = new Venda
        {
            ClienteId = request.ClienteId,
            ProdutoId = request.ProdutoId,
            Quantidade = request.Quantidade,
            PrecoUnitario = produto.Preco,
            ValorTotal = valorTotal,
            DataVenda = DateTime.Now
        };

        // 9. Baixar estoque
        bool estoqueAtualizado =
            _produtoRepository.BaixarEstoque(
                produto.Id,
                request.Quantidade);

        if (!estoqueAtualizado)
        {
            throw new ArgumentException(
                "Não foi possível atualizar o estoque.");
        }

        // 10. Salvar venda
        return _vendaRepository.Add(venda);
    }

    public IEnumerable<Venda> GetAll()
    {
        return _vendaRepository.GetAll();
    }

    public Venda? GetById(int id)
    {
        return _vendaRepository.GetById(id);
    }
}