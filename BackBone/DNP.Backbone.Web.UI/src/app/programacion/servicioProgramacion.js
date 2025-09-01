(function () {
    'use strict';

    angular.module('backbone').factory('servicioProgramacion', servicioProgramacion);
    servicioProgramacion.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    // descripción de la columnas
    var columnasPorDefectoProyecto = ['CAPITULO', 'FECHA DESDE', 'FECHA HASTA'];

    function servicioProgramacion($q, $http, $location, constantesBackbone) {

        return {
            columnasPorDefectoProyecto: columnasPorDefectoProyecto,
            obtenerTramites: obtenerTramites,
            eliminarProgramacion: eliminarProgramacion,
            crearPeriodo,
            iniciarProceso,
            //guardarConfiguracion: guardarConfiguracion,
            ObtenerTipoEstados: ObtenerTipoEstados,
        }

        function obtenerTramites(tipoEntidad, filtros) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneProgramacionObtenerPorTipoEntidad}`;
            return $http.get(url, { params: { filtros: filtros } });
        }

        /**
         * @description Obtiene los tipos de estados disponibles del proceso actual de cada programación
         */
        function ObtenerTipoEstados(){
            try {
                let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneProgramacionObtenerEstadoProcesos}`;
                return $http.get(url);
            }
            catch(exception){
                throw {message: `servicioProgramacion.ObtenerTipoEstados: ${exception.message}`};
            }
        }

        function eliminarProgramacion(programacion) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneProgramacionEliminar}`;
            return $http.post(url, programacion);
        }

        function crearPeriodo(tipoEntidad) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneProgramacionCrearPeriodo}?tipoEntidad=${tipoEntidad}`;
            return $http.post(url, tipoEntidad);
        }

        function iniciarProceso(tipoEntidad) {
            let url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneProgramacionIniciarProceso}?tipoEntidad=${tipoEntidad}`;
            return $http.post(url, tipoEntidad);
        }

        function guardarConfiguracion(item) {
            let url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarConfiguracionMensaje;
            return $http.post(url, item);
        }
    }
})();