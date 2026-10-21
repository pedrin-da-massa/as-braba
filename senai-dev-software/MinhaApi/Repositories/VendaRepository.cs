using MinhaApi.Models;
using MySqlConnector;

namespace MinhaApi.Repositories;

public class VendaRepository : IVendaRepository
{
    private readonly string _connectionString;

    public VendaRepository(IConfiguration config)
    {
        _connectionString =
            config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "A conexão DefaultConnection não foi configurada.");
    }

    // Listar todas as vendas com JOIN de clientes e produtos
    public IEnumerable<Venda> GetAll()
    {
        var vendas = new List<Venda>();

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            SELECT
                v.id,
                v.cliente_id,
                c.nome AS nome_cliente,
                v.produto_id,
                p.nome AS nome_produto,
                v.quantidade,
                v.preco_unitario,
                v.valor_total,
                v.data_venda
            FROM vendas v
            INNER JOIN clientes c ON c.id = v.cliente_id
            INNER JOIN produtos p ON p.id = v.produto_id
            ORDER BY v.id DESC;
            """;

        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            vendas.Add(new Venda
            {
                Id = reader.GetInt32("id"),
                ClienteId = reader.GetInt32("cliente_id"),
                NomeCliente = reader.GetString("nome_cliente"),
                ProdutoId = reader.GetInt32("produto_id"),
                NomeProduto = reader.GetString("nome_produto"),
                Quantidade = reader.GetInt32("quantidade"),
                PrecoUnitario = reader.GetDecimal("preco_unitario"),
                ValorTotal = reader.GetDecimal("valor_total"),
                DataVenda = reader.GetDateTime("data_venda")
            });
        }

        return vendas;
    }

    // Buscar uma venda pelo ID com JOIN
    public Venda? GetById(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            SELECT
                v.id,
                v.cliente_id,
                c.nome AS nome_cliente,
                v.produto_id,
                p.nome AS nome_produto,
                v.quantidade,
                v.preco_unitario,
                v.valor_total,
                v.data_venda
            FROM vendas v
            INNER JOIN clientes c ON c.id = v.cliente_id
            INNER JOIN produtos p ON p.id = v.produto_id
            ORDER BY v.id DESC;
            WHERE v.id = @Id;
            """;

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new Venda
        {
            Id = reader.GetInt32("id"),
            ClienteId = reader.GetInt32("cliente_id"),
            NomeCliente = reader.GetString("nome_cliente"),
            ProdutoId = reader.GetInt32("produto_id"),
            NomeProduto = reader.GetString("nome_produto"),
            Quantidade = reader.GetInt32("quantidade"),
            PrecoUnitario = reader.GetDecimal("preco_unitario"),
            ValorTotal = reader.GetDecimal("valor_total"),
            DataVenda = reader.GetDateTime("data_venda")
        };
    }

    // Registrar a venda e baixar o estoque na mesma transação
    public Venda RealizarVendaComBaixaEstoque(Venda venda)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var transaction = conn.BeginTransaction();

        try
        {
            const string sqlEstoque = """
                UPDATE produtos
                SET estoque = estoque - @Quantidade
                WHERE id = @ProdutoId
                  AND estoque >= @Quantidade
                  AND ativo = TRUE;
                """;

            using (var cmdEstoque = new MySqlCommand(
                sqlEstoque, conn, transaction))
            {
                cmdEstoque.Parameters.AddWithValue(
                    "@ProdutoId", venda.ProdutoId);

                cmdEstoque.Parameters.AddWithValue(
                    "@Quantidade", venda.Quantidade);

                int linhasAfetadas = cmdEstoque.ExecuteNonQuery();

                if (linhasAfetadas == 0)
                {
                    throw new InvalidOperationException(
                        "Produto inexistente, inativo ou estoque insuficiente.");
                }
            }

            const string sqlVenda = """
                INSERT INTO vendas
                    (cliente_id, produto_id, quantidade,
                     preco_unitario, valor_total, data_venda)
                VALUES
                    (@ClienteId, @ProdutoId, @Quantidade,
                     @PrecoUnitario, @ValorTotal, @DataVenda);
                """;

            using var cmdVenda = new MySqlCommand(
                sqlVenda, conn, transaction);

            cmdVenda.Parameters.AddWithValue(
                "@ClienteId", venda.ClienteId);

            cmdVenda.Parameters.AddWithValue(
                "@ProdutoId", venda.ProdutoId);

            cmdVenda.Parameters.AddWithValue(
                "@Quantidade", venda.Quantidade);

            cmdVenda.Parameters.AddWithValue(
                "@PrecoUnitario", venda.PrecoUnitario);

            cmdVenda.Parameters.AddWithValue(
                "@ValorTotal", venda.ValorTotal);

            cmdVenda.Parameters.AddWithValue(
                "@DataVenda", venda.DataVenda);

            cmdVenda.ExecuteNonQuery();

            using var cmdId = new MySqlCommand(
                "SELECT LAST_INSERT_ID();", conn, transaction);

            venda.Id = Convert.ToInt32(cmdId.ExecuteScalar());

            transaction.Commit();

            return venda;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    // Inserção simples mantida para compatibilidade com o repositório
    public Venda Add(Venda venda)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            INSERT INTO vendas
                (cliente_id, produto_id, quantidade,
                 preco_unitario, valor_total, data_venda)
            VALUES
                (@ClienteId, @ProdutoId, @Quantidade,
                 @PrecoUnitario, @ValorTotal, @DataVenda);

            SELECT LAST_INSERT_ID();
            """;

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@ClienteId", venda.ClienteId);
        cmd.Parameters.AddWithValue("@ProdutoId", venda.ProdutoId);
        cmd.Parameters.AddWithValue("@Quantidade", venda.Quantidade);
        cmd.Parameters.AddWithValue("@PrecoUnitario", venda.PrecoUnitario);
        cmd.Parameters.AddWithValue("@ValorTotal", venda.ValorTotal);
        cmd.Parameters.AddWithValue("@DataVenda", venda.DataVenda);

        venda.Id = Convert.ToInt32(cmd.ExecuteScalar());

        return venda;
    }
}