using MinhaApi.Models;
using MySqlConnector;

namespace MinhaApi.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly string _connectionString;

    public ProdutoRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    // GET ALL - Busca os produtos usando SQL
    public IEnumerable<Produto> GetAll()
    {
        var lista = new List<Produto>();

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = "SELECT id, nome, preco, estoque, ativo FROM produtos";

        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Produto
            {
                Id = reader.GetInt32("id"),
                Nome = reader.GetString("nome"),
                Preco = reader.GetDecimal("preco"),
                Estoque = reader.GetInt32("estoque"),
                Ativo = reader.GetBoolean("ativo")
            });
        }

        return lista;
    }

    // GET BY ID
    public Produto? GetById(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = "SELECT id, nome, preco, estoque, ativo FROM produtos WHERE id = @Id";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Produto
            {
                Id = reader.GetInt32("id"),
                Nome = reader.GetString("nome"),
                Preco = reader.GetDecimal("preco"),
                Estoque = reader.GetInt32("estoque"),
                Ativo = reader.GetBoolean("ativo")
            };
        }

        return null;
    }

    // POST - Adiciona produto usando SQL
    public void Add(Produto p)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = @"INSERT INTO produtos (nome, preco, estoque, ativo) 
                       VALUES (@Nome, @Preco, @Estoque, @Ativo);
                       SELECT LAST_INSERT_ID();";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Nome", p.Nome);
        cmd.Parameters.AddWithValue("@Preco", p.Preco);
        cmd.Parameters.AddWithValue("@Estoque", p.Estoque);
        cmd.Parameters.AddWithValue("@Ativo", p.Ativo);

        // Executa a inserção e recupera o ID gerado pelo MySQL
        var idGerado = cmd.ExecuteScalar();

        p.Id = Convert.ToInt32(idGerado);
    }

    // PUT - Atualiza produto
    public void Update(Produto p)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = @"UPDATE produtos 
                       SET nome = @Nome,
                           preco = @Preco,
                           estoque = @Estoque,
                           ativo = @Ativo
                       WHERE id = @Id";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Id", p.Id);
        cmd.Parameters.AddWithValue("@Nome", p.Nome);
        cmd.Parameters.AddWithValue("@Preco", p.Preco);
        cmd.Parameters.AddWithValue("@Estoque", p.Estoque);
        cmd.Parameters.AddWithValue("@Ativo", p.Ativo);

        cmd.ExecuteNonQuery();
    }

    // DELETE - Remove produto
    public void Delete(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = "DELETE FROM produtos WHERE id = @Id";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }
}