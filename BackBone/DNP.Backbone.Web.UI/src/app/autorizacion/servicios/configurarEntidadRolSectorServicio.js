(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('configurarEntidadRolSectorServicio', configurarEntidadRolSectorServicio);

    configurarEntidadRolSectorServicio.$inject = ['$q','$http', 'constantesAutorizacion', 'utilidades'];

    function configurarEntidadRolSectorServicio($q, $http, constantesAutorizacion, utilidades) {

        return {
            cambiarEstadoConfiguracionRolSector: cambiarEstadoConfiguracionRolSector,
            editarConfiguracionRolSector: editarConfiguracionRolSector,
            guardarConfiguracionRolSector: guardarConfiguracionRolSector,
            obtenerConfiguracionesRolSector: obtenerConfiguracionesRolSector,
            obtenerEntidadesPorSectorTerritorial: obtenerEntidadesPorSectorTerritorial,
            obtenerRolesPorEntidadTerritorial: obtenerRolesPorEntidadTerritorial,
            obtenerSectoresPorEntidadTerritorial: obtenerSectoresPorEntidadTerritorial

        };

        function cambiarEstadoConfiguracionRolSector() {
            // Post: apiBackboneCambiarEstadoConfiguracionRolSector: 'api/AutorizacionNegocio/CambiarEstadoConfiguracionRolSector'
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerSectoresPorEntidadTerritorial, configuracion)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function editarConfiguracionRolSector() {
            // Post: apiBackboneEditarConfiguracionRolSector: 'api/AutorizacionNegocio/EditarConfiguracionRolSector',
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerSectoresPorEntidadTerritorial, configuracion)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function guardarConfiguracionRolSector() {
            // Post: apiBackboneGuardarConfiguracionRolSector: 'api/AutorizacionNegocio/GuardarConfiguracionRolSector',
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerSectoresPorEntidadTerritorial, configuracion)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerConfiguracionesRolSector() {
            return $q(function(resolve, reject) {

                var result = [
                    {
                        "TipoEntidad": "Nacional",
                        "EntidadesTerritoriales": [],
                        "Roles": [{
                            Id: 0,
                            Nombre: 'Verificador'
                        }, {
                            Id: 1,
                            Nombre: 'Formulador'
                        }, {
                            Id: 2,
                            Nombre: 'Formulador Oficial'
                        }],
                        "Sectores": [{
                            Id: 0,
                            Nombre: 'Educacion'
                        }, {
                            Id: 1,
                            Nombre: 'Transporte'
                        }],
                        "EntidadesDestino": [],
                        "Configuraciones": [{
                            Id: 0,
                            RolNegocioEntidad: {
                                Rol: {
                                    Id: 0,
                                    Nombre: 'Verificador'
                                },
                                Entidad: {
                                    Id: 0,
                                    Nombre: 'Secretaría de Obras Públicas'
                                }
                            },
                            Sector: {
                                Id: 0,
                                Nombre: 'Educacion'
                            },
                            Activado: true
                        }, {
                            Id: 1,
                            RolNegocioEntidad: {
                                Rol: {
                                    Id: 0,
                                    Nombre: 'Verificador'
                                },
                                Entidad: {
                                    Id: 0,
                                    Nombre: 'Secretaría de Obras Públicas'
                                }
                            },
                            Sector: {
                                Id: 0,
                                Nombre: 'Educacion'
                            },
                            Activado: true
                        }, {
                            Id: 2,
                            RolNegocioEntidad: {
                                Rol: {
                                    Id: 0,
                                    Nombre: 'Verificador'
                                },
                                Entidad: {
                                    Id: 0,
                                    Nombre: 'Secretaría de Obras Públicas'
                                }
                            },
                            Sector: {
                                Id: 0,
                                Nombre: 'Educacion'
                            },
                            Activado: true
                        }]
                    }, {
                        "TipoEntidad": "Territorial",
                        "EntidadesTerritoriales": [{
                            Id: 0,
                            Nombre: 'Bogotá'
                        }, {
                            Id: 1,
                            Nombre: 'Medellín'
                        }, {
                            Id: 2,
                            Nombre: 'Cali'
                        }],
                        "Roles": [{
                            Id: 0,
                            Nombre: 'Verificador'
                        }, {
                            Id: 1,
                            Nombre: 'Formulador'
                        }, {
                            Id: 2,
                            Nombre: 'Formulador Oficial'
                        }],
                        "Sectores": [],
                        "EntidadesDestino": [],
                        "Configuraciones": [{
                            Id: 0,
                            RolNegocioEntidad: {
                                Rol: {
                                    Id: 0,
                                    Nombre: 'Verificador'
                                },
                                Entidad: {
                                    Id: 0,
                                    Nombre: 'Secretaría de Obras Públicas'
                                }
                            },
                            Sector: {
                                Id: 0,
                                Nombre: 'Educacion'
                            },
                            Activado: true
                        }, {
                            Id: 1,
                            RolNegocioEntidad: {
                                Rol: {
                                    Id: 0,
                                    Nombre: 'Verificador'
                                },
                                Entidad: {
                                    Id: 0,
                                    Nombre: 'Secretaría de Obras Públicas'
                                }
                            },
                            Sector: {
                                Id: 0,
                                Nombre: 'Educacion'
                            },
                            Activado: true
                        }, {
                            Id: 2,
                            RolNegocioEntidad: {
                                Rol: {
                                    Id: 0,
                                    Nombre: 'Verificador'
                                },
                                Entidad: {
                                    Id: 0,
                                    Nombre: 'Secretaría de Obras Públicas'
                                }
                            },
                            Sector: {
                                Id: 0,
                                Nombre: 'Educacion'
                            },
                            Activado: true
                        }]
                    },
                    {
                        "TipoEntidad": "SGR",
                        "EntidadesTerritoriales": [],
                        "Roles": [{
                            Id: 0,
                            Nombre: 'Verificador'
                        }, {
                            Id: 1,
                            Nombre: 'Formulador'
                        }, {
                            Id: 2,
                            Nombre: 'Formulador Oficial'
                        }],
                        "Sectores": [{
                            Id: 0,
                            Nombre: 'Educacion'
                        }, {
                            Id: 1,
                            Nombre: 'Transporte'
                        }],
                        "EntidadesDestino": [],
                        "Configuraciones": [{
                            Id: 0,
                            RolNegocioEntidad: {
                                Rol: {
                                    Id: 0,
                                    Nombre: 'Verificador'
                                },
                                Entidad: {
                                    Id: 0,
                                    Nombre: 'Secretaría de Obras Públicas'
                                }
                            },
                            Sector: {
                                Id: 0,
                                Nombre: 'Educacion'
                            },
                            Activado: true
                        }, {
                            Id: 1,
                            RolNegocioEntidad: {
                                Rol: {
                                    Id: 0,
                                    Nombre: 'Verificador'
                                },
                                Entidad: {
                                    Id: 0,
                                    Nombre: 'Secretaría de Obras Públicas'
                                }
                            },
                            Sector: {
                                Id: 0,
                                Nombre: 'Educacion'
                            },
                            Activado: true
                        }, {
                            Id: 2,
                            RolNegocioEntidad: {
                                Rol: {
                                    Id: 0,
                                    Nombre: 'Verificador'
                                },
                                Entidad: {
                                    Id: 0,
                                    Nombre: 'Secretaría de Obras Públicas'
                                }
                            },
                            Sector: {
                                Id: 0,
                                Nombre: 'Educacion'
                            },
                            Activado: true
                        }]
                    }
                ];
                resolve(result);
            });
            //var configuracion = {
            //    params: {
            //        usuarioDnp: usuarioDNP,
            //        nombreAplicacion: nombreAplicacionBackbone
            //    }
            //};
            //return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerConfiguracionesRolSector, configuracion)
            //    .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        }

        function obtenerEntidadesPorSectorTerritorial() {
            return $q(function (resolve, reject) {
                resolve([{
                    Id: 0,
                    Nombre: 'Secretaría de Obras Públicas'
                }, {
                    Id: 1,
                    Nombre: 'Secretaría de Medio Ambiente'
                }]);
            });
            //var configuracion = {
            //    params: {
            //        usuarioDnp: usuarioDNP,
            //        nombreAplicacion: nombreAplicacionBackbone
            //    }
            //};
            //return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEntidadesPorSectorTerritorial, configuracion)
            //    .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        } 
        
        function obtenerRolesPorEntidadTerritorial() {
            return $q(function (resolve, reject) {
                resolve([{
                    Id: 0,
                    Nombre: 'Verificador'
                }, {
                    Id: 1,
                    Nombre: 'Formulador'
                }, {
                    Id: 2,
                    Nombre: 'Formulador Oficial'
                }]);
            });
            //var configuracion = {
            //    params: {
            //        usuarioDnp: usuarioDNP,
            //        nombreAplicacion: nombreAplicacionBackbone
            //    }
            //};
            //return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerRolesPorEntidadTerritorial, configuracion)
            //    .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        } 

        function obtenerSectoresPorEntidadTerritorial() {
            return $q(function (resolve, reject) {
                resolve([{
                    Id: 0,
                    Nombre: 'Educacion'
                }, {
                    Id: 1,
                    Nombre: 'Transporte'
                }]);
            });
            //var configuracion = {
            //    params: {
            //        usuarioDnp: usuarioDNP,
            //        nombreAplicacion: nombreAplicacionBackbone
            //    }
            //};
            //return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerSectoresPorEntidadTerritorial, configuracion)
            //    .then(utilidades.httpRequestComplete, utilidades.httpRequestError);
        } 
        
    }
})();