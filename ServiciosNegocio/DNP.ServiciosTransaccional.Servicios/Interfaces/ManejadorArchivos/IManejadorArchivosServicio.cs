using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosTransaccional.Servicios.Dto;

namespace DNP.ServiciosTransaccional.Servicios.Interfaces.ManejadorArchivos
{
    public interface IManejadorArchivosServicio
    {
        Task<IList<ResponseArchivoViewModelDto>> CargarArchivos(ArchivoViewModelDto archivo, Stream file, string usuarioDNP);
        Task<bool> Clonar(Dictionary<string, object> parametros,  string usuarioDNP);
    }
}
