using ApiNexusERP.DTOs;
using AutoMapper;
using NugetModelsNexusERP.Models;

namespace ApiNexusERP.Mappings
{
    public class NexusProfile: Profile
    {
        public NexusProfile()
        {
            CreateMap<Departamento, DepartamentoDTO>();
            CreateMap<DepartamentoDTO, Departamento>();
        }
    }
}
