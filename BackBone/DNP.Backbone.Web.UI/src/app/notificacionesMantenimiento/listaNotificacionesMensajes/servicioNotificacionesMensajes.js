(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('servicioNotificacionesMensajes', servicioNotificacionesMensajes);

    servicioNotificacionesMensajes.$inject = ['$http', 'constantesBackbone', '$q'];

    function servicioNotificacionesMensajes($http, constantesBackbone, $q) {

        return {            
            marcarNotificacionComoLeida: marcarNotificacionComoLeida,
            obtenerListaMensajesNotificaciones: obtenerListaMensajesNotificaciones,
            obtenerPdfNotificacionesMensajes: obtenerPdfNotificacionesMensajes,
            ObtenerExcel: ObtenerExcel
        }

        function obtenerListaMensajesNotificaciones(filtro) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerMensajeNotificacion}`;
            return $http.post(url, filtro);
        }

        function marcarNotificacionComoLeida(model) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneMarcarNotificacionComoLeida}`;
            return $http.post(url, model);
        }

        /**
         * 
         * @description. Obtiene el archivo binario Excel de los datos proporcionados.
         * @param {Array} datos. Arreglo de datos con la estructura UsuarioNotificacionDto 
         */
        function ObtenerExcel(datos) {
            try {
                var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerExcelListaNotificacion}`;
                return $http.post(url, datos);
            }
            catch(exception){
                throw { message: `servicioNotificaciones.ObtenerExcel => ${exception.message}` };
            }
        }

        function obtenerPdfNotificacionesMensajes(filtros) {
            const dto = [];
            filtros.forEach(element => {
                dto.push({
                    Fecha: element.fecha,
                    Notificacion: element.notificacion,
                    NombreNotificacion: element.nombreNotificacion,
                    Estado: element.estado
                });
            });

            var url = urlPDFBackbone + constantesBackbone.apiBackbonePDFNotificacionesMensajes;
            return $http.post(url, dto, {
                responseType: 'blob'
            });
        }
    }
})();