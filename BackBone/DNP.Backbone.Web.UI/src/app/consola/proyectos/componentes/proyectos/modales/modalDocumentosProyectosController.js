(function () {
    'use strict';
    modalDocumentosProyectosController.$inject = [
        'objProyecto',
        '$uibModalInstance',
        'utilidades',
        'servicioConsolaProyectos',
        'FileSaver',
        'archivoServicios'
    ];
    function modalDocumentosProyectosController(
        objProyecto,
        $uibModalInstance,
        utilidades,
        servicioConsolaProyectos,
        FileSaver,
        archivoServicios
    ) {
        var vm = this;
        console.log('objProyecto', objProyecto);
        vm.Proyecto = objProyecto;

        vm.filtro = {
            proyectoId: objProyecto.ProyectoId,
            origen: null,
            vigencia: null,
            periodo: null,
            tipoDocumento: null,
            tramite: null,
            ficha: null,
            procesoOrigen: null,
            NombreDocumento: null,
            proceso: 'Migracion'
        }
        vm.cerrar = $uibModalInstance.dismiss;
        vm.listaDatos;
        vm.seleccionar;
        vm.totalRegistros;
        vm.popOverOptionsNombreProyecto = {
            isOpen: false,
            templateUrl: 'nombreProyecto.html',
            toggle: function () {
                vm.popOverOptionsNombreProyecto.isOpen = !vm.popOverOptionsNombreProyecto.isOpen;
            }
        };
        vm.buscar = function () {
            vm.listaDatos = [];
            servicioConsolaProyectos.obtenerSoportesProyectos(vm.filtro)
                .then(response => {
                    vm.listaDatos = response.data.Documentos;
                    vm.totalRegistros = vm.listaDatos.length;
                    vm.listaOrigenes = response.data.Origenes;
                    vm.listaVigencias = response.data.Vigencias;
                    vm.listaPeriodos = response.data.Periodos;
                    vm.listaTiposDocumento = response.data.TiposDocumento;
                    vm.listaProcesos = response.data.ProcesosOrigen;
                })
        }

        vm.cambioProceso = function (proceso) {
            vm.filtro.proceso = proceso;
            vm.origen = null;
            vm.vigencia = null;
            vm.periodo = null;
            vm.tipoDocumento = null;
            vm.tramite = null;
            vm.ficha = null;
            vm.procesoOrigen = null;
            vm.NombreDocumento = null;
            vm.buscar();
        }


        vm.init = function () {
            vm.buscar();
        }

        vm.hasClass = function (elementoId, clase) {
            var elemento = angular.element(document.querySelector('#' + elementoId));
            return elemento.hasClass(clase);
        }
        vm.restablecerFiltros = function () {
            vm.filtro.origen = null;
            vm.filtro.vigencia = null;
            vm.filtro.periodo = null;
            vm.filtro.tipoDocumento = null;
            vm.filtro.tramite = null;
            vm.filtro.ficha = null;
            vm.filtro.procesoOrigen = null;
            vm.filtro.NombreDocumento = null;
            vm.buscar();
        }

        vm.seleccionarTodo = function () {
            vm.listaDatos.forEach(item => {
                item.Seleccionado = vm.seleccionar;
            });
        }

        vm.descargar = function () {
            var datos = vm.listaDatos.filter(x => x.Seleccionado);
            try {

                if (datos !== undefined && datos !== null) {

                    servicioConsolaProyectos.descargarArchivos(datos).then(response => {
                        let dataResponse = response.data;

                        if (Boolean(dataResponse.EsExcepcion) === true) {
                            console.log('controladorProyecto.DescargarArchivosProyecto => ', String(dataResponse.ExcepcionMensaje));
                            toastr.error('Ocurrió un error al descargar Archivos Proyecto');
                        }
                        else {
                            let file = dataResponse.Datos;

                            var bytes = Uint8Array.from(atob(String(file.FileContent)).split('').map(char => char.charCodeAt(0)));
                            var blob = new Blob([bytes], { type: String(file.ContentType) });

                            FileSaver.saveAs(blob, String(file.FileName));
                        }
                    });
                }
            }
            catch (execption) {
                console.log('controladorProyecto.DescargarArchivosProyecto => ', exception);
                toastr.error('Ocurrió un error al descargar Archivos Proyecto');
            }
        }
        vm.descargarArchivoBlob = function (entity) {
            archivoServicios.obtenerArchivoBytes(entity.idArchivoBlob, 'tramites').then(function (retorno) {

                var binary = atob(retorno.replace(/\s/g, ''));
                var len = binary.length;
                var buffer = new ArrayBuffer(len);
                var view = new Uint8Array(buffer);
                for (var i = 0; i < len; i++) {
                    view[i] = binary.charCodeAt(i);
                }

                var blob = new Blob([view], {
                    type: entity.ContenType
                });
                var downloadUrl = URL.createObjectURL(blob);
                var a = document.createElement("a");
                a.href = downloadUrl;
                a.download = entity.NombreDocumento.trimEnd();
                document.body.appendChild(a);
                a.click();
            }, function (error) {
                utilidades.mensajeError("Error inesperado al descargar");
            });
        }
    }
    
    angular.module('backbone').controller('modalDocumentosProyectosController', modalDocumentosProyectosController).directive('maxlength', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var maxLength = parseInt(attrs.maxlength, 10);

                element.on('input', function () {
                    var value = element.val();

                    if (value.length > maxLength) {
                        element.val(value.substring(0, maxLength));
                        element.triggerHandler('input');
                    }
                });
            }
        };
    });
})();