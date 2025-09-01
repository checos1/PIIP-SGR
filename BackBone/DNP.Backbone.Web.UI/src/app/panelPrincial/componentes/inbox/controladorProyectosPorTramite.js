(function () {
    'use strict';
    angular.module('backbone').controller('proyectosPorTramiteController', proyectosPorTramiteController);

    proyectosPorTramiteController.$inject = [
        '$scope',
        '$log',
        '$q',
        '$sessionStorage',
        '$timeout',
        '$location',
        '$routeParams',
        'FileSaver',
        'Blob',
        'servicioPanelPrincipal',
        'sesionServicios'
    ];

    function proyectosPorTramiteController(
        $scope,
        $log,
        $q,
        $sessionStorage,
        $timeout,
        $location,
        $routeParams,
        FileSaver,
        Blob,
        servicioPanelPrincipal,
        sesionServicios) {
        var vm = this;

        //variables
        vm.gridOptions;
        vm.plantillaProyectoBpin = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaProyectoBpin.html';
        vm.consultarAccionFlujoTemplate = 'src/app/panelPrincial/componentes/inbox/plantillas/plantillaConsultarAccionFlujo.html';
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
        vm.consultarAccion = consultarAccion;

        //Métodos
        vm.init = init;

        function downloadExcel(){
            var peticionObtenerInbox = {
                IdUsuario: usuarioDNP,
                IdObjeto: idTipoProyecto,
                Aplicacion: nombreAplicacionBackbone,
                ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles()
            };
            servicioPanelPrincipal.obtenerExcelTramitesProyectos(peticionObtenerInbox,  $routeParams.id).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob,"ProyectosDelTramites.xls");
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
            servicioPanelPrincipal.obtenerPdfInboxProyectosTramites(peticionObtenerInbox, $routeParams.id).then(
                function (data) {
                    servicioPanelPrincipal.imprimirPdfProyectosTramites(data.data).then(function (retorno) {
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
                        field: 'accionFlujo',
                        displayName: 'Nombre/Accion Flujo',
                        enableHiding: false,
                        enableColumnMenu: false,
                        width: '20%',
                        cellTemplate: vm.consultarAccionFlujoTemplate,
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
            const idInstancia = $routeParams.id;

            servicioPanelPrincipal.obtenerProyectosPorTramite(vm.peticionObtenerInbox, idInstancia).then(exito, error);

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

        function consultarAccion(idInstancia, nombreProyecto, idObjetoNegocio, idEntidad) {

            $sessionStorage.idInstancia = $sessionStorage.idInstanciaFlujoPrincipal = idInstancia;
            $sessionStorage.nombreProyecto = nombreProyecto;
            $sessionStorage.idObjetoNegocio = idObjetoNegocio;
            $sessionStorage.idEntidad = idEntidad;
            $timeout(function () {
                $location.path('/Acciones/ConsultarAcciones');
            }, 300);
        }
    }

    angular.module('backbone')
        .component('proyectosPorTramite', {
            templateUrl: "/src/app/panelPrincial/componentes/inbox/proyectosTramite.template.html",
            controller: 'proyectosPorTramiteController',
            controllerAs: 'vm'
        });

})();