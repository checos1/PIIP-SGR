(function () {
    'use strict';

    perfilesController.$inject = [
        '$scope',
        'servicioUsuarios',
        'constantesAutorizacion',
        '$uibModal',
        'FileSaver',
        'utilidades'
    ];

    function perfilesController(
        $scope,
        servicioUsuarios,
        constantesAutorizacion,
        $uibModal,
        FileSaver,
        utilidades
    ) {
        var vm = this;

        /// Filtro
        vm.mostrarFiltro = false;
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.buscar = buscar;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.perfilFiltro = "";

        /// Modais
        vm.abrirModalCrearEditar = abrirModalCrearEditar;
        vm.abrirModalEliminar = abrirModalEliminar;

        /// Actions
        vm.downloadExcel = downloadExcel;
        vm.downloadPdf = downloadPdf;

        /// Plantilla de acciones
        vm.plantillaAccionesTabla = 'src/app/usuarios/perfiles/plantillas/plantillaAccionesTabla.html';

        /// Definiciones de componente
        vm.opciones = {
            nivelJerarquico: 0,
            gridOptions: {
                //showHeader: true,
                paginationPageSizes: [5, 10, 15, 25, 50, 100],
                paginationPageSize: 5,
                //expandableRowHeight: 200
            }
        };
        /// Definiciones de columna componente
        vm.columnDef = [{
            field: 'NombrePerfil',
            displayName: 'Perfil',
            enableHiding: false,
            width: '48%',
            cellTooltip: (row, col) => row.entity[col.field]
        }, {
            field: 'RolesConcat',
            displayName: 'Roles',
            enableHiding: false,
            width: '40%',
            cellTooltip: (row, col) => row.entity[col.field]
        }, {
            field: 'accion',
            displayName: 'Acción',
            headerCellClass: 'text-center',
            enableFiltering: false,
            enableHiding: false,
            enableSorting: false,
            enableColumnMenu: false,
            cellTemplate: vm.plantillaAccionesTabla,
            width: '10%'
        }];

        /// Comienzo
        vm.init = function () {
            listarPerfiles();
        }

        /// Getters
        function listarPerfiles() {
            vm.datos = null;

            servicioUsuarios.obtenerPerfiles(vm.perfilFiltro)
                .then(function (response) {
                    vm.datos = response.data;
                }, function (error) {
                    //TODO: error handler
                    console.log("error", error);
                })
        }


        /// Actions
        function downloadExcel() {
            servicioUsuarios.obtenerExcelPerfiles(vm.perfilFiltro).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob, "Perfiles.xls");
            }, function (error) {
                console.log("error", error);
            });
        }

        function downloadPdf() {
            servicioUsuarios.imprimirPdfPerfiles(vm.datos).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob, nombreDelArchivo(retorno));
            });
        };

        function conmutadorFiltro() {
            vm.perfilFiltro = "";
            vm.mostrarFiltro = !vm.mostrarFiltro;
        }

        function buscar() {
            return listarPerfiles(idTipoProyecto)
        }

        function limpiarCamposFiltro() {
            vm.perfilFiltro = "";
            listarPerfiles();
        }

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

        /// end Actions

        /// Modal Crear/Editar
        function abrirModalCrearEditar(objPerfil) {
            $uibModal.open({
                templateUrl: 'src/app/usuarios/perfiles/modales/modalAccionPerfil.html',
                controller: 'modalAccionPerfilController',
                resolve: {
                    objPerfil: objPerfil
                }
            }).result.then(buscar, buscar);
        };


        // Modal Eliminar
        function abrirModalEliminar(objPerfil) {
            utilidades.mensajeWarning("Confirma la exclusión del registro?", function funcionContinuar() {
                servicioUsuarios.eliminarPerfil(objPerfil)
                    .then(function (response) {
                        if (response.data) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            buscar();
                        } else {
                            utilidades.mensajeError("Error al realizar la operación", false);
                        }
                    })
            }, function () { /* do nothing */  })
        }
    }

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone.usuarios').controller('perfilesController', perfilesController);
})();


