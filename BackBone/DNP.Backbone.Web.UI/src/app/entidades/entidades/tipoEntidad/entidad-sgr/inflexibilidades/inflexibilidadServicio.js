(function () {

    'use strict';

    angular.module('backbone.entidades').factory('inflexibilidadServicio', inflexibilidadServicio);
    inflexibilidadServicio.$inject = ['$http', 'constantesBackbone', 'FileSaver', 'Blob', 'constantesArchivos','utilidades'];

    function inflexibilidadServicio($http, constantesBackbone, FileSaver, Blob, constantesArchivos, utilidades) {
        return {
            
            obtenerInflexibilidadPorEntidadId: obtenerInflexibilidadPorEntidadId,
            guardarInflexibilidad: guardarInflexibilidad,
            guardarInflexibilidadPagos: guardarInflexibilidadPagos,
            eliminarInflexibilidad: eliminarInflexibilidad,
            obtenerInflexibilidadPagos: obtenerInflexibilidadPagos,
            obtenerExcel: obtenerExcel,
            obtenerPdf: obtenerPdf,
            actualizarIdArchivo,
        };

        function obtenerInflexibilidadPorEntidadId(idEntidad, filtro) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerInflexibilidadPorEntidadId + "?idEntidad=" + idEntidad;
            return $http.post(url, filtro);
        }

        function guardarInflexibilidad(inflexibilidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarInflexibilidad;
            return $http.post(url, inflexibilidad);
        }   

        function guardarInflexibilidadPagos(pagos) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarInflexibilidadPagos;
            return $http.post(url, pagos);
        }   

        function eliminarInflexibilidad(id) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarInflexibilidad + id;
            return $http.post(url);
        }
        
        function obtenerInflexibilidadPagos(idInflexibilidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerInflexibilidadPagos + "?idInflexibilidad=" + idInflexibilidad;
            return $http.get(url);
        }

        function obtenerExcel(obj) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcel;
            return $http.post(url, obj, { responseType: 'arraybuffer' });
        }

        function obtenerPdf(obj) {
            var url = urlPDFBackbone + constantesBackbone.apiBackboneInflexibilidadImprimirPDF;
            return $http.post(url, obj, { responseType: 'blob' });
        }

        function actualizarIdArchivo(pago) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarIdArchivoInflexibilidadPago;
            return $http.post(url, pago);
        }
    }
})();