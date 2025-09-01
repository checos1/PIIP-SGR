(function () {
    'use strict';
    angular.module('backbone').factory('documentoServicio', documentoServicio);

    documentoServicio.$inject = ['$http', 'constantesBackbone'];

    
    function documentoServicio($http, constantesBackbone) {
        return {
            consultarDocumento: consultarDocumento,
            crearDocumento: crearDocumento,
            ActualizarDocumento: ActualizarDocumento,
            EliminarDocumento: EliminarDocumento,
            EstadoDocumento: EstadoDocumento,
            ReferenciasDocumento: ReferenciasDocumento,

            ConsultarUsoDocumento: ConsultarUsoDocumento,
            EstadoUsoDocumento: EstadoUsoDocumento,
            EliminarUsoDocumento: EliminarUsoDocumento
        };

        function consultarDocumento(idDocumento) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDocumentoConsultar;
            url += "?NombreDocumento=" + idDocumento;
            return $http.get(url);
        }

        function crearDocumento(NombreDocumento, Codigo, Activo) {
            const url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDocumentoCrear;
            return $http.post(url, {
                                   NombreDocumento: NombreDocumento,
                                   Codigo: Codigo,
                                   Activo: Activo
                                   });
        }

        function ActualizarDocumento(Id, NombreDocumento, Codigo, Activo) {            
            const url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDocumentoActualizar;
            return $http.put(url, {
                Id: Id,
                NombreDocumento: NombreDocumento,
                Codigo: Codigo,
                Activo: Activo
            });
        }

        function EliminarDocumento(IdDocumento) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDocumentoEliminar;
            url += "?IdDocumento=" + IdDocumento;
            return $http.delete(url, { IdDocumento: IdDocumento });
        }

        function EstadoDocumento(item) {            
            const url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDocumentoEstado;
            return $http.post(url, {
                Id: item.Id,
                Activo: item.Activo
            });
        }


        function ReferenciasDocumento() {
            const url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDocumentoReferencias;
            return $http.get(url);
        }

        function ConsultarUsoDocumento(Operacion, Id, TipoTramiteId) {
            const url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDocumentoConsultarUso;
            return $http.get(url);
        }

        function EstadoUsoDocumento(item) {            
            const url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDocumentoUsoEstado;
            return $http.put(url, {
                Id: item.ID,
                Activo: item.Activo
            }); 
        }

        function EliminarUsoDocumento(Id) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDocumentoUsoEliminar;
            url += "?Id=" + Id;
            return $http.delete(url, { Id: Id });
        }        
    }
})();