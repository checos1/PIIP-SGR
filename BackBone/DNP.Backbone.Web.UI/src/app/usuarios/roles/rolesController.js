(function () {
    'use strict';

    rolesController.$inject = [
        '$scope',
        'servicioUsuarios',
        'constantesAutorizacion',
        '$uibModal',
        'utilidades',
        'FileSaver',
        'sesionServicios',
        'OpcionesContante'
    ];

    function rolesController(
        $scope,
        servicioUsuarios,
        constantesAutorizacion,
        $uibModal,
        utilidades,
        FileSaver,
        sesionServicios,
        OpcionesContante
    ) {
        var vm = this;

        vm.tienePermiso = tienePermiso;

        /// Filtro
        vm.mostrarFiltro = false;
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.filtrar = filtrar;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.roleFiltro = "";

        /// Modais
        vm.abrirModalCrearEditar = abrirModalCrearEditar;
        vm.abrirModalEliminar = abrirModalEliminar;

        /// Actions
        vm.downloadPdf = downloadPdf;
        vm.downloadExcel = downloadExcel;

        /// Plantilla de acciones
        vm.plantillaAccionesTabla1 = 'src/app/usuarios/roles/plantillas/plantillaAccionesTabla.html';

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
            field: 'Nombre',
            displayName: 'Roles',
            enableHiding: false,
            width: '30%',
            cellTooltip: (row, col) => row.entity[col.field]
        }, {
            field: 'OpcionesConcat',
            displayName: 'Opciones',
            enableHiding: false,
            width: '58%',
            cellTooltip: (row, col) => row.entity[col.field]
        }, {
            field: 'accion',
            displayName: 'Acción',
            headerCellClass: 'text-center',
            enableFiltering: false,
            enableHiding: false,
            enableSorting: false,
            enableColumnMenu: false,
            cellTemplate: vm.plantillaAccionesTabla1,
            width: '10%'
        }];

        /// Comienzo
        vm.init = function () {
            listarRoles();

            //teste
            //console.log("Permissão para editar usuario:", vm.tienePermiso(OpcionesContante.UsuariosEditarUsuario));
            //console.log("Permissão para eliminar usuario:", vm.tienePermiso(OpcionesContante.UsuariosEliminarUsuario));
            //console.log("Permissão para invitar usuario:", vm.tienePermiso(OpcionesContante.UsuariosInvitarUsuario));
        }

        //Permisos
        function tienePermiso(idOpcionDNP) {
            if (!idOpcionDNP)
                return false;

            return sesionServicios.tieneOpcionVinculada(idOpcionDNP);;
        }

        /// Getters
        function listarRoles() {
            vm.datos = null;

            servicioUsuarios.obtenerRoles(vm.roleFiltro)
                .then(function (response) {
                    vm.datos = response.data;
                }, function (error) {
                    //TODO: error handler
                    console.log("error", error);
                });
        }

        /// Actions
        function downloadExcel() {
            servicioUsuarios.obtenerExcelRoles(vm.roleFiltro).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob, "Roles.xls");
            }, function (error) {
                console.log("error", error);
            });
        }

        function downloadPdf() {
            servicioUsuarios.imprimirPdfRoles(vm.datos).then(function (retorno) {
                var blob = new Blob([retorno.data], {
                    type: "application/octet-stream"
                });
                FileSaver.saveAs(blob, nombreDelArchivo(retorno));
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

        function conmutadorFiltro() {
            limpiarCamposFiltro();
            vm.mostrarFiltro = !vm.mostrarFiltro;
        }

        function filtrar() {
            return listarRoles()
        }

        function limpiarCamposFiltro() {
            vm.roleFiltro = "";
            listarRoles();
        }

        /// Modal Crear/Editar
        function abrirModalCrearEditar(objRol) {
            $uibModal.open({
                templateUrl: 'src/app/usuarios/roles/modales/modalAccionRole.html',
                controller: 'modalAccionRoleController',
                openedClass: 'roles-modal',
                resolve: {
                    objRol: objRol,
                }
            }).result.then(function (result) {
                listarRoles();
            }, function (reason) {
                listarRoles();
            });
        };


        // Modal Eliminar
        function abrirModalEliminar(objRole) {
            utilidades.mensajeWarning("Confirma la exclusión del registro?", function funcionContinuar() {
                servicioUsuarios.eliminarRol({ IdRol: objRole.IdRol, Nombre: objRole.Nombre })
                    .then(function (response) {
                        if (response.data) {
                            if (response.data.Exito) {
                                utilidades.mensajeSuccess("Operación realizada con éxito!");
                                listarRoles();
                            }
                            else {
                                utilidades.mensajeError(response.data.Mensaje);
                            }
                        } else {
                            utilidades.mensajeError("Error al realizar la operación");
                        }
                    })
            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            })
        }

    }

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone.usuarios').controller('rolesController', rolesController);
})();
