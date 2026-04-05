using ApiNexusERP.DTOs;
using AutoMapper;
using NugetModelsNexusERP.Models;

namespace ApiNexusERP.Mappings
{
    public class NexusProfile: Profile
    {
        public NexusProfile()
        {
            //DEPARTAMENTOS
            CreateMap<Departamento, DepartamentoDTO>();
            CreateMap<DepartamentoDTO, Departamento>();

            //CLIENTES
            CreateMap<Cliente, ClienteDTO>();
            CreateMap<ClienteDTO, Cliente>();

            //EMPRESAS
            CreateMap<Empresa, EmpresaDTO>();
            CreateMap<EmpresaDTO, Empresa>();
        }
    }
}
