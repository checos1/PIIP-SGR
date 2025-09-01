using DNP.ServiciosNegocio.Persistencia.Interfaces;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Entidades
{
    using AutoMapper;
    using Comunes.Utilidades;
    using Interfaces.Entidades;
    using Dominio.Dto.Entidades;
    using Modelo;
    using System.Linq;
    using System;

    public class EntidadAccionesPersistencia : Persistencia, IEntidadAccionesPersistencia
    {
        #region Incializacion

            public EntidadAccionesPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
            {
            }
        #endregion

        #region Consulta

        public List<EntidadDestinoDto> ObtenerEntidadesDestino(string bPin, List<RolDto> listaRoles, Guid? instanciaId)
        {
            var listadoRoles = Contexto.uspGetEntidadesDestinoRoles(bPin, JsonUtilidades.ACadenaJson(listaRoles), instanciaId);

            List<EntidadDestinoDto> listadoRetorno = new List<EntidadDestinoDto>();
            foreach (var rol in listadoRoles.ToList())
            {
                listadoRetorno.Add(MapearRoles(rol));
            }

            if(listaRoles.Any(x=>x.IdRol.ToString() == "B2BB6FC2-94C8-4394-9518-251523E2A07A"))
            {
                var listadoAprobadores = Contexto.uspGetEntidadesAprobadoras(bPin);
                foreach (var rol in listadoAprobadores)
                {
                    var item = MapearRolesAprobadores(rol);
                    if(!listadoRetorno.Any(x=>x.IdEntidadMGA == item.IdEntidadMGA))
                        listadoRetorno.Add(MapearRolesAprobadores(rol));
                }
            }
            return listadoRetorno;
        }
        #endregion

        #region Metodos utilitarios
            private EntidadDestinoDto MapearRoles(object rol)
            {
                Mapper.Reset();
                Mapper.Initialize(cfg => cfg.CreateMap<uspGetEntidadesDestinoRoles_Result, EntidadDestinoDto>()
                                            .ForMember(dto => dto.IdEntidadMGA, opt => opt.MapFrom(ent => ent.IdEntidad))
                                            .ForMember(dto => dto.NombreEntidadMGA, opt => opt.MapFrom(ent => ent.NombreEntidad)));
                return Mapper.Map<EntidadDestinoDto>(rol);
            }

        private EntidadDestinoDto MapearRolesAprobadores(object rol)
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<uspGetEntidadesAprobadoras_Result, EntidadDestinoDto>()
                                        .ForMember(dto => dto.IdEntidadMGA, opt => opt.MapFrom(ent => ent.IdEntidad))
                                        .ForMember(dto => dto.NombreEntidadMGA, opt => opt.MapFrom(ent => ent.NombreEntidad)));
            return Mapper.Map<EntidadDestinoDto>(rol);
        }
        #endregion
    }
}
