(function () {
    'use strict';

    consolaAlertasController.$inject = [
        '$scope', 
        'constantesAutorizacion', 
        'configurarEntidadRolSectorServicio', 
        'constantesCondicionFiltro',
        'servicioConsolaAlertas', 
        'sesionServicios', 
        'backboneServicios', 
        'FileSaver', 
        'Blob',
        '$uibModal'
    ];
     
    function consolaAlertasController(
        $scope, 
        constantesAutorizacion, 
        configurarEntidadRolSectorServicio, 
        constantesCondicionFiltro,
        servicioConsolaAlertas, 
        sesionServicios, 
        backboneServicios, 
        FileSaver, 
        Blob,
        $uibModal) {

        var vm = this;

        //variables
        vm.listaTipoEntidad = [];
        vm.listaFiltroEstadoAlerta = [];
        vm.listaFiltroTipoAlerta = [];
        vm.tipoEntidad = null;
        vm.mostrarFiltro = false;
        vm.peticion;
        vm.gridOptions;
        vm.plantillaProyectoBpin = 'src/app/monitoreo/plantillas/plantillaProyectoBpin.html';
        vm.mostrarMensaje = false;
        vm.Mensaje;
        vm.accionesAlertaTemplate = 'src/app/monitoreo/plantillas/plantillaAccionesAlerta.html';
        vm.estadoAlertaTemplate = 'src/app/monitoreo/plantillas/plantillaEstadoAlerta.html';

        vm.roles = obtenerRoles();

        //Métodos
        vm.cambiarEstadoAlerta = cambiarEstadoAlerta;
        vm.cambioTipoEntidad = cambioTipoEntidad;
        vm.listaConfiguracionesRolSector = obtenerConfiguracionesRolSector();
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.init = init;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.buscar = buscar;
        vm.downloadPdf = downloadPdf;
        vm.downloadExcel = downloadExcel;
        vm.abrirModalCrearAlertaConfiguracion = abrirModalCrearAlertaConfiguracion;
        vm.eliminarAlertaConfiguracion = eliminarAlertaConfiguracion;
        vm.listaConsolaAlertas = listaConsolaAlertas;

        function downloadPdf() {
            servicioConsolaAlertas.obtenerPdfConsolaAlertas(vm.peticion, vm.filtro).then(
                function (data) {
                    servicioConsolaAlertas.imprimirPdfConsolaAlertas(data.data).then(function (retorno) {
                        FileSaver.saveAs(retorno.data, nombreDelArchivo(retorno));
                    });
                }, function (error) {
                    vm.Mensaje = error.data.Message;
                    mostrarMensajeRespuesta();
                }
            );
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

        function downloadExcel() {
            servicioConsolaAlertas.obtenerExcelConsolaAlertas(vm.peticion, vm.filtro).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                });
                FileSaver.saveAs(blob, "ConsolaAlertasConfig.xls");
            }, function (error) {
                vm.Mensaje = error.data.Message;
                mostrarMensajeRespuesta();
            });
        }

        vm.filtro = {
            nombreAlerta: {
                campo: 'NombreAlerta',
                valor: null,
                tipo: constantesCondicionFiltro.contiene
            },
            tipoAlerta: {
                campo: 'TipoAlerta',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            },
            mensajeAlerta: {
                campo: 'MensajeAlerta',
                valor: null,
                tipo: constantesCondicionFiltro.contiene
            },
            estadoAlerta: {
                campo: 'Estado',
                valor: null,
                tipo: constantesCondicionFiltro.igual
            }
        };

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

        function cambioTipoEntidad(tipoEntidad) {
            vm.tipoEntidad = tipoEntidad;
            vm.filtro.tipoEntidad.valor = vm.tipoEntidad;
        }

        function tipoEntidadPresenteEnLaConfiguracion(tipoEntidad) {
            var tipoConfiguracion = _.find(vm.listaConfiguracionesRolSector, {
                TipoEntidad: tipoEntidad
            });
            return tipoConfiguracion ? true : false;
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

        function conmutadorFiltro() {
            limpiarCamposFiltro();
            vm.mostrarFiltro = !vm.mostrarFiltro;
        }

        function limpiarCamposFiltro() {
            vm.filtro.estadoAlerta.valor = null;
            vm.filtro.tipoAlerta.valor = null;
            vm.filtro.mensajeAlerta.valor = null;
            vm.filtro.nombreAlerta.valor = null;
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
                    field: 'NombreAlerta',
                    displayName: "Nombre de la Alerta",
                    width: "29%",
                    cellTooltip: (row, col) => row.entity[col.field]
                },
                {
                    field: 'TipoAlertaDescripcion',
                    displayName: "Tipo",
                    width: "12%",
                    cellTooltip: (row, col) => row.entity[col.field]
                },
                {
                    field: 'MensajeAlerta',
                    displayName: "Mensaje de la Alerta",
                    width: "35%",
                    cellTooltip: (row, col) => row.entity[col.field]
                },
                {
                    field: 'EstadoAlerta',
                    displayName: "Estado",
                    cellTemplate: vm.estadoAlertaTemplate,
                    width: "12%"
                },
                {
                    field: 'Accion',
                    displayName: 'Acción',
                    enableFiltering: false,
                    enableHiding: false,
                    enableSorting: false,
                    enableColumnMenu: false,
                    cellTemplate: vm.accionesAlertaTemplate,
                    width: '12%'
                }];

                vm.gridOptions.data = [];
                listaConsolaAlertas();
                listarTipoAlerta();
                listarEstado();
            }
        }

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        function listaConsolaAlertas() {
            servicioConsolaAlertas.obtenerConsolaAlertas(vm.peticion, vm.filtro).then(exito, error);

            function exito(respuesta) {
                if (respuesta && respuesta.data) {
                    vm.gridOptions.data = respuesta.data;
                }
            }

            function error(respuesta) {
                vm.gridOptions.data = [];
            }
        }

        function listarTipoAlerta() {
            servicioConsolaAlertas.obtenerListaTipoAlerta(vm.peticion).then(exito, error);

            function exito(respuesta) {
                if (respuesta && respuesta.data) {
                    vm.listaFiltroTipoAlerta = respuesta.data;
                }
            }

            function error(respuesta) {
                vm.listaFiltroTipoAlerta = [];
            }
        }

        function listarEstado() {
            servicioConsolaAlertas.obtenerListaEstado(vm.peticion).then(exito, error);

            function exito(respuesta) {
                if (respuesta && respuesta.data) {
                    vm.listaFiltroEstadoAlerta = respuesta.data;
                }
            }

            function error(respuesta) {
                vm.listaFiltroEstadoAlerta = [];
            }
        }

        function mostrarMensajeRespuesta() {
            if (vm.Mensaje) {
                vm.mostrarMensaje = true;
            } else {
                vm.mostrarMensaje = false;
            }
        }

        function buscar() {
            listaConsolaAlertas();
        }

        function abrirModalCrearAlertaConfiguracion(idAlerta = null) {
            
            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/monitoreo/template/modales/crearAlertaConfiguracion.html',
                controller: 'crearAlertaConfiguracionController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "dialog-modal-alerta",
                resolve: {
                    idAlerta: idAlerta, peticion: vm.peticion, listarGrid: function () { return listaConsolaAlertas; }
                },
            });

            modalInstance.result.then(function (selectedItem) {
               
            }, function () {
                // $log.info('Modal dismissed at: ' + new Date());
            });
        }

        function eliminarAlertaConfiguracion(idAlerta) {

            swal({
                title: "",
                text: "¿Realmente quieres eliminar la alerta?",
                type: "error",
                closeOnConfirm: true,
                html: true,
                showCancelButton: true,
            },
                (isConfirm) => {
                    if (isConfirm) {
                        eliminar(idAlerta);
                    }
                });
            
        }

        function eliminar(idAlerta) {
            servicioConsolaAlertas.eliminarAlertaConfiguracion(vm.peticion, idAlerta).then(exito, error);

            function exito(respuesta) {
                toastr.success("Alerta eliminado con éxito")
                listaConsolaAlertas();
            }

            function error(respuesta) {
                toastr.error("Error inesperado al eliminar la alerta");
            }
        }

        function cambiarEstadoAlerta(model) {
            servicioConsolaAlertas.guardarAlertaConfig(vm.peticion, model);
        }

        };

        


    
    angular.module('backbone').controller('consolaAlertasController', consolaAlertasController);
})();