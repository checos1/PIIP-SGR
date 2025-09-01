(function () {
    "use strict";
    angular.module('backbone').controller('seleccionarProyectosCtrl', seleccionarProyectosCtrl);

    seleccionarProyectosCtrl.$inject = ["$uibModal", "$scope", "$uibModalInstance", "$filter", 'flujoServicios', 'appSettings', 'proyectos', 'nombreOpcion', 'idsEntidadesMga', 'idOpcionDnp', '$sessionStorage',];

    // ReSharper disable once InconsistentNaming
    function seleccionarProyectosCtrl($uibModal, $scope, $uibModalInstance, $filter, flujoServicios, appSettings, proyectos, nombreOpcion, idsEntidadesMga, idOpcionDnp, $sessionStorage) {
        var vm = this;

        /* Métodos */
        vm.continuar = continuar;
        vm.cancelar = cancelar;

        /* Variables */
        vm.instanciaModal = $uibModalInstance;
        vm.lang = 'es';
        vm.listaDeProyectos = proyectos || [];
        vm.totalRegistros = vm.listaDeProyectos.length;
        vm.numeroPorPagina = appSettings.topePaginacion;
        vm.maximoPaginas = 10;
        vm.mensajeResultado = '';
        vm.nombreOpcion = nombreOpcion;
        vm.idsEntidadesMga = idsEntidadesMga;
        vm.idOpcionDnp = idOpcionDnp;

        vm.filtroSectorId;
        vm.filtroEntidadId;
        vm.filtroCodigoBpin;
        vm.filtroProyectoNombre;
        vm.filtroEstado;
        vm.filtroTieneInstancia;

        vm.listaFiltroSectores = [];
        vm.listaFiltroEntidads = [];
        vm.listaFiltroCodigoBpin = [];
        vm.listaFiltroProyectoNombre = [];
        vm.listaFiltroEstados = [];

        vm.buscar = buscar;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.abrirModalInstanciasProyecto = abrirModalInstanciasProyecto;

        var accionesTemplate = '<div class="text-center"> <button type="button" class="btn btn-default"  ng-click="grid.appScope.vm.continuar(row.entity)" tooltip-placement="bottom" uib-tooltip="Crear Instancia"><span class="icon-instance"></span></button></div>';
        var instanciaTemplate = 'src/app/comunes/templates/modales/seleccionarProyectos/plantillas/plantillaInstanciaProyecto.html';
        
        ////////////
        
        vm.grillaProyectos = {
            rowHeight: 36,
            enableRowSelection: false,
            enableRowHeaderSelection: false,
            rowSelection: false,
            noUnselect: false,
            multiSelect: false,
            enableVerticalScrollbar: 0,
            showGridFooter: true,            
            gridFooterTemplate:
            '<div class="ui-grid-cell-contents" ng-show="true"><strong id="idLabelTotalRegistros">Total registros: {{grid.appScope.vm.filasFiltradas}} / {{grid.appScope.vm.totalRegistros}}.<strong></div>',
            enableFiltering: true,
            totalItems: vm.listaDeProyectos.length,
            paginationPageSize: vm.numeroPorPagina,
            minRowsToShow: vm.listaDeProyectos.length < vm.numeroPorPagina ? vm.listaDeProyectos.length : vm.numeroPorPagina,
            enablePaginationControls: false,
            paginationCurrentPage: 1,            
            columnDefs: [
                {
                    displayName: 'Sector',
                    field: 'SectorNombre',
                    enableSorting: true,
                    width: "20%"
                },
                {
                    displayName: 'Entidad',
                    field: 'EntidadNombre',
                    enableSorting: true,
                    width: "15%"
                },
                {
                    displayName: 'Codigo BPIN',
                    field: 'CodigoBpin',
                    enableSorting: true,
                    width: "15%"
                },
                {
                    displayName: 'Proyecto',
                    field: 'ProyectoNombre',
                    enableSorting: true,
                    width: "20%"
                },
                {
                    displayName: 'Estado',
                    field: 'Estado',
                    enableSorting: true,
                    width: "10%"
                },
                {
                    name: 'InstanciaEnProceso',
                    displayName: 'Instancia en proceso',
                    enableSorting: false,
                    enableFiltering: false,
                    width: "10%",
                    cellTemplate: instanciaTemplate
                },
                {
                    name: 'Acciones',
                    displayName: 'Acciones',
                    enableSorting: false,
                    enableFiltering: false,
                    width: "10%",
                    cellTemplate: accionesTemplate
                }
            ],
            data: vm.listaDeProyectos,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;

                gridApi.core.on.rowsRendered($scope, function () {
                    var activas = $scope.gridApi.grid.getVisibleRows();
                    vm.filasFiltradas = activas.length;
                });    
            }

        };

        this.$onInit = function () {
            listarSectores();
            listarEntidads();
            listarCodigoBpin();
            listarProyectoNombre();
            listarEstados();
            vm.mensajeResultado = vm.listaDeProyectos.length > 0 ? $filter('language')('TextoNoResultadosBusqueda') : $filter('language')('NoHayProyectosDisponibles');
        }

        function continuar(proyectoSeleccionado) {
            vm.instanciaModal.close(proyectoSeleccionado);
        }

        function cancelar() {
            $uibModalInstance.dismiss(null);
        }; 

        function buscar() {

            var IdFlujo = $sessionStorage.IdFlujo
            var TipoTramiteId = $sessionStorage.TipoTramiteId

            flujoServicios.obtenerProyectosPorEntidadesyEstados(idsEntidadesMga, vm.idOpcionDnp, null, vm.filtroSectorId,
                vm.filtroEntidadId, vm.filtroCodigoBpin, vm.filtroProyectoNombre, vm.filtroTieneInstancia, IdFlujo, TipoTramiteId).then(exito, error);

            function exito(proyectos) {
                
                if (proyectos !== undefined) {
                    if (Array.isArray(proyectos)) {
                        vm.listaDeProyectos = proyectos;
                    } else {
                        vm.listaDeProyectos = [];
                    }
                    vm.grillaProyectos.data = vm.listaDeProyectos;
                    vm.filasFiltradas = vm.listaDeProyectos.length > 0;
                    vm.totalRegistros = vm.listaDeProyectos.length;
                }
            }

            function error() {

            }

        }

        function limpiarCamposFiltro() {
            vm.filtroSectorId = null;
            vm.filtroEntidadId = null;
            vm.filtroCodigoBpin = null;
            vm.filtroProyectoNombre = null;
            vm.filtroEstado = null;
            vm.filtroTieneInstancia = null;
        }

        function listarSectores() {
            const sectores = proyectos.map(item => {
                const obj = {};
                
                obj.Id = item.SectorId;
                obj.Name = item.SectorNombre;

                return obj;
            });

            vm.listaFiltroSectores = sectores.filter((v, i, a) => a.findIndex(t => (t.Id === v.Id)) === i);
        }

        function listarEntidads() {
            const entidads = proyectos.map(item => {
                const obj = {};
                
                obj.Id = item.EntidadId;
                obj.Name = item.EntidadNombre;

                return obj;
            });

            vm.listaFiltroEntidads = entidads.filter((v, i, a) => a.findIndex(t => (t.Id === v.Id)) === i);
        }

        function listarCodigoBpin() {
            const codBpins = proyectos.map(item => {
                const obj = {};
                if (item.CodigoBpin) {
                    obj.Id = item.CodigoBpin;
                    obj.Name = item.CodigoBpin;
                }
                return obj;
            }).filter(x => x.Id);

            vm.listaFiltroCodigoBpin = codBpins.filter((v, i, a) => a.findIndex(t => (t.Id === v.Id)) === i);
        }

        function listarProyectoNombre() {
            const proyectoNombres = proyectos.map(item => {
                const obj = {};
                if (item.ProyectoNombre) {
                    obj.Id = item.ProyectoNombre;
                    obj.Name = item.ProyectoNombre;
                }
                return obj;
            }).filter(x => x.Id);

            vm.listaFiltroProyectoNombre = proyectoNombres.filter((v, i, a) => a.findIndex(t => (t.Id === v.Id)) === i);
        }

        function listarEstados() {
            const estados = proyectos.map(item => {
                const obj = {};

                obj.Id = item.Estado;
                obj.Name = item.Estado;

                return obj;
            });

            vm.listaFiltroEstados = estados.filter((v, i, a) => a.findIndex(t => (t.Id === v.Id)) === i);
        }

        function abrirModalInstanciasProyecto(entity = null) {
            
            let objProyecto = {
                bpin: entity.CodigoBpin,
                tipoEntidad: null
            }

            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/comunes/templates/modales/seleccionarProyectos/modales/modalInstanciasSeleccionarProyecto.html',
                controller: 'modalInstanciasSeleccionarProyectoController',
                controllerAs: "vm",
                openedClass: "consola-modal-instancias",
                size: 'lg',
                resolve: {
                    objProyecto: objProyecto
                }
            });

            modalInstance.result.then(function (selectedItem) {


            }, function () {

            });

        }

        
    }    

}());
