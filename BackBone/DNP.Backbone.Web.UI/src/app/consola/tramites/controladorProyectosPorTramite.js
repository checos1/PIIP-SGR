(function () {
    'use strict';
    angular.module('backbone').controller('proyectosPorTramiteConsolaController', proyectosPorTramiteConsolaController);

    proyectosPorTramiteConsolaController.$inject = [
        '$scope',
        '$log',
        '$q',
        '$routeParams',
        'FileSaver',
        'Blob',
        'servicioConsolaTramites',
        'sesionServicios'
    ];

    function proyectosPorTramiteConsolaController(
        $scope,
        $log,
        $q,
        $routeParams,
        FileSaver,
        Blob,
        servicioConsolaTramites,
        sesionServicios) {
        var vm = this;

        //variables
        vm.gridOptions;
        vm.plantillaProyectoBpin = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaProyectoBpin.html';
        vm.downloadPdf = downloadPdf;
        vm.downloadExcel = downloadExcel;
        vm.peticionObtenerInbox = {
            // ReSharper disable once UndeclaredGlobalVariableUsing
            IdUsuario: usuarioDNP,
            IdObjeto: idTipoProyecto,
            // ReSharper disable once UndeclaredGlobalVariableUsing
            Aplicacion: nombreAplicacionBackbone,
            ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
        };
        vm.mostrarMensaje = false;
        vm.Mensaje;
        vm.descripcionTramite = '';

        //Métodos
        vm.init = init;

        function downloadExcel(){
            var peticionObtenerInbox = {
                IdUsuario: usuarioDNP,
                IdObjeto: idTipoProyecto,
                Aplicacion: nombreAplicacionBackbone,
                ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
            };
            servicioConsolaTramites.obtenerExcelProyectosTramites(peticionObtenerInbox,  $routeParams.id).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob,"ConsolaProyectosDelTramites.xls");
            }, function(error) {
                vm.Mensaje = error.data;
                mostrarMensajeRespuesta();
            });
        }

        function downloadPdf() {
            var peticionObtenerInbox = {
                IdUsuario: usuarioDNP,
                IdObjeto: idTipoProyecto,
                Aplicacion: nombreAplicacionBackbone,
                ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
            };
            servicioConsolaTramites.obtenerPdfProyectosTramites(peticionObtenerInbox, $routeParams.id).then(
                function (data) {
                    servicioConsolaTramites.imprimirPdfProyectosTramites(data.data).then(function (retorno) {
                        var blob = new Blob([retorno.data], {
                            type: "application/octet-stream"
                        });
                        FileSaver.saveAs(blob,nombreDelArchivo(retorno));                        
                    });
                }, function(error) {
                    vm.Mensaje = error.data;
                    mostrarMensajeRespuesta()
                });
        };

        function nombreDelArchivo(response) {
            var filename = "";
            var disposition = response.headers("content-disposition");
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) {
                    filename = matches[1].replace(/['"]/g, '');
                }
            }
            return filename;
        }

        function init() {
            if (!vm.gridOptions) {

                vm.gridOptions = {
                    enableColumnMenus: false,
                    paginationPageSizes: [5, 10, 25, 50, 100],
                    paginationPageSize: 5,
                    onRegisterApi: onRegisterApi
                }

                vm.gridOptions.columnDefs = [{
                        field: 'TipoProyecto',
                        displayName: "Tipo Proyecto",
                        width: "15%"
                    },
                    {
                        field: 'SectorNombre',
                        displayName: "Sector",
                        width: "10%"
                    },
                    {
                        field: 'NombreEntidad',
                        displayName: "Entidad",
                        width: "20%"
                    },
                    {
                        field: 'NombreObjetoNegocio',
                        displayName: "Proyecto/ BPIN",
                        width: "30%",
                        cellTemplate: vm.plantillaProyectoBpin,
                        cellClass: function(grid, row, col, rowRenderIndex, colRenderIndex) {
                            return 'ui-grid-description';
                        }                        
                    },
                    {
                        field: 'Operacion',
                        displayName: "Operacion",
                        width: "10%"
                    },
                    {
                        field: 'ValorTotal',
                        displayName: "Valor",
                        width: "15%",
                        type: "number",
                        cellFilter: 'currency:""'
                    }
                ];

                vm.gridOptions.data = [];
                listaProyectos();
            }
        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        function listaProyectos() {
            // chamar o serviço do backend para popular a lista
            const idTramite = $routeParams.id;

            servicioConsolaTramites.obtenerProyectosPorTramite(vm.peticionObtenerInbox, idTramite).then(exito, error);

            function exito(respuesta) {
                if (respuesta.data && respuesta.data.ListaProyectos && respuesta.data.ListaProyectos.length) {
                    vm.gridOptions.data = respuesta.data.ListaProyectos;
                    vm.descripcionTramite = respuesta.data.ListaProyectos[0].TramiteDto.Descripcion;
                } else {
                    vm.gridOptions.data = [];
                    vm.descripcionTramite = respuesta.data.NombreTramite;
                }

            }

            function error(respuesta) {
                vm.gridOptions.data = [];
            }
        }

        function mostrarMensajeRespuesta() {
            // ReSharper disable once UndeclaredGlobalVariableUsing
            if (vm.Mensaje) {
                vm.mostrarMensaje = true;
            } else {
                vm.mostrarMensaje = false;
            }
        }
    }

    angular.module('backbone')
        .component('proyectosPorTramiteConsola', {
            templateUrl: "/src/app/consola/tramites/proyectosTramite.template.html",
            controller: 'proyectosPorTramiteConsolaController',
            controllerAs: 'vm'
        });

})();