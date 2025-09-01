(function () {
    'use strict';

    configurarEntidadRolSectorController.$inject = ['$scope', 'configurarEntidadRolSectorServicio', 'listaEntidadRolSector', 'uiGridConstants', 'utilidades', 'constantesAutorizacion'];

    function configurarEntidadRolSectorController($scope, configurarEntidadRolSectorServicio, listaEntidadRolSector, uiGridConstants, utilidades, constantesAutorizacion) {

        var vm = this;
        vm.maximoPaginas = 10;
        vm.dtoNacional = _.find(listaEntidadRolSector, { TipoEntidad: constantesAutorizacion.tipoEntidadNacional });
        vm.dtoTerritorial = _.find(listaEntidadRolSector, { TipoEntidad: constantesAutorizacion.tipoEntidadTerritorial });
        vm.lang = "es";
        vm.listaNacional = vm.dtoNacional.Configuraciones;
        vm.listaTerritorial = vm.dtoTerritorial.Configuraciones;

        vm.entidadesTerritoriales = vm.dtoTerritorial.EntidadesTerritoriales;

        vm.rolesNacionales = vm.dtoNacional.Roles;
        vm.sectoresNacionales = vm.dtoNacional.Sectores;

        // Funciones
        vm.adicionarConfiguracionTerritorial = adicionarConfiguracionTerritorial;
        vm.adicionarConfiguracionNacional = adicionarConfiguracionNacional;
        vm.adicionarConfiguracion = adicionarConfiguracion;
        vm.cambioTipoEntidad = cambioTipoEntidad;
        vm.editarConfiguracion = editarConfiguracion;
        vm.retornoCancelar = retornoCancelar;
        vm.retornoGuardar = retornoGuardar;

        // Templates
        vm.editarFilaTemplate = 'src/app/autorizacion/plantillas/plantillaEditarFila.html';
        vm.switchTemplate = 'src/app/autorizacion/plantillas/plantillaSwitch.html';
        vm.rolTemplate = 'src/app/autorizacion/plantillas/plantillaRol.html';
        vm.sectorTemplate = 'src/app/autorizacion/plantillas/plantillaSector.html';
        vm.entidadTemplate = 'src/app/autorizacion/plantillas/plantillaEntidad.html';
        vm.entidadTerritorialTemplate = 'src/app/autorizacion/plantillas/plantillaEntidadTerritorial.html';
        vm.filaEntidadSectorTemplate = 'src/app/autorizacion/plantillas/plantillaFilaEntidadRolSector.html';

        // Variables
        vm.tipoEntidad = constantesAutorizacion.tipoEntidadNacional;
        vm.abrirEditarConfiguracion = false;
        vm.deshabilitadoPorEdicion = vm.listaNacional.length === 0;
        vm.creandoConfiguracionNacional = vm.listaNacional.length === 0;
        vm.creandoConfiguracionTerritorial = vm.listaTerritorial.length === 0;
        vm.totalRegistros = vm.listaNacional.length;
        vm.filasFiltradas = vm.totalRegistros;
        vm.listaVisible = [];

        vm.definicionColumnasTerritorial = [
            {
                field: 'EntidadTerritorial',
                displayName: "Entidad Territorial",
                enableHiding: false,
                width: '20%',
                cellTemplate: vm.entidadTerritorialTemplate
            }, {
                field: 'Rol',
                displayName: "Rol",
                enableHiding: false,
                width: '20%',
                cellTemplate: vm.rolTemplate
            }, {
                field: 'Sector',
                displayName: "Sector",
                enableHiding: false,
                width: '20%',
                cellTemplate: vm.sectorTemplate
            }, {
                field: 'Entidad',
                displayName: "Entidad",
                enableHiding: false,
                width: '20%',
                cellTemplate: vm.entidadTemplate
            }, {
                field: 'Activado',
                displayName: "Estado",
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                cellTemplate: vm.switchTemplate,
                width: '10%'
            }, {
                field: 'Id',
                displayName: "Acciones",
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                cellTemplate: vm.editarFilaTemplate,
                width: '10%'
            }
        ];
        vm.definicionColumnasNacional = [
            {
                field: 'Rol',
                displayName: 'Rol',
                enableHiding: false,
                width: '24%',
                cellTemplate: vm.rolTemplate
            }, {
                field: 'Sector',
                displayName: "Sector",
                enableHiding: false,
                width: '24%',
                cellTemplate: vm.sectorTemplate
            }, {
                field: 'Entidad',
                displayName: "Entidad",
                enableHiding: false,
                width: '24%',
                cellTemplate: vm.entidadTemplate
            }, {
                field: 'Activado',
                displayName: "Estado",
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                cellTemplate: vm.switchTemplate,
                width: '14%'
            }, {
                field: 'Id',
                displayName: "Acciones",
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                cellTemplate: vm.editarFilaTemplate,
                width: '14%'
            },
        ];

        vm.numeroPorPagina = 10;

        vm.gridOptions = {
            rowTemplate: vm.filaEntidadSectorTemplate,
            rowHeight: 32,

            showGridFooter: false,

            enablePaginationControls: false,
            paginationPageSize: vm.numeroPorPagina,
            useExternalPagination: false,
            useExternalSorting: false,
            paginationCurrentPage: 1,

            enableVerticalScrollbar: 1,

            enableFiltering: true,
            useExternalFiltering: false,
            columnDefs: vm.definicionColumnasNacional,

            data: vm.listaNacional,
            onRegisterApi: onRegisterApi
        };

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;

            gridApi.core.on.filterChanged($scope, function () {

            });
            gridApi.core.on.rowsRendered($scope, function () {
                vm.filasFiltradas = $scope.gridApi.core.getVisibleRows($scope.gridApi.grid).length;

                var datosFiltrados = $scope.gridApi.core.getVisibleRows($scope.gridApi.grid);;

                $scope.filasFiltradasRegistros = [];
                _.each(datosFiltrados, function (data) {
                    $scope.filasFiltradasRegistros.push(data.entity);

                });

                _.each($scope.gridApi.grid.rows, function (value) {
                    value.entity.Seleccionado = false;

                    var previas = _.filter(vm.listaMetadatosPorAplicacion, function (o) { return o.Id === value.entity.Id });
                    if (vm.listaVisible.length > 0 || previas.length > 0) {
                        value.entity.Seleccionado = true;
                    }
                });
            });
        }

        function adicionarConfiguracion() {
            if (vm.tipoEntidad === constantesAutorizacion.tipoEntidadNacional) {
                vm.adicionarConfiguracionNacional();
            }

            if (vm.tipoEntidad === constantesAutorizacion.tipoEntidadTerritorial) {
                vm.adicionarConfiguracionTerritorial();
            }
        }

        function adicionarConfiguracionTerritorial() {
            vm.creandoConfiguracionTerritorial = true;
            vm.deshabilitadoPorEdicion = true;
        }

        function adicionarConfiguracionNacional() {
            vm.creandoConfiguracionNacional = true;
            vm.deshabilitadoPorEdicion = true;
        }

        function cambioTipoEntidad(tipoEntidad) {
            if (tipoEntidad === constantesAutorizacion.tipoEntidadNacional) {
                vm.deshabilitadoPorEdicion = vm.listaNacional.length === 0 ? true : false;
                vm.creandoConfiguracionNacional = vm.listaNacional.length === 0 ? true : false;
                vm.gridOptions.columnDefs = vm.definicionColumnasNacional;
                vm.listaVisible = vm.listaNacional;
                vm.gridOptions.data = vm.listaNacional;
                vm.totalRegistros = vm.listaNacional.length;
            }
            if (tipoEntidad === constantesAutorizacion.tipoEntidadTerritorial) {
                vm.deshabilitadoPorEdicion = vm.listaTerritorial.length === 0 ? true : false;
                vm.creandoConfiguracionTerritorial = vm.listaTerritorial.length === 0 ? true : false;
                vm.gridOptions.columnDefs = vm.definicionColumnasTerritorial;
                vm.listaVisible = vm.listaTerritorial;
                vm.gridOptions.data = vm.listaTerritorial;
                vm.totalRegistros = vm.listaTerritorial.length;
            }
        }

        function editarConfiguracion(fila) {
            fila.editable = true;
            fila.abrirEditarConfiguracionTerritorial = vm.tipoEntidad === constantesAutorizacion.tipoEntidadTerritorial ? true : false;
            fila.abrirEditarConfiguracionNacional = vm.tipoEntidad === constantesAutorizacion.tipoEntidadNacional ? true : false;;
            vm.deshabilitadoPorEdicion = true;
            vm.configuracionEnEdicion = fila;
            vm.gridOptions.enableFiltering = false;
            vm.gridOptions.enableSorting = false;
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.COLUMN);
        }

        function retornoGuardar() {
            vm.creandoConfiguracionTerritorial = false;
            vm.creandoConfiguracionNacional = false;
            vm.deshabilitadoPorEdicion = false;
            if (vm.configuracionEnEdicion) {
                vm.configuracionEnEdicion.editable = false;
                vm.gridOptions.enableFiltering = true;
                vm.gridOptions.enableSorting = true;
                $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
                $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.COLUMN);
            }
            // Todo: Obtener configuraciones
            setTimeout(function () {
                utilidades.mensajeSuccess('Configuración guardada exitosamente.', false, function () { })
            }, 1000);

        }

        function retornoCancelar() {
            vm.creandoConfiguracionTerritorial = false;
            vm.creandoConfiguracionNacional = false;
            vm.deshabilitadoPorEdicion = false;
            if (vm.configuracionEnEdicion) {
                vm.configuracionEnEdicion.editable = false;
                vm.gridOptions.enableFiltering = true;
                vm.gridOptions.enableSorting = true;
                $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
                $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.COLUMN);
            }
        }
    };
    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').controller('configurarEntidadRolSectorController', configurarEntidadRolSectorController);
})();