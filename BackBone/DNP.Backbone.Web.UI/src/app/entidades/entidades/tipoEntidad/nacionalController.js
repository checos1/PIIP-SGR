(function () {
    'use strict';
    angular.module('backbone.entidades').controller('nacionalController', nacionalController);

    nacionalController.$inject = [
        '$scope',
        'servicioEntidades',
        'constantesAutorizacion',
        '$uibModal',
        'backboneServicios'
    ];

    function nacionalController($scope, servicioEntidades, constantesAutorizacion, $uibModal, backboneServicios) {
        var vm = this;

        /// Plantilla de acciones
        vm.plantillaAccionesTablaCabeza = 'src/app/entidades/entidades/tipoEntidad/plantillas/plantillaAccionesTablaCabeza.html';
        vm.plantillaAccionesTabla = 'src/app/entidades/entidades/tipoEntidad/plantillas/plantillaAccionesTabla.html';
        vm.plantillaEntidadesTemplate = 'src/app/entidades/entidades/tipoEntidad/plantillas/plantillaEntidadesTemplate.html';
        vm.plantillaSubGrid = 'src/app/entidades/entidades/tipoEntidad/plantillas/plantillaSubGrid.html';

        vm.accionCrearEditarEntidad = accionCrearEditarEntidad;
        vm.accionEliminarEntidad = accionEliminarEntidad;
        vm.accionCrearSubEntidad = accionCrearSubEntidad;
        vm.accionEditarSubEntidad = accionEditarSubEntidad;
        vm.accionFlujosViabilidad = accionFlujosViabilidad;

        vm.subGridEvento = subGridEvento;
        vm.listarEntidades = listarEntidades;
        vm.limparFiltro = limparFiltro;
        vm.listaEntidadesOrigem;
        vm.columnDefPrincial = [{
            field: 'Entidad',
            displayName: 'Entidad',
            enableHiding: false,
            width: '85%',
            cellTemplate: vm.plantillaEntidadesTemplate
        }, {
            field: 'accion',
            displayName: 'Acción',
            headerCellClass: 'text-center',
            enableFiltering: false,
            enableHiding: false,
            enableSorting: false,
            enableColumnMenu: false,
            cellTemplate: vm.plantillaAccionesTablaCabeza,
            width: '13%'
            }];


        vm.columnDef = [{
                field: 'Entidad',
                displayName: 'Sub-Entidad',
                enableHiding: false,
                width: '90%',
                cellTemplate: vm.plantillaEntidadesTemplate
            },
            {
                field: 'accion',
                displayName: 'Acción',
                headerCellClass: 'text-center',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                cellTemplate: vm.plantillaAccionesTabla,
                width: '9%'
            }
        ];

        // grid main
        vm.gridOptions;

        vm.consultarPermiso = backboneServicios.consultarPermiso;

       
        function subGridEvento(gridApi) {
            gridApi.expandable.on.rowExpandedStateChanged($scope, function (row) {
                if (row.isExpanded) {
                    servicioEntidades.obtenerSubEntidadesPorIdEntidad(row.entity.IdEntidad)
                        .then(function (response) {
                            if (response.data != null)
                                row.entity.subGridOptions.data = response.data;
                        });
                }
            });

            gridApi.expandable.on.rowExpandedBeforeStateChanged($scope, function (row) {
                $scope.vm.gridOptions.expandableRowHeight = 90 + row.entity.SubEntidades.length * 30;
            })
        }

        /// Comienzo
        vm.$onInit = function () {
            if (!vm.gridOptions) {
                vm.gridOptions = {
                    enableSorting: true,
                    columnDefs: vm.columnDefPrincial,
                    expandableRowTemplate: vm.plantillaSubGrid,
                    expandableRowScope: {
                        subGridVariable: 'subGridScopeVariable'
                    },
                    //     expandableRowHeight: '80px',
                    enableOnDblClickExpand: false,
                    enableColumnResizing: false,
                    showGridFooter: false,
                    enablePaginationControls: true,
                    useExternalPagination: false,
                    useExternalSorting: false,
                    paginationCurrentPage: 1,
                    enableVerticalScrollbar: 1,
                    enableFiltering: false,
                    showHeader: false,
                    useExternalFiltering: false,
                    paginationPageSizes: [5, 10, 15, 25, 50, 100],
                    paginationPageSize: 10,
                    onRegisterApi: subGridEvento
                };

                listarEntidades();
            }
        }

        /// Getters
        function listarEntidades(entidadeFiltro, cabezaSector, actualizar, sigla, codigo) {
            //console.log(filtro);

            if (vm.listaEntidadesOrigem && !actualizar) {
                if (entidadeFiltro && !cabezaSector && !sigla && !codigo)
                    vm.gridOptions.data = vm.listaEntidadesOrigem.filter(x => x.NombreCompleto.indexOf(entidadeFiltro) != -1);
                else if (entidadeFiltro && cabezaSector && !sigla && !codigo)
                    vm.gridOptions.data = vm.listaEntidadesOrigem.filter(x => x.NombreCompleto.indexOf(entidadeFiltro) != -1 && x.CabezaSector == cabezaSector);
                else if (!entidadeFiltro && cabezaSector && !sigla && !codigo)
                    vm.gridOptions.data = vm.listaEntidadesOrigem.filter(x => x.CabezaSector == cabezaSector);
                else if (!entidadeFiltro && !cabezaSector && sigla && !codigo)
                    vm.gridOptions.data = vm.listaEntidadesOrigem.filter(x => x.Sigla == sigla);
                else if (!entidadeFiltro && !cabezaSector && !sigla && codigo)
                    vm.gridOptions.data = vm.listaEntidadesOrigem.filter(x => x.Codigo == codigo);
                else
                    vm.gridOptions.data = vm.listaEntidadesOrigem;

            } else {
                servicioEntidades.obtenerEntidadesPorTipo('Nacional')
                    .then(function (response) {
                        vm.gridOptions.data = response.data;
                        if (response.data != null && response.data.length > 0) {
                            vm.gridOptions.data.forEach(item => {
                                item.subGridOptions = {
                                    columnDefs: vm.columnDef,
                                    appScopeProvider: $scope,
                                    paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                    paginationPageSize: 5,
                                    disableRowExpandable: !item.TieneHijo,
                                    data: []
                                }
                            });
                        }
                        vm.listaEntidadesOrigem = vm.gridOptions.data;
                        $scope.$parent.vm.obtenerSubEntidades();
                        $scope.$parent.vm.actualizarListaEntidadesFiltro();
                    });
            }
        }

        function limparFiltro() {
            vm.gridOptions.data = vm.listaEntidadesOrigem;
        }

        function accionCrearEditarEntidad(row) {
            vm.accionCrearEditarEntidad(row);
        }

        function accionEliminarEntidad(row) {
            vm.accionEliminarEntidad(row);
        }

        function accionCrearSubEntidad(row) {
            vm.accionCrearSubEntidad(row);
        }

        function accionEditarSubEntidad(row) {
            vm.accionEditarSubEntidad(row);
        }

        function accionFlujosViabilidad(row) {
            vm.accionFlujosViabilidad(row);
        }

        $scope.$parent.vmChildren.length = 0;
        $scope.$parent.vmChildren.push(vm);
    }

    angular.module('backbone.entidades')
        .component('nacional', {
            templateUrl: "/src/app/entidades/entidades/tipoEntidad/tabla.template.html",
            controller: 'nacionalController',
            controllerAs: 'vm',
            bindings: {
                accionCrearEditarEntidad: '&',
                accionEliminarEntidad: '&',
                accionCrearSubEntidad: '&',
                accionEditarSubEntidad: '&',
                accionFlujosViabilidad: '&',
            }
        });
})();