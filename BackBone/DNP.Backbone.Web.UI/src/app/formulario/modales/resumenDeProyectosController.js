(function () {
    'use strict';

    resumenDeProyectosController.$inject = [
        'backboneServicios',
        'sesionServicios',
        'configurarEntidadRolSectorServicio',
        '$scope',
        'servicioResumenDeProyectos',
        'utilidades',
        'constantesCondicionFiltro',
        '$sessionStorage'
    ];

    function resumenDeProyectosController(
        backboneServicios,
        sesionServicios,
        configurarEntidadRolSectorServicio,
        $scope,
        servicioResumenDeProyectos,
        utilidades,
        constantesCondicionFiltro,
        $sessionStorage
    ) {
        var vm = this;
        vm.listaProyectoContraCredito = [];
        vm.esPrimer = true;
        vm.user = {};

        vm.idInstancia = $sessionStorage.idInstanciaIframe;


        vm.Plantillas = {
            accionesTemplate: 'src/app/formulario/modales/accionesTemplate.html'
        };

        //#region Métodos

        vm.listaConfiguracionesRolSector = obtenerConfiguracionesRolSector();
        vm.init = init;
        vm.obtenerRoles = obtenerRoles();

        vm.cambiarPrimer = cambiarPrimer;
        vm.cambiarSecundo = cambiarSecundo;

        vm.generarInstancias = generarInstancias;
        vm.eliminarProyectoTramite = eliminarProyectoTramite;
        vm.buscar = buscar;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;

        //#endregion

        vm.filtro = {
            bpin: {
                campo: 'ObjetoNegocioId',
                valor: null,
                tipo: constantesCondicionFiltro.contiene
            },
            nombreProyecto: {
                campo: 'NombreProyecto',
                valor: null,
                tipo: constantesCondicionFiltro.contiene
            }
        };

        function cambiarPrimer() {
            vm.esPrimer = true;
        }

        function cambiarSecundo() {
            vm.esPrimer = false;
        }


        const columnasDef = [{
            field: 'TipoProyecto',
            displayName: "TIPO",
            width: "15%",
            headerCellClass: 'ui-grid-cell-header',
            cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                if (grid.getCellValue(row, col) == 'Contracredito') {
                    return 'ui-grid-cell-align-left-bold ';
                }
                return 'ui-grid-cell-align-center';
                ;
            }
        },
        {
            field: 'NombreEntidad',
            displayName: "ENTIDAD",
            width: "25%",
            headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
            cellClass: 'ui-grid-cell-align-center'
        },
        {
            field: 'IdObjetoNegocio',
            displayName: "BPIN",
            headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
            cellClass: 'ui-grid-cell-align-center',
            width: "25%"
        },
        {
            field: 'NombreObjetoNegocio',
            displayName: "PROYECTO",
            width: "25%",
            headerCellClass: 'ui-grid-cell-header ui-grid-cell-align-center',
            cellClass: 'ui-grid-cell-align-center'
        },

        {
            field: 'accion',
            displayName: 'ACCIÓN',
            width: '10%',

            cellTemplate: vm.Plantillas.accionesTemplate,

            enableFiltering: false,
            enableHiding: false,
            enableSorting: false,
            enableColumnMenu: false,
            pinnedRight: true,
        }
        ];

        vm.gridOptions = {
            enableColumnMenus: false,
            enableSorting: false,
            paginationPageSizes: [5, 10, 25, 50, 100],
            paginationPageSize: 10,
            columnDefs: columnasDef,
            onRegisterApi: onRegisterApi,
            data: []
        };

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        function obtenerRoles() {
            var roles = sesionServicios.obtenerUsuarioIdsRoles();

            if (backboneServicios.estaAutorizado() && roles != null && roles.length > 0) {
                vm.peticion = {
                    IdUsuario: usuarioDNP,
                    IdObjeto: idTipoProyecto,
                    Aplicacion: nombreAplicacionBackbone,
                    ListaIdsRoles: roles
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

        async function init() {

            listaProyectos();
            return;
        }

        function listaProyectos() {
            const idTramite = vm.idInstancia;

            servicioResumenDeProyectos.obtenerProyectosPorTramite(vm.peticion, idTramite, vm.filtro).then(exito, error);

            function exito(respuesta) {
                if (respuesta.data && respuesta.data.ListaProyectos && respuesta.data.ListaProyectos.length) {
                    vm.gridOptions.data = respuesta.data.ListaProyectos;
                } else {
                    vm.gridOptions.data = [];
                }

            }

            function error(respuesta) {
                vm.gridOptions.data = [];
            }
        }

        function generarInstancias() {

            servicioResumenDeProyectos.generarInstancias(vm.peticion, vm.gridOptions.data).then(exito, error);

            function exito(respuesta) {
                //parent.postMessage("cerrarModal", window.location.origin);
                //utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
            }

            function error(respuesta) {
                utilidades.mensajeError("Error al realizar la Operación", false);
            }
        }

        function eliminarProyectoTramite(proyecto) {

            utilidades.mensajeWarning("El proyecto se excluirá y no aparecerá en la lista. Desea continuar?", function funcionContinuar() {

                servicioResumenDeProyectos.eliminarProyectoTramite(vm.peticion, proyecto.ProyectoTramiteId).then(exito, error);

                function exito(respuesta) {
                    listaProyectos();
                    utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                }

                function error(respuesta) {
                    utilidades.mensajeError("Error al realizar la Operación", false);
                }

            }, function funcionCancelar() {
                return;
            });


        }

        function buscar() {
            listaProyectos();
        }

        function limpiarCamposFiltro() {
            vm.filtro.bpin = null;
            vm.filtro.nombreProyecto = null;
        }
    }

    angular.module('backbone').controller('resumenDeProyectosController', resumenDeProyectosController);
})();