(function () {

    'use strict';

    angular.module('backbone').controller('listaMensajesMantenimientoController', listaMensajesMantenimientoController);
    listaMensajesMantenimientoController.$inject = [
        '$scope',
        '$location',
        '$uibModal',
        '$localStorage',
        'constantesAutorizacion',
        'configurarEntidadRolSectorServicio',
        'mensajeServicio',
        'backboneServicios',
        'sesionServicios',
        'MensajeMantenimientoModel'
    ];

    function listaMensajesMantenimientoController(
        $scope,
        $location,
        $uibModal,
        $localStorage,
        constantesAutorizacion,
        configurarEntidadRolSectorServicio,
        mensajeServicio,
        backboneServicios,
        sesionServicios,
        MensajeMantenimientoModel) {

        var vm = this;

        //#region Variables

        vm.listaFiltroEstadoMensajes = [];
        vm.listaEntidad = [];
        vm.tipoEntidad = null;
        vm.columnasDisponiblesPorAgregar = ['Tipo Mensaje', 'Estado', 'Restringe Acesso'];
        vm.columnas = ['Nombre Mensaje', 'Fecha Inicio', 'Fecha Final'];
        vm.mostrarFiltro = false;
        vm.peticion = {};
        vm.accionesMensajesTemplate = 'src/app/mensajesMantenimiento/plantillas/plantillaAccionesMensaje.html';
        vm.tipoMensajeTemaplate = 'src/app/mensajesMantenimiento/plantillas/plantillaTipoMensaje.html';
        vm.roles = obtenerRoles();

        vm.filtro = {
            NombreMensaje: null,
            FechaCreacionInicio: null,
            FechaCreacionFin: null,
            EstadoMensaje: null,
            TipoMensaje: null,
            TieneRestringeAcesso: null,
            MensajeTemplate: null
        }

        const columnasDef = [
            {
                field: 'NombreMensaje',
                displayName: "Nombre Mensaje",
                width: "29%",
                headerCellClass: 'ui-grid-cell-header',
                cellTooltip: (row, col) => row.entity[col.field]
            },
            {
                field: 'FechaCreacionInicio',
                displayName: "Fecha/Hora Inicio",
                width: "15%",
                type: "date",
                cellFilter: 'date:"dd/MM/yyyy hh:mm"',
                headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
                cellClass: 'ui-grid-cell-align-center'
            },
            {
                field: 'FechaCreacionFin',
                displayName: "Fecha/Hora Final",
                width: "15%",
                type: "date",
                cellFilter: 'date:"dd/MM/yyyy hh:mm"',
                headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
                cellClass: 'ui-grid-cell-align-center'
            },
            {
                field: 'TipoMensajeDesc',
                displayName: "Opiciones",
                cellTemplate: vm.tipoMensajeTemaplate,
                headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
                cellClass: 'ui-grid-cell-align-center',
                width: "13%"
            },
            {
                field: 'TipoEntidad',
                displayName: "Tipo Entidad",
                cellTemplate: vm.tipoEntidad,
                width: "13%",
                headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
                cellClass: 'ui-grid-cell-align-center'
            },
            {
                field: 'accion',
                displayName: 'Acción',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                cellTemplate: vm.accionesMensajesTemplate,
                width: '15%',
                headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
                cellClass: 'ui-grid-cell-align-center'
            }
        ];

        vm.gridOptions = {
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 25, 50, 100],
            paginationPageSize: 5,
            columnDefs: columnasDef,
            onRegisterApi: onRegisterApi,
            data: []
        };

        vm.disclaimer = { show: false };

        const _visualzadores = {
            disclaimer: function(mensaje) {
                vm.disclaimer = {
                    template: mensaje.MensajeTemplate,
                    type: mensaje.EstiloTipoMensaje,
                    preVisualizacion: true,
                    show: true
                };
            },
            popUp: function (mensaje) {
                mensajeServicio.preVisualizarModalMensaje(mensaje);
            }
        }

        //#endregion
        
        //#region Métodos

        vm.listaConfiguracionesRolSector = obtenerConfiguracionesRolSector();
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.init = init;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.buscar = buscar;
        vm.crearEditarNuevaMensaje = crearEditarNuevaMensaje;
        vm.eliminarMensaje = eliminarMensaje;
        vm.obtenerClassTipoMensaje = obtenerClassTipoMensaje;
        vm.visualizarMensaje = visualizarMensaje;
        vm.tieneColumna = tieneColumna;
        vm.abrirModalAdicionarColumnas = abrirModalAdicionarColumnas;
        vm.cambioTipoEntidad = cambioTipoEntidad;

        //#endregion

        function obtenerRoles() {
            var roles = sesionServicios.obtenerUsuarioIdsRoles();

            if (backboneServicios.estaAutorizado() && roles != null && roles.length > 0) {
                vm.peticion = {
                    IdUsuarioDNP: usuarioDNP,
                    Aplicacion: nombreAplicacionBackbone,
                    IdsRoles: roles
                };
            }
        }

        function obtenerConfiguracionesRolSector() {
            var parametros = {
                usuarioDnp: usuarioDNP,
                nombreAplicacion: nombreAplicacionBackbone
            }

            return configurarEntidadRolSectorServicio.obtenerConfiguracionesRolSector(parametros).then(function (respuesta) {
                vm.listaConfiguracionesRolSector = respuesta;
                vm.listaTipoEntidad = crearListaTipoEntidad();
            });
        }

        function crearListaTipoEntidad() {
            return [{
                    Nombre: constantesAutorizacion.tipoEntidadNacional,
                    Descripcion: constantesAutorizacion.tipoEntidadNacional,
                    Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadNacional)
                },
                {
                    Nombre: constantesAutorizacion.tipoEntidadTerritorial,
                    Descripcion: constantesAutorizacion.tipoEntidadTerritorial,
                    Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadTerritorial)
                },
                {
                    Nombre: constantesAutorizacion.tipoEntidadSGR,
                    Descripcion: constantesAutorizacion.tipoEntidadSGR,
                    Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadSGR)
                },
                {
                    nombre: constantesAutorizacion.tipoEntidadPrivadas,
                    Descripcion: constantesAutorizacion.tipoEntidadPrivadas,
                    Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadPrivadas)
                },
                {
                    Nombre: constantesAutorizacion.tipoEntidadPublicas,
                    Descripcion: "Públicas",
                    Deshabilitado: !tipoEntidadPresenteEnLaConfiguracion(constantesAutorizacion.tipoEntidadPublicas)
                }
            ];
        }

        function tipoEntidadPresenteEnLaConfiguracion(tipoEntidad) {
            var tipoConfiguracion = _.find(vm.listaConfiguracionesRolSector, {
                TipoEntidad: tipoEntidad
            });
            return tipoConfiguracion ? true : false;
        }

        function cambioTipoEntidad(tipoEntidad) {
            limpiarCamposFiltro();
            vm.tipoEntidad = tipoEntidad;
            vm.filtro.TipoEntidad = tipoEntidad;
            _listaConsolaMensajes();
        }

        function conmutadorFiltro() {
            limpiarCamposFiltro();
            vm.mostrarFiltro = !vm.mostrarFiltro;
        }

        function limpiarCamposFiltro() {
            for (const prop in vm.filtro)
                vm.filtro[prop] = null;
        }

        async function init() {
            //buscar por defecto
            vm.filtro.TipoEntidad = 'Nacional';

            await _listaConsolaMensajes().then(function () {
                let tipoEntidad;
                for (let i = 0; i < vm.listaTipoEntidad.length; i++) {
                    tipoEntidad = vm.listaTipoEntidad[i];

                    if (!tipoEntidad.Deshabilitado) {
                        vm.tipoEntidad = tipoEntidad.Nombre;
                        break;
                    }
                }
            });

            buscarColumnasLocalStorage();
        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        async function _listaConsolaMensajes() {
            return mensajeServicio.obtenerListaMensaje(vm.peticion, vm.filtro)
                .then((respuesta) => {
                    if (respuesta && (respuesta.data || []).length) {
                        vm.gridOptions.data = (respuesta.data || []).map(x => new MensajeMantenimientoModel(x));
                        return;
                    }

                    vm.gridOptions.data = [];
                })
                .catch(error => {
                    vm.gridOptions.data = [];
                    console.log(error);
                    toastr.error("Hubo un error al cargar las mensajes de mantenimiento");
                });
        }

        function obtenerClassTipoMensaje(tipoMensaje) {
            const estilos = {
                "PopUp": "background-estado activo",
                "Disclaimer": "background-estado atencion",
                "Restringe": "background-estado inactivo"
            }
            
            return estilos[tipoMensaje];
        }

        function visualizarMensaje(mensaje) {
            try {
                const tipoMensaje = {
                    1: "popUp",
                    2: "disclaimer"
                }[mensaje.TipoMensaje];
    
                _visualzadores[tipoMensaje](mensaje);
            } catch (error) {
                toastr.error("Hubo un error al cargar visualización de la mensaje");
            }
        }

        function buscar() {
            vm.filtro.TipoEntidad = vm.tipoEntidad;
            _listaConsolaMensajes();
        }

        function crearEditarNuevaMensaje($entity) {
            if (!$entity){
                $location.path("/mensajeMantenimiento");
                return;
            }
            
            $location.path(`/mensajeMantenimiento/${$entity.Id}`);  
        }

        function abrirModalAdicionarColumnas() {

            const modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/configurarColumnas/plantillaConfigurarColumnas.html',
                controller: 'controladorConfigurarColumnas',
                controllerAs: "vm",
                size: 'lg',
                resolve: {
                    items: function () {
                        return {
                            columnasActivas: vm.columnas,
                            columnasDisponibles: vm.columnasDisponiblesPorAgregar,
                        };
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                if (!$localStorage.tipoFiltro) {
                    $localStorage.tipoFiltro = {
                        "mensajesMantenimiento": {
                            'columnasActivas': selectedItem.columnasActivas,
                            'columnasDisponibles': selectedItem.columnasDisponibles
                        }
                    };
                } else {
                    $localStorage.tipoFiltro["mensajesMantenimiento"] = {
                        'columnasActivas': selectedItem.columnasActivas,
                        'columnasDisponibles': selectedItem.columnasDisponibles
                    }
                }

                buscarColumnasLocalStorage();
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
        };

        function buscarColumnasLocalStorage() {
            if (!$localStorage.tipoFiltro)
                return;

            if ($localStorage.tipoFiltro["mensajesMantenimiento"]) {
                vm.columnas = $localStorage.tipoFiltro["mensajesMantenimiento"].columnasActivas;
                vm.columnasDisponiblesPorAgregar = $localStorage.tipoFiltro["mensajesMantenimiento"].columnasDisponibles;
            }
        }

        function tieneColumna(columna){
            return vm.columnas.find(x => x == columna) != null;
        }

        function eliminarMensaje(id) {
            swal({
                title: "",
                text: "¿Realmente quieres eliminar la mensaje de mantenimiento?",
                type: "error",
                closeOnConfirm: true,
                html: true,
                showCancelButton: true,
            }, (isConfirm) => {
                if (isConfirm) {
                    mensajeServicio.eliminar(vm.peticion, id)
                        .then(() => {
                            toastr.success("Mensaje eliminada con éxito");
                            vm.buscar();
                        })
                        .catch(error => {
                            console.log(error)
                            toast.error("Hubo un error al eliminar el mensaje de mantenimiento");
                        });
                }
            });
        }
    }
})();