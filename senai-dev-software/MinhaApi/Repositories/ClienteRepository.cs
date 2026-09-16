using MinhaApi.Models;
using MySqlConnector;

namespace MinhaApi.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly string _connectionString;

    public ClienteRepository(IConfiguration config)
    {
        _connectionString =
            config.GetConnectionString("DefaultConnection")!;
    }

    // GET ALL
    public IEnumerable<Cliente> GetAll()
    {
        var lista = new List<Cliente>();

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = @"
            SELECT id, nome, email, cpf, ativo
            FROM clientes";

        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Cliente
            {
                Id = reader.GetInt32("id"),
                Nome = reader.GetString("nome"),
                Email = reader.GetString("email"),
                Cpf = reader.IsDBNull(reader.GetOrdinal("cpf"))
                    ? null
                    : reader.GetString("cpf"),
                Ativo = reader.GetBoolean("ativo")
            });
        }

        return lista;
    }

    // GET BY ID
    public Cliente? GetById(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = @"
            SELECT id, nome, email, cpf, ativo
            FROM clientes
            WHERE id = @Id";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Cliente
            {
                Id = reader.GetInt32("id"),
                Nome = reader.GetString("nome"),
                Email = reader.GetString("email"),
                Cpf = reader.IsDBNull(reader.GetOrdinal("cpf"))
                    ? null
                    : reader.GetString("cpf"),
                Ativo = reader.GetBoolean("ativo")
            };
        }

        return null;
    }

    // POST
    public void Add(Cliente cliente)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = @"
            INSERT INTO clientes
                (nome, email, cpf, ativo)
            VALUES
                (@Nome, @Email, @Cpf, @Ativo);

            SELECT LAST_INSERT_ID();";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
        cmd.Parameters.AddWithValue("@Email", cliente.Email);
        cmd.Parameters.AddWithValue("@Cpf",
            cliente.Cpf ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Ativo", cliente.Ativo);

        var idGerado = cmd.ExecuteScalar();

        cliente.Id = Convert.ToInt32(idGerado);
    }

    // PUT
    public void Update(Cliente cliente)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = @"
            UPDATE clientes
            SET
                nome = @Nome,
                email = @Email,
                cpf = @Cpf,
                ativo = @Ativo
            WHERE id = @Id";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Id", cliente.Id);
        cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
        cmd.Parameters.AddWithValue("@Email", cliente.Email);
        cmd.Parameters.AddWithValue("@Cpf",
            cliente.Cpf ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Ativo", cliente.Ativo);

        cmd.ExecuteNonQuery();
    }

    // DELETE - SOFT DELETE
    public void Delete(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string sql = @"
            UPDATE clientes
            SET ativo = false
            WHERE id = @Id";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }
}