(function () {
    'use strict';
    angular.module('backbone').factory('aclaracionLeyendaServicio', aclaracionLeyendaServicio);

    aclaracionLeyendaServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone', 'servicioFichasProyectos', 'utilidades', 'FileSaver'];


    function aclaracionLeyendaServicio($q, $http, $location, constantesBackbone, servicioFichasProyectos, utilidades, FileSaver) {
        return {
            obtenerProyectos: obtenerProyectos,
            ObtenerProyectoAsociacion: ObtenerProyectoAsociacion,
            asociarProyecto: asociarProyecto,
            obtenerDatosProyectoTramite: obtenerDatosProyectoTramite,
            obtenerErroresTramite: obtenerErroresTramite,
            eliminarAsociacion: eliminarAsociacion,
            obtenerModificacionLeyenda: obtenerModificacionLeyenda,
            actualizaModificacionLeyenda: actualizaModificacionLeyenda,
        };

        function obtenerProyectos(bpin) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosBpin + "?bpin=" + bpin;
            return $http.post(url);
        }

        function ObtenerProyectoAsociacion(bpin, tramiteid) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoAsociacionVFO;
            url += "?bpin=" + bpin;
            url += "&tramiteid=" + tramiteid;
            url += "&tipoTramite=AL";
            return $http.get(url);
        }

        function asociarProyecto(asociarProyectoDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneAsociarProyectoVFOPLN;
            return $http.post(url, asociarProyectoDto);
        }

        function obtenerDatosProyectoTramite(tramiteId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDatosProyectoTramite;
            url += "?tramiteId=" + tramiteId;
            return $http.get(url);
        }

        function obtenerErroresTramite(guiMacroproceso, idInstancia, accionid) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerErroresTramite;
            url += "?guiMacroproceso=" + guiMacroproceso;
            url += "&idInstancia=" + idInstancia;
            url += "&accionid=" + accionid;
            return $http.get(url);
        }

        function eliminarAsociacion(EliminarAsociacionDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarAsociacionVFOPLN;
            return $http.post(url, EliminarAsociacionDto);
        }

        function obtenerModificacionLeyenda(tramiteId, proyectoId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerModificacionLeyenda + "?tramiteId=" + tramiteId + "&proyectoId=" + proyectoId;
            return $http.get(url).then((respuesta)=> respuesta.data);
        }

        function actualizaModificacionLeyenda(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneActualizaModificacionLeyenda;
            return $http.post(url, parametros);
        }
    }
})();