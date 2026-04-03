using ApiNexusERP.DTOs;
using ApiNexusERP.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetModelsNexusERP.Helpers;
using NugetModelsNexusERP.Models;
using System.Threading.Tasks;

namespace ApiNexusERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosController : ControllerBase
    {
        private RepositoryDepartamentos repo;
        private HelperSessionContextAccessor contextAccessor;

        public DepartamentosController(RepositoryDepartamentos repo, HelperSessionContextAccessor contextAccessor)
        {
            this.repo = repo;
            this.contextAccessor = contextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<List<DepartamentoDTO>>> GetDepartamentos()
        {
            List<Departamento> departamentos = await this.repo.GetDepartamentosAsync();
            var listaDTO = departamentos.Select(d => new DepartamentoDTO
            {
                Id = d.Id,
                Nombre = d.Nombre,
                PresupuestoAnual = d.PresupuestoAnual
            }).ToList();

            return Ok(listaDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Departamento>> FindDepartamento(int id)
        {
            Departamento departamento = await this.repo.FindDepartamentoAsync(id);

            var dto = new DepartamentoDTO
            {
                Id = departamento.Id,
                Nombre = departamento.Nombre,
                PresupuestoAnual = departamento.PresupuestoAnual
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Post(DepartamentoDTO dto)
        {
            int idEmpresa = this.contextAccessor.GetEmpresaIdSession();

            Departamento departamento = new Departamento
            {
                EmpresaId = idEmpresa,
                Nombre = dto.Nombre,
                PresupuestoAnual = dto.PresupuestoAnual
            };

            Departamento nuevoDep = await this.repo.CreateDepartamentoAsync(departamento);
            dto.Id = nuevoDep.Id;
            return Ok(dto);
        }

        [HttpPut]
        public async Task<ActionResult> Put(DepartamentoDTO dto)
        {
            Departamento departamento = new Departamento
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                PresupuestoAnual = dto.PresupuestoAnual
            };
            Departamento departamentoActualizado = await this.repo.UpdateDepartamentoAsync(departamento);

            if (departamentoActualizado == null)
            {
                return NotFound(new { mensaje = "No se ha encontrado el departamento para modificar." });
            }

            return Ok(new { mensaje = "Modificado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool eliminado = await this.repo.DeleteDepartamentoAsync(id);

            if (!eliminado)
            {
                return NotFound(new { mensaje = "No se ha encontrado el departamento para eliminar." });
            }

            return Ok(new { mensaje = "Eliminado correctamente." });
        }


    }
}
