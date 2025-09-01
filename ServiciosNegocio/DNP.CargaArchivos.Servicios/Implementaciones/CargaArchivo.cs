namespace DNP.CargaArchivos.Servicios.Implementaciones
{
    using System;
    using System.Collections.Generic;
    using Dominio.Dto.CargaArchivo;
    using Interfaces.CargaArchivo;

    public class CargaArchivo : ICargaArchivo
    {
        public FormularioCargaArchivoDto ConsultarCargaArchivo()
        {
            var formularioCargaArchivo = new FormularioCargaArchivoDto()
            {
                Nombre = "Mock Formulario Carga Archivo",
                Descripcion = "Mock Formulario Carga Archivo prueba PBI 5224",
                Version = 1,
                TipoOperaciones = MockListaOperaciones(),
                FechaCreacion = DateTime.Now,
                Activo = true

            };

            return formularioCargaArchivo;
        }

        private List<TipoOperacionesDto> MockListaOperaciones()
        {
            var lista = new List<TipoOperacionesDto>();
            const string nombre = "Mock Tipo Operaciones";

            for (var i = 1; i <= 15; i++)
            {
                lista.Add(new TipoOperacionesDto()
                {
                    Id = Guid.NewGuid(),
                    Nombre = $"{nombre} {i}"
                });
            }

            return lista;
        }

    }
}

