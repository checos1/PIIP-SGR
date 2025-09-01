using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DNP.ServiciosNegocio.Dominio.Dto.Proyectos
{
    [ExcludeFromCodeCoverage]
    public class ObjectivosAjusteJustificacionDto
    {
        public int Proyectoid { get; set; }
        public String BPIN { get; set; }
        public String Justificacion { get; set; }
        public List<ObjetivoJustificacionDto> Objetivos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class VigenciaAjusteJustificacionDto
    {
        public int Vigencia { get; set; }
        public String CostoAjuste { get; set; }
        public String CostoFirme { get; set; }
        public String Diferencia { get; set; }
        public String CostoAjusteSinFormato { get; set; }
        public String CostoFirmeSinFormato { get; set; }
        public String DiferenciaSinFormato { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ProductoAjusteJustificacionDto
    {
        public int ProductoId { get; set; }
        public string Producto { get; set; }
        public List<EtapaDto> Etapas { get; set; }
        public List<DetalleAjusteDto> DetalleAjuste { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class EtapaDto
    {
        public string Etapa { get; set; }
        public List<VigenciaAjusteJustificacionDto> Vigencias { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ObjetivoJustificacionDto
    {
        public int ObjetivoEspecificoId { get; set; }
        public string ObjetivoEspecifico { get; set; }
        public List<ProductoAjusteJustificacionDto> Productos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class DetalleAjusteDto
    {
        public int ActividadId { get; set; }
        public string Actividad { get; set; }
        public String TipoActividad { get; set; }
        public int Entregable { get; set; }
    }
}
