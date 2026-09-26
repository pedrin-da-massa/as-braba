
using System.Collections.Generic;
using System.Threading.Tasks;
using MinhaApi.Models;

namespace MinhaApi.Services
{
    public interface IDepartamentoService
    {
        Task<IEnumerable<Departamentos>> GetAllDepartamentosAsync();
        Task<Departamentos> GetDepartamentoByIdAsync(int id);
        Task AddDepartamentoAsync(Departamentos departamento);
        Task UpdateDepartamentoAsync(Departamentos departamento);
        Task DeleteDepartamentoAsync(int id);
    }
}