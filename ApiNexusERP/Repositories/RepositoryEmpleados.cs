using Microsoft.EntityFrameworkCore;
using NugetModelsNexusERP.Data;
using NugetModelsNexusERP.Models;
using System.Threading.Tasks;

namespace ApiNexusERP.Repositories
{
    public class RepositoryEmpleados
    {
        private NexusContext context;

        public RepositoryEmpleados(NexusContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> FindEmpleadoAsync(int idEmpleado)
        {
            return await this.context.Empleados
                .FirstOrDefaultAsync(e => e.Id == idEmpleado);
        }

        public async Task<List<Empleado>> GetEmpleadosDepartamentoAsync(int idDepartamento)
        {
            return await this.context.Empleados
                .Where(e => e.DepartamentoId == idDepartamento)
                .ToListAsync();
        }

        public async Task<int> GetNumeroTotalEmpleados()
        {
            return await this.context.Empleados.CountAsync();
        }

        public async Task<decimal> GetSalarioPromedioAnualAsync()
        {
            return await this.context.Empleados.AverageAsync(e => (decimal?)e.SalarioBrutoAnual) ?? 0;
        }

        public async Task<decimal> GetSalarioPromedioAnualPorDepartamentoAsync(int idDepartamento)
        {
            return await this.context.Empleados
                .Where(e => e.DepartamentoId == idDepartamento)
                .AverageAsync(e => (decimal?)e.SalarioBrutoAnual) ?? 0;
        }
    }
}
