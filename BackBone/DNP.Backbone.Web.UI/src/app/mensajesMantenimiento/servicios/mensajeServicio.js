(function () {
    'use strict';

    angular.module('backbone').factory('mensajeServicio', mensajeServicio);
    mensajeServicio.$inject = ['$http', 'constantesBackbone', '$uibModal', 'sesionServicios', '$rootScope',];

    function mensajeServicio($http, constantesBackbone, $uibModal, sesionServicios, $rootScope) {
        return {
            obtenerMensajePorId: function (peticion, idMensaje) {
                const parametros = {
                    ParametrosDto: peticion,
                    FiltroDto: {
                        Ids: [idMensaje]
                    }
                };

                return $http.post(`${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerListaMensajes}?noblockui`, parametros);
            },

            obtenerListaMensaje: function (peticion, filtro) {
                const mensajesConfigFiltroDto = {
                    ParametrosDto: peticion, 
                    MensajeMantenimientoDto: {}, 
                    FiltroDto: Object.assign(filtro, { 
                        TiposEntidades: filtro.TipoEntidad && [filtro.TipoEntidad],
                        EstadosMensajes: filtro.EstadoMensaje && [filtro.EstadoMensaje],
                        TiposMensajes: filtro.TipoMensaje && [filtro.TipoMensaje],
                        TieneRestringeAcesso: filtro.TieneRestringeAcesso || null  
                    })
                };
                
                return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerListaMensajes + "?noblockui", mensajesConfigFiltroDto);
            },

            obtenerListaTiposEntidades: function(tipoEntidad) {
                var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerEntidadPorTipoEntidad}${tipoEntidad}`;
                return $http.get(url);
            },

            guardarMensaje: function (peticion, mensaje) {
                const parametros = {
                    ParametrosDto: peticion,
                    MensajeMantenimientoDto: mensaje
                };

                return $http.post(`${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneCrearActualizarMensajeMantenimiento}`, parametros);
            },

            eliminar: function (peticion, ...ids) {
                const requisicion = {
                    ParametrosDto: peticion,
                    MensajeMantenimientoDto: {},
                    FiltroDto: {
                        Ids: ids
                    }
                };
                
                return $http({
                    method: 'DELETE',
                    url: `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneEliminarMensajeMantenimiento}`,
                    data: requisicion,
                    headers: {
                        'Content-type': 'application/json;charset=utf-8'
                    }
                });
            },

            visualizarModalMensaje: function(mensaje, preVisualizacion) {
                return $uibModal.open({
                    templateUrl: "src/app/mensajesMantenimiento/template/modales/visualizarMensajePopUp/visualizarMensajePopUpTemplate.html",
                    controller: "visualizarMensajePopUpController",
                    controllerAs: "vm",
                    size: 'lg',
                    openedClass: 'dialog-modal-mensaje',
                    resolve: {
                        params: {
                            template: mensaje.MensajeTemplate,
                            type: mensaje.EstiloTipoMensaje,
                            restringeAcesso: mensaje.RestringeAcesso || false,
                            preVisualizacion: preVisualizacion || false
                        }
                    }
                });
            },

            preVisualizarModalMensaje: function(mensage) {
                return this.visualizarModalMensaje(mensage, true);
            },

            obtenerUsuarioMensajesMantenimiento: function() {
                const idRoles = sesionServicios.obtenerUsuarioIdsRoles();
                const peticion = {
                    IdUsuarioDNP: usuarioDNP,
                    Aplicacion: nombreAplicacionBackbone,
                    IdsRoles: idRoles
                };

                const filtro = {
                    EntidadesIds: sesionServicios.obtenerUsuarioIdsEntidades(),
                    RolesIds: idRoles,
                    FechaComprobacion: new Date(),
                    ComprobarMensajes: true,
                    EstadoMensaje: 'Habilitado'
                };

                return this.obtenerListaMensaje(peticion, filtro).then(respuesta => {
                    $rootScope.$broadcast('MensajesMantenimientoConfirmada', respuesta.data);
                })  
            }
            
        };
    }
})();