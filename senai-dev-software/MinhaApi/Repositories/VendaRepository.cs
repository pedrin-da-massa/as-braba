using MinhaApi.Models;

using MySqlConnector;

namespace MinhaApi.Repositories;

public class VendaRepository : IVendaRepository
{
    private readonly string _connectionString;

    public VendaRepository(IConfiguration config)
    {
        _connectionString =
            config.GetConnectionString("DefaultConnection")!;
    }

    // POST - registra uma venda
    public Venda Add(Venda venda)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = @"
            INSERT INTO vendas
            (
                cliente_id,
                produto_id,
                quantidade,
                preco_unitario,
                valor_total,
                data_venda
            )
            VALUES
            (
                @ClienteId,
                @ProdutoId,
                @Quantidade,
                @PrecoUnitario,
                @ValorTotal,
                @DataVenda
            );

            SELECT LAST_INSERT_ID();";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@ClienteId", venda.ClienteId);
        cmd.Parameters.AddWithValue("@ProdutoId", venda.ProdutoId);
        cmd.Parameters.AddWithValue("@Quantidade", venda.Quantidade);
        cmd.Parameters.AddWithValue("@PrecoUnitario", venda.PrecoUnitario);
        cmd.Parameters.AddWithValue("@ValorTotal", venda.ValorTotal);
        cmd.Parameters.AddWithValue("@DataVenda", venda.DataVenda);

        var idGerado = cmd.ExecuteScalar();

        venda.Id = Convert.ToInt32(idGerado);

        return venda;
    }

    // GET - lista todas as vendas
    public IEnumerable<Venda> GetAll()
    {
        var lista = new List<Venda>();

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = @"
            SELECT
                id,
                cliente_id,
                produto_id,
                quantidade,
                preco_unitario,
                valor_total,
                data_venda
            FROM vendas
            ORDER BY id DESC";

        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Venda
            {
                Id = reader.GetInt32("id"),
                ClienteId = reader.GetInt32("cliente_id"),
                ProdutoId = reader.GetInt32("produto_id"),
                Quantidade = reader.GetInt32("quantidade"),
                PrecoUnitario = reader.GetDecimal("preco_unitario"),
                ValorTotal = reader.GetDecimal("valor_total"),
                DataVenda = reader.GetDateTime("data_venda")
            });
        }

        return lista;
    }

    // GET - busca venda por ID
    public Venda? GetById(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = @"
            SELECT
                id,
                cliente_id,
                produto_id,
                quantidade,
                preco_unitario,
                valor_total,
                data_venda
            FROM vendas
            WHERE id = @Id";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Venda
            {
                Id = reader.GetInt32("id"),
                ClienteId = reader.GetInt32("cliente_id"),
                ProdutoId = reader.GetInt32("produto_id"),
                Quantidade = reader.GetInt32("quantidade"),
                PrecoUnitario = reader.GetDecimal("preco_unitario"),
                ValorTotal = reader.GetDecimal("valor_total"),
                DataVenda = reader.GetDateTime("data_venda")
            };
        }

        return null;
    }
}