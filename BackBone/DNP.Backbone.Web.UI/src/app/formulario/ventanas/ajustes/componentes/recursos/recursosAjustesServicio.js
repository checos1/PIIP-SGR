(function () {
    'use strict';
    angular.module('backbone').factory('recursosAjustesServicio', recursosAjustesServicio);

    recursosAjustesServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function recursosAjustesServicio($q, $http, $location, constantesBackbone) {
        return {
            //obtenerFuentesFinanciacionProyecto: obtenerFuentesFinanciacionProyecto,
            obtenerFuentesFinanciacionVigenciaProyecto: obtenerFuentesFinanciacionVigenciaProyecto,
            eliminarFuentesFinanciacionProyecto: eliminarFuentesFinanciacionProyecto,
            obtenerFuentesProgramarSolicitado: obtenerFuentesProgramarSolicitado,
            guardarFuentesFinanciacionRecursosAjustes: guardarFuentesFinanciacionRecursosAjustes,

            obtenerDatosAdicionales: obtenerDatosAdicionales,
            obtenerTipoCofinanciador: obtenerTipoCofinanciador,
            obtenerFondos: obtenerFondos,
            obtenerRubros: obtenerRubros,
            obtenerProyectos: obtenerProyectos,
            agregarDatosAdicionales: agregarDatosAdicionales,
            eliminarDatosAdicionales: eliminarDatosAdicionales,

            obtenerListaEntidades: obtenerListaEntidades,
            obtenerListaEtapas: obtenerListaEtapas,
            obtenerListaTipoEntidad: obtenerListaTipoEntidad,
            obtenerSectores: obtenerSectores,
            agregarFuentesFinanciacionProyecto: agregarFuentesFinanciacionProyecto,
            obtenerListaRecursos: obtenerListaRecursos,
            obtenerRegionalizacionGeneral: obtenerRegionalizacionGeneral,
            guardarRegionalizacionFuentes: guardarRegionalizacionFuentes,

        };

        //function obtenerFuentesFinanciacionProyecto(Bpin, usuarioDNP, idFormulario) {
        //    var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuenteFinanciacionConsultar + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
        //    return $http.get(url);
        //}

        function obtenerFuentesFinanciacionVigenciaProyecto(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuenteFinanciacionVigenciaConsultar + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function eliminarFuentesFinanciacionProyecto(fuenteId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuenteFinanciacionEliminar + "?fuenteId=" + fuenteId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url);
        }

        function obtenerFuentesProgramarSolicitado(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuentesProgramarSolicitado + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        /*TODO*/
        function guardarFuentesFinanciacionRecursosAjustes(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneguardarFuentesFinanciacionRecursosAjustes + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        /*Modal adicionales*/
        function agregarDatosAdicionales(parametros, usuarioDNP, idFormulario, idInstancia, idAccion, idAplicacion) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDatosAdicionalesAgregar + "?usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url, parametros);
        }

        function obtenerDatosAdicionales(fuenteId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDatosAdicionalesConsultar + "?fuenteId=" + fuenteId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function obtenerTipoCofinanciador(peticion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaTipoCofinanciador;
            return $http.post(url, peticion);
        }

        function obtenerFondos(peticion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaFondos;
            return $http.post(url, peticion);
        }

        function obtenerRubros(peticion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaRubros;
            return $http.post(url, peticion);
        }

        function obtenerProyectos(peticion, bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosBpin + "?bpin=" + bpin;
            return $http.post(url, peticion);
        }

        function eliminarDatosAdicionales(cofinanciadorId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneDatosAdicionalesEliminar + "?cofinanciadorId=" + cofinanciadorId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url);
        }

        /*Modal Agregar Fuente*/

        function obtenerListaEntidades(peticionobtenerProyectos, idTipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEntidadesTotal;
            return $http.post(url, peticionobtenerProyectos);
        }

        function obtenerListaEtapas(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaEtapas;
            return $http.post(url, parametros);
        }

        function obtenerListaTipoEntidad(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoTipoEntidad;
            return $http.post(url, parametros);
        }

        function obtenerSectores(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaSectoresEntity;
            return $http.post(url, parametros);
        }

        function agregarFuentesFinanciacionProyecto(parametros, usuarioDNP, idFormulario, idInstancia, idAccion, idAplicacion) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFuenteFinanciacionAgregar + "?usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url, parametros);
        }

        function obtenerListaRecursos(peticion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaTiposRecursos;
            return $http.post(url, peticion);
        }

        function obtenerRegionalizacionGeneral(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneRegionalizacionGeneral + "?bpin=" + bpin ;
            return $http.get(url);
        }

        function guardarRegionalizacionFuentes(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneguardarRegionalizacionFuentesFinanciacionAjustes + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

    }
})();