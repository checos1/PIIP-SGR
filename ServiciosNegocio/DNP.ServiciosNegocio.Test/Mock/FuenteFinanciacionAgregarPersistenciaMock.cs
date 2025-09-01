using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Test.Mock
{
    using System;
    using Comunes.Dto.Formulario;

    public class FuenteFinanciacionAgregarPersistenciaMock : IFuenteFinanciacionAgregarPersistencia
    {
        public string ConsultarCostosPIIPvsFuentesPIIP(string bpin)
        {
            throw new NotImplementedException();
        }

        public string ConsultarResumenFteFinanciacion(string bpin)
        {
            throw new NotImplementedException();
        }

        public FuenteFinanciacionResultado EliminarFuentesFinanciacionProyecto(int fuentesFinanciacionId)
        {
            throw new NotImplementedException();
        }

        public string FuentesFinanciacionRecursosAjustesAgregar(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuario)
        {
            throw new NotImplementedException();
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar, string usuario)
        {
            
        }

        public void GuardarFuenteFinanciacion(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        public string ObtenerDetalleAjustesFuenteFinanciacion(string bpin, string usuario)
        {
            throw new NotImplementedException();
        }

        public ProyectoFuenteFinanciacionAgregarDto ObtenerFuenteFinanciacionAgregar(string bpin)

        {
            if (bpin.Equals("2017011000042"))
            {
                return new ProyectoFuenteFinanciacionAgregarDto()
                {
                    ProyectoId = 2029,
                    BPIN = "2017011000042",
                    FuentesFinanciacionAgregar = new List<FuenteFinanciacionAgregarDto> {
                    new FuenteFinanciacionAgregarDto()
                    {
                       FuenteId= 105,
                       IdGrupoRecurso= 2,
                       CodigoGrupoRecurso= "2",
                       NombreGrupoRecurso= "Empresa",
                       IdTipoEntidad= 2,
                       CodigoTipoEntidad= "2",
                       NombreTipoEntidad= "Empresas públicas",
                       IdEntidad= null,
                       CodigoEntidad= null,
                       NombreEntidad= "Centro cultural del Oriente Colombiano",
                       IdTipoRecurso= 3,
                       CodigoTipoRecurso= "3",
                       NombreTipoRecurso= "Propios"
                    }
                    },

                };
            }
            else
            {
                return null;
            }
        }

        public string ObtenerFuenteFinanciacionAgregarN(string bpin)
        {
            throw new NotImplementedException();
        }

        public ProyectoFuenteFinanciacionAgregarDto ObtenerFuenteFinanciacionAgregarPreview()
        {
            return new ProyectoFuenteFinanciacionAgregarDto()
            {
                ProyectoId = 2029,
                BPIN = "2017011000042",
                FuentesFinanciacionAgregar = new List<FuenteFinanciacionAgregarDto> {
                    new FuenteFinanciacionAgregarDto()
                    {
                       FuenteId= 105,
                       IdGrupoRecurso= 2,
                       CodigoGrupoRecurso= "2",
                       NombreGrupoRecurso= "Empresa",
                       IdTipoEntidad= 2,
                       CodigoTipoEntidad= "2",
                       NombreTipoEntidad= "Empresas públicas",
                       IdEntidad= null,
                       CodigoEntidad= null,
                       NombreEntidad= "Centro cultural del Oriente Colombiano",
                       IdTipoRecurso= 3,
                       CodigoTipoRecurso= "3",
                       NombreTipoRecurso= "Propios"
                    },
                    new FuenteFinanciacionAgregarDto()
                    {
                       FuenteId= 106,
                       IdGrupoRecurso= 2,
                       CodigoGrupoRecurso= "2",
                       NombreGrupoRecurso= "Empresa",
                       IdTipoEntidad= 2,
                       CodigoTipoEntidad= "2",
                       NombreTipoEntidad= "Empresas públicas",
                       IdEntidad= null,
                       CodigoEntidad= null,
                       NombreEntidad= "Centro cultural del Oriente Colombiano",
                       IdTipoRecurso= 3,
                       CodigoTipoRecurso= "3",
                       NombreTipoRecurso= "Propios"
                    },
                     new FuenteFinanciacionAgregarDto()
                    {
                       FuenteId= 107,
                       IdGrupoRecurso= 2,
                       CodigoGrupoRecurso= "2",
                       NombreGrupoRecurso= "Empresa",
                       IdTipoEntidad= 2,
                       CodigoTipoEntidad= "2",
                       NombreTipoEntidad= "Empresas públicas",
                       IdEntidad= null,
                       CodigoEntidad= null,
                       NombreEntidad= "Centro cultural del Oriente Colombiano",
                       IdTipoRecurso= 3,
                       CodigoTipoRecurso= "3",
                       NombreTipoRecurso= "Propios"
                    }
                    }

            };
        }

        public string ObtenerFuenteFinanciacionVigencia(string bpin)
        {
            throw new NotImplementedException();
        }

        public string ObtenerResumenCostosVsSolicitado(string bpin)
        {
            throw new NotImplementedException();
        }

        public string ObtenerDetalleAjustesJustificaionFacalizacionPT(string bpin, string usuario)
        {
            return string.Empty;
        }

        public OperacionCreditoDatosGeneralesDto ObtenerOperacionCreditoDatosGenerales(string bpin, Guid? instanciaId)
        {
            throw new NotImplementedException();
        }

        public FuenteFinanciacionResultado GuardarOperacionCreditoDatosGenerales(OperacionCreditoDatosGeneralesDto OperacionCreditoDatosGeneralesDto, string usuario)
        {
            throw new NotImplementedException();
        }

        public OperacionCreditoDetallesDto ObtenerOperacionCreditoDetalles(string bpin, Guid? instanciaId)
        {
            throw new NotImplementedException();
        }
        FuenteFinanciacionResultado GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto OperacionCreditoDetallesDto, string usuario)
        {
            throw new NotImplementedException();
        }

        FuenteFinanciacionResultado IFuenteFinanciacionAgregarPersistencia.GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto OperacionCreditoDetallesDto, string usuario)
        {
            throw new NotImplementedException();
        }
    }
}
