using DNP.Backbone.Dominio.Dto.ManejadorArchivos;
using DNP.Backbone.Dominio.Dto.Transversal;
using DNP.Backbone.Dominio.Dto.Transversales;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.ManejadorArchivos
{
    public interface IManejadorArchivosServicios
    {
        Task<ArchivoInfoViewModelDto> ObtenerArchivoInfo(string Id, string coleccion, string usuarioDNP);
        Task<IList<ArchivoInfoDto>> ObtenerListadoArchivos(string coleccion, Dictionary<string, object> parametros, string usuarioDNP);
        Task<bool> GuardarArchivoRepositorio(ArchivoInfoBsonDto parametros, string usuarioDNP);
        Task<bool> CambiarEstadoDataArchivo(string coleccion, string idArchivo, string status, string usuarioDNP);
        Task<byte[]> ObtenerArchivoBytes(string coleccion, string IdArchivoBlob, string usuarioDNP);
        Task<bool> EliminarArchivo(string coleccion, string idArchivo, string status, string usuarioDNP);
        Task<bool> ActualizarArchivo(string coleccion, string id, ArchivoInfoViewModelDto archivo, string usuarioDNP);
        Task<IList<ResponseArchivoViewModelDto>> CargarArchivos(ArchivoViewModelDto archivo, Stream file, string usuarioDNP);
        Task<string> GetAuthorizationApiArchivos(UsuariosArchivosLoginDto parametros, string usuarioDNP);
        Task<ArchivoDto> ObtenerArchivoDocumentos(List<DocumentosDto> datos, string usuarioDNP);
        Task<List<ArchivoInfoDto>> ValidarArchivoSgr(string coleccion, Dictionary<string, object> parametros, string usuarioDNP);
    }
}
