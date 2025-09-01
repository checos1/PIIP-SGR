(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone.entidades').factory('servicioEntidades', servicioEntidades);

    servicioEntidades.$inject = ['$http', 'constantesBackbone', '$location', '$timeout', '$q'];

    function servicioEntidades($http, constantesBackbone, $location, $timeout, $q) {

        return {
            obtenerJsonLocal: obtenerJsonLocal,
            obtenerEntidadesPorTipo: obtenerEntidadesPorTipo,
            obtenerSubEntidadesPorIdEntidad: obtenerSubEntidadesPorIdEntidad,
            eliminarEntidad: eliminarEntidad,
            obtenerExcelEntidades: obtenerExcelEntidades,
            obtenerPdfEntidades: obtenerPdfEntidades,
            guardarEntidad: guardarEntidad,
            obtenerSectores: obtenerSectores,
            obtenerDepartamentos: obtenerDepartamentos,
            obtenerEntidadPorEntidadId: obtenerEntidadPorEntidadId,
            obtenerCRType: obtenerCRType,
            obtenerFase: obtenerFase,
            obtenerMatrizFlujo: obtenerMatrizFlujo,
            guardarFlujo: guardarFlujo,
            obtenerEntidadesPorUnidadesResponsables: obtenerEntidadesPorUnidadesResponsables,
            ActualizarUnidadResponsable: ActualizarUnidadResponsable,
            obtenerListadoEntidadesXUsuarioAutenticado: obtenerListadoEntidadesXUsuarioAutenticado,
            obtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado: obtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado,
            obtenerListadoPerfilesXEntidadBanco: obtenerListadoPerfilesXEntidadBanco,
            obtenerListadoPerfilesXEntidad: obtenerListadoPerfilesXEntidad,
            obtenerSectoresParaEntidades: obtenerSectoresParaEntidades,
            obtenerExisteEntidadDestinoInstancia: obtenerExisteEntidadDestinoInstancia,
            obtenerMatrizEntidadDestino: obtenerMatrizEntidadDestino,
            actualizarMatrizEntidadDestino: actualizarMatrizEntidadDestino,
            validarFlujoConInstanciaActiva: validarFlujoConInstanciaActiva,
            obtenerUsuariosPorNombreIdentificacion: obtenerUsuariosPorNombreIdentificacion,
            obtenerSectoresPorUsuarioEntidad: obtenerSectoresPorUsuarioEntidad,
            guardarSectoresPorUsuarioEntidad: guardarSectoresPorUsuarioEntidad,
            obtenerSectoresSgp: obtenerSectoresSgp,
            obtenerFlowCatalogSgp: obtenerFlowCatalogSgp,
            registrarObservador: registrarObservador,
            notificarCambio: notificarCambio,
            limpiarObservadores: limpiarObservadores
        }

        var observadores = [];

        function registrarObservador(callback) {
            if (!observadores) {
                observadores = [];
            }
            observadores.push(callback);
        }

        function notificarCambio(datos) {
            observadores.forEach(function (observador) {
                observador(datos);
            });
        }

        function limpiarObservadores() {
            observadores = [];
        }

        function obtenerListadoEntidadesXUsuarioAutenticado() {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerListadoEntidadesXUsuarioAutenticado}`;
            return $http.get(url);
        }

        function obtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado(tipoEntidad) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerListadoEntidadesXTipoEntidadYUsuarioAutenticado}?tipoEntidad=${tipoEntidad}`;
            return $http.get(url);
        }

        function obtenerListadoPerfilesXEntidadBanco(idEntidad, resourceGroupId) {

            var deferred = $q.defer();

            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerListadoPerfilesXEntidadBanco}?idEntidad=${idEntidad}&resourceGroupId=${resourceGroupId}`;
            deferred.resolve($http.get(url));

            return deferred.promise;
        }

        function obtenerListadoPerfilesXEntidad(idEntidad) {

            var deferred = $q.defer();

            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerListadoPerfilesXEntidad}?idEntidad=${idEntidad}`;
            deferred.resolve($http.get(url));

            return deferred.promise;
        }

        function obtenerEntidadesPorTipo(tipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEntidadPorTipoEntidad + tipoEntidad;
            return $http.get(url);
        }

        function obtenerEntidadesPorUnidadesResponsables() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEntidadesPorUnidadesResponsables;
            return $http.get(url);
        }

        function obtenerSectoresSgp() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerSectoresSGP;
            return $http.get(url);
        }     

        function obtenerFlowCatalogSgp() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerFlowCatalogSGP;
            return $http.get(url);
        }     

        function obtenerMatrizEntidadDestino(listMatrizEntidadDestinoDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerMatrizEntidadDestinoSGP;
            const dto = {
                ListMatrizEntidad: listMatrizEntidadDestinoDto
            };
            return $http.post(url, dto);
        }

        function actualizarMatrizEntidadDestino(dto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarMatrizEntidadDestinoSGP;
            
            return $http.post(url, dto);
        }

        function validarFlujoConInstanciaActiva(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneValidarFlujoConInstanciaActiva;
            return $http.post(url, parametros);

        }

        function obtenerSectoresParaEntidades() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerSectoresParaEntidades;
            return $http.get(url);
        }

        function obtenerSectores() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerSectores;
            return $http.get(url);
        }     

        function obtenerDepartamentos() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDepartamentos;
            return $http.get(url);
        }     

        function obtenerCRType() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerCRType;
            return $http.get(url);
        }     

        function obtenerFase() {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerFase;
            return $http.get(url);
        }     
        function obtenerMatrizFlujo(entityTypeCatalogOptionId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerMatrizFlujo + "?entidadResponsableId=" + entityTypeCatalogOptionId;;
            return $http.get(url);
        }     

        function obtenerEntidadPorEntidadId(idEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEntidadPorEntidadId + "?idEntidad=" + idEntidad;
            return $http.get(url, { idEntidad: idEntidad});
        }     
        
        function obtenerSubEntidadesPorIdEntidad(idEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerSubEntidadesPorEntidadId + idEntidad;
            return $http.get(url);
        }  

        function eliminarEntidad(idEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarEntidad + idEntidad;
            return $http.post(url);
        }

        function guardarEntidad(objEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarEntidad;
            return $http.post(url, objEntidad);
        }

        function ActualizarUnidadResponsable(objEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarUnidadResponsable;
            return $http.post(url, objEntidad);
        }

        function obtenerExisteEntidadDestinoInstancia(entidadId) {
            var url = apiFormularioServicioBaseUri + "api/Flujo/ExisteEntidadDestinoInstancia/" + entidadId;
            return $http.get(url);
        }

        function guardarFlujo(flujos){
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarFlujo;
            return $http.post(url, flujos);
        }        

        function obtenerExcelEntidades(entidadesFiltro) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerExcelEntidades;
            return $http.post(url, entidadesFiltro, { responseType: 'arraybuffer' });
        }

        function obtenerPdfEntidades(entidadesFiltro) {
            var url = urlPDFBackbone + constantesBackbone.apiBackboneObtenerPDFEntidades;
            return $http.post(url, entidadesFiltro, { responseType: 'blob' });
        }

        //--------p/ mock------------//
        function obtenerJsonLocal(nombreJson) {
            var url = 'http://localhost:3024/src/assets/' + nombreJson + '.json';

            return $http({
                method: 'GET',
                'Content-Type': 'application/json;charset=utf-8',
                url: url
            });
        }

        function obtenerUsuariosPorNombreIdentificacion(filtro) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerUsuariosPorNombreIdentificacion;
            return $http.post(url, filtro);
        }

        function obtenerSectoresPorUsuarioEntidad(filtro) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerSectoresPorUsuarioEntidad;
            return $http.post(url, filtro);
        }

        function guardarSectoresPorUsuarioEntidad(data) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarSectoresPorUsuarioEntidad;
            return $http.post(url, data);
        }
    }
})();
