(function () {
    "use strict";
    angular.module('backbone').controller('seleccionarProyectosCtrlSGR', seleccionarProyectosCtrlSGR);

    seleccionarProyectosCtrlSGR.$inject = [
        '$scope',
        '$uibModalInstance',
        '$filter',
        'flujoServicios',
        'appSettings',
        'proyectos',
        'nombreOpcion',
        'idsEntidadesMga',
        'idOpcionDnp',
        'utilidades'
    ];

    // ReSharper disable once InconsistentNaming
    function seleccionarProyectosCtrlSGR(
        $scope,
        $uibModalInstance,
        $filter,
        flujoServicios,
        appSettings,
        proyectos,
        nombreOpcion,
        idsEntidadesMga,
        idOpcionDnp,
        utilidades
    ) {
        var vm = this;

        /* Métodos */
        vm.continuar = continuar;
        vm.cerrar = cancelar;
        vm.obtenerIconoBoton = obtenerIconoBoton;

        /* Variables */
        vm.instanciaModal = $uibModalInstance;
        vm.lang = 'es';
        vm.listaDeProyectos = proyectos || [];
        vm.totalRegistros = vm.listaDeProyectos.length;
        vm.numeroPorPagina = appSettings.topePaginacion;
        /*        vm.numeroPorPagina = 1;*/
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

        var accionesTemplate = '<div class="text-center"> <button type="button" class="btn" ng-click="grid.appScope.vm.continuar(row.entity)" uib-tooltip={{row.entity.NombreFlujo}} tooltip-placement="bottom"><span><img style="width: 20px;height:20px" src={{grid.appScope.vm.obtenerIconoBoton(row.entity)}} /></span></button></div>';
        var instanciaTemplate = 'src/app/comunes/templates/modales/seleccionarProyectos/plantillas/plantillaInstanciaProyecto.html';
        var proyetoTemplate = '<div class=scrollable>{{row.entity.ProyectoNombre}}</div>';

        vm.grillaProyectos = {
            rowHeight: 36,
            enableRowSelection: false,
            enableRowHeaderSelection: false,
            rowSelection: false,
            noUnselect: false,
            multiSelect: false,
            enableVerticalScrollbar: true,
            showGridFooter: false,
            //gridg:
            //'<div class="ui-grid-cell-contents" ng-show="true"><strong id="idLabelTotalRegistros">Total registros: {{grid.appScope.vm.filasFiltradas}} / {{grid.appScope.vm.totalRegistros}}.<strong></div>',
            enableFiltering: true,
            totalItems: vm.listaDeProyectos.length,
            paginationPageSize: vm.numeroPorPagina,
            minRowsToShow: vm.listaDeProyectos.length < vm.numeroPorPagina ? vm.listaDeProyectos.length : vm.numeroPorPagina,
            enablePaginationControls: false,
            paginationCurrentPage: 1,
            columnDefs: [
                {
                    name: '.',
                    displayName: '',
                    enableSorting: false,
                    enableFiltering: false,
                    width: "5%",
                    enableColumnMenu: false,
                    headerCellClass: 'ui-grid-top-panel'
                },
                {
                    displayName: 'ID',
                    field: 'ProyectoId',
                    enableSorting: true,
                    width: "10%",
                    enableColumnMenu: false,
                    headerCellClass: 'ui-grid-top-panel'
                },
                {
                    displayName: 'Código BPIN',
                    field: 'CodigoBpin',
                    enableSorting: true,
                    width: "15%",
                    enableColumnMenu: false,
                    headerCellClass: 'ui-grid-top-panel'
                },
                {
                    displayName: 'Proyecto',
                    field: 'ProyectoNombre',
                    enableSorting: true,
                    width: "30%",
                    enableVerticalScrollbar: true,
                    enableColumnMenu: false,
                    headerCellClass: 'ui-grid-top-panel',
                    cellClass: 'ui-grid-cell',
                    cellTemplate: proyetoTemplate,
                },
                {
                    displayName: 'Entidad que presenta',
                    field: 'EntidadNombre',
                    enableSorting: true,
                    width: "30%",
                    enableColumnMenu: false,
                    headerCellClass: 'ui-grid-top-panel',
                    cellClass: 'ui-grid-cell'
                },
                {
                    name: 'Acciones',
                    displayName: 'Acciones',
                    enableSorting: false,
                    enableFiltering: false,
                    width: "10%",
                    cellTemplate: accionesTemplate,
                    enableColumnMenu: false,
                    headerCellClass: 'ui-grid-top-panel'
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
            inicializarVariables(vm.nombreOpcion);
            vm.mensajeResultado = vm.listaDeProyectos.length > 0 ? $filter('language')('TextoNoResultadosBusqueda') : $filter('language')('NoHayProyectosDisponibles');
        };

        function inicializarVariables(nombreOpcion) {

            switch (nombreOpcion) {
                case "Aprobación de proyectos SGR":
                    vm.flujoNombre = 'aprobación';
                    break;
                case "priorización de proyectos SGR":
                    vm.flujoNombre = 'priorización';
                    break;
                case "Designación de ejecutor SGR":
                    vm.flujoNombre = 'designación';
                    break;
                default:
                    console.log( "nombre de opcion desconocido.");
            }

            if (vm.flujoNombre === 'aprobación')
                vm.articulo = '4.4.2';
            else if (vm.flujoNombre === 'priorización')
                vm.articulo = '4.4.1';
        };

        function continuar(proyectoSeleccionado) {
            if (proyectoSeleccionado.PermiteCrearInstancia == 2) {
                utilidades.mensajeError('Ya existe un proceso de ' + vm.flujoNombre + ' en curso y activo para la entidad/instancia ' + proyectoSeleccionado.EntidadProcesoNombre);
                return;
            }

            if (proyectoSeleccionado.PermiteCrearInstancia == 0) {
                utilidades.mensajeError('En estos momentos tal ejercicio debe ser completado por la entidad/instancia '
                    + proyectoSeleccionado.EntidadProceso + ' que corresponde a ' + proyectoSeleccionado.TurnoActual + ' cofinanciadores.'
                    , null
                    , 'Según la definición del orden establecido en el artículo ' + vm.articulo + ' del Acuerdo Único del SGR, aún no le corresponde a la entidad/instancia realizar el proceso de ' + vm.flujoNombre + '.'
                    , null);
                return;
            }

            let mensaje = "El proceso será creado."
            if (vm.flujoNombre === 'priorización') mensaje = "El proceso de priorización será creado."

            if (vm.flujoNombre === 'priorización') {
                mensaje = 'El subproceso de priorización del<br/>proyecto para la metodología seleccionada será creado. Para iniciar su diligenciamiento, usted podrá acceder a él desde “Mis Procesos”.';
            } else {
                mensaje = mensaje + " Para iniciar su diligenciamiento, usted podrá acceder a él desde mis procesos o la consola de procesos."
            } 


            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                return flujoServicios.TieneInstanciaActiva(proyectoSeleccionado.CodigoBpin).then(
                    function (resultado) {
                        if (!resultado.data)
                            vm.instanciaModal.close(proyectoSeleccionado);
                        else {
                            utilidades.mensajeError('Ya existe un proceso de ' + vm.flujoNombre + ' en curso y activo.');
                            return;
                        }
                    }
                );
            }, function funcionCancelar(reason) {
                return;
            }, null, null, mensaje);
        };

        function obtenerIconoBoton(proyectoSeleccionado) { 
            if (proyectoSeleccionado.PermiteCrearInstancia == 0)
                return 'Img/u72.svg';

            if (proyectoSeleccionado.PermiteCrearInstancia == 1)
                return 'Img/u70.svg';

            if (proyectoSeleccionado.PermiteCrearInstancia == 2)
                return 'Img/u71.svg';
        };

        function cancelar() {
            $uibModalInstance.close(false);
        };
    };

}());
