(function () {
    'use strict';

    modalFichasController.$inject = [
        '$uibModalInstance',
        'utilidades',
        'FileSaver',
        'archivoServicios',
        '$sessionStorage',
        'entity',
        'esProyecto'
    ];

    function modalFichasController(
        $uibModalInstance,
        utilidades,
        FileSaver,
        archivoServicios,
        $sessionStorage,
        entity,
        esProyecto
    ) {
        var vm = this;

        vm.gridOptions;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.listaFichas = [];
        vm.gridOptions = {
            columnDefs: [{
                field: 'nombre',
                displayName: 'Descripción de la ficha',
                enableHiding: false,
                width: '42%'
            },
            {
                field: 'tipoFicha',
                displayName: 'Tipo ficha',
                enableHiding: false,
                width: '25%'
            },
            {
                field: 'fechaCreacion',
                displayName: 'Fecha',
                cellFilter: 'date:\'dd/MM/yyyy hh:mm\'',
                enableHiding: false,
                width: '20%'
            },
            {
                field: 'accion',
                displayName: 'Acción',
                headerCellClass: 'text-center',
                enableFiltering: false,
                enableHiding: false,
                enableSorting: false,
                enableColumnMenu: false,
                cellTemplate: '<div class="text-center"><button class="btnaccion" ng-click="grid.appScope.vm.obtenerDocumento(row.entity)" tooltip-placement="Auto" uib-tooltip="Descargar">' +
                    '    <span style="cursor:pointer"> <img src="Img/iconsgrid/iconAccionDownload.png"></span>' +
                    '</button></div>',
                width: '12%'
            }],
            enableVerticalScrollbar: true,
            enableSorting: true,
            showHeader: true,
            showGridFooter: false
        };
        vm.obtenerDocumento = obtenerDocumento;

        function cargarFichas() {
            let parametrosArchivos = {};

            if (esProyecto) {
                parametrosArchivos = {
                    IdObjetoNegocio: entity.IdObjetoNegocio,
                    Tipo: 'Ficha'
                };
            }
            else {
                parametrosArchivos = {
                    IdInstancia: entity.IdInstancia,
                    Tipo: 'Ficha'
                };
            }
            archivoServicios.obtenerListadoArchivos(parametrosArchivos, $sessionStorage.idAplicacion).then(res => {
                let fichasFiltradas = res.filter(p => p.status === "Nuevo");
                let data = fichasFiltradas.map(x => ({
                    tipoFicha: x.metadatos.tipoficha,
                    fechaCreacion: x.metadatos.fechacreacion,
                    usuario: x.usuario,
                    contentType: x.metadatos.contenttype,
                    idArchivoBlob: x.metadatos.idarchivoblob,
                    extension: x.metadatos.extension,
                    coleccion: x.coleccion,
                    nombre: x.nombre.slice(0, ((utilidades.obtenerExtensionArchivo(x.nombre).length + 1) * -1)),
                    nombreCompleto: x.nombre
                }));
                vm.gridOptions.data = data;
            }, err => {
                console.log(err);
            })
        }

        function obtenerDocumento(entity){
            archivoServicios.obtenerArchivoBytes(entity.idArchivoBlob, entity.coleccion).then(function (retorno) {
                const blob = utilidades.base64toBlob(retorno, entity.contentType);
                //const blob = new Blob([retorno], { type: entity.contentType });
                FileSaver.saveAs(blob, entity.nombreCompleto);
            }, function (error) {
                toastr.error("Error inesperado al descargar");
            });
        }

        /// Comienzo
        vm.init = function () {
            vm.gridOptions.data = [];
            cargarFichas();
        }
    }

    angular.module('backbone').controller('modalFichasController', modalFichasController);
})();