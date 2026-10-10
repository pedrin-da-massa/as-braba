
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
        // Validar a quantidade
        if (request.Quantidade <= 0)
        {
            throw new ArgumentException(
                "A quantidade deve ser maior que zero.");
        }

        // Verificar se o cliente existe
        var cliente = _clienteRepository.GetById(request.ClienteId);

        if (cliente == null)
        {
            throw new KeyNotFoundException(
                "Cliente não encontrado.");
        }

        if (!cliente.Ativo)
        {
            throw new ArgumentException(
                "O cliente está inativo.");
        }

        // Verificar se o produto existe
        var produto = _produtoRepository.GetById(request.ProdutoId);

        if (produto == null)
        {
            throw new KeyNotFoundException(
                "Produto não encontrado.");
        }

        if (!produto.Ativo)
        {
            throw new ArgumentException(
                "O produto está inativo.");
        }

        // Verificar se há estoque suficiente
        if (produto.Estoque < request.Quantidade)
        {
            throw new ArgumentException(
                "Estoque insuficiente para o produto informado.");
        }

        // Criar a venda e calcular o valor total
        var venda = new Venda
        {
            ClienteId = request.ClienteId,
            ProdutoId = request.ProdutoId,
            Quantidade = request.Quantidade,
            PrecoUnitario = produto.Preco,
            ValorTotal = produto.Preco * request.Quantidade,
            DataVenda = DateTime.Now
        };

        // Registrar a venda e baixar o estoque na mesma transação
        return _vendaRepository.RealizarVendaComBaixaEstoque(venda);
    }

    public IEnumerable<Venda> GetAll()
    {
        return _vendaRepository.GetAll();
    }

    public Venda? GetById(int id)
    {
        if (id <= 0)
        {
            return null;
        }

        return _vendaRepository.GetById(id);
    }
}