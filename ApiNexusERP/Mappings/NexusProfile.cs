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

            //NOMINAS
            CreateMap<NominaDetalle, NominaDetalleDTO>();
            CreateMap<NominaDetalleDTO, NominaDetalle>(MemberList.None);

            CreateMap<Nomina, NominaDTO>()
                .ForMember(dest => dest.NombreCompletoEmpleado, opt => opt.MapFrom(src => src.Empleado.Nombre + " " + src.Empleado.Apellidos))
                .ForMember(dest => dest.DniEmpleado, opt => opt.MapFrom(src => src.Empleado.Dni))
                .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.NominaDetalles));

            CreateMap<NominaDTO, Nomina>(MemberList.None)
                .ForMember(dest => dest.NominaDetalles, opt => opt.MapFrom(src => src.Detalles));
        }
    }
}
