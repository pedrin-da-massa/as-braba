using MinhaApi.Models;
using MinhaApi.Repositories;

namespace MinhaApi.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _repo;

    public ProdutoService(IProdutoRepository repo)
    {
        _repo = repo;
    }

    // GET - Lista todos os produtos
    public IEnumerable<Produto> GetAll()
    {
        return _repo.GetAll();
    }

    // GET - Busca um produto pelo ID
    public Produto? GetById(int id)
    {
        return _repo.GetById(id);
    }

    // POST - Cria um novo produto
    public Produto Create(Produto produto)
    {
        if (produto == null)
            throw new ArgumentNullException(nameof(produto));

        if (produto.Preco < 0)
            throw new ArgumentException("Preço não pode ser negativo.");

        if (produto.Estoque < 0)
            throw new ArgumentException("Estoque não pode ser negativo.");

        if (string.IsNullOrWhiteSpace(produto.Nome))
            throw new ArgumentException("Nome do produto é obrigatório.");

        _repo.Add(produto);

        return produto;
    }

    // PUT - Atualiza um produto existente
    public Produto? Update(int id, Produto produto)
    {
        if (produto == null)
            return null;

        var existente = _repo.GetById(id);

        if (existente == null)
            return null;

        if (produto.Preco < 0)
            throw new ArgumentException("Preço não pode ser negativo.");

        if (produto.Estoque < 0)
            throw new ArgumentException("Estoque não pode ser negativo.");

        if (string.IsNullOrWhiteSpace(produto.Nome))
            throw new ArgumentException("Nome do produto é obrigatório.");

        produto.Id = id;

        _repo.Update(produto);

        return produto;
    }

    // DELETE - Exclui um produto
    public bool Delete(int id)
    {
        var produto = _repo.GetById(id);

        if (produto == null)
            return false;

        _repo.Delete(id);

        return true;
    }
}