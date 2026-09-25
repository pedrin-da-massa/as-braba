using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhaApi.Models
{
    [Table("fornecedor_departamentos")]
    public class Departamentos
    {
        [Key, Column(Order = 0)]
        public int IdFornecedor { get; set; }

        [Key, Column(Order = 1)]
        public int IdDepartamento { get; set; }
    }
}