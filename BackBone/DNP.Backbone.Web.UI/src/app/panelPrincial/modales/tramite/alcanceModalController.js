(function () {
    'use strict';

    angular.module('backbone')
        .controller('alcanceModalController', alcanceModalController);

    alcanceModalController.$inject = [
        'servicioAlcanceModal',
        '$scope',
        '$uibModalInstance',
        'IdInstancia',
        'utilidades',
        'TramiteId',
        '$sessionStorage',
        'archivoServicios',
        "$filter"
    ];

    function alcanceModalController(
        servicioAlcanceModal,
        $scope,
        $uibModalInstance,
        IdInstancia,
        utilidades,
        TramiteId,
        $sessionStorage,
        archivoServicios,
        $filter
    ) {
        var vm = this;

        //variable
        vm.listMotivos = [];
        vm.peticion;
        vm.gridOptions;
        vm.estadoAlertaTemplate = 'src/app/panelPrincial/modales/tramite/plantillaSeleccionar.html';
        vm.cerrar = cerrar;
        vm.guardar = guardar;
        //Métodos
        vm.cambiarEstadoAlerta = cambiarEstadoAlerta;
        vm.init = init;
        vm.listaConsolaAlertas = listaConsolaAlertas;
        vm.refrescarPagina = refrescarPagina;

        function init() {
            if (!vm.gridOptions) {

                vm.gridOptions = {
                    enableColumnMenus: false,
                    paginationPageSizes: [10, 25, 50, 100],
                    paginationPageSize: 10,
                    onRegisterApi: onRegisterApi
                }

                vm.gridOptions.columnDefs = [{
                    field: 'Descripcion',
                    displayName: "MOTIVO DEL ALCANCE",
                    width: "29%",
                    cellTooltip: (row, col) => row.entity[col.field]
                },
                {
                    field: 'EstadoAlerta',
                    displayName: "SELECCIONAR",
                    cellTemplate: vm.estadoAlertaTemplate,
                    width: "12%"
                }];

                vm.gridOptions.data = [];
                listaConsolaAlertas();
            }
        }
        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        function listaConsolaAlertas() {
            servicioAlcanceModal.obtenerTipoMotivo().then((result) => {
                if (result.data.length > 0) {
                    vm.gridOptions.data = result.data;
                }
                else {
                    vm.gridOptions.data = [];
                }
            });
        }


        function cambiarEstadoAlerta(model, estado) {
            if (estado) {
                vm.listMotivos.push(model.Id);
            }
            else {
                vm.listMotivos = vm.listMotivos.filter(function (elem) {
                    return elem != model.Id;
                });
            }
        }

        function cerrar() {
            $uibModalInstance.close(false);
        }

        function refrescarPagina() {
            location.reload();
        }

        function guardar() {
            if (TramiteId == undefined || TramiteId == null) {
                utilidades.mensajeError("No se pudo obtener el trámite asociado a la instancia.");
                return;
            }
                
            if (vm.tipoProceso == '' || vm.tipoProceso == undefined || vm.tipoProceso == null) {
                utilidades.mensajeError("Debe seleccionar un tipo de alcance del trámite.");
                return;
            }
                
            if (vm.listMotivos.length == 0) {
                utilidades.mensajeError("Debe seleccionar mínimo un motivo de alcance.");
                return;
            }
                
            if (vm.descripcion == '' || vm.descripcion == undefined || vm.descripcion == null) {
                utilidades.mensajeError("Debe ingresar una descripción para el alcance del trámite.");
                return;
            }
                
            var data = new Object();
            data.TramiteId = TramiteId;
            data.IdInstancia = IdInstancia;
            data.TipoMotivo = vm.listMotivos;
            data.Descripcion = vm.descripcion;
            data.Motivo = vm.tipoProceso;
            servicioAlcanceModal.crearAlcance(data).then((result) => {
                if (result != undefined || result != null) {
                    
                    if (vm.tipoProceso == 'MA') {
                        let param = {
                            idinstancia: result.data.InstanciaId
                        };
                        archivoServicios.obtenerListadoArchivos(param, 'tramites').then(function (response) {
                            if (response === undefined || typeof response === 'string') {
                                utilidades.mensajeSuccess("Se creo el trámite con No. " + result.data.NumeroTramite,
                                    false,
                                    vm.refrescarPagina,
                                    null);
                                $uibModalInstance.close(false);
                            } else {
                                if (response.length > 0) {
                                    response.forEach(archivo => {
                                        let archivoDuplicar = {
                                            fecha: new Date(),
                                            nombre: archivo.nombre,
                                            urlArchivo: archivo.urlArchivo,
                                            usuario: archivo.usuario,
                                            metadatos: {
                                                extension: archivo.metadatos.extension,
                                                idinstancia: result.data.NuevaInstanciaId,
                                                idaccion: archivo.metadatos.idaccion,
                                                section: archivo.metadatos.section,
                                                codigoproceso: result.data.NumeroTramite,
                                                descripciontramite: archivo.metadatos.descripciontramite,
                                                tipodocumentosoporte: archivo.metadatos.tipodocumentosoporte,
                                                idarchivoblob: archivo.metadatos.idarchivoblob,
                                                contenttype: archivo.metadatos.contenttype
                                            },
                                            status: archivo.status,
                                            coleccion: archivo.coleccion
                                        };
                                        archivoServicios.guardarArchivoRepositorio(archivoDuplicar).then(function (response) {
                                            console.log(response);
                                        }, error => {
                                            console.log(error);
                                        });
                                    });
                                    utilidades.mensajeSuccess("Se creo el trámite con No. " + result.data.NumeroTramite,
                                        false,
                                        vm.refrescarPagina,
                                        null);
                                    $uibModalInstance.close(false);
                                }
                                else {
                                    utilidades.mensajeSuccess("Se creo el trámite con No. " + result.data.NumeroTramite,
                                        false,
                                        vm.refrescarPagina,
                                        null);
                                    $uibModalInstance.close(false);
                                }
                            }
                        }, error => {
                            console.log(error);
                        });
                    }
                    else {
                        utilidades.mensajeSuccess('Se anuló el alcance exitosamente',
                            false,
                            vm.refrescarPagina,
                            null);
                        $uibModalInstance.close(false);
                    }
                }
                
            }).catch(function (e) {
                utilidades.mensajeError(e.data.ExceptionMessage);
            });
        }
    }
})();