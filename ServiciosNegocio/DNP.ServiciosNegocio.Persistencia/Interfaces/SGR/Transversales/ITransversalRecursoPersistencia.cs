using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.CadenaValor;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;
using DNP.ServiciosNegocio.Dominio.Dto.Productos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Transversales
{

    public interface ITransversalRecursoPersistencia
    {
        DesagregarRegionalizacionDto ObtenerDesagregarRegionalizacionSgr(string bpin);
        //DatosGeneralesProyectosDto ObtenerDatosGeneralesProyectoSgr(int? pProyectoId, Guid pNivelId);
        FocalizacionPoliticaSgrDto ObtenerFocalizacionPoliticasTransversalesFuentesSgr(string bpin);
        RespuestaGeneralDto GuardarFocalizacionCategoriasAjustesSgr(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario);
        //PoliticasTCrucePoliticasDto ObtenerPoliticasTransversalesCrucePoliticasSgr(string bpin, int IdFuente);
        //IndicadoresPoliticaDto ObtenerDatosIndicadoresPoliticaSgr(string bpin);
        //IndicadoresPoliticaDto ObtenerDatosCategoriaProductosPoliticaSgr(string Bpin, int fuenteId, int politicaId);

        string ObtenerPoliticasTransversalesProyectoSgr(string Bpin);
        RespuestaGeneralDto EliminarPoliticasProyectoSgr(int proyectoId, int politicaId);
        string AgregarPoliticasTransversalesAjustesSgr(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario);
        string ConsultarPoliticasCategoriasIndicadoresSgr(Guid instanciaId);
        string ObtenerPoliticasTransversalesCategoriasSgr(Guid instanciaId);
        RespuestaGeneralDto EliminarCategoriaPoliticasProyectoSgr(int proyectoId, int politicaId, int categoriaId);
        ResultadoProcedimientoDto ModificarPoliticasCategoriasIndicadoresSgr(CategoriasIndicadoresDto parametrosGuardar, string usuario);
        string ObtenerCrucePoliticasAjustesSgr(Guid instanciaId);
        string GuardarCrucePoliticasAjustesSgr(ParametrosGuardarDto<List<CrucePoliticasAjustesDto>> parametrosGuardar, string name);
        string ObtenerPoliticasTransversalesResumenSgr(Guid instanciaId);
    }
}

