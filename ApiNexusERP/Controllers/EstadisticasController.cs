using ApiNexusERP.DTOs;
using ApiNexusERP.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetModelsNexusERP.Helpers;

namespace ApiNexusERP.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticasController : ControllerBase
    {
        private RepositoryEstadisticas repo;
        private HelperSessionContextAccessor contextAccessor;

        public EstadisticasController(RepositoryEstadisticas repo, HelperSessionContextAccessor contextAccessor)
        {
            this.repo = repo;
            this.contextAccessor = contextAccessor;
        }

        [HttpGet("[action]/{anio}")]
        public async Task<ActionResult<List<ReporteMensualDTO>>> Ingresos(int anio)
        {
            int empresaId = this.contextAccessor.GetEmpresaIdSession();
            var data = await this.repo.GetIngresosPorMesAsync(anio, empresaId);
            return Ok(data);
        }

        [HttpGet("[action]/{anio}")]
        public async Task<ActionResult<List<ReporteMensualDTO>>> Gastos(int anio)
        {
            int empresaId = this.contextAccessor.GetEmpresaIdSession();
            var data = await this.repo.GetGastosPorMesAsync(anio, empresaId);
            return Ok(data);
        }

        [HttpGet("[action]/{anio}")]
        public async Task<ActionResult<List<ReporteDepartamentoDTO>>> CostesDepartamento(int anio)
        {
            int empresaId = this.contextAccessor.GetEmpresaIdSession();
            var data = await this.repo.GetCostesPorDepartamentoAsync(anio, empresaId);
            return Ok(data);
        }
    }
}
