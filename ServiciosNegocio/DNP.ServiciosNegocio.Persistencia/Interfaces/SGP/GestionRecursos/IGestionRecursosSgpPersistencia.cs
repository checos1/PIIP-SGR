using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
using DNP.ServiciosNegocio.Dominio.Dto.Productos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.GestionRecursos
{

    public interface IGestionRecursosSgpPersistencia
    {
        string ObtenerlocalizacionSgp(string bpin);
        string ObtenerFocalizacionPoliticasTransversalesFuentesSgp(string bpin);

        string ObtenerPoliticasTransversalesProyectoSgp(string Bpin);

        TramitesResultado EliminarPoliticasProyectoSgp(int tramiteidProyectoId, int politicaId);

        TramitesResultado AgregarPoliticasTransversalesSgp(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario);

        string ConsultarPoliticasCategoriasIndicadoresSgp(Guid instanciaId);

        TramitesResultado ModificarPoliticasCategoriasIndicadoresSgp(CategoriasIndicadoresDto parametrosGuardar, string usuario);

        string ObtenerPoliticasTransversalesCategoriasSgp(string instanciaId);

        TramitesResultado EliminarCategoriaPoliticasProyectoSgp(int proyectoId, int politicaId, int categoriaId);

        TramitesResultado GuardarFocalizacionCategoriasAjustesSgp(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario);

        string GetCategoriasSubcategoriasSgp(int padreId, Nullable<int> entidadId, int esCategoria, int esGruposEtnicos);

        TramitesResultado GuardarCategoriasPoliticaTransversalesAjustesSgp(ParametrosGuardarDto<FocalizacionCategoriasAjusteDto> parametrosGuardar, string usuario);

        string ObtenerCrucePoliticasAjustesSgp(Guid instanciaId);

        string ObtenerPoliticasTransversalesResumenSgp(Guid instanciaId);

        TramitesResultado GuardarCrucePoliticasAjustesSgp(ParametrosGuardarDto<List<CrucePoliticasAjustesDto>> parametrosGuardar, string name);

        string ObtenerDesagregarRegionalizacionSgp(string bpin);

        string ObtenerFuenteFinanciacionVigenciaSgp(string bpin);
        string ObtenerFuentesProgramarSolicitadoSgp(string bpin);

        TramitesResultado EliminarFuentesFinanciacionProyectoSgp(int fuentesFinanciacionId);

        TramitesResultado GuardarFuentesProgramarSolicitadoSgp(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuario);

        string ObtenerDatosAdicionalesFuenteFinanciacionSgp(int fuenteId);

        TramitesResultado GuardarDatosAdicionalesSgp(ParametrosGuardarDto<DatosAdicionalesDto> parametrosGuardar, string usuario);

        TramitesResultado EliminarDatosAdicionalesSgp(int coFinanciacionId);

        TramitesResultado GuardarFuenteFinanciacionSgp(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar, string usuario);
        string ObtenerDatosIndicadoresPoliticaSgp(string Bpin);
        string ObtenerDatosCategoriaProductosPoliticaSgp(string Bpin, int fuenteId, int politicaId);
        string GuardarDatosSolicitudRecursosSgp(ParametrosGuardarDto<CategoriaProductoPoliticaDto> categoriaProductoPoliticaDto, string usuario);
    }
}
