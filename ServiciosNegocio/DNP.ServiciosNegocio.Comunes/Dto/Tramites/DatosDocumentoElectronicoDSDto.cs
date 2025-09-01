using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Comunes.Dto.Tramites
{
    [ExcludeFromCodeCoverage]
    public class DatosDocumentoElectronicoDSDto
    {
        public DatosDocumentoElectronicoDto datosDocumentoElectronicoDto { get; set; }
        public DatosRadicadoDto datosRadicadoDto { get; set; }

        public UsuarioRadicacionDto usuarioRadica { get; set; }

        public DatosDocumentoElectronicoDSDto() { }

        public static DatosDocumentoElectronicoDSDto GenerarAnexoRadicadoEntrada(string radicadoId, string analistaDestino, string anexoBase64)
        {
            return new DatosDocumentoElectronicoDSDto
            {
                datosDocumentoElectronicoDto = new DatosDocumentoElectronicoDto
                {
                    fileBase64Bin = anexoBase64,
                    extension = "pdf",
                    nombre = Guid.NewGuid().ToString()
                },
                datosRadicadoDto = new DatosRadicadoDto
                {
                    esPrincipal = true,
                    NoRadicado = Convert.ToDecimal(radicadoId),
                    observacion = "Anexo de radicado de entrada"
                },
                usuarioRadica = new UsuarioRadicacionDto
                {
                    Login = analistaDestino
                }
            };
        }
    }
}
