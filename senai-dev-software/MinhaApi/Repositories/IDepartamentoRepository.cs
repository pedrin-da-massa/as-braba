using MinhaApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinhaApi.Repositories
{
    public interface IDepartamentoRepository
    {
        Task<IEnumerable<Departamentos>> GetAllAsync();
        Task<Departamentos> GetByIdAsync(int id);
        Task<Departamentos> CreateAsync(Departamentos departamento);
        Task<Departamentos> UpdateAsync(int id, Departamentos departamento);
        Task<bool> DeleteAsync(int id);
    }
}