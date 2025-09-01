using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public class SeccionCapituloServicioMock : ISeccionCapituloServicio
    {
        public Task<List<SeccionCapituloDto>> ConsultarSeccionCapitulos(string guiMacroproceso, int IdProyecto, string IdInstancia)
        {
            throw new System.NotImplementedException();
        }
        public Task<List<SeccionCapituloDto>> ConsultarSeccionCapitulosByMacroproceso(string guiMacroproceso,string NivelId,string FlujoId)
        {
            throw new System.NotImplementedException();
        }
        public Task<RespuestaGeneralDto> ValidarSeccionCapitulos(string guidMacroproceso, int IdProyecto, string IdInstancia)
        {
            throw new System.NotImplementedException();
        }
        public Task<CapituloModificado> ObtenerCapitulosModificados(string capitulo, string seccion, string guiMacroproceso, int idProyecto, string idInstancia)
        {
            throw new System.NotImplementedException();
        }
        public Task<List<ErroresProyectoDto>> ObtenerErroresProyecto(string GuidMacroproceso, int idProyecto, string GuidInstancia)
        {
            throw new System.NotImplementedException();
        }
        public Task<List<ErroresTramiteDto>> ObtenerErroresTramite(string GuidMacroproceso, string GuidInstancia, string AccionId, string usuarioDNP, bool tieneCDP)
        {
            throw new System.NotImplementedException();
        }
        public Task<List<ErroresTramiteDto>> ObtenerErroresViabilidad(string GuiMacroproceso, int ProyectoId, string NivelId, string InstanciaId)
        {
            throw new System.NotImplementedException();
        }
        public Task<List<SeccionesTramiteDto>> ObtenerSeccionesTramite(string GuidMacroproceso, string GuidInstancia)
        {
            throw new System.NotImplementedException();
        }
        public Task<SeccionesCapitulos> EliminarCapituloModificado(CapituloModificado capituloModificado)
        {
            try
            {
                SeccionesCapitulos resultado = new SeccionesCapitulos();
                var result = string.Empty;//consulta de base de datos


                if (string.IsNullOrEmpty(result))
                {
                    resultado.Exito = true;
                    resultado.Mensaje = "Capitulo modificado Eliminado Exitosamente!";
                    return Task.FromResult(resultado);
                }
                else
                {
                    resultado.Exito = false;
                    resultado.Mensaje = "No se elimino la información";
                    return Task.FromResult(resultado);
                }

            }
            catch (ServiciosNegocioException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Task<List<ErroresPreguntasDto>> ObtenerErroresAprobacionRol(string GuiMacroproceso, int idProyecto, string GuidInstancia) {
            throw new System.NotImplementedException();
        }

        public Task<List<SeccionesTramiteDto>> ObtenerSeccionesPorFase(string GuidInstancia, string GuidFaseNivel)
        {
            return new Task<List<SeccionesTramiteDto>>(() => new List<SeccionesTramiteDto>()
            {
                new SeccionesTramiteDto(){ Id=1, Nombre="Pestaña 1", NombreModificado="Pestna1", NombrePestana="Pestana1", Porcentaje=100 },
                new SeccionesTramiteDto(){ Id=1, Nombre="Pestaña 2", NombreModificado="Pestna2", NombrePestana="Pestana2", Porcentaje=50 },
                new SeccionesTramiteDto(){ Id=1, Nombre="Pestaña 3", NombreModificado="Pestna3", NombrePestana="Pestana3", Porcentaje=0 },
            });
        }

        public Task<List<ErroresProyectoDto>> ObtenerErroresSeguimiento(string GuidMacroproceso, int idProyecto, string GuidInstancia)
        {
            return new Task<List<ErroresProyectoDto>>(() => new List<ErroresProyectoDto>());
        }

        //public Task<List<ErroresProyectoDto>> ObtenerErroresProgramacion(string GuidInstancia, string AccionId)
        //{
        //    //return new Task<List<ErroresProyectoDto>>(() => new List<ErroresProyectoDto>()
        //    //{
        //    //    new ErroresProyectoDto() {Seccion = "aprobacion",Capitulo = "confirmacionapr",Errores = "NULL" }
        //    //});
        //    throw new System.NotImplementedException();


        //}

        public async Task<List<ErroresProyectoDto>> ObtenerErroresProgramacion(string GuidInstancia, string AccionId)
        {
            List<ErroresProyectoDto> lista = new List<ErroresProyectoDto>();
            await Task.Run(() =>
            {
                ErroresProyectoDto error = new ErroresProyectoDto();
                error.Seccion = "viabilidadtecnico";
                error.Capitulo = "generales";
                error.Errores = "";
                lista.Add(error);
            });



            return lista;
        }



    }
}
