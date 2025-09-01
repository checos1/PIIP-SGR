(function () {
    'use strict';

    angular.module('backbone').factory('serviciosComponenteNotificaciones', serviciosComponenteNotificaciones);

    serviciosComponenteNotificaciones.$inject = [
        '$http', 
        'constantesBackbone',
        'autorizacionServicios',
        'servicioConsolaMonitoreo',
        'array.extensions'
    ];


    function serviciosComponenteNotificaciones(
        $http,
        constantesBackbone,
        autorizacionServicios,
        servicioConsolaMonitoreo) {

        return {
            obtenerNotificacionesSinLeer: obtenerNotificacionesSinLeer,
            obtenerNotificacionesCantidadAlertas: obtenerNotificacionesCantidadAlertas,
            NotificarUsuarios: NotificarUsuarios,
        }

        function obtenerNotificacionesSinLeer() {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerNotificacionesPorResponsable);
        }

        async function obtenerNotificacionesCantidadAlertas() {
            // Global variables
            const _usuarioDNP = usuarioDNP;
            const _idTipoProyecto = idTipoProyecto;
            const _nombreAplicacionBackbone = nombreAplicacionBackbone;
            
            var peticion = {
                IdUsuario: _usuarioDNP,
                IdObjeto: _idTipoProyecto,
                Aplicacion: _nombreAplicacionBackbone,
                ListaIdsRoles: await _obtenerUsuarioIdsRoles(_usuarioDNP)
            };

            const filtroProyecto = {
                ProyectoParametrosDto: peticion,
                ProyectoFiltroDto: { bpin: null },
                columnasVisibles: []
            };
        
            const url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerConsolaMonitoreo}?noblockui`;
            const respuesta = await $http.post(url, filtroProyecto);

            if (respuesta.data.GruposEntidades && !respuesta.data.GruposEntidades.length)
                return null;

            const listaGrupoEntidades = respuesta.data.GruposEntidades;
            let listaProyectosIds = listaGrupoEntidades.reduce((lista, grupoEntidade) => {
                grupoEntidade.ListaEntidades.forEach(entidad => {
                    entidad.ObjetosNegocio.forEach(negocio => {
                        if(lista.find(x => x.id == negocio.ProyectoId))
                            return;

                        lista.push({ id: negocio.ProyectoId });
                    });
                });

                return lista
            }, []);

            if(!listaProyectosIds.length)
                return null;
            
            return servicioConsolaMonitoreo.obtenerCondicionesParaAccionAlertas(peticion, listaProyectosIds.map(x => x.id))
                .then(response => {
                    if(!response.data)
                        return null;

                    listaProyectosIds = listaProyectosIds.distinctBy(x => x.id, (value) => ({ 
                        id: value, 
                        TieneAlerta: false 
                    }));

                    return Object.keys(response.data)
                        .reduce((lista, proyectoId) => {
                            const proyecto = listaProyectosIds.find(x => x.id == proyectoId)
                            if(!proyecto)
                                return lista;
                            
                            proyecto.TieneAlerta = response.data[proyectoId];
                            lista.push(proyecto);
                            return lista;
                        }, [])
                });
        }

        async function _obtenerUsuarioIdsRoles(usuario) {
            return new Promise((resolve, reject) => {
                return autorizacionServicios.obtenerRolesPorUsuario(usuario)
                    .then(roles => {
                        const ids = (roles || []).map(x => x.IdRol);
                        resolve(ids);   
                    })
                    .catch(error => reject(error));
            });
        }

        function NotificarUsuarios(notificacion, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneNotificarUsuariosRValidadorPoliticaTransversal + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, notificacion);
        }

    }
})();
