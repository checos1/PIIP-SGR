(function () {
    'use strict';

    angular.module('backbone')
        .controller('historicoVersionesAnterioresModalController', historicoVersionesAnterioresModalController);

    historicoVersionesAnterioresModalController.$inject = [
        '$scope',
        '$uibModalInstance',
        '$filter',
        'IdInstancia',
        'IdNivel',
        'CodigoProceso',
        'NombreProceso',
        'IdObjetoNegocio',
        'archivoServicios',
        'utilidades',
        'IdAccion',
        'listaAnteriores',
        'archivoUltimaVersion',
        'pasoArchivoUltimaVersion',
        'versionUltimaVersion'
    ];

    function historicoVersionesAnterioresModalController(
        $scope,
        $uibModalInstance,
        $filter,
        IdInstancia,
        IdNivel,
        CodigoProceso,
        NombreProceso,
        IdObjetoNegocio,
        archivoServicios,
        utilidades,
        IdAccion,
        listaAnteriores,
        archivoUltimaVersion,
        pasoArchivoUltimaVersion,
        versionUltimaVersion
    ) {
        const vm = this;
        vm.columnas = ["Origen", "Tipo Documento", "Documento", "Descripcion", "", "", ""];
        vm.buscando = false;
        vm.sinResultados = false;
        vm.lang = "es";
        vm.mostrarMensajeProyectos = false;
        vm.Mensaje = "";
        vm.codigoProceso = CodigoProceso;
        vm.nombreProceso = NombreProceso;
        vm.idObjetoNegocio = IdObjetoNegocio;
        vm.tituloconregistros = 'Archivos cargados';
        vm.titulogrilla = vm.titulosinregistros = 'Aún no se han agregado archivos al paso actual';
        vm.totalRegistros = 0;
        vm.totalRegSeleccionados = 0;
        vm.listaArchivos = [];
        vm.listaArchivosUltimasVersiones = [];
        vm.listadocumentosObligatorios = [];
        vm.listaObjetosAnteriores = [];
        vm.idTipodeDocumento = 0;
        vm.detalleArchivoUltimaVersion = "";
        vm.detallePasoArchivo = "";
        vm.versionUltimaVersion = "";
        vm.disabled = false;
        vm.buscar = buscar;
        vm.showBtn = true;
        vm.restablecerBusqueda = restablecerBusqueda;
        vm.seleccionarTodos = seleccionarTodos;
        vm.generarDescargaArchivos = generarDescargaArchivos;
        vm.limpiarSeleccionDescarga = limpiarSeleccionDescarga;
        vm.todosSeleccionados = false;
        vm.cerrar = $uibModalInstance.dismiss;

        vm.model = {
            logs: [{
                origen: "",
                tipodedocumento: "",
                caracteristicadocumento: "",
                descdocumento: "",
                tipodescarga: "",
                tipoeliminar: "",
                tipoobs: ""
            }]
        };

        vm.gridOptions;

        function onRegisterApi(gridApi) {
            $scope.gridApi = gridApi;
        }

        function mostrarResultados() {
            return vm.buscando === false && vm.sinResultados === false;
        }

        function mostrarMensajeRespuesta() {
            if (vm.Mensaje) {
                vm.mostrarMensajeProyectos = true;
            } else {
                vm.mostrarMensajeProyectos = false;
            }
        }

        function cerrar() {
            $uibModalInstance.close(false);
        }

        async function init() {
            try {
                vm.listaObjetosAnteriores = listaAnteriores;
                vm.idTipodeDocumento = CodigoProceso
                vm.detalleArchivoUltimaVersion = archivoUltimaVersion;
                vm.detallePasoArchivo = pasoArchivoUltimaVersion;
                vm.versionUltimaVersion = versionUltimaVersion;
                listarArchivosVersionesAnteriores(listaAnteriores);
            }
            catch (err) {
                console.log('Ocurrió un error al intentar recuperar la lista de archivos.' + err);
            }
        }

        function listarArchivosVersionesAnteriores(llistaArchivos) {
            // Crear un mapa para agrupar archivos por tipo de documento
            const archivosPorTipo = {};
            vm.totalRegistros = 0;
            vm.totalRegSeleccionados = 0;
            vm.listaArchivosUltimasVersiones = []
            vm.listaArchivosUltimasVersionesOriginal = []

            llistaArchivos.forEach(registroarchivo => {
                const tipo = registroarchivo.tipoDocumentoId;

                if (tipo === vm.idTipodeDocumento) {
                    const tipoRolBusqueda = registroarchivo.datosDocumento;
                    var textoRolBusqueda = '';

                    if (tipoRolBusqueda !== '') {
                        var partesBusqueda = tipoRolBusqueda.match(/^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})\.\s*(.*)$/);
                        if (partesBusqueda === null) {
                            var segmento = tipoRolBusqueda.split(' ');
                            textoRolBusqueda = segmento.slice(2).join(' ');
                        }
                        else {
                            textoRolBusqueda = partesBusqueda[2].trim();
                        }
                    }

                    vm.listaArchivosUltimasVersiones.push({
                        codigoProceso: registroarchivo.codigoProceso,
                        descripcion: registroarchivo.descripcion,
                        fecha: registroarchivo.fecha,
                        nombreArchivo: registroarchivo.nombreArchivo,
                        usuario: registroarchivo.usuario,
                        tipoDocumento: registroarchivo.tipoDocumento === undefined ? '' : registroarchivo.tipoDocumento,
                        tipoDocumentoId: registroarchivo.tipoDocumentoId === undefined ? '' : registroarchivo.tipoDocumentoId,
                        datosDocumento: registroarchivo.datosDocumento,
                        nombreDocumento: registroarchivo.nombreDocumento,
                        idArchivoBlob: registroarchivo.idArchivoBlob,
                        ContenType: registroarchivo.ContenType,
                        idMongo: registroarchivo.idMongo,
                        nombre: registroarchivo.nombre,
                        rolRegistro: textoRolBusqueda,
                        versionesAnteriores: false,
                        versionDocumentoSoporte: registroarchivo.versiondocumentosoporte
                    });
                    if (llistaArchivos.length > 0) {
                        vm.titulogrilla = vm.tituloconregistros;
                        vm.totalRegistros++;
                    }
                    else {
                        vm.titulogrilla = vm.titulosinregistros;
                    }
                }
            });

            // Resultado final
            const resultado = {
                ultimasVersiones: [],
                versionesAnteriores: []
            };

            vm.listaArchivosUltimasVersionesOriginal = vm.listaArchivosUltimasVersiones;
            return resultado;
        }

        function restablecerBusqueda() {
            limpiarCamposFiltro();
            limpiarBusqueda();
        }

        function limpiarCamposFiltro() {
            if (vm.ejecutorFiltro != undefined) {
                vm.ejecutorFiltro.documento = '';
                vm.ejecutorFiltro.fechaHasta = null;
                vm.ejecutorFiltro.fechaDesde = null;
            }
        }

        function seleccionarTodos() {
            vm.listaArchivosUltimasVersiones = vm.listaArchivosUltimasVersionesOriginal;
            const listadoRegistroSeleccionados = vm.listaArchivosUltimasVersiones;
            vm.listaArchivosUltimasVersiones = [];
            vm.totalRegistros = 0;
            vm.totalRegSeleccionados = 0;
            var estadoColumnaSeleccion = false;

            if (vm.todosSeleccionados === true) {
                estadoColumnaSeleccion = true;
            }

            listadoRegistroSeleccionados.forEach(regseleccionado => {

                const tipoRolBusqueda = regseleccionado.datosDocumento;
                var partesBusqueda = tipoRolBusqueda.match(/^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})\.\s*(.*)$/);
                var textoRolBusqueda = partesBusqueda[2].trim();

                vm.listaArchivosUltimasVersiones.push({
                    codigoProceso: regseleccionado.codigoProceso,
                    descripcion: regseleccionado.descripcion,
                    fecha: regseleccionado.fecha,
                    nombreArchivo: regseleccionado.nombreArchivo,
                    usuario: regseleccionado.usuario,
                    tipoDocumento: regseleccionado.tipoDocumento === undefined ? '' : regseleccionado.tipoDocumento,
                    tipoDocumentoId: regseleccionado.tipoDocumentoId === undefined ? '' : regseleccionado.tipoDocumentoId,
                    datosDocumento: regseleccionado.datosDocumento,
                    nombreDocumento: regseleccionado.nombreDocumento,
                    idArchivoBlob: regseleccionado.idArchivoBlob,
                    ContenType: regseleccionado.ContenType,
                    idMongo: regseleccionado.idMongo,
                    nombre: regseleccionado.nombre,
                    rolRegistro: textoRolBusqueda,
                    versionesAnteriores: regseleccionado.versionesAnteriores,
                    versionDocumentoSoporte: regseleccionado.versiondocumentosoporte,
                    seleccionado: estadoColumnaSeleccion
                });

                if (vm.listaArchivosUltimasVersiones.length > 0) {
                    vm.titulogrilla = vm.tituloconregistros;
                    vm.totalRegistros++;
                    if (estadoColumnaSeleccion === true) {
                        vm.totalRegSeleccionados++;
                    }
                }
                else {
                    vm.titulogrilla = vm.titulosinregistros;
                }
            });
        }

        function limpiarBusqueda() {
            vm.listaArchivosUltimasVersiones = vm.listaArchivosUltimasVersionesOriginal;
            vm.totalRegistros = 0;
            vm.totalRegSeleccionados = 0;
            const listadoRegistroArchivos = vm.listaArchivosUltimasVersiones;
            vm.listaArchivosUltimasVersiones = [];

            listadoRegistroArchivos.forEach(regarchivo => {
                //const tipoRolBusqueda = regarchivo.datosDocumento;
                //var partesBusqueda = tipoRolBusqueda.match(/^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})\.\s*(.*)$/);
                //var textoRolBusqueda = partesBusqueda[2].trim();

                vm.listaArchivosUltimasVersiones.push({
                    codigoProceso: regarchivo.codigoProceso,
                    descripcion: regarchivo.descripcion,
                    fecha: regarchivo.fecha,
                    nombreArchivo: regarchivo.nombreArchivo,
                    usuario: regarchivo.usuario,
                    tipoDocumento: regarchivo.tipoDocumento === undefined ? '' : regarchivo.tipoDocumento,
                    tipoDocumentoId: regarchivo.tipoDocumentoId === undefined ? '' : regarchivo.tipoDocumentoId,
                    datosDocumento: regarchivo.datosDocumento,
                    nombreDocumento: regarchivo.nombreDocumento,
                    idArchivoBlob: regarchivo.idArchivoBlob,
                    ContenType: regarchivo.ContenType,
                    idMongo: regarchivo.idMongo,
                    nombre: regarchivo.nombre,
                    /* rolRegistro: '',*/
                    versionesAnteriores: regarchivo.versionesAnteriores,
                    versionDocumentoSoporte: regarchivo.versionDocumentoSoporte,
                    seleccionado: false
                });
                if (vm.listaArchivosUltimasVersiones.length > 0) {
                    vm.titulogrilla = vm.tituloconregistros;
                    vm.totalRegistros++;
                }
                else {
                    vm.titulogrilla = vm.titulosinregistros;
                }
            });
        }

        function listarResultadoBusqueda(fechaDesde, fechaHasta, textoNombreDocumentoSoporte) {
            vm.listaArchivosUltimasVersiones = vm.listaArchivosUltimasVersionesOriginal;
            vm.totalRegistros = 0;
            vm.totalRegSeleccionados = 0;
            const listadoRegistroArchivosAnteriores = vm.listaArchivosUltimasVersiones;
            vm.listaArchivosUltimasVersiones = [];

            listadoRegistroArchivosAnteriores.forEach(regarchivo => {
                var fechaBusqueda = new Date(regarchivo.fecha.split('/').reverse().join('-'));
                const tipoRolBusqueda = regarchivo.datosDocumento;
                var partesBusqueda = tipoRolBusqueda.match(/^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})\.\s*(.*)$/);

                // Asigna las partes a variables diferentes
                var textoRolBusqueda = partesBusqueda[2].trim();
                var cadenaNombreArchivoProyecto = regarchivo.nombreArchivo.toLowerCase();
                var contenidoNombreArchivo = cadenaNombreArchivoProyecto.includes(textoNombreDocumentoSoporte);

                if ((contenidoNombreArchivo === true || textoNombreDocumentoSoporte === '') && (fechaBusqueda >= fechaDesde && fechaBusqueda <= fechaHasta)) {
                    vm.listaArchivosUltimasVersiones.push({
                        codigoProceso: regarchivo.codigoProceso,
                        descripcion: regarchivo.descripcion,
                        fecha: regarchivo.fecha,
                        nombreArchivo: regarchivo.nombreArchivo,
                        usuario: regarchivo.usuario,
                        tipoDocumento: regarchivo.tipoDocumento === undefined ? '' : regarchivo.tipoDocumento,
                        tipoDocumentoId: regarchivo.tipoDocumentoId === undefined ? '' : regarchivo.tipoDocumentoId,
                        datosDocumento: regarchivo.datosDocumento,
                        nombreDocumento: regarchivo.nombreDocumento,
                        idArchivoBlob: regarchivo.idArchivoBlob,
                        ContenType: regarchivo.ContenType,
                        idMongo: regarchivo.idMongo,
                        nombre: regarchivo.nombre,
                        rolRegistro: textoRolBusqueda,
                        versionesAnteriores: regarchivo.versionesAnteriores,
                        versionDocumentoSoporte: regarchivo.versionDocumentoSoporte,
                        seleccionado: false
                    });

                    if (vm.listaArchivosUltimasVersiones.length > 0) {
                        vm.titulogrilla = vm.tituloconregistros;
                        vm.totalRegistros++;
                    }
                    else {
                        vm.titulogrilla = vm.titulosinregistros;
                    }
                }
            });
        }

        function buscar() {
            var objFechaDesde = document.getElementById("ddlFechaDesde").value;
            var objFechaHasta = document.getElementById("ddlFechaHasta").value;
            var textoNombreDocumentoSoporte = document.getElementById("txtNombreDocumento").value;

            if (objFechaDesde === '' && objFechaHasta === '' && textoNombreDocumentoSoporte === '') {
                utilidades.mensajeError("No se han seleccionado criterios de busqueda.");
            }
            else {
                var fechaDesde = new Date(objFechaDesde);
                var fechaHasta = new Date(objFechaHasta);

                if (isNaN(fechaDesde.getTime())) {
                    fechaDesde = new Date('1980-01-01');
                }

                if (isNaN(fechaHasta.getTime())) {
                    fechaHasta = new Date();
                }

                if (textoNombreDocumentoSoporte !== '') {
                    textoNombreDocumentoSoporte = textoNombreDocumentoSoporte.toLowerCase();
                }

                listarResultadoBusqueda(fechaDesde, fechaHasta, textoNombreDocumentoSoporte);
            }
        }

        function limpiarSeleccionDescarga() {
            vm.listaArchivosUltimasVersiones = vm.listaArchivosUltimasVersionesOriginal;
            const listadoRegistroSeleccionados = vm.listaArchivosUltimasVersiones;
            vm.listaArchivosUltimasVersiones = [];
            vm.totalRegistros = 0;
            vm.totalRegSeleccionados = 0;
            var estadoColumnaSeleccion = false;

            listadoRegistroSeleccionados.forEach(regseleccionado => {

                const tipoRolBusqueda = regseleccionado.datosDocumento;
                var partesBusqueda = tipoRolBusqueda.match(/^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})\.\s*(.*)$/);
                var textoRolBusqueda = partesBusqueda[2].trim();

                vm.listaArchivosUltimasVersiones.push({
                    codigoProceso: regseleccionado.codigoProceso,
                    descripcion: regseleccionado.descripcion,
                    fecha: regseleccionado.fecha,
                    nombreArchivo: regseleccionado.nombreArchivo,
                    usuario: regseleccionado.usuario,
                    tipoDocumento: regseleccionado.tipoDocumento === undefined ? '' : regseleccionado.tipoDocumento,
                    tipoDocumentoId: regseleccionado.tipoDocumentoId === undefined ? '' : regseleccionado.tipoDocumentoId,
                    datosDocumento: regseleccionado.datosDocumento,
                    nombreDocumento: regseleccionado.nombreDocumento,
                    idArchivoBlob: regseleccionado.idArchivoBlob,
                    ContenType: regseleccionado.ContenType,
                    idMongo: regseleccionado.idMongo,
                    nombre: regseleccionado.nombre,
                    versionesAnteriores: regseleccionado.versionesAnteriores,
                    versionDocumentoSoporte: regseleccionado.versionDocumentoSoporte,
                    rolRegistro: textoRolBusqueda,
                    seleccionado: estadoColumnaSeleccion
                });

                if (vm.listaArchivosUltimasVersiones.length > 0) {
                    vm.titulogrilla = vm.tituloconregistros;
                    vm.totalRegistros++;
                }
                else {
                    vm.titulogrilla = vm.titulosinregistros;
                }
            });
            vm.todosSeleccionados = false;
        }

        vm.actualizarSeleccionados = function () {
            vm.totalRegSeleccionados = vm.listaArchivosUltimasVersiones.filter(function (objArchivo) {
                return objArchivo.seleccionado;
            }).length;
        };

        function descargarArchivoBlob(entity) {
            archivoServicios.obtenerArchivoBytes(entity.idArchivoBlob, "tramites").then(function (retorno) {
                const blob = utilidades.base64toBlob(retorno, entity.ContenType);
                const downloadUrl = URL.createObjectURL(blob);
                const a = document.createElement("a");
                a.href = downloadUrl;
                a.download = entity.nombreArchivo.trimEnd();
                document.body.appendChild(a);
                a.click();
            }, function (error) {
                utilidades.mensajeError("Error inesperado al descargar");
            });
        }

        function generarDescargaArchivos() {
            const listadoRegistroSeleccionados = vm.listaArchivosUltimasVersionesOriginal;

            listadoRegistroSeleccionados.forEach(regseleccionado => {
                if (regseleccionado.seleccionado === true) {
                    descargarArchivoBlob(regseleccionado);
                }
            });
        }

        this.$onInit = function () {

            if (!vm.gridOptions) {
                vm.gridOptions = {
                    enablePaginationControls: true,
                    useExternalPagination: false,
                    useExternalSorting: false,
                    paginationCurrentPage: 1,
                    enableVerticalScrollbar: 1,
                    enableFiltering: false,
                    showHeader: true,
                    useExternalFiltering: false,
                    paginationPageSizes: [10, 15, 25, 50, 100],
                    paginationPageSize: 10,
                    onRegisterApi: onRegisterApi
                };

                console.log(vm.gridOptions);
                vm.buscando = false;
                vm.sinResultados = false;
                mostrarMensajeRespuesta();
            }
        };

        vm.init = init;
        vm.mostrarResultados = mostrarResultados;
        vm.cerrar = cerrar;
    }
})();