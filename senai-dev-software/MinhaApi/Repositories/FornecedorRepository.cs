
using Microsoft.Extensions.Configuration;
using MinhaApi.Models;
using MySqlConnector;

namespace MinhaApi.Repositories;

public class FornecedorRepository : IFornecedorRepository
{
    private readonly string _connectionString;

    public FornecedorRepository(IConfiguration config)
    {
        _connectionString =
            config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "A conexão DefaultConnection não foi configurada.");
    }

    public IEnumerable<Fornecedor> GetAll()
    {
        var lista = new List<Fornecedor>();

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            SELECT id, nome, email, cnpj, telefone, ativo
            FROM fornecedor
            ORDER BY id;
            """;

        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            lista.Add(MapearFornecedor(reader));

        return lista;
    }

    public Fornecedor? GetById(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            SELECT id, nome, email, cnpj, telefone, ativo
            FROM fornecedor
            WHERE id = @Id;
            """;

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();

        return reader.Read() ? MapearFornecedor(reader) : null;
    }

    public void Add(Fornecedor fornecedor)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            INSERT INTO fornecedor
                (nome, email, cnpj, telefone, ativo)
            VALUES
                (@Nome, @Email, @Cnpj, @Telefone, @Ativo);

            SELECT LAST_INSERT_ID();
            """;

        using var cmd = new MySqlCommand(sql, conn);
        AdicionarParametros(cmd, fornecedor);

        fornecedor.Id = Convert.ToInt32(cmd.ExecuteScalar());
    }

    public void Update(Fornecedor fornecedor)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            UPDATE fornecedor
            SET nome = @Nome,
                email = @Email,
                cnpj = @Cnpj,
                telefone = @Telefone,
                ativo = @Ativo
            WHERE id = @Id;
            """;

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Id", fornecedor.Id);
        AdicionarParametros(cmd, fornecedor);

        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            UPDATE fornecedor
            SET ativo = FALSE
            WHERE id = @Id;
            """;

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }

    private static void AdicionarParametros(
        MySqlCommand cmd,
        Fornecedor fornecedor)
    {
        cmd.Parameters.AddWithValue("@Nome", fornecedor.Nome);
        cmd.Parameters.AddWithValue("@Email", fornecedor.Email);
        cmd.Parameters.AddWithValue("@Cnpj", fornecedor.Cnpj);
        cmd.Parameters.AddWithValue("@Telefone", fornecedor.Telefone);
        cmd.Parameters.AddWithValue("@Ativo", fornecedor.Ativo);
    }

    private static Fornecedor MapearFornecedor(MySqlDataReader reader)
    {
        return new Fornecedor
        {
            Id = reader.GetInt32("id"),
            Nome = reader.GetString("nome"),
            Email = reader.GetString("email"),
            Cnpj = reader.GetString("cnpj"),
            Telefone = reader.IsDBNull(reader.GetOrdinal("telefone"))
                ? string.Empty
                : reader.GetString("telefone"),
            Ativo = reader.GetBoolean("ativo")
        };
    }
}
