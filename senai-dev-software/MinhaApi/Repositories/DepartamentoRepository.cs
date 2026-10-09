using System;
using System.Collections.Generic;
using MySqlConnector;

namespace MinhaApi.Repositories
{
    public class DepartamentoRepository
    {
        private readonly string connectionString;

        public DepartamentoRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Departamento> Listar()
        {
            var departamentos = new List<Departamento>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM Departamentos", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departamentos.Add(new Departamento
                        {
                            Id = reader.GetInt32("Id"),
                            Nome = reader.GetString("Nome")
                        });
                    }
                }
            }
            return departamentos;
        }

        public Departamento BuscarPorId(int id)
        {
            Departamento departamento = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM Departamentos WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        departamento = new Departamento
                        {
                            Id = reader.GetInt32("Id"),
                            Nome = reader.GetString("Nome")
                        };
                    }
                }
            }
            return departamento;
        }

        public void Criar(Departamento departamento)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("INSERT INTO Departamentos (Nome) VALUES (@Nome)", connection);
                command.Parameters.AddWithValue("@Nome", departamento.Nome);
                command.ExecuteNonQuery();
            }
        }

        public void Atualizar(Departamento departamento)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("UPDATE Departamentos SET Nome = @Nome WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Nome", departamento.Nome);
                command.Parameters.AddWithValue("@Id", departamento.Id);
                command.ExecuteNonQuery();
            }
        }

        public void Deletar(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("DELETE FROM Departamentos WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }

    public class Departamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}