namespace DNP.EncabezadoPie.Servicios.Implementaciones
{
    using System;
    using System.Collections.Generic;
    using Interfaces.EncabezadoPieBasico;
    using Dominio.Dto;
    using Persistencia.Interfaces.EncabezadoPie;
    using System.Threading.Tasks;

    public class EncabezadoPieServicio : IEncabezadPieoBasicoServicio
    {
        private readonly IEncabezadoPiePersistencia _encabezadoPiePersistencia;

        public EncabezadoPieServicio(IEncabezadoPiePersistencia encabezadoPiePersistencia)
        {
            _encabezadoPiePersistencia = encabezadoPiePersistencia;
        }



        public EncabezadoPieBasicoDto ConsultarEncabezadoPieBasico(ParametrosEncabezadoPieDto parametros)
        {
           var listaEncabezado  = _encabezadoPiePersistencia.ConsultarEncabezadoPieBasico(parametros);

            return listaEncabezado;
        }

        public EncabezadoPieBasicoDto ConsultarEncabezadoPieBasicoPreview()
        {
            return _encabezadoPiePersistencia.ConsultarEncabezadoPieBasicoPreview();
        }

        public EncabezadoGeneralDto ObtenerEncabezadoGeneral(ParametrosEncabezadoGeneral parametros)
        {
            return _encabezadoPiePersistencia.ObtenerEncabezadoGeneral(parametros);            
        }
    }
}

