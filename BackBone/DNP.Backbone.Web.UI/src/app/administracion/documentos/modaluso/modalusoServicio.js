(function () {
    'use strict';
    angular.module('backbone').factory('modalusoServicio', modalusoServicio);

    modalusoServicio.$inject = ['$http', 'constantesBackbone'];


    function modalusoServicio($http, constantesBackbone) {
        return {
            //consultarDocumento: consultarDocumento,
            crearDocumento: crearUsoDocumento,
            actualizarDocumento: actualizarUsoDocumento
            //eliminar: eliminar
        };
        /**
        function consultarDocumento(idDocumento) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDocumentoConsultarUso;
            url += "?idDocumento=" + idDocumento;
            return $http.get(url);
        }*/

        function crearUsoDocumento(objdocumento) {           
            
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDocumentoCrearUso;
            return $http.post(url, objdocumento);
        }
        
        function actualizarUsoDocumento(objdocumento) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDocumentoActualizarUso;
            return $http.put(url, objdocumento);
        }
        /**
        function eliminar(idDocumento) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDocumentoElimninarUso;
            return $http.delete(url, idDocumento);
        } */
    }
})();