(function () {
    'use strict';

    angular.module('backbone').factory('servicioResumenDeProyectos', servicioResumenDeProyectos);

    servicioResumenDeProyectos.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    function servicioResumenDeProyectos($q, $http, $location, constantesBackbone) {

        return {
            obtenerProyectosPorTramite: obtenerProyectosPorTramite,
            generarInstancias: generarInstancias,
            eliminarProyectoTramite: eliminarProyectoTramite
        }

        function obtenerProyectosPorTramite(peticion, instanciaId, filtro) {

            var listaFiltrados = [];
            Object.keys(filtro).forEach(f => {
                if (filtro[`${f}`].valor) {
                    listaFiltrados.push(filtro[`${f}`]);
                }
            });

            const dto = {
                parametrosInboxDto: peticion,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticion.IdUsuario,
                    tramiteId: instanciaId,
                    filtroGradeDtos: listaFiltrados,
                    InstanciaId: instanciaId
                },
                columnasVisibles: []
            };
           
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosPorTramite, dto);
        }

        function generarInstancias(peticion, proyectos) {
            const dto = {
                IdUsuarioDNP: peticion.IdUsuario,
                UsuarioId: peticion.IdUsuario,
                Proyectos: proyectos,
                RolId: peticion.ListaIdsRoles[0]
            };
            
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGenerarInstancias, dto);
        }

        function eliminarProyectoTramite(peticion, proyectoId) {
            const dto = {
                TramiteFiltroDto: { ProyectoId: proyectoId, IdUsuarioDNP: peticion.IdUsuario }
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarProyectoTramite, dto);
        }

        
    }
})();