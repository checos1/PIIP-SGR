(function () {
    'use strict';

    centroAyudaController.$inject = [
        '$scope',
        '$rootScope',
        'utilidades',
        '$uibModal',
        'servicioCentroAyuda',
        'AyudaTipoConstante',
        'AyudaTemaListaItemModel',
        'FileSaver'
    ];

    function centroAyudaController(
        $scope,
        $rootScope,
        utilidades,
        $uibModal,
        servicioCentroAyuda,
        AyudaTipoConstante,
        AyudaTemaListaItemModel,
        FileSaver
    ) {

        var vm = this;
        vm.init = init;
        vm.datos = [];
        vm.tipoTab = 'Centro de Ayuda';
        vm.abaAyuda = true;
        vm.abaVideo = false;
        vm.listaOrigem = [];


        vm.aDescargarPdf_OnClick = aDescargarPdf_OnClick;
        vm.downloadExcel = downloadExcel;


        /// Filtros
        vm.mostrarFiltro = false;
        vm.conmutadorFiltro = conmutadorFiltro;
        vm.filtrarAyuda = filtrarAyuda;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.agregarNuevaAyuda = agregarNuevaAyuda;
        vm.actualizarTema = actualizarTema;
        vm.crearSubTema = crearSubTema;
        vm.actualizarSubTema = actualizarSubTema;
        vm.actualizarItemAyudaVideo = actualizarItemAyudaVideo;
        vm.eliminarTema = eliminarTema;
        vm.BusquedaRealizada = false;
        vm.arrowIcoDown2 = "glyphicon-chevron-down-busqDNP";
        vm.arrowIcoUp2 = "glyphicon-chevron-up-busqDNP";
        vm.totalRegistrosEntontrados = 0;

        vm.filtros = {
            general: null,
            especifico: null,
            tema: null,
        };
        vm.listarVideos = listarVideos;
        vm.listarAyudas = listarAyudas;
        vm.cambiarFiltroTab = cambiarFiltroTab;

        /// Plantilla de acciones
        vm.plantillaAccionesTablaCabeza = 'src/app/centroAyuda/plantillas/plantillaAccionesTablaCabeza.html';
        vm.plantillaAccionesTablaVideos = 'src/app/centroAyuda/plantillas/plantillaAccionesTablaVideos.html';
        vm.plantillaAccionesTabla = 'src/app/centroAyuda/plantillas/plantillaAccionesTabla.html';
        vm.plantillaSubGrid = 'src/app/centroAyuda/plantillas/plantillaSubGrid.html';

        //Acordeon
        vm.AbrilNivel = function (idArticulo) {
            vm.listaOrigem.forEach(function (value, index) {
                if (value.Id == idArticulo) {
                    if (value.estadoArticulo == '+')
                        value.estadoArticulo = '-';
                    else
                        value.estadoArticulo = '+';
                }
            });
        }

        vm.columnDefPrincial = [{
            field: 'Nombre',
            displayName: 'Temas',
            enableHiding: false,
            width: '80%',
        }, {
            field: 'accion',
            displayName: 'Acción',
            headerCellClass: 'text-center',
            enableFiltering: false,
            enableHiding: false,
            enableSorting: false,
            enableColumnMenu: false,
            cellTemplate: vm.plantillaAccionesTablaCabeza,
            width: '15%'
        }];

        vm.columnDef = [{
            field: 'Nombre',
            displayName: 'Temas',
            enableHiding: false,
            width: '90%',
            cellTooltip: (row, col) => row.entity[col.field]
        },
        {
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

        vm.columnDefVideos = [{
            field: 'Nombre',
            displayName: 'Video',
            enableHiding: false,
            width: '90%',
        },
        {
            field: 'accion',
            displayName: 'Acción',
            headerCellClass: 'text-center',
            enableFiltering: false,
            enableHiding: false,
            enableSorting: false,
            enableColumnMenu: false,
            cellTemplate: vm.plantillaAccionesTablaVideos,
            width: '9%'
        }];
        // grid main
        vm.gridOptions = {
            enableSorting: true,
            columnDefs: vm.columnDefPrincial,
            expandableRowTemplate: vm.plantillaSubGrid,
            enableOnDblClickExpand: true,
            enableColumnResizing: false,
            showGridFooter: false,
            enablePaginationControls: true,
            useExternalPagination: false,
            useExternalSorting: false,
            paginationCurrentPage: 1,
            enableVerticalScrollbar: 1,
            enableFiltering: false,
            showHeader: true,
            useExternalFiltering: false,
            paginationPageSizes: [5, 10, 15, 25, 50, 100],
            paginationPageSize: 10,
            subGridOptions: [],
            data: []
        };

        vm.subGridOptions = function (data) {
            return {
                columnDefs: vm.columnDef,
                appScopeProvider: $scope,
                paginationPageSizes: [5, 10, 15, 25, 50, 100],
                paginationPageSize: 10,
                showHeader: false,
                data: data
            }
        }

        vm.gridOptionsVideos = {
            columnDefs: vm.columnDefVideos,
            enableExpandable: false,
            enableExpandableRowHeader: false,
            paginationPageSizes: [5, 10, 15, 25, 50, 100],
            paginationPageSize: 10,
            showHeader: true,
            data: []
        };

        function downloadExcel() {
            toastr.warning("Método no Aplicado");
        }

        /**
         * 
         * @desciption .Obtiene los datos actuales y los descarga creando un archivo PDF de la información
         * @param {Array} datos. Arreglo de la información a mostrar en el PDF de descarga.
        */
        vm.DescargarTemarioPDF = function (datos) {
            try {
                if (datos !== undefined && datos !== null) {


                    servicioCentroAyuda.ObtenerPdfTemario(datos).then(response => {


                        let dataResponse = response.data;

                        // verificar si existe una exepción en la descarga
                        if (Boolean(dataResponse.EsExcepcion) === true) {

                            console.log('centroAyudaCtrl.DescargarPDF => ', String(dataResponse.ExcepcionMensaje));
                            toastr.error('Ocurrió un error al descargar PDF');
                        }
                        else {

                            let file = dataResponse.Datos;

                            // obtener el archivo binario y convertirlo en un blob de JavaScript
                            let bytes = new Uint8Array(file.FileContents);
                            let blob = new Blob([bytes], { type: file.ContentType });

                            FileSaver.saveAs(blob, file.FileDownloadName);
                        }

                    });

                }
            }
            catch (exception) {
                console.log('centroAyudaCtrl.DescargarPDF => ', exception);
                toastr.error('Ocurrió un error al descargar PDF');
            }
        };

        /**
             * 
             * @desciption .Obtiene los datos actuales y los descarga creando un archivo PDF de la información
             * @param {Array} datos. Arreglo de la información a mostrar en el PDF de descarga.
            */
        vm.DescargarVideosPDF = function (datos) {
            try {
                if (datos !== undefined && datos !== null) {


                    servicioCentroAyuda.ObtenerPdfVideos(datos).then(response => {


                        let dataResponse = response.data;

                        // verificar si existe una exepción en la descarga
                        if (Boolean(dataResponse.EsExcepcion) === true) {

                            console.log('centroAyudaCtrl.DescargarPDF => ', String(dataResponse.ExcepcionMensaje));
                            toastr.error('Ocurrió un error al descargar PDF');
                        }
                        else {

                            let file = dataResponse.Datos;

                            // obtener el archivo binario y convertirlo en un blob de JavaScript
                            let bytes = new Uint8Array(file.FileContents);
                            let blob = new Blob([bytes], { type: file.ContentType });

                            FileSaver.saveAs(blob, file.FileDownloadName);
                        }

                    });

                }
            }
            catch (exception) {
                console.log('centroAyudaCtrl.DescargarPDF => ', exception);
                toastr.error('Ocurrió un error al descargar PDF');
            }
        };

        /**
         * 
         * @description. Evento provocado al presionar el botón de descargar PDF
         * @param {Event} event. Evento provocado 
         * @param {HTMLElement} sender. Componente HTML que provoca el evento 
         */
        function aDescargarPdf_OnClick(event, sender) {
            try {

                if (vm.abaAyuda)
                    servicioCentroAyuda.obtenerListaTemas({ Tipo: 1, SoloActivos: true }).then(function (response) {

                        let listaDatos = response.data;

                        if (listaDatos != null && listaDatos.length > 0) {

                            vm.DescargarTemarioPDF(listaDatos);
                        }
                    }, function (error) {
                        //TODO: error handler
                        console.log("centroAyudaCtrl.aDescargarPdf_OnClick[obtenerListaTemas] =>", error);
                        toastr.error("Error al obtener la información");
                    });

                if (vm.abaVideo)
                    servicioCentroAyuda.obtenerListaTemas({ Tipo: 2, SoloActivos: true }).then(function (response) {

                        debugger;
                        let listaDatos = response.data;

                        if (listaDatos != null && listaDatos.length > 0) {
                            vm.DescargarVideosPDF(listaDatos);
                        }
                    }, function (error) {
                        //TODO: error handler
                        console.log("centroAyudaCtrl.aDescargarPdf_OnClick[obtenerListaVideos] =>", error);
                        toastr.error("Error al obtener la información");
                    });
            }
            catch (exception) {
                console.log('centroAyudaCtrl.aDescargarPdf_OnClick => ', excepcion);
                toastr.error('Error en la descarga de PDF');
            }
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

        /// Comienzo
        function init() {
            vm.listarAyudas();
        }

        function filtrarAyuda() {
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusq.svg"
            if (vm.tipoTab === 'Centro de Ayuda') {
                vm.gridOptions.data = vm.listaOrigem;
                vm.gridOptions.data.forEach(item => {
                    item.subGridOptions = vm.subGridOptions(item.SubItems);
                });
                
                if (vm.filtros.general) {
                    var filtro = toNormalForm(vm.filtros.general.toUpperCase());
                    vm.gridOptions.data = vm.gridOptions.data.filter(x => toNormalForm(x.Nombre.toUpperCase()).includes(filtro));
                    vm.totalRegistrosEntontrados += item.gridOptions.data.length;
                }

                if (vm.filtros.especifico) {
                    var filtro = toNormalForm(vm.filtros.especifico.toUpperCase());
                    vm.gridOptions.data.forEach(item => {
                        item.subGridOptions.data = item.subGridOptions.data.filter(x => toNormalForm(x.Nombre.toUpperCase()).includes(filtro));
                        vm.totalRegistrosEntontrados += item.subGridOptions.data.length;
                    });
                }
                vm.gridOptions.data.forEach(item => {
                    if (item.subGridOptions.data.length == 0) {
                        vm.gridOptions.data = vm.gridOptions.data.filter(x => x.Id != item.Id);
                    }
                });
            } else {
                vm.gridOptionsVideos.data = vm.listaOrigem;
                var filtro = toNormalForm(vm.filtros.tema.toUpperCase());
                vm.gridOptionsVideos.data = vm.gridOptionsVideos.data.filter(x => toNormalForm(x.Nombre.toUpperCase()).includes(filtro));
                vm.totalRegistrosEntontrados += item.gridOptionsVideos.data.length;
            }

            vm.BusquedaRealizada = true;
        }
        function toNormalForm(str) {
            return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
        }
        function conmutadorFiltro() {
            if (vm.mostrarFiltro) {
                limpiarCamposFiltro();
            }

            vm.mostrarFiltro = !vm.mostrarFiltro;

            var idSpanArrow = 'arrow-IdPanelBuscador';
            var arrowCapitulo = document.getElementById(idSpanArrow);
            var arrowClasses = arrowCapitulo.classList;
            for (var i = 0; i < arrowClasses.length; i++) {
                if (arrowClasses[i] == vm.arrowIcoDown2) {
                    document.getElementById(idSpanArrow).src = "Img/IcoBusqDow.svg";
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp2);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown2);
                    break;
                } else if (arrowClasses[i] == vm.arrowIcoUp2) {
                    document.getElementById(idSpanArrow).src = "Img/IcoBusqUp.svg";
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown2);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp2);
                    break;
                }
            }
        }

        function limpiarCamposFiltro() {
            vm.filtros = {
                general: null,
                especifico: null,
                tema: null,
            };

            if (vm.tipoTab === 'Centro de Ayuda') {
                vm.gridOptions.data = vm.listaOrigem;
                vm.gridOptions.data.forEach(item => {
                    item.subGridOptions = vm.subGridOptions(item.SubItems);
                });
            } else {
                vm.gridOptionsVideos.data = vm.listaOrigem;
            }
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
            vm.BusquedaRealizada = false;
            vm.totalRegistrosEntontrados = 0;
        }

        /// Getters

        function listarAyudas() {
            vm.abaAyuda = true;
            vm.abaVideo = false;
            servicioCentroAyuda.obtenerListaTemas({ Tipo: 1, SoloActivos: true })
                .then(function (response) {
                    vm.datos = response.data;
                    vm.listaOrigem = response.data;
                    if (vm.datos != null && vm.datos.length > 0) {
                        vm.gridOptions.data = vm.datos;
                        vm.gridOptions.data.forEach(item => {
                            try {

                                item.estadoArticulo = "+";
                                item.subGridOptions = vm.subGridOptions(item.SubItems);
                            } catch (e) {
                                console.log(e);
                            }
                        });
                    }
                }, function (error) {
                    //TODO: error handler
                    console.log("error", error);
                });
        }

        function listarVideos() {
            vm.abaVideo = true;
            vm.abaAyuda = false;
            servicioCentroAyuda.obtenerListaTemas({ Tipo: 2, SoloActivos: true })
                .then(function (response) {
                    vm.datos = response.data;
                    vm.listaOrigem = response.data;
                    if (vm.datos != null && vm.datos.length > 0) {
                        vm.gridOptionsVideos.data = vm.datos;
                    }
                }, function (error) {
                    //TODO: error handler
                    console.log("error", error);
                });
        }

        // Actions
        function cambiarFiltroTab(tipoTab) {
            if (vm.tipoTab === tipoTab)
                return;
            vm.tipoTab = tipoTab;
            vm.tipoTab === 'Centro de Ayuda' ? vm.listarAyudas() : vm.listarVideos();
        }

        function agregarNuevaAyuda() {
            if (!vm.tipoTab)
                return toastr.warning("Seleccione una aba antes de agregar una nueva ayuda");

            const tiposAyuda = {
                'Centro de Ayuda': crearTema,
                'Videos': crearItemAyudaVideo
            }

            tiposAyuda[vm.tipoTab]();
        }

        function crearTema() {
            const params = {
                Title: "Adicionar Tema General",
                TipoAyuda: AyudaTipoConstante.AyudaGeneral
            }

            _crearEditarItemAyuda(params);
        }

        function crearSubTema({ $row }) {
            const params = {
                Title: "Adicionar Tema Específico",
                TipoAyuda: AyudaTipoConstante.AyudaEspecifico,
                TemaGeneralId: $row.Id
            }

            _crearEditarItemAyuda(params);
        }

        function crearItemAyudaVideo() {
            const params = {
                Title: "Adicionar Video",
                TipoAyuda: AyudaTipoConstante.AyudaVideo
            }

            _crearEditarItemAyuda(params);
        }

        function actualizarTema({ $row }) {
            const params = {
                Title: "Actualizar Tema General",
                TipoAyuda: AyudaTipoConstante.AyudaGeneral,
                Item: $row
            }

            _crearEditarItemAyuda(params);
        }

        function actualizarSubTema({ $row }) {
            const params = {
                Title: "Actualizar Tema Específico",
                TipoAyuda: AyudaTipoConstante.AyudaEspecifico,
                Item: $row,
                TemaGeneralId: $row.TemaGeneralId
            }

            _crearEditarItemAyuda(params);
        }

        function actualizarItemAyudaVideo({ $row }) {
            const params = {
                Title: "Actualizar Video",
                TipoAyuda: AyudaTipoConstante.AyudaVideo,
                Item: $row
            }

            _crearEditarItemAyuda(params);
        }

        function _crearEditarItemAyuda(params) {
            const modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/centroAyuda/modales/crearEditarcentroAyudaItem/crearEditarCentroAyudaItem.html',
                controller: 'crearEditarCentroAyudaItemController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "modal__centro__ayuda",
                resolve: {
                    params: params
                },
            });

            modalInstance.result.then(() => vm.tipoTab === 'Centro de Ayuda' ? vm.listarAyudas() : vm.listarVideos());
        }

        function eliminarTema({ $row, TipoAyuda }) {
            swal({
                title: "",
                text: "¿Realmente quieres eliminar el registro?",
                type: "error",
                closeOnConfirm: true,
                html: true,
                showCancelButton: true,
            }, (isConfirm) => {
                if (isConfirm) {
                    servicioCentroAyuda.eliminarAyudaTema($row.Id)
                        .then(() => {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            vm.tipoTab === 'Centro de Ayuda' ? vm.listarAyudas() : vm.listarVideos();
                            $rootScope.$broadcast('OnDeleteItemAyuda', {
                                TipoAyuda: TipoAyuda,
                                ItemAyuda: $row
                            });
                        })
                        .catch(error => {
                            if (error.status == 400) {
                                const { Data } = error.data;
                                _mostarToast(Data || ["Hubo un error al eliminar el registro"]);
                                return;
                            }

                            toastr.error("Hubo un error al eliminar el registro");
                        });
                }
            });
        }
    }

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').controller('centroAyudaController', centroAyudaController);
})();
