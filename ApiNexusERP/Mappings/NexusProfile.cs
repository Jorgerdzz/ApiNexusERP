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

            //EMPLEADOS
            CreateMap<Empleado, EmpleadoDTO>()
                .ForMember(dest => dest.NombreDepartamento, opt => opt.MapFrom(src => src.Departamento.Nombre))
                .ForMember(dest => dest.IbanEnmascarado, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.Iban) || src.Iban.Length < 4
                    ? src.Iban
                    : $"**** **** **** {src.Iban.Substring(src.Iban.Length - 4)}"));
            CreateMap<EmpleadoDTO, Empleado>();
        }
    }
}
