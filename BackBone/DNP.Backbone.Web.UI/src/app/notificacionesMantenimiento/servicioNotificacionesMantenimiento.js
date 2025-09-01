(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('servicioNotificacionesMantenimiento', servicioNotificacionesMantenimiento);

    servicioNotificacionesMantenimiento.$inject = ['$http', 'constantesBackbone', '$q', '$uibModal'];

    function servicioNotificacionesMantenimiento($http, constantesBackbone, $q, $uibModal) {

        return {
            obtenerListaNotificaciones: obtenerListaNotificaciones,
            obtenerListaProcedimientosAlmacenados: obtenerListaProcedimientosAlmacenados,
            obtenerUsuariosPorProcedimientoId: obtenerUsuariosPorProcedimientoId,
            guardarConfigNotificacion: guardarConfigNotificacion,
            eliminarNotificacion: eliminarNotificacion,
            obtenerPDF: obtenerPDF,
            visualizarContenidoNotificacion: visualizarContenidoNotificacion
        }

        function obtenerListaProcedimientosAlmacenados() {
            const url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerProcedimentosAlmacenados}`;
            return $http.get(url);
        }

        function obtenerListaNotificaciones(filtro) {
            const url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerListaNotificaciones}`;
            return $http.post(url, filtro);
        }
        
        function obtenerUsuariosPorProcedimientoId(id) {
            const url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerUsuariosPorProcedimentosId}/${id}`;
            return $http.get(url);
        }

        function guardarConfigNotificacion(model) {
            const url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneGuardarConfigNotificaciones}`;
            return $http.post(url, model);
        }

        function eliminarNotificacion(id) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarNotificacion + '?ids=' + id;
            return $http.delete(url, id);
        }

        /**
         * 
         * @description. Obtiene un archivo PDF desde la vista html creada con los datos proporcionados
         * @param {Array} data. Información en forma de Lista de objetos. Ver clase Dominio.Dto.UsuarioNotificacion.UsuarioNotificacionConfigDto 
         */
        function obtenerPDF(datos){
            try {
                let url = `${urlPDFBackbone}${constantesBackbone.apiBackboneObtenerPdfMensajeMantenimiento}`;
                return $http.post(url, {datos : datos });
            }
            catch(excepcion){
                throw { message: `servicioNotificacionesMantenimiento.obtenerPDF => ${excepcion.message}`}
            }
        }

        function visualizarContenidoNotificacion(model) {
            return $uibModal.open({
                templateUrl: 'src/app/notificacionesMantenimiento/modales/visualizarNotificacion/visualizarNotificacion.html',
                controller: 'visualizarNotificacionController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "modal__notificacion__exhibicion",
                resolve: {
                    params: {
                        Notificacion: model
                    }
                }
            });
        }
    }
})();