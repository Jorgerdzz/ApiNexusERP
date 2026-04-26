using ApiNexusERP.DTOs;
using Microsoft.EntityFrameworkCore;
using NugetModelsNexusERP.Data;

namespace ApiNexusERP.Repositories
{
    public class RepositoryEstadisticas
    {
        private NexusContext context;

        public RepositoryEstadisticas(NexusContext context)
        {
            this.context = context;
        }

        public async Task<List<ReporteMensualDTO>> GetIngresosPorMesAsync(int anio, int empresaId)
        {
            return await this.context.Facturas
                .Where(f => f.FechaEmision.Year == anio && f.EmpresaId == empresaId)
                .GroupBy(f => f.FechaEmision.Month)
                .Select(g => new ReporteMensualDTO
                {
                    Mes = g.Key,
                    Total = g.Sum(f => f.TotalFactura)
                })
                .OrderBy(r => r.Mes)
                .ToListAsync();
        }

        public async Task<List<ReporteMensualDTO>> GetGastosPorMesAsync(int anio, int empresaId)
        {
            return await this.context.ControlGastos
                .Where(c => c.Anio == anio && c.EmpresaId == empresaId)
                .GroupBy(c => c.Mes)
                .Select(g => new ReporteMensualDTO
                {
                    Mes = g.Key,
                    Total = g.Sum(c => c.ImporteGasto)
                })
                .OrderBy(r => r.Mes)
                .ToListAsync();
        }

        public async Task<List<ReporteDepartamentoDTO>> GetCostesPorDepartamentoAsync(int anio, int empresaId)
        {
            return await this.context.ControlGastos
                .Include(c => c.Departamento)
                .Where(c => c.Anio == anio && c.EmpresaId == empresaId)
                .GroupBy(c => c.Departamento.Nombre)
                .Select(g => new ReporteDepartamentoDTO
                {
                    Departamento = g.Key ?? "Sin Departamento",
                    Total = g.Sum(c => c.ImporteGasto)
                })
                .OrderByDescending(r => r.Total)
                .ToListAsync();
        }
    }
}
