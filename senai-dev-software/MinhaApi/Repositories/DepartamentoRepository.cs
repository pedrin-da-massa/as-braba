using MinhaApi.Models;
using MySqlConnector;

namespace MinhaApi.Repositories;

public class DepartamentoRepository : IDepartamentoRepository
{
    private readonly string _connectionString;

    public DepartamentoRepository(IConfiguration config)
    {
        _connectionString =
            config.GetConnectionString("DefaultConnection")!;
    }

    public IEnumerable<Departamento> GetAll()
    {
        var lista = new List<Departamento>();

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            SELECT id, nome, ativo
            FROM departamentos
            """;

        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Departamento
            {
                Id = reader.GetInt32("id"),
                Nome = reader.GetString("nome"),
                Ativo = reader.GetBoolean("ativo")
            });
        }

        return lista;
    }

    public Departamento? GetById(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            SELECT id, nome, ativo
            FROM departamentos
            WHERE id = @Id
            """;

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();

        if (!reader.Read())
            return null;

        return new Departamento
        {
            Id = reader.GetInt32("id"),
            Nome = reader.GetString("nome"),
            Ativo = reader.GetBoolean("ativo")
        };
    }

    public void Add(Departamento departamento)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            INSERT INTO departamentos (nome, ativo)
            VALUES (@Nome, @Ativo);

            SELECT LAST_INSERT_ID();
            """;

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Nome", departamento.Nome);
        cmd.Parameters.AddWithValue("@Ativo", departamento.Ativo);

        departamento.Id = Convert.ToInt32(cmd.ExecuteScalar());
    }

    public void Update(Departamento departamento)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            UPDATE departamentos
            SET nome = @Nome, ativo = @Ativo
            WHERE id = @Id
            """;

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Id", departamento.Id);
        cmd.Parameters.AddWithValue("@Nome", departamento.Nome);
        cmd.Parameters.AddWithValue("@Ativo", departamento.Ativo);

        cmd.ExecuteNonQuery();
    }

    public bool Exists(string nome)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            SELECT COUNT(*)
            FROM departamentos
            WHERE LOWER(nome) = LOWER(@Nome)
            """;

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Nome", nome.Trim());

        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    public bool HasActiveFornecedores(int departamentoId)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        const string sql = """
            SELECT COUNT(*)
            FROM fornecedor_departamentos fd
            INNER JOIN fornecedor f
                ON f.id = fd.IdFornecedor
            WHERE fd.IdDepartamento = @IdDepartamento
              AND f.ativo = 1
            """;

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@IdDepartamento", departamentoId);

        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }
}