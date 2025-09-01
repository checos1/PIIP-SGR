(function () {

    angular.module('backbone').controller('crearModificarConfiguracionController', crearModificarConfiguracionController);

    crearModificarConfiguracionController.$inject = ['$q', '$scope', 'utilidades', 'configurarEntidadRolSectorServicio'];

    function crearModificarConfiguracionController($q, $scope, utilidades, configurarEntidadRolSectorServicio) {
        var vm = this;
        vm.configuracionNacional = {
            activado: false,
            rol: null,
            sector: null,
            entidadDestino: null
        };
        vm.configuracionTerritorial = {
            activado: false,
            entidadTerritorial: false,
            rol: null,
            sector: null,
            entidadDestino: null
        };
        vm.entidadesDestinoNacionales = [];
        vm.rolesTerritoriales = [];
        vm.sectoresTerritoriales = [];
        vm.entidadesDestinoTerritoriales = [];

        vm.cerrarEdicion = cerrarEdicion;
        vm.confirmarGuardarNacional = confirmarGuardarNacional;
        vm.confirmarGuardarTerritorial = confirmarGuardarTerritorial;
        vm.confirmarSalirSinGuardar = confirmarSalirSinGuardar;
        vm.guardarConfiguracion = guardarConfiguracion;
        vm.limpiarNacionales = limpiarNacionales;
        vm.limpiarTerritoriales = limpiarTerritoriales;
        vm.seleccionarSectorNacional = seleccionarSectorNacional;
        vm.seleccionarEntidadTerritorial = seleccionarEntidadTerritorial;
        vm.seleccionarSectorTerritorial = seleccionarSectorTerritorial;

        function cerrarEdicion() {
            vm.retornoCancelar();
            vm.estaEditando = false;
            vm.limpiarNacionales();
            vm.limpiarTerritoriales();
        }

        function confirmarGuardarNacional() {
            if (vm.configuracionNacionalForm && vm.configuracionNacionalForm.$dirty) {
                utilidades.mensajeWarning('¿Está seguro de guardar?', function () {
                    guardarConfiguracion().then(function () {
                        vm.limpiarNacionales();
                        vm.retornoGuardar();
                    }).catch(function (e) {
                        setTimeout(function () {
                            utilidades.mensajeError('Hubo un error al guardar la configuración', function () { })
                        }, 1000);
                    });
                }, function () { });
            }
            if (!vm.configuracionNacional.rol || !vm.configuracionNacional.sector || !vm.configuracionNacional.entidadDestino) {
                utilidades.mensajeError('Todos los campos son obligatorios', function () { })
                return;
            }
            if (!vm.configuracionNacionalForm || !vm.configuracionNacionalForm.$dirty) {
                vm.retornoCancelar();
                return;
            }
        }

        function confirmarGuardarTerritorial() {
            if (vm.configuracionTerritorialForm && vm.configuracionTerritorialForm.$dirty) {
                utilidades.mensajeWarning('¿Está seguro de guardar?', function () {
                    guardarConfiguracion().then(function () {
                        vm.limpiarTerritoriales();
                        vm.retornoGuardar();
                    }).catch(function () {
                        utilidades.mensajeError('Hubo un error al guardar la configuración', function () { })
                    });
                }, function () { });
            }            
        }

        function confirmarSalirSinGuardar() {
            if ((vm.configuracionNacionalForm && vm.configuracionNacionalForm.$dirty) || (vm.configuracionTerritorialForm && vm.configuracionTerritorialForm.$dirty)) {
                utilidades.mensajeWarning('¿Está seguro que desea salir sin guardar?', function () {
                    vm.limpiarNacionales();
                    vm.limpiarTerritoriales();
                    $scope.$apply(vm.cerrarEdicion);
                }, function () { });
            } else {
                vm.cerrarEdicion();
            }
        }

        function guardarConfiguracion() {
            // Todo: Guardar Configuracion
            return $q(function (resolve, reject) {
                resolve(200);
            });
        }

        function limpiarTerritoriales() {
            vm.creandoConfiguracionTerritorial = false;
            vm.rolesTerritoriales = [];
            vm.sectoresTerritoriales = [];
            vm.entidadesDestinoTerritoriales = [];
            vm.configuracionTerritorial = {
                activado: false,
                entidadTerritorial: null,
                rol: null,
                sector: null,
                entidadDestino: null
            };
        }

        function limpiarNacionales() {
            vm.creandoConfiguracionNacional = false;
            vm.entidadesDestinoNacionales = [];
            vm.configuracionNacional = {
                activado: false,
                rol: null,
                sector: null,
                entidadDestino: null
            };
        }

        function seleccionarSectorNacional(sector) {
            //TODO: Consumir servicio de obtencion de entidades por sector
            configurarEntidadRolSectorServicio.obtenerEntidadesPorSectorTerritorial().then(function (entidades) {
                $scope.$apply(function () {
                    vm.entidadesDestinoNacionales = entidades;
                });
            });
        }

        function seleccionarSectorTerritorial(sector) {
            //TODO: Consumir servicio de obtencion de entidades por sector
            configurarEntidadRolSectorServicio.obtenerEntidadesPorSectorTerritorial().then(function (entidades) {
                $scope.$apply(function () {
                    vm.entidadesDestinoTerritoriales = entidades;
                });
            });
        }

        function seleccionarEntidadTerritorial(entidadTerritorial) {
            //TODO: Consumir servicio de obtencion de roles por entidad territorial
            configurarEntidadRolSectorServicio.obtenerRolesPorEntidadTerritorial().then(function (roles) {
                $scope.$apply(function () {
                    vm.rolesTerritoriales = roles;
                });
            });

            //TODO: Consumir servicio de obtencion de sectores por entidad territorial
            configurarEntidadRolSectorServicio.obtenerSectoresPorEntidadTerritorial().then(function (sectores) {
                $scope.$apply(function () {
                    vm.sectoresTerritoriales = sectores;
                });
            });
        }

        $scope.$watch('vm.creandoConfiguracionNacional', function () {
            vm.estaEditando = false;
            if (vm.creandoConfiguracionNacional && vm.configuracionEnEdicion) {
                vm.configuracionNacional = {
                    activado: vm.configuracionEnEdicion.Activado,
                    rol: vm.configuracionEnEdicion.RolNegocioEntidad.Rol,
                    sector: vm.configuracionEnEdicion.Sector
                };
                if (vm.configuracionNacional.sector) vm.seleccionarSectorNacional(vm.sectorNacional);
                vm.configuracionNacional.entidadDestino = vm.configuracionEnEdicion.RolNegocioEntidad.Entidad;
                vm.estaEditando = true;
            }
        });
        $scope.$watch('vm.creandoConfiguracionTerritorial', function () {
            vm.estaEditando = false;
            if (vm.creandoConfiguracionTerritorial && vm.configuracionEnEdicion) {
                vm.configuracionTerritorial = {
                    activado: vm.configuracionEnEdicion.Activado,
                    entidadTerritorial: vm.configuracionEnEdicion.EntidadTerritorial
                };
                if (vm.configuracionTerritorial.entidadTerritorial) vm.seleccionarEntidadTerritorial(vm.configuracionTerritorial.entidadTerritorial);
                vm.configuracionTerritorial.rol = vm.configuracionEnEdicion.RolNegocioEntidad.Rol;
                vm.configuracionTerritorial.sector = vm.configuracionEnEdicion.Sector;
                if (vm.configuracionTerritorial.sector) vm.seleccionarSectorTerritorial(vm.configuracionTerritorial.sector);
                vm.configuracionTerritorial.entidadDestino = vm.configuracionEnEdicion.RolNegocioEntidad.Entidad;
                vm.estaEditando = true;
            }
        });
    }

    angular.module('backbone')
        .component('crearModificarConfiguracion', {
            templateUrl: "/src/app/autorizacion/componentes/crearModificarConfiguracion/crearModificarConfiguracion.html",
            controller: 'crearModificarConfiguracionController',
            controllerAs: 'vm',
            bindings: {
                creandoConfiguracionTerritorial: '=',
                creandoConfiguracionNacional: '=',
                tipoEntidad: '=',
                retornoCancelar: '&',
                retornoGuardar: '&',

                entidadesTerritoriales: '=',
                rolesNacionales: '=',
                sectoresNacionales: '=',

                configuracionEnEdicion: "="
            }
        });

})();

