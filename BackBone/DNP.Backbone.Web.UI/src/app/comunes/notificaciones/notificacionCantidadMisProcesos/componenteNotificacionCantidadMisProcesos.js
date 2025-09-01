(function (usuarioDNP, idTipoProyecto, idTipoTramite) {

    angular.module('backbone').component('notificacionCantidadMisProcesos', {
        templateUrl: '/src/app/comunes/notificaciones/notificacionCantidadMisProcesos/pantallaCantidadMisProcesos.html',
        controller: controladorNotificacionCantidadMisProcesos,
        controllerAs: 'vm',
        bindings: {
            useHtmlMenu: '=',
            useHtmlNotification: '='
        }
    });

    controladorNotificacionCantidadMisProcesos.$inject = [
        '$q',
        'serviciosComponenteNotificacionCantidadProyectos',
        'serviciosComponenteNotificacionCantidadTramites',
        '$scope',
        'backboneServicios',
        '$timeout',
        '$localStorage',
        '$interval',
        'constantesBackbone'
    ];

    function controladorNotificacionCantidadMisProcesos(
        $q,
        serviciosComponenteNotificacionCantidadProyectos,
        serviciosComponenteNotificacionCantidadTramites,
        $scope,
        backboneServicios,
        $timeout,
        $localStorage,
        $interval,
        constantesBackbone
    ) {
        var vm = this;
        vm.cantidadDeProyectos = 0;
        vm.cantidadDeTramites = 0;
        vm.notificacionesYaFueronCargadas = false;
        vm.peticionTramitesFinalizada = false;
        vm.peticionProyectosFinalizada = false;
        vm.tramites = null;
        vm.proyectos = null;
        vm.totalProyectos = 0;

        $interval( () => procesarInformacion(), 1800);

        function procesarInformacion() {
            if ($localStorage.cantidadesMisproyectos != undefined) {
                vm.totalProyectos = $localStorage.cantidadesMisproyectos[0].PProyecto + $localStorage.cantidadesMisproyectos[0].GRProyecto + $localStorage.cantidadesMisproyectos[0].EJProyecto + $localStorage.cantidadesMisproyectos[0].EVProyecto;
                vm.totalTramites = $localStorage.cantidadesMisproyectos[0].EJTramite + $localStorage.cantidadesMisproyectos[0].EJProgramacion;
                vm.totalProcesos = vm.totalProyectos + vm.totalTramites;
            }
            /*if (!(vm.peticionTramitesFinalizada && vm.peticionProyectosFinalizada)) {
                return;
            }
            setTotales();*/
        }

        function setTotales() {
            var entidades = vm.proyectos.concat(vm.tramites);
            vm.misProcesos = {
                totales: vm.cantidadDeProyectos + vm.cantidadDeTramites,
                pantallaInicio: {
                    planeacion: {
                        totales: entidades.filter(f => [constantesBackbone.idEtapaViabilidad, constantesBackbone.idEtapaFormulacion].findIndex(key => key == f.etapa) > -1).length,
                        proyectos: {
                            totales: vm.proyectos.filter(f => [constantesBackbone.idEtapaViabilidad, constantesBackbone.idEtapaFormulacion].findIndex(key => key == f.etapa) > -1).length
                        },
                        tramites: {
                            totales: vm.tramites.filter(f => [constantesBackbone.idEtapaViabilidad, constantesBackbone.idEtapaFormulacion].findIndex(key => key == f.etapa) > -1).length
                        }
                    },
                    programacion: {
                        totales: entidades.filter(f => f.etapa == constantesBackbone.idEtapaProgramacion).length,
                        proyectos: {
                            totales: vm.proyectos.filter(f => f.etapa == constantesBackbone.idEtapaProgramacion).length
                        },
                        tramites: {
                            totales: vm.tramites.filter(f => f.etapa == constantesBackbone.idEtapaProgramacion).length
                        }
                    },
                    ejecucion: {
                        totales: entidades.filter(f => f.etapa == constantesBackbone.idEtapaEjecucion).length,
                        proyectos: {
                            totales: vm.proyectos.filter(f => f.etapa == constantesBackbone.idEtapaEjecucion).length
                        },
                        tramites: {
                            totales: vm.tramites.filter(f => f.etapa == constantesBackbone.idEtapaEjecucion).length
                        }
                    },
                    seguimiento: {
                        totales: 0,
                        proyectos: {
                            totales: 0
                        },
                        tramites: {
                            totales: 0
                        }
                    },
                    evaluacion: {
                        totales: 0,
                        proyectos: {
                            totales: 0
                        },
                        tramites: {
                            totales: 0
                        }
                    }
                },
                misProcesos: {
                    proyectos: {
                        planeacion: {
                            totales: vm.proyectos.filter(f => [constantesBackbone.idEtapaViabilidad, constantesBackbone.idEtapaFormulacion].findIndex(key => key == f.etapa) > -1).length
                        },
                        programacion: {
                            totales: vm.proyectos.filter(f => f.etapa == constantesBackbone.idEtapaProgramacion).length
                        },
                        ejecucion: {
                            totales: vm.proyectos.filter(f => f.etapa == constantesBackbone.idEtapaEjecucion).length
                        },
                        seguimiento: {
                            totales: 0
                        },
                        evaluacion: {
                            totales: 0
                        },
                        totales: vm.proyectos.length
                    },
                    tramites: {
                        planeacion: {
                            totales: vm.tramites.filter(f => [constantesBackbone.idEtapaViabilidad, constantesBackbone.idEtapaFormulacion].findIndex(key => key == f.etapa) > -1).length
                        },
                        programacion: {
                            totales: vm.tramites.filter(f => f.etapa == constantesBackbone.idEtapaProgramacion).length
                        },
                        ejecucion: {
                            totales: vm.tramites.filter(f => f.etapa == constantesBackbone.idEtapaEjecucion).length
                        },
                        seguimiento: {
                            totales: 0
                        },
                        evaluacion: {
                            totales: 0
                        },
                        totales: vm.tramites.length
                    }
                }
            }

            $localStorage.misProcesos = vm.misProcesos;
        }

        vm.$onInit = function () {
            /*if (backboneServicios.estaAutorizado() && vm.notificacionesYaFueronCargadas == false) {
                vm.notificacionesYaFueronCargadas = true;
                obtenerNotificacionesCantidadDeProyectos();
                obtenerNotificacionesCantidadDeTramites();
            }*/
        };

        $scope.$on('AutorizacionConfirmada', function () {
            /*if (vm.notificacionesYaFueronCargadas == false) {
                vm.notificacionesYaFueronCargadas = true;
                obtenerNotificacionesCantidadDeProyectos();
                obtenerNotificacionesCantidadDeTramites();
            }*/
        });

        function obtenerNotificacionesCantidadDeProyectos() {
            vm.proyectos = [];
            vm.cantidadDeProyectos = 0;
            $timeout(function () {
                serviciosComponenteNotificacionCantidadProyectos.obtenerNotificacionesCantidadDeProyectos().then(
                    function (result) {
                        if (result.data.GruposEntidades && result.data.GruposEntidades.length > 0) {
                            const listaGrupoEntidades = result.data.GruposEntidades;
                            listaGrupoEntidades.forEach(grupoEntidade => {
                                grupoEntidade.ListaEntidades.forEach(entidad => {
                                    vm.cantidadDeProyectos += entidad.ObjetosNegocio.length;
                                    entidad.ObjetosNegocio.forEach(objetoNegocio => {
                                        vm.proyectos.push({ tipo: 'proyecto', etapa: objetoNegocio.Etapa.toUpperCase(), idNegocio: objetoNegocio.IdObjetoNegocio });
                                    });
                                });
                            });
                        }
                        vm.peticionProyectosFinalizada = true;
                    },
                    function (error) {
                        vm.cantidadDeProyectos = 0;
                        vm.peticionProyectosFinalizada = true;
                    }
                );
            }, 3000);
        }

        function obtenerNotificacionesCantidadDeTramites() {
            vm.tramites = [];
            vm.cantidadDeTramites = 0;
            $timeout(function () {
                serviciosComponenteNotificacionCantidadTramites.obtenerNotificacionesCantidadDeTramites().then(
                    function (result) {
                        if (result.data.ListaGrupoTramiteEntidad && result.data.ListaGrupoTramiteEntidad.length > 0) {
                            const listaGrupoEntidades = result.data.ListaGrupoTramiteEntidad;
                            listaGrupoEntidades.forEach(entidad => {
                                entidad.GrupoTramites.forEach(tramite => {
                                    vm.cantidadDeTramites += tramite.ListaTramites.length;
                                    tramite.ListaTramites.forEach(objetoNegocio => {
                                        vm.tramites.push({ tipo: 'tramite', etapa: objetoNegocio.Etapa.toUpperCase(), idNegocio: objetoNegocio.IdObjetoNegocio });
                                    });
                                });
                            });
                        }
                        vm.peticionTramitesFinalizada = true;
                    },
                    function (error) {
                        vm.cantidadDeTramites = 0;
                        vm.peticionTramitesFinalizada = true;
                    }
                );
            }, 3000);
        }

    }
})(usuarioDNP, idTipoProyecto, idTipoTramite);