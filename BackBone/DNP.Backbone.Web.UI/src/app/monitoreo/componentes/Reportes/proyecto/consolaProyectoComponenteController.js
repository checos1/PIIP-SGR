(function () {
    "use strict";
    angular
        .module("backbone")
        .controller(
            "consolaProyectoComponenteController",
            consolaProyectoComponenteController
        );

    consolaProyectoComponenteController.$inject = [
        "$scope",
        "$filter",
        "sesionServicios",
        "backboneServicios",
        "servicioConsolaMonitoreo",
        '$routeParams',
        'FileSaver',
        'Blob'
    ];

    function consolaProyectoComponenteController(
        $scope,
        $filter,
        sesionServicios,
        backboneServicios,
        servicioConsolaMonitoreo,
        $routeParams,
        FileSaver,
        Blob
    ) {
        var vm = this;
        //#region  variables
        vm.gridOptions;
        vm.Mensaje = "";        
        vm.plantillaProyectoReportes = "src/app/monitoreo/plantillas/plantillaProyecto.html";
        vm.peticion = obtenerPeticion();
        vm.IdProyecto = $routeParams.proyectoid;
        vm.CodigoBpin = $routeParams.codigobpin;
        vm.ProyectoNombre = $routeParams.proyectonombre;
        vm.embedType = 'report';
        vm.DescargarReportes = DescargarReportes;
        vm.ReporteId;
        vm.ReporteName;        
        vm.fulscreen= fulscreen;
        vm.embedFiltro = {
            $schema: "http://powerbi.com/product/schema#basic",
            target: {
                table: "Proyectos",
                column: "BPIN",
            },
            operator: "In",
            values: [],
            displaySettings: {
                isLockedInViewMode: true,
                isHiddenInViewMode: false,
                displayName: "Filtro por BPIN",
            },
        };
        vm.listaFileFormat = [];

        function fulscreen(element){      
            var el = document.getElementById('element');       
            el.requestFullscreen();
            }

        function DescargarReportes(fileFormat, type) {
            if(vm.ReporteId){
                var filtro = {
                    ReportId: vm.ReporteId,
                    FileFormat: fileFormat
                };
                servicioConsolaMonitoreo.descargarReportesPowerBI(vm.peticion, filtro).then(function (retorno) {
                    FileSaver.saveAs(retorno.data, vm.ReporteName + type);
                    toastr.success("Exporte con éxito")
                }, function (error) {
                    toastr.error("Error inesperado al exportar");
                });
            }
        }

                //Métodos
        vm.init = init;

        function init() {
            if (!vm.gridOptions) {

                vm.gridOptions = {
                    enableColumnMenus: false,
                    paginationPageSizes: [5, 10, 25, 50, 100],
                    paginationPageSize: 5,
                    enableGridMenu: false,
                    onRegisterApi: onRegisterApi
                }

                vm.gridOptions.columnDefs = [{
                        field: 'Name',
                        displayName: "Nombre Reporte",
                        width: "100%",
                        enableHiding: false,
                        cellTemplate: vm.plantillaProyectoReportes
                    },
                    {
                        field: 'Id',
                        enableFiltering: false,
                        enableHiding: true,
                        enableSorting: false,
                        enableColumnMenu: false,
                        visible: false
                    }
                ];

                vm.gridOptions.data = [];
                listaReportes();
                listaFileFormat();
            }
        }

        function listaFileFormat() {

            servicioConsolaMonitoreo.obtenerListaFileFormat(vm.peticion).then(function (retorno) {
                vm.listaFileFormat = retorno.data;
            }, function (error) {
                
            });

        }

        function listaReportes() {
            var filtro = {};
            // {
            //     UserName: "",
            //     Roles: "",
            //     ReportId: "",
            //     WorkspaceId: "",
            //     DashboardId: "",
            //     ReportName: "",
            //   };

            servicioConsolaMonitoreo.obtenerReportesPowerBI(vm.peticion, filtro).then(exito, error);

            function exito(respuesta) {
                if (respuesta.data && respuesta.data.Reportes) {
                    vm.gridOptions.data = respuesta.data.Reportes;
                }
            }

            function error(respuesta) {
                vm.gridOptions.data = [];
            }
        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        function obtenerPeticion() {
            var roles = sesionServicios.obtenerUsuarioIdsRoles();
            if (
                backboneServicios.estaAutorizado() &&
                roles != null &&
                roles.length > 0
            )
                return {
                    IdUsuario: usuarioDNP,
                    Aplicacion: nombreAplicacionBackbone,
                    IdObjeto: "bc154cba-50a5-4209-81ce-7c0ff0aec2ce",
                    ListaIdsRoles: roles,
                };

            return {};
        }

        vm.consultarAccionPowerBI = function (entity) {
            vm.ReporteId = entity.Id;
            vm.ReporteName = entity.Name;
            var filtro = {
                ReportId: entity.Id
            };
            servicioConsolaMonitoreo
                .obtenerConsolaMonitoreoReportes(vm.peticion,filtro)
                .then(
                    function (retorno) {
                        vm.embedFiltro.values = [vm.CodigoBpin];
                        vm.embedConfig = retorno.data;
                    },
                    function (error) {
                        vm.Mensaje = error.data.Message;
                        // mostrarMensajeRespuesta();
                    }
                );
        };
        //#endregion
    }

    angular.module("backbone").component("monitoreoProyecto", {
        templateUrl:
            "/src/app/monitoreo/componentes/Reportes/Proyecto/consolaProyectoComponente.html",
        controller: "consolaProyectoComponenteController",
        controllerAs: "vm",
    });
})();
