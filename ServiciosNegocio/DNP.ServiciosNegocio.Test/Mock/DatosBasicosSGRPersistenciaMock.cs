using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos;

namespace DNP.ServiciosNegocio.Test.Mock
{
    using System;
    using Comunes.Dto.Formulario;

    public class DatosBasicosSGRPersistenciaMock : IDatosBasicosSGRPersistencia
    {
        public void GuardarDefinitivamente(ParametrosGuardarDto<DatosBasicosSGRDto> parametrosGuardar, string usuario)
        {
            
        }

        public DatosBasicosSGRDto ObtenerDatosBasicosSGR(string bpin)
        {
            if (bpin.Equals("2017011000042"))
            {
                return new DatosBasicosSGRDto()
                {
                    DatosBasicosSGRId = 1,
                    ProyectoId = 145896,
                    Bpin = "2017011000042",
                    NumeroPresentacion = "2",
                    FechaVerificacionRequisitos = new DateTime(2019,1,1),
                    ObjetivoSGRId = 1,
                    ObjetivoSGR = "objetivo1",
                    EjecutorPropuestoId = 2,
                    NitEjecutorPropuesto = "NitEjecutorPropuesto",
                    InterventorPropuestoId = 3,
                    NitInterventorPropuesto = "NitInterventorPropuesto",
                    InterventorPropuesto = "InterventorPropuesto",
                    TiempoEstimadoEjecucionFisicaFinanciera = 1,
                    EstimacionCostosFasesPosteriores = (decimal)1245.00,
                    FuentesInterventoria = new List<FuentesInterventoriaDto> {
                    new FuentesInterventoriaDto()
                    {
                        ProgramacionFuenteId = 2,
                        Vigencia = "2019",
                        GrupoRecurso = "PGN",
                        TipoEntidadId = 10,
                        TipoEntidad = "Nacion",
                        EntidadId = 8,
                        Entidad = "Entidad",
                        TipoRecursoId = 25,
                        TipoRecurso = "Propios",
                        Solicitado = 124578,
                        ValorAprobadoBienio1 = "258",
                        ValorAprobadoBienio2 = "268",
                        ValorAprobadoBienio3 = "278",
                        ValorAprobadoBienio4 = "288"
                    }

                    }

    };
            }
            else
            {
                return null;
            }
        }

        public DatosBasicosSGRDto ObtenerDatosBasicosSGRPreview()
        {
            return new DatosBasicosSGRDto()
            {
                DatosBasicosSGRId = 1,
                ProyectoId = 145896,
                Bpin = "2017011000042",
                NumeroPresentacion = "2",
                FechaVerificacionRequisitos = new DateTime(2019, 1, 1),
                ObjetivoSGRId = 1,
                ObjetivoSGR = "objetivo1",
                EjecutorPropuestoId = 2,
                NitEjecutorPropuesto = "NitEjecutorPropuesto",
                InterventorPropuestoId = 3,
                NitInterventorPropuesto = "NitInterventorPropuesto",
                InterventorPropuesto = "InterventorPropuesto",
                TiempoEstimadoEjecucionFisicaFinanciera = 1,
                EstimacionCostosFasesPosteriores = (decimal)1245.00,
                FuentesInterventoria = new List<FuentesInterventoriaDto> {
                    new FuentesInterventoriaDto()
                    {
                        ProgramacionFuenteId = 2,
                        Vigencia = "2019",
                        GrupoRecurso = "PGN",
                        TipoEntidadId = 10,
                        TipoEntidad = "Nacion",
                        EntidadId = 8,
                        Entidad = "Entidad",
                        TipoRecursoId = 25,
                        TipoRecurso = "Propios",
                        Solicitado = 124578,
                        ValorAprobadoBienio1 = "258",
                        ValorAprobadoBienio2 = "268",
                        ValorAprobadoBienio3 = "278",
                        ValorAprobadoBienio4 = "288"
                    }

                    }

            };
        }

        private FuentesInterventoriaDto ObtenerFilaBienio()
        {
            return new FuentesInterventoriaDto();
        }


        }
}
