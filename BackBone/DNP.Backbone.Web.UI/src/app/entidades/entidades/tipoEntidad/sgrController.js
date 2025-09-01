(function () {
    'use strict';
    angular.module('backbone.entidades').controller('sgrController', sgrController);

    sgrController.$inject = [
        '$scope',
        'servicioEntidades',
        'constantesAutorizacion',
        '$uibModal',
        '$sce',
        '$location',
        'backboneServicios'
    ];


    function sgrController($scope, servicioEntidades, constantesAutorizacion, $uibModal, $sce, $location, backboneServicios) {

        var vm = this;
        vm.consultarPermiso = backboneServicios.consultarPermiso;

        // Acciones
        vm.accionCrearSubEntidade = accionCrearSubEntidade;
        vm.accionFlujosViabilidad = accionFlujosViabilidad;
        vm.accionInflexibilidades = accionInflexibilidades;
        vm.accionEditar = accionEditar;
        vm.accionEliminar = accionEliminar;
        vm.accionAdherencia = accionAdherencia;
        vm.accionDelegado = accionDelegado;

        /// Plantilla de acciones
        vm.plantillaAccionesTablaCabeza = 'src/app/entidades/entidades/tipoEntidad/plantillas/plantillaAccionesTablaCabeza.html';
        vm.plantillaAccionesTabla = 'src/app/entidades/entidades/tipoEntidad/plantillas/plantillaAccionesTabla.html';
        vm.plantillaEntidadesTemplate = 'src/app/entidades/entidades/tipoEntidad/plantillas/plantillaEntidadesTemplate.html';
        vm.plantillaSubGrid = 'src/app/entidades/entidades/tipoEntidad/plantillas/plantillaSubGrid.html';
        vm.plantillaPopoverSgr = 'src/app/entidades/entidades/tipoEntidad/plantillas/plantillaPopoverSgr.html';

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
            cellTemplate: vm.plantillaPopoverSgr,
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
        }];

        // grid main
        vm.gridOptions;

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

        vm.popOverOptions = {
            isOpen: false,
            templateUrl: 'popover.html',
            //toggle: function () {
            //    vm.popOverOptions.isOpen = !vm.popOverOptions.isOpen;
            //}
        };

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
                    enableColumnResizing: false,
                    enableOnDblClickExpand: false,
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
                    onRegisterApi: subGridEvento,
                };
                listarEntidades();
            }
        }

        /// Getters
        function listarEntidades(entidadeFiltro, cabezaSector, actualizar) {
            //console.log(filtro);

            if (vm.listaEntidadesOrigem && !actualizar) {
                if (entidadeFiltro)
                    vm.gridOptions.data = vm.listaEntidadesOrigem.filter(x => x.NombreCompleto.indexOf(entidadeFiltro) != -1);
                else
                    vm.gridOptions.data = vm.listaEntidadesOrigem;

            } else {
                servicioEntidades.obtenerEntidadesPorTipo('SGR')
                    .then(function (response) {
                        vm.gridOptions.data = response.data;
                        if (response.data != null && response.data.length > 0) {
                            vm.gridOptions.data.forEach(item => {
                                item.subGridOptions = {
                                    columnDefs: vm.columnDef,
                                    appScopeProvider: $scope,
                                    paginationPageSizes: [5, 10, 15, 25, 50, 100],
                                    paginationPageSize: 10,
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

        /// Acciones
        function accionFlujosViabilidad({ $row }) {
            //vm.popOverOptions.toggle();

            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/flujos-viabilidad/flujosViabilidadModal.html',
                controller: 'flujosViabilidadModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-flujos",
                resolve: {
                    entidad: {
                        IdEntidad: $row.IdEntidad,
                        NombreCompleto: $row.NombreCompleto
                    }
                },
            }).result.then(() => { }, () => { });
        }
        function accionInflexibilidades({ $row }) {
            $location.path(`/entidad/inflexibilidades/${$row.IdEntidad}`)
                .search({ params: $row.NombreCompleto });
        }
        function accionAdherencia({ $row }) {
            //vm.popOverOptions.toggle();

            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/adherencia/adherenciaModal.html',
                controller: 'adherenciaModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    entidad: {
                        IdEntidad: $row.IdEntidad,
                        NombreCompleto: $row.NombreCompleto
                    }                    
                },
            });
        }
        function accionDelegado({ $row }) {
            //vm.popOverOptions.toggle();

            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/entidades/entidades/tipoEntidad/entidad-sgr/modales/delegado/delegadoModal.html',
                controller: 'delegadoModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-delegado",
                resolve: {
                    entidad: {
                        IdEntidad: $row.IdEntidad,
                        NombreCompleto: $row.NombreCompleto
                    }
                },
    });
}

       
function limparFiltro() {
    vm.gridOptions.data = vm.listaEntidadesOrigem;
}

function accionEditar(row) {
    vm.accionCrearEditarEntidad(row);
}

function accionEliminar(row) {
    vm.accionEliminarEntidad(row);
}

function accionCrearSubEntidade(row) {
   // console.log('crear');
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
    .component('sgr', {
        templateUrl: "/src/app/entidades/entidades/tipoEntidad/tabla.template.html",
        controller: 'sgrController',
        controllerAs: 'vm',
        bindings: {
            accionCrearEditarEntidad: '&',
            accionEliminarEntidad: '&',
            accionCrearSubEntidad: '&',
            accionEditarSubEntidad: '&',
            accionFlujosViabilidad: '&',
        }
    });
}) (); 
