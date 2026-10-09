using System;
using System.Collections.Generic;
using System.Linq;

namespace MinhaApi.Services
{
    public class Departamento
    {
        public string Nome { get; set; }
    }

    public class DepartamentoService
    {
        private readonly List<Departamento> _departamentos = new List<Departamento>();

        public void AdicionarDepartamento(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O campo nome é obrigatório.");
            }

            if (_departamentos.Any(d => d.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Já existe um departamento com o mesmo nome.");
            }

            _departamentos.Add(new Departamento { Nome = nome });
        }

        public IEnumerable<Departamento> ObterDepartamentos()
        {
            return _departamentos;
        }
    }
}