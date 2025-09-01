(function () {
    'use strict';

    modalAccionRoleController.$inject = [
        'objRol',
        '$uibModalInstance',
        'servicioUsuarios',
        'utilidades',
    ];

    function modalAccionRoleController(
        objRol,
        $uibModalInstance,
        servicioUsuarios,
        utilidades,
    ) {
        var vm = this;

        vm.listaOpciones;
        vm.nombre;
        vm.gridOptions;        
        
        vm.addOpcion = addOpcion;
        vm.eliminarOpcion = eliminarOpcion;
        vm.guardarRol = guardarRol;        
        vm.cerrar = $uibModalInstance.dismiss;

        /// Comienzo
        vm.init = function () {
            servicioUsuarios.obtenerOpciones("")
                .then(function (response) {
                    vm.listaOpciones = response.data;
                });

            vm.gridOptions = {
                columnDefs: [
                    {
                        field: 'Nombre',
                        displayName: 'Opciones',
                        enableHiding: false,
                        width: '68%'
                    },
                    {
                        field: 'NombreTipoOpcion',
                        displayName: 'Tipo Opción',
                        enableHiding: false,
                        width: '17%'
                    },
                    {
                        field: 'accion',
                        displayName: 'Acción',
                        headerCellClass: 'text-center',
                        enableFiltering: false,
                        enableHiding: false,
                        enableSorting: false,
                        enableColumnMenu: false,
                        cellTemplate: '<div class="text-center"><button class="btnaccion" ng-click="grid.appScope.vm.eliminarOpcion(row.entity)" tooltip-placement="Auto" uib-tooltip="Eliminar">' +
                            '    <span  style="cursor:pointer"> <img class="grid-icon-accion"  src="Img/iconsgrid/iconAccionEliminar.png"></span>' +
                            '</button></div>',
                        width: '15%'
                    }
                ],
                enableVerticalScrollbar: true,
                enableSorting: true,
                showHeader: true,
                showGridFooter: false
            };

            if (objRol != null) {
                vm.idRol = objRol.IdRol;
                vm.nombre = objRol.Nombre;
                obtenerOpciones();
            }
        }

        function addOpcion() {
            let opcion = vm.listaOpciones.filter(x => x.IdOpcion == vm.idOpcion.toString())[0];
            let opcionGrid = vm.gridOptions.data.filter(x => x.IdOpcion == vm.idOpcion)[0];

            if (!opcionGrid) {
                vm.gridOptions.data.push({ IdOpcion: opcion.IdOpcion, Nombre: opcion.Nombre, Agregar: true })
            }
        }

        function eliminarOpcion(row) {
            let rowIndex = vm.gridOptions.data.indexOf(row);
            vm.gridOptions.data.splice(rowIndex, 1);
        }

        function guardarRol() {
            var rolDto = {
                Nombre: vm.nombre,
                UsuarioDNP: usuarioDNP,
                Agregar: true,
                Opciones: vm.gridOptions.data,
            };

            if (vm.idRol) {
                rolDto.IdRol = objRol.IdRol;
                rolDto.Agregar = false;
            }

            servicioUsuarios.guardarRol(rolDto)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.cerrar()
                    } else
                        utilidades.mensajeError(response.data.Mensaje, false);
                });
        }

        function obtenerOpciones() {
            var peticionObtenerOpciones = {
                usuario: usuarioDNP,
                idRol: vm.idRol
            };

            if (vm.idRol) {
                vm.gridOptions.data = [];
                servicioUsuarios.obtenerOpcionesDeRol(peticionObtenerOpciones)
                    .then(function (response) {
                        if (response != null) {
                            vm.gridOptions.data = response.data;
                        }
                            
                    })
            }
        }

    }

    angular.module('backbone.usuarios').controller('modalAccionRoleController', modalAccionRoleController);
})();