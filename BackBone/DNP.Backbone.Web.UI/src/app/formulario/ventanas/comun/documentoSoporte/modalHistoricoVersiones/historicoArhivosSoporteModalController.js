(function () {
    'use strict';

    angular.module('backbone')
        .controller('historicoArhivosSoporteModalController', historicoArhivosSoporteModalController);

    historicoArhivosSoporteModalController.$inject = [
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
        '$uibModal',
        'appSettings',
        'documentoSoporteServicios',
        '$sessionStorage'
    ];

    function historicoArhivosSoporteModalController(
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
        $uibModal,
        appSettings,
        documentoSoporteServicios,
        $sessionStorage
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
        vm.totalRegSeleccionados = 0;
        vm.listaArchivosUltimasVersiones = [];
        vm.listadocumentosObligatorios = [];
        vm.IdInstanciaProyecto = 0;
        vm.listaArchivosVersionesAnteriores = [];
        vm.buscar = buscar;
        vm.disabled = false;
        vm.showBtn = true;
        $scope.datos = [];
        $scope.currentPage = 0;
        $scope.pageSize = 4;
        $scope.pages = [];
        vm.restablecerBusqueda = restablecerBusqueda;
        vm.seleccionarTodos = seleccionarTodos;
        vm.generarDescargaArchivos = generarDescargaArchivos;
        vm.limpiarSeleccionDescarga = limpiarSeleccionDescarga;
        vm.todosSeleccionados = false;
        vm.listaArchivos = [];
        vm.totalRegistrosObligatorio = [];
        vm.totalRegistros = 0;
        vm.proyectoId = $sessionStorage.proyectoId;

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

        $scope.configPages = function () {
            if (vm.listaArchivosUltimasVersiones !== undefined && vm.listaArchivosUltimasVersiones.length !== 0) {
                $scope.pages.length = 0;
                var ini = $scope.currentPage - 3;
                var fin = $scope.currentPage + 5;
                if (ini < 1) {
                    ini = 1;
                    fin = Math.ceil(vm.listaArchivosUltimasVersiones.length / $scope.pageSize);
                } else {
                    if (ini >= Math.ceil(vm.listaArchivosUltimasVersiones.length / $scope.pageSize) - 6) {
                        ini = Math.ceil(vm.listaArchivosUltimasVersiones.length / $scope.pageSize) - 6;
                        fin = Math.ceil(vm.listaArchivosUltimasVersiones.length / $scope.pageSize);
                    }
                }

                if (ini < 1) ini = 1;
                for (var i = ini; i <= fin; i++) {
                    $scope.pages.push({
                        no: i
                    });
                }

                if ($scope.currentPage >= $scope.pages.length)
                    $scope.currentPage = $scope.pages.length - 1;
            }
        };

        $scope.setPage = function (index) {
            $scope.currentPage = index - 1;
            setTimeout(function () {
                chgPage();
            }, 500);
        };

        function chgPage() {
            var isValid = true;
        }

        $scope.startFromGrid = function () {
            return function (input, start) {
                start = +start;
                return input.slice(start);
            }
        }

        async function init() {
            try {
                vm.IdInstanciaProyecto = IdInstancia;
                consultarArchivosTramite();
            }
            catch (err) {
                console.log('Ocurrió un error al intentar recuperar la lista de archivos ' + err);
            }
        }

        function restablecerBusqueda() {
            limpiarCamposFiltro();
            limpiarBusqueda();
        }

        function limpiarCamposFiltro() {
            if (vm.ejecutorFiltro != undefined) {
                vm.ejecutorFiltro.documento = '';
                vm.ejecutorFiltro.tipoDocumentodId = null;
                vm.ejecutorFiltro.tipoRolId = null;
                vm.ejecutorFiltro.tipoPasosdId = null;
                vm.ejecutorFiltro.tipoPasosdOrigenId = null;
                vm.ejecutorFiltro.fechaHasta = null;
                vm.ejecutorFiltro.fechaDesde = null;
            }
        }

        function ObtenerArchivosPIIP() {
            // Archivos PIIP
            let param = {
                //idInstancia: IdInstancia,
                //section: undefined,
                //idAccion: undefined,
                //idNivel: undefined,
                //idRol: undefined
                proyectoId: vm.proyectoId.toString()
            };

            return documentoSoporteServicios.ObtenerListadoArchivosPIIP(param, "tramites").then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.tieneArchivosAdjuntos = false;
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    response.forEach(archivo => {
                        if (archivo.status !== 'Eliminado') {
                            if (archivo.nombre.length > 60) {
                                var descripcionTmp = archivo.nombre.split(" ");
                                archivo.nombre = '';
                                descripcionTmp.map(function (item) {
                                    var ctaTmp = 0;
                                    if (item.length > 60) {
                                        ctaTmp = Math.floor(item.length / 60);
                                        for (var i = 0; i < ctaTmp; i++) {
                                            archivo.nombre += item.substring(60 * i, 60) + " ";
                                        }
                                    }
                                    if ((item.length % 50) > 0) {
                                        archivo.nombre += item.substring(60 * ctaTmp, (60 * ctaTmp) + 60) + " ";
                                    }
                                });
                                descripcionTmp = archivo.metadatos.descripcion.split(" ");
                                archivo.metadatos.descripcion = '';
                                descripcionTmp.map(function (item) {
                                    var ctaTmp = 0;
                                    if (item.length > 60) {
                                        ctaTmp = Math.floor(item.length / 60);
                                        for (var i = 0; i < ctaTmp; i++) {
                                            archivo.metadatos.descripcion += item.substring(60 * i, 60) + " ";
                                        }
                                    }
                                    if ((item.length % 50) > 0) {
                                        archivo.metadatos.descripcion += item.substring(60 * ctaTmp, (60 * ctaTmp) + 60) + " ";
                                    }
                                });
                            }
                            vm.listaArchivos.push({
                                origen: 'PIIP',
                                codigoProceso: archivo.metadatos.codigoproceso,
                                descripcion: archivo.metadatos.descripcion,
                                fecha: moment(archivo.fecha).format("DD/MM/YYYY"),
                                nombreArchivo: archivo.nombre,
                                usuario: archivo.metadatos.usuario === undefined ? 'No registrado' : archivo.metadatos.usuario,
                                tipoDocumento: archivo.metadatos.tipodocumento === undefined ? '' : archivo.metadatos.tipodocumento,
                                tipoDocumentoId: archivo.metadatos.tipodocumentoid === undefined ? '' : archivo.metadatos.tipodocumentoid,
                                datosDocumento: archivo.metadatos.datosdocumento === undefined ? '' : archivo.metadatos.datosdocumento,
                                nombreDocumento: archivo.nombre,
                                idArchivoBlob: archivo.metadatos.idarchivoblob,
                                ContenType: archivo.metadatos.contenttype,
                                idMongo: archivo.id,
                                nombre: archivo.nombre,
                                versiondocumentosoporte: archivo.metadatos.versiondocumentosoporte === undefined ? '1' : archivo.metadatos.versiondocumentosoporte,
                                pasoDocumentoSoporte: archivo.metadatos.pasodocumento === undefined ? 'No definido' : archivo.metadatos.pasodocumento
                            });
                            if (vm.listaArchivos.length > 0) {
                                vm.titulogrilla = vm.tituloconregistros;
                                vm.totalRegistros++;
                            } else {
                                vm.titulogrilla = vm.titulosinregistros;
                                vm.totalRegistros = 0;
                            }
                        }
                    });
                }
            });
        }

        function ObtenerArchivosSUIFP() {
            // Archivos SUIFP
            vm.filtro = {
                proyectoId: vm.proyectoId.toString(),
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

            return documentoSoporteServicios.ObtenerListadoArchivosSUIFP(vm.filtro)
                .then(function (response) {
                    console.log("Archivos SUIFP");
                    console.log(response);
                    if (response.data != null) {
                        vm.listaDatos = response.data.Documentos;
                        //vm.totalRegistros = vm.listaDatos.length;
                        //vm.listaOrigenes = response.data.Origenes;
                        //vm.listaVigencias = response.data.Vigencias;
                        //vm.listaPeriodos = response.data.Periodos;
                        //vm.listaTiposDocumento = response.data.TiposDocumento;
                        //vm.listaProcesos = response.data.ProcesosOrigen;

                        console.log(vm.listaDatos);
                        var tipoDoc = 0;

                        vm.listaDatos.forEach(archivo => {
                            tipoDoc = tipoDoc + 1;
                            var tipoDocDesc = 'SUIFP00' + tipoDoc;

                            vm.listaArchivos.push({
                                origen: 'SUIFP',
                                codigoProceso: archivo.metadatos.codigoproceso === undefined ? 'No registrado' : archivo.metadatos.codigoproceso,
                                descripcion: archivo.metadatos.descripcion === undefined ? 'Sin descripcion' : archivo.metadatos.descripcion,
                                fecha: moment(archivo.fecha).format("DD/MM/YYYY"),
                                nombreArchivo: archivo.nombre,
                                usuario: archivo.metadatos.usuario === undefined ? 'No registrado' : archivo.metadatos.usuario,
                                tipoDocumento: archivo.metadatos.tipodocumento === undefined ? tipoDocDesc : archivo.metadatos.tipodocumento,
                                tipoDocumentoId: archivo.metadatos.tipodocumentoid === undefined ? '' : archivo.metadatos.tipodocumentoid,
                                datosDocumento: archivo.metadatos.datosdocumento === undefined ? '' : archivo.metadatos.datosdocumento,
                                nombreDocumento: archivo.nombre,
                                idArchivoBlob: archivo.metadatos.idarchivoblob,
                                ContenType: archivo.metadatos.contenttype,
                                idMongo: archivo.id,
                                nombre: archivo.nombre,
                                versiondocumentosoporte: archivo.metadatos.versiondocumentosoporte === undefined ? '1' : archivo.metadatos.versiondocumentosoporte,
                                pasoDocumentoSoporte: archivo.metadatos.pasodocumento === undefined ? 'No definido' : archivo.metadatos.pasodocumento
                            });
                            if (vm.listaArchivos.length > 0) {
                                vm.titulogrilla = vm.tituloconregistros;
                                vm.totalRegistros++;
                            } else {
                                vm.titulogrilla = vm.titulosinregistros;
                                vm.totalRegistros = 0;
                            }
                        });
                    }
                    console.log("No data");
                });
        }

        function ObtenerArchivosMGA() {

            // Archivos MGA
            vm.filtro = {
                status: "Nuevo",
                idobjetonegocio: vm.proyectoId.toString()
            };

            return documentoSoporteServicios.ObtenerListadoArchivosMGA(vm.filtro, "ArchivosMGA")
                .then(function (response) {
                    if (response === undefined || typeof response === 'string') {
                        vm.tieneArchivosAdjuntos = false;
                    } else {
                        var tipoDoc = 0;

                        response.forEach(archivo => {
                            tipoDoc = tipoDoc + 1;
                            var tipoDocDesc = 'MGA00' + tipoDoc;

                            vm.listaArchivos.push({
                                origen: 'MGA',
                                codigoProceso: archivo.metadatos.codigoproceso === undefined ? 'No registrado' : archivo.metadatos.codigoproceso,
                                descripcion: archivo.metadatos.descripcion === undefined ? 'Sin descripcion' : archivo.metadatos.descripcion,
                                fecha: moment(archivo.fecha).format("DD/MM/YYYY"),
                                nombreArchivo: archivo.nombre,
                                usuario: archivo.metadatos.usuario === undefined ? 'No registrado' : archivo.metadatos.usuario,
                                tipoDocumento: archivo.metadatos.tipodocumento === undefined ? tipoDocDesc : archivo.metadatos.tipodocumento,
                                tipoDocumentoId: archivo.metadatos.tipodocumentoid === undefined ? '' : archivo.metadatos.tipodocumentoid,
                                datosDocumento: archivo.metadatos.datosdocumento === undefined ? '' : archivo.metadatos.datosdocumento,
                                nombreDocumento: archivo.nombre,
                                idArchivoBlob: archivo.metadatos.idarchivoblob,
                                ContenType: archivo.metadatos.contenttype,
                                idMongo: archivo.id,
                                nombre: archivo.nombre,
                                versiondocumentosoporte: archivo.metadatos.versiondocumentosoporte === undefined ? '1' : archivo.metadatos.versiondocumentosoporte,
                                pasoDocumentoSoporte: archivo.metadatos.pasodocumento === undefined ? 'No definido' : archivo.metadatos.pasodocumento
                            });
                            if (vm.listaArchivos.length > 0) {
                                vm.titulogrilla = vm.tituloconregistros;
                                vm.totalRegistros++;
                            } else {
                                vm.titulogrilla = vm.titulosinregistros;
                                vm.totalRegistros = 0;
                            }
                        });
                    }
                });
        }

        async function consultarArchivosTramite() {
            vm.listaArchivos = [];
            vm.totalRegistrosObligatorio = [];
            vm.totalRegistros = 0;

            try {
                await ObtenerArchivosPIIP();
                await ObtenerArchivosMGA();
                await ObtenerArchivosSUIFP();

                const archivosOrganizados = organizarArchivosPorVersiones(vm.listaArchivos);
                vm.listaArchivosVersionesAnteriores = archivosOrganizados.versionesAnteriores;
                console.log(archivosOrganizados);
            } catch (error) {
                console.error("Error al consultar archivos del trámite:", error);
            }
        }

        function organizarArchivosPorVersiones(llistaArchivos) {
            // Crear un mapa para agrupar archivos por tipo de documento
            const archivosPorTipo = {};
            const archivosPorRol = {};
            const archivosPorPasos = {};
            const archivosPorOrigen = {};

            vm.totalRegistros = 0;
            vm.listaArchivosUltimasVersiones = [];
            vm.listaArchivosUltimasVersionesOriginal = [];
            vm.listadoArchivosPorTipo = [];
            vm.listadoArchivosPorRol = [];
            vm.listadoArchivosPorPasos = [];
            vm.listadoArchivosPorOrigen = [];

            llistaArchivos.forEach(registroarchivo => {
                const tipo = registroarchivo.tipoDocumento;

                // Si no existe la clave para este tipo de documento, crear un array vacío
                if (!archivosPorTipo[tipo]) {
                    archivosPorTipo[tipo] = [];
                    vm.listadoArchivosPorTipo.push({
                        Id: registroarchivo.tipoDocumentoId === undefined ? '' : registroarchivo.tipoDocumentoId,
                        Name: registroarchivo.tipoDocumento === undefined ? '' : registroarchivo.tipoDocumento
                    });
                }

                // Agregar archivo a la lista de su tipo correspondiente
                archivosPorTipo[tipo].push(registroarchivo);

            });

            //Código para llenar el combo de Tipos de Rol
            var contadorRol = 0;
            llistaArchivos.forEach(registroarchivorol => {
                const tipoRol = registroarchivorol.datosDocumento;

                if (tipoRol !== '') {
                    var partes = tipoRol.match(/^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})\.\s*(.*)$/);

                    if (partes === null) {
                        const regex = /^(\d{2}\/\d{2}\/\d{4} \d{2}:\d{2})\.\s*(.*)$/;
                        partes = tipoRol.match(regex);
                    }

                    // Asigna las partes a variables diferentes
                    var fechaRol = partes[1];
                    var textoRol = partes[2].trim();

                    // Si no existe la clave para este tipo de documento, crear un array vacío
                    if (!archivosPorRol[textoRol]) {
                        archivosPorRol[textoRol] = [];
                        vm.listadoArchivosPorRol.push({
                            Id: contadorRol,
                            Name: textoRol
                        });

                        contadorRol = contadorRol + 1;
                    }

                    // Agregar archivo a la lista de su tipo correspondiente
                    archivosPorRol[textoRol].push(registroarchivorol);
                }
            });

            //Código para llenar la lista del combo de origen
            var contadorOrigen = 0;
            llistaArchivos.forEach(registroarchivoorigen => {
                const tipoOrigen = registroarchivoorigen.origen;

                // Si no existe la clave para este tipo de documento, crear un array vacío
                if (!archivosPorOrigen[tipoOrigen]) {
                    archivosPorOrigen[tipoOrigen] = [];
                    vm.listadoArchivosPorOrigen.push({
                        Id: contadorOrigen,
                        Name: tipoOrigen
                    });

                    contadorOrigen = contadorOrigen + 1;
                }

                // Agregar archivo a la lista de su tipo correspondiente
                archivosPorOrigen[tipoOrigen].push(registroarchivoorigen);
            });

            //Código para llenar la lista del combo de Pasos
            var contadorPasos = 0;
            llistaArchivos.forEach(registroarchivopasos => {
                const tipoPasos = registroarchivopasos.pasoDocumentoSoporte;

                // Si no existe la clave para este tipo de documento, crear un array vacío
                if (!archivosPorPasos[tipoPasos]) {
                    archivosPorPasos[tipoPasos] = [];
                    vm.listadoArchivosPorPasos.push({
                        Id: contadorPasos,
                        Name: tipoPasos
                    });

                    contadorPasos = contadorPasos + 1;
                }

                // Agregar archivo a la lista de su tipo correspondiente
                archivosPorPasos[tipoPasos].push(registroarchivopasos);
            });

            // Resultado final
            const resultado = {
                ultimasVersiones: [],
                versionesAnteriores: []
            };

            // Para cada grupo de archivos de un tipo de documento
            for (const tipo in archivosPorTipo) {
                // Ordenar archivos de este tipo por versión descendente (de mayor a menor)
                archivosPorTipo[tipo].sort((a, b) => b.versiondocumentosoporte - a.versiondocumentosoporte);

                // Tomar la primera versión (la más reciente)
                const ultimaVersion = archivosPorTipo[tipo][0];
                resultado.ultimasVersiones.push(ultimaVersion);
                var tieneVersionesAnteriores = false;

                // Tomar las versiones anteriores (si existen)
                const anteriores = archivosPorTipo[tipo].slice(1);
                if (anteriores.length > 0) {
                    resultado.versionesAnteriores = resultado.versionesAnteriores.concat(ultimaVersion);
                    resultado.versionesAnteriores = resultado.versionesAnteriores.concat(anteriores);
                    tieneVersionesAnteriores = true;
                }

                vm.listaArchivosUltimasVersiones.push({
                    origen: ultimaVersion.origen,
                    codigoProceso: ultimaVersion.codigoProceso,
                    descripcion: ultimaVersion.descripcion,
                    fecha: ultimaVersion.fecha,
                    nombreArchivo: ultimaVersion.nombreArchivo,
                    usuario: ultimaVersion.usuario,
                    tipoDocumento: ultimaVersion.tipoDocumento === undefined ? '' : ultimaVersion.tipoDocumento,
                    tipoDocumentoId: ultimaVersion.tipoDocumentoId === undefined ? '' : ultimaVersion.tipoDocumentoId,
                    datosDocumento: ultimaVersion.datosDocumento,
                    nombreDocumento: ultimaVersion.nombreDocumento,
                    idArchivoBlob: ultimaVersion.idArchivoBlob,
                    ContenType: ultimaVersion.ContenType,
                    idMongo: ultimaVersion.idMongo,
                    nombre: ultimaVersion.nombre,
                    versionesAnteriores: tieneVersionesAnteriores,
                    pasoDocumentoSoporte: ultimaVersion.pasoDocumentoSoporte,
                    versionDocumentoSoporte: ultimaVersion.versiondocumentosoporte,
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
            vm.listaArchivosUltimasVersionesOriginal = vm.listaArchivosUltimasVersiones;
            vm.listaTipoDocumentoSoporte = vm.listadoArchivosPorTipo;
            vm.listaTipoRolDocumentoSoporte = vm.listadoArchivosPorRol;
            vm.listaTipoPasosDocumentoSoporte = vm.listadoArchivosPorPasos;
            vm.listadoArchivosPorOrigen = vm.listadoArchivosPorOrigen;
            $scope.configPages();

            return resultado;
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

                vm.listaArchivosUltimasVersiones.push({
                    codigoProceso: regseleccionado.codigoProceso,
                    descripcion: regseleccionado.descripcion,
                    fecha: regseleccionado.fecha,
                    nombreArchivo: regseleccionado.nombreArchivo,
                    tipoDocumento: regseleccionado.tipoDocumento === undefined ? '' : regseleccionado.tipoDocumento,
                    tipoDocumentoId: regseleccionado.tipoDocumentoId === undefined ? '' : regseleccionado.tipoDocumentoId,
                    datosDocumento: regseleccionado.datosDocumento,
                    nombreDocumento: regseleccionado.nombreDocumento,
                    idArchivoBlob: regseleccionado.idArchivoBlob,
                    ContenType: regseleccionado.ContenType,
                    idMongo: regseleccionado.idMongo,
                    nombre: regseleccionado.nombre,
                    versionesAnteriores: regseleccionado.versionesAnteriores,
                    pasoDocumentoSoporte: regseleccionado.pasoDocumentoSoporte,
                    origen: regseleccionado.origen,
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

                vm.listaArchivosUltimasVersiones.push({
                    codigoProceso: regarchivo.codigoProceso,
                    descripcion: regarchivo.descripcion,
                    fecha: regarchivo.fecha,
                    nombreArchivo: regarchivo.nombreArchivo,
                    tipoDocumento: regarchivo.tipoDocumento === undefined ? '' : regarchivo.tipoDocumento,
                    tipoDocumentoId: regarchivo.tipoDocumentoId === undefined ? '' : regarchivo.tipoDocumentoId,
                    datosDocumento: regarchivo.datosDocumento,
                    nombreDocumento: regarchivo.nombreDocumento,
                    idArchivoBlob: regarchivo.idArchivoBlob,
                    ContenType: regarchivo.ContenType,
                    idMongo: regarchivo.idMongo,
                    nombre: regarchivo.nombre,
                    versionesAnteriores: regarchivo.versionesAnteriores,
                    pasoDocumentoSoporte: regarchivo.pasoDocumentoSoporte,
                    origen: regarchivo.origen,
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

        function listarResultadoBusqueda(tipoDocumentoSoporteIdNumerico, fechaDesde, fechaHasta, tipoRolDescripcion, tipoPasosDescripcion, tipoOrigenDescripcion, textoNombreDocumentoSoporte) {
            vm.listaArchivosUltimasVersiones = vm.listaArchivosUltimasVersionesOriginal;
            vm.totalRegistros = 0;
            vm.totalRegSeleccionados = 0;
            const listadoRegistroArchivos = vm.listaArchivosUltimasVersiones;
            vm.listaArchivosUltimasVersiones = [];

            listadoRegistroArchivos.forEach(regarchivo => {
                var fechaBusqueda = new Date(regarchivo.fecha.split('/').reverse().join('-'));
                const tipoRolBusqueda = regarchivo.datosDocumento;
                var partesBusqueda = tipoRolBusqueda.match(/^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})\.\s*(.*)$/);

                // Asigna las partes a variables diferentes
                if (partesBusqueda === null) {
                    const regex1 = /^(\d{2}\/\d{2}\/\d{4} \d{2}:\d{2})\.\s*(.*)$/;
                    partesBusqueda = tipoRolBusqueda.match(regex1);
                }

                if (partesBusqueda != null)
                    var textoRolBusqueda = partesBusqueda[2].trim();

                var cadenaNombreArchivoProyecto = regarchivo.nombreArchivo.toLowerCase();
                var contenidoNombreArchivo = cadenaNombreArchivoProyecto.includes(textoNombreDocumentoSoporte);

                if ((contenidoNombreArchivo === true || textoNombreDocumentoSoporte === '') && (tipoPasosDescripcion === regarchivo.pasoDocumentoSoporte || tipoPasosDescripcion === '') && (tipoOrigenDescripcion === regarchivo.origen || tipoOrigenDescripcion === '') && (tipoRolDescripcion === textoRolBusqueda || tipoRolDescripcion === '') && (tipoDocumentoSoporteIdNumerico === regarchivo.tipoDocumentoId || tipoDocumentoSoporteIdNumerico === 0) && (fechaBusqueda >= fechaDesde && fechaBusqueda <= fechaHasta)) {
                    vm.listaArchivosUltimasVersiones.push({
                        codigoProceso: regarchivo.codigoProceso,
                        descripcion: regarchivo.descripcion,
                        fecha: regarchivo.fecha,
                        nombreArchivo: regarchivo.nombreArchivo,
                        tipoDocumento: regarchivo.tipoDocumento === undefined ? '' : regarchivo.tipoDocumento,
                        tipoDocumentoId: regarchivo.tipoDocumentoId === undefined ? '' : regarchivo.tipoDocumentoId,
                        datosDocumento: regarchivo.datosDocumento,
                        nombreDocumento: regarchivo.nombreDocumento,
                        idArchivoBlob: regarchivo.idArchivoBlob,
                        ContenType: regarchivo.ContenType,
                        idMongo: regarchivo.idMongo,
                        nombre: regarchivo.nombre,
                        versionesAnteriores: regarchivo.versionesAnteriores,
                        pasoDocumentoSoporte: regarchivo.pasoDocumentoSoporte,
                        origen: regarchivo.origen,
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
            // var nit = document.getElementById("txtNit").value;
            var tipoDocumentoSoporteId = document.getElementById("ddlTipoDocumento").value;
            tipoDocumentoSoporteId = tipoDocumentoSoporteId.replace('number:', '');

            var tipoRolDescripcion = document.getElementById("ddlTipoRol").value;
            tipoRolDescripcion = tipoRolDescripcion.replace('number:', '');

            var tipoRolDescripcioNombre = '';

            var tipoPasosDescripcion = document.getElementById("ddlTipoPasos").value;
            tipoPasosDescripcion = tipoPasosDescripcion.replace('number:', '');

            var tipoPasosDescripcionNombre = '';

            var tipoOrigenDescripcion = document.getElementById("ddlTipoOrigen").value;
            tipoOrigenDescripcion = tipoOrigenDescripcion.replace('number:', '');

            var tipoOrigenDescripcionNombre = '';

            var objFechaDesde = document.getElementById("ddlFechaDesde").value;
            var objFechaHasta = document.getElementById("ddlFechaHasta").value;

            var textoNombreDocumentoSoporte = document.getElementById("txtNombreDocumento").value;

            if (tipoDocumentoSoporteId === '?' && tipoRolDescripcion === '?' && tipoPasosDescripcion === '?' && tipoOrigenDescripcion === '?' && objFechaDesde === '' && objFechaHasta === '' && textoNombreDocumentoSoporte === '') {
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

                if (tipoDocumentoSoporteId === '?') {
                    tipoDocumentoSoporteId = '0';
                }

                if (tipoRolDescripcion === '?') {
                    tipoRolDescripcion = '';
                }
                else {
                    var tipoRolNumerico = parseInt(tipoRolDescripcion, 10);
                    vm.listaTipoRolDocumentoSoporte.forEach(regroles => {
                        const idRolRegistro = regroles.Id;

                        if (idRolRegistro === tipoRolNumerico) {
                            tipoRolDescripcioNombre = regroles.Name;
                        }
                    });
                }

                if (tipoPasosDescripcion === '?') {
                    tipoPasosDescripcion = '';
                }
                else {
                    var tipoPasoNumerico = parseInt(tipoPasosDescripcion, 10);
                    vm.listaTipoPasosDocumentoSoporte.forEach(regpasos => {
                        const idPaso = regpasos.Id;

                        if (idPaso === tipoPasoNumerico) {
                            tipoPasosDescripcionNombre = regpasos.Name;
                        }
                    });
                }

                if (tipoOrigenDescripcion === '?') {
                    tipoOrigenDescripcion = '';
                }
                else {
                    var tipoOrigenNumerico = parseInt(tipoOrigenDescripcion, 10);
                    vm.listadoArchivosPorOrigen.forEach(regorigen => {
                        const idOrigen = regorigen.Id;

                        if (idOrigen === tipoOrigenNumerico) {
                            tipoOrigenDescripcionNombre = regorigen.Name;
                        }
                    });
                }

                var tipoDocumentoSoporteIdNumerico = parseInt(tipoDocumentoSoporteId, 10);
                listarResultadoBusqueda(tipoDocumentoSoporteIdNumerico, fechaDesde, fechaHasta, tipoRolDescripcioNombre, tipoPasosDescripcionNombre, tipoOrigenDescripcionNombre, textoNombreDocumentoSoporte);
            }
        }

        function generarDescargaArchivos() {

            const listadoRegistroSeleccionados = vm.listaArchivosUltimasVersiones;

            listadoRegistroSeleccionados.forEach(regseleccionado => {
                if (regseleccionado.seleccionado === true) {
                    descargarArchivoBlob(regseleccionado);
                }
            });
        }

        function limpiarSeleccionDescarga() {
            vm.listaArchivosUltimasVersiones = vm.listaArchivosUltimasVersionesOriginal;
            const listadoRegistroSeleccionados = vm.listaArchivosUltimasVersiones;
            vm.listaArchivosUltimasVersiones = [];
            vm.totalRegistros = 0;
            vm.totalRegSeleccionados = 0;
            var estadoColumnaSeleccion = false;

            listadoRegistroSeleccionados.forEach(regseleccionado => {

                vm.listaArchivosUltimasVersiones.push({
                    codigoProceso: regseleccionado.codigoProceso,
                    descripcion: regseleccionado.descripcion,
                    fecha: regseleccionado.fecha,
                    nombreArchivo: regseleccionado.nombreArchivo,
                    tipoDocumento: regseleccionado.tipoDocumento === undefined ? '' : regseleccionado.tipoDocumento,
                    tipoDocumentoId: regseleccionado.tipoDocumentoId === undefined ? '' : regseleccionado.tipoDocumentoId,
                    datosDocumento: regseleccionado.datosDocumento,
                    nombreDocumento: regseleccionado.nombreDocumento,
                    idArchivoBlob: regseleccionado.idArchivoBlob,
                    ContenType: regseleccionado.ContenType,
                    idMongo: regseleccionado.idMongo,
                    nombre: regseleccionado.nombre,
                    versionesAnteriores: regseleccionado.versionesAnteriores,
                    pasoDocumentoSoporte: regseleccionado.pasoDocumentoSoporte,
                    origen: regseleccionado.origen,
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
            var coleccion = "tramites";

            if (entity.origen == "MGA")
                coleccion = "ArchivosMGA";

            archivoServicios.obtenerArchivoBytes(entity.idArchivoBlob, coleccion).then(function (retorno) {
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

        vm.verLogVersionesAnteriores = function (cadenaTipoDocumento, cadenaNombreTipoDocumento, nombreDocumentoUltimaVersion, nombrepasoDocumentoSoporte, versionPasoUltimaVersion) {
            console.log(vm.IdInstanciaProyecto);
            console.log(cadenaTipoDocumento);
            $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/formulario/ventanas/comun/documentoSoporte/modalHistoricoVersiones/historicoVersionesAnterioresModal.html',
                controller: 'historicoVersionesAnterioresModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    IdInstancia: () => vm.IdInstanciaProyecto,
                    IdNivel: () => vm.IdNivel,
                    CodigoProceso: () => cadenaTipoDocumento,
                    NombreProceso: () => cadenaNombreTipoDocumento,
                    IdObjetoNegocio: () => 4,
                    IdAccion: () => 4,
                    listaAnteriores: () => vm.listaArchivosVersionesAnteriores,
                    archivoUltimaVersion: () => nombreDocumentoUltimaVersion,
                    pasoArchivoUltimaVersion: () => nombrepasoDocumentoSoporte,
                    versionUltimaVersion: () => versionPasoUltimaVersion
                },
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

                console.log(vm.gridOptions)

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