var crucePoliticasTransversalesProgramacionCtrl = null;

(function () {
    'use strict';
    angular.module('backbone').controller('crucePoliticasTransversalesProgramacion', crucePoliticasTransversalesProgramacion);
    crucePoliticasTransversalesProgramacion.$inject = [
        '$scope',
        '$uibModal',
        '$log',
        '$q',
        '$sessionStorage',
        '$localStorage',
        '$timeout',
        '$location',
        '$filter',
        'comunesServicio',
        'utilidades'
    ];

    function crucePoliticasTransversalesProgramacion(
        $scope,
        $uibModal,
        $log,
        $q,
        $sessionStorage,
        $localStorage,
        $timeout,
        $location,
        $filter,
        comunesServicio,
        utilidades) {
        var vm = this;
        crucePoliticasTransversalesProgramacionCtrl = vm;
        //variables
        vm.lang = "es";
        vm.TabActivo = 1;
        vm.EditarCrucePolitica = false;
        vm.nombreComponente = "crucepoliticastransversalesprogramacion";
        vm.listaPoliticasProyecto = [];

        vm.ingresarCrucePolitica = ingresarCrucePolitica;
        vm.abrirMensajePoliticasProyecto = abrirMensajePoliticasProyecto;
        vm.obtenerPoliticasTransversales = this.obtenerPoliticasTransversales;
        vm.notificacionErrores = null;
        vm.erroresActivos = [];
        vm.validacionGuardado = null;
        vm.recargaGuardado = null;
        vm.recargaGuardadoCostos = null;
        vm.seccionCapitulo = null;
        vm.mostrarcrucepoliticas = false;

        /*Inicio Cruce*/
        vm.CancelarValores = CancelarValores;
        vm.EditarValores = EditarValores;
        vm.GuardarValores = GuardarValores;
        vm.ConvertirNumero = ConvertirNumero;
        vm.descargarPlantila = descargarPlantila;
        vm.HabilitaEditar = HabilitaEditar;
        vm.limpiarArchivo = limpiarArchivo;
        vm.validarArchivo = validarArchivo;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.nombrearchivo = "Seleccione Archivo";
        vm.RegistrarCargaCruce = RegistrarCargaCruce;
        let ValoresCruce = [];
        $("#btnCruceValidarArchivo").attr('disabled', true);
        $("#btnCruceLimpiarArchivo").attr('disabled', true);
        $("#btnCruceArchivoSeleccionado").attr('disabled', true);
        $("#btnExaminarArchivo").attr('disabled', true);
        vm.PoliticaIdP = 0;
        vm.PoliticaIdD = 0;
        vm.DimensionPrincipalId = 0;
        vm.NombrePoliticaIdP = '';
        vm.NombrePoliticaIdD = '';
        vm.cadena = '';
        /*Fin Cruce*/
        vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1

        $scope.$watch('vm.tramiteproyectoid', function () {
            if (vm.tramiteproyectoid != '') {
                var proyectoCargado = comunesServicio.getProyectoCargado();
                if (proyectoCargado.toString() === vm.proyectoid) {
                    vm.listaPoliticasProyectos = [];
                    obtenerCrucePoliticasProgramacion();
                    vm.mostrarpoliticas = true;
                    vm.mostrarcrucepoliticas = false;
                }
            }
        });

        vm.init = function () {

            vm.notificarrefresco({ handler: vm.refrescarPoliticas, nombreComponente: vm.nombreComponente });

        };

        vm.refrescarPoliticas = function () {
            vm.listaPoliticasProyectos = [];
            obtenerCrucePoliticasProgramacion();
            vm.mostrarpoliticas = true;
            vm.mostrarcrucepoliticas = false;
        }

        $scope.$watch('vm.calendariopoliticastransversales', function () {
            if (vm.calendariopoliticastransversales !== undefined && vm.calendariopoliticastransversales !== '')
                vm.habilitaBotones = vm.calendariopoliticastransversales === 'true' && !$sessionStorage.soloLectura ? true : false;
        });

        function obtenerCrucePoliticasProgramacion() {
            return comunesServicio.obtenerCrucePoliticasProgramacion(vm.tramiteproyectoid).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        var arregloGeneral = jQuery.parseJSON(arreglolistas);
                        var arregloDatosGenerales = arregloGeneral.Politicas;
                        var listaPoliticasProy = [];
                        for (var pl = 0; pl < arregloDatosGenerales.length; pl++) {
                            var politicasProyecto = {
                                PoliticaIdP: arregloDatosGenerales[pl].PoliticaIdP,
                                PoliticaIdD: arregloDatosGenerales[pl].PoliticaIdD,
                                DimensionPrincipalId: arregloDatosGenerales[pl].DimensionPrincipalId,
                                PoliticaP: arregloDatosGenerales[pl].PoliticaP,
                                PoliticaD: arregloDatosGenerales[pl].PoliticaD,
                            }
                            listaPoliticasProy.push(politicasProyecto);
                        }
                        vm.listaPoliticasProyectos = angular.copy(listaPoliticasProy);
                    }
                });
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        function abrirMensajePoliticasProyecto() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' >Políticas transversales asociadas</span>");
        }

        vm.getNombreReducido = function (data, maxCaracteres) {
            if (data.length > maxCaracteres) {
                var dataNueva = data.slice(0, maxCaracteres);
                return dataNueva + '...';
            } else return data
        }

        function ingresarCrucePolitica(PoliticaIdP, PoliticaIdD, DimensionPrincipalId, PoliticaP, PoliticaD) {
            vm.NombrePoliticaIdP = PoliticaP;
            vm.NombrePoliticaIdD = PoliticaD
            ObtenerDatosCrucePoliticas(PoliticaIdP, PoliticaIdD, DimensionPrincipalId)
            vm.mostrarcrucepoliticas = true;
            $("#btnCruceValidarArchivo").attr('disabled', true);
            $("#btnCruceLimpiarArchivo").attr('disabled', true);
            $("#btnCruceArchivoSeleccionado").attr('disabled', true);
            $("#btnExaminarArchivo").attr('disabled', true);
        }

        function ObtenerDatosCrucePoliticas(PoliticaIdP, PoliticaIdD, DimensionPrincipalId) {
            vm.PoliticaIdP = PoliticaIdP;
            vm.PoliticaIdD = PoliticaIdD;
            vm.DimensionPrincipalId = DimensionPrincipalId;
            vm.cadena = PoliticaIdP + ";" + PoliticaIdD + ":" + vm.tramiteproyectoid + "," + vm.DimensionPrincipalId
            return comunesServicio.obtenerResumenSolicitudConceptoProgramacion(vm.cadena).then(
                function (respuesta) {
                    if (respuesta.data !== '') {
                        $scope.regionalizacion = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                    }
                    else {
                        $scope.regionalizacion = [];
                    }
                });
        }
        function ObtenerDatosCrucePoliticasDetalle(cadena) {
            return comunesServicio.obtenerResumenSolicitudConceptoProgramacion(cadena).then(
                function (respuesta) {
                    if (respuesta.data !== '') {
                        $scope.regionalizacion = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                    }
                    else {
                        $scope.regionalizacion = [];
                    }
                });
        }

        /* ------------------------Inicio Validaciones ---------------------------------*/

        vm.limpiarErrores = function () {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-categoriapolitica-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
            var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-valorcategoriapolitica-error");
            if (campoObligatorioProyectos != undefined) {
                campoObligatorioProyectos.innerHTML = "";
                campoObligatorioProyectos.classList.add('hidden');
            }
        }

        vm.validarCategoriasPoliticas = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-categoriapolitica-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValoresCategorias = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-valorcategoriapolitica-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            'FOCPOL001': vm.validarCategoriasPoliticas,
            'FOCPOL002': vm.validarValoresCategorias,
        }
        /* ------------------------Fin Validaciones ---------------------------------*/

        /* --------------------------------------------------------------------------*/
        /*Inicio Cruce Detallado*/
        function CancelarValores(regionalizacion) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                //vm.mostrarcrucepoliticas = false;
                return comunesServicio.obtenerCrucePoliticasProgramacion(vm.tramiteproyectoid).then(
                    function (respuesta) {
                        if (respuesta.data !== '') {
                            utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                        }
                        vm.EditarCrucePolitica = false;
                        $("#btnExaminarArchivo").attr('disabled', true);
                    });
            }, function funcionCancelar(reason) {
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function EditarValores() {
            vm.EditarCrucePolitica = true;
            $("#btnExaminarArchivo").attr('disabled', false);
        }

        function GuardarValores(regionalizacion) {
            ValoresCruce = [];
            let Programacion = {};
            angular.forEach(regionalizacion.Recursos, function (series) {
                let valores = {
                    ProyectoId: vm.tramiteproyectoid,
                    Bpin: vm.tramiteproyectoid,
                    FuenteId: 0,
                    PoliticaId: vm.PoliticaIdP,
                    LocalizacionId: series.LocalizacionId,
                    PoliticaDependienteId: vm.PoliticaIdD,
                    PeriodoProyectoId: 0,
                    Vigencia: 0,
                    ValorPoliticaPrincipal: series.Valor,
                    ValorCruceDependientePrincipal: series.SolicitadoPolPrincipal,
                    PersonaPoliticaPrincipal: series.EntityTypeCatalogOptionid,
                    PersonaCruce: 0,
                    DimensionId: vm.DimensionPrincipalId
                };
                ValoresCruce.push(valores);
            });

            Programacion = ValoresCruce;
            return comunesServicio.guardarCrucePoliticasProgramacion(Programacion).then(
                function (respuesta) {
                    if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        if (respuesta.data.Exito) {
                            vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                            utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito. ");
                            vm.EditarCrucePolitica = false;
                            vm.modificodatos = '2';
                            ObtenerDatosCrucePoliticasDetalle(vm.cadena);
                        }
                        else {
                            utilidades.mensajeError(respuesta.data.Mensaje);
                            var ErrorCruceMsgGuardar = document.getElementById("ErrorCruceMsgGuardar");
                            ErrorCruceMsgGuardar.innerHTML = '<span>' + respuesta.data.Mensaje + "</span>";
                        }
                    } else {
                        utilidades.mensajeError("", null, "Error al realizar la operación");
                    }
                });
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 12;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 15;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 4);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 4) {
                        tamanioPermitido = n[0].length + 4;
                        event.target.value = n[0] + '.' + n[1].slice(0, 4);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            } else {
                if (tamanio > 12 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 12) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 15) {
                    event.preventDefault();
                }
            }
        }

        vm.validarTamanio = function (event) {

            if (Number.isNaN(event.target.value)) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == null) {
                event.target.value = "0"
                return;
            }

            if (event.target.value == "") {
                event.target.value = "0"
                return;
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            }
        }

        vm.actualizaFila = function (event, regionalizacion) {
            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            event.target.value = parseFloat(event.target.value.replace(",", "."));
            var TSolicitadoPolPrincipal = 0;
            var TSolicitadoPolContenida = 0;
            var TValor = 0;

            angular.forEach(regionalizacion.Recursos, function (series) {
                TSolicitadoPolPrincipal += parseFloat(series.SolicitadoPolPrincipal);
                TSolicitadoPolContenida += parseFloat(series.SolicitadoPolContenida);
                TValor += parseFloat(series.Valor);
            });
            regionalizacion.TSolicitadoPolPrincipal = TSolicitadoPolPrincipal;
            regionalizacion.TSolicitadoPolContenida = TSolicitadoPolContenida;
            regionalizacion.TValor = TValor;

            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        //Inicio Plantilla Cruce Políticas
        function descargarPlantila() {
            exportarCruceExcel();
        }

        function exportarCruceExcel() {
            utilidades.mensajeWarning("Si ocurren inconvenientes de descarga o visualización, es necesario actualizar la aplicación.", function funcionContinuar() {
                const filename = 'Template_.xlsx';
                const COL_PARAMS = ['hidden', 'wpx', 'width', 'wch', 'MDW'];
                const STYLE_PARAMS = ['fill', 'font', 'alignment', 'border'];
                var styleConf = {
                    'E4': {
                        fill: { fgColor: { rgb: 'FFFF0000' } }
                    }
                }

                var columns = [
                    {
                        name: 'LocalizacionId', title: 'LocalizacionId'
                    },
                    {
                        name: 'Localizacion', title: 'Localizacion'
                    },
                    {
                        name: 'Valor', title: 'Valor'
                    }
                ];

                let colNames = columns.map(function (item) {
                    return item.title;
                })

                var wb = XLSX.utils.book_new();

                wb.Props = {
                    Title: "Plantilla cruce de politicas programacion",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };

                wb.SheetNames.push("CrucePoliticas");

                const header = colNames;
                const data = [];
                $scope.regionalizacion.Recursos.forEach(regionalizar => {
                    data.push({
                        LocalizacionId: regionalizar.LocalizacionId,
                        Localizacion: regionalizar.Localizacion,
                        Valor: regionalizar.Valor
                    });
                });

                const worksheet = XLSX.utils.json_to_sheet(data, {
                    header: [
                        "LocalizacionId",
                        "Localizacion",
                        "Valor"
                    ]
                });
                for (let col of [2]) {
                    formatColumn(worksheet, col, "#,##")
                }
                worksheet['!cols'] = [];
                worksheet['!cols'][0] = { hidden: true };
                wb.Sheets["CrucePoliticas"] = worksheet;

                var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
                saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'PlantillaCrucePoliticas.xlsx');
            }, function funcionCancelar(reason) {
            }, false, false, "Este archivo es compatible con Office 365");
        }

        function formatColumn(worksheet, col) {
            var fmtnumero2 = "##,##";
            var fmtnumero4 = "#,####";
            const range = XLSX.utils.decode_range(worksheet['!ref'])
            for (let row = range.s.r + 1; row <= range.e.r; ++row) {
                const ref = XLSX.utils.encode_cell({ r: row, c: col })
                if (ref != "K0" || ref != "L0" || ref != "N0" || ref != "O0") {
                    worksheet[ref].z = fmtnumero2;
                    worksheet[ref].t = 'n';
                }
                if (ref === "S1") {
                    worksheet[ref].z = fmtnumero2;
                    worksheet[ref].t = 'n';
                }
                if (ref === "T1") {
                    worksheet[ref].z = fmtnumero4;
                    worksheet[ref].t = 'n';
                }
            }
        }

        function s2ab(s) {
            var buf = new ArrayBuffer(s.length); //convert s to arrayBuffer
            var view = new Uint8Array(buf);  //create uint8array as viewer
            for (var i = 0; i < s.length; i++) view[i] = s.charCodeAt(i) & 0xFF; //convert to octet
            return buf;
        }

        function HabilitaEditar(band) {
            vm.HabilitaEditarBandera = band;
        }

        function adjuntarArchivo() {
            document.getElementById('filecruce').value = "";
            document.getElementById('filecruce').click();
        }

        function limpiarArchivo() {
            $scope.filesfocalizacion = [];
            document.getElementById('filecruce').value = "";
            vm.activarControles('inicio');
        }

        function validarArchivo() {
            var resultado = true;
            ValoresCruce = [];

            if (filecruce.files.length > 0) {

                let file = document.getElementById("filecruce").files[0];
                if ($scope.validaCruceNombreArchivo(file.name)) {
                    if (typeof (FileReader) != "undefined") {
                        var reader = new FileReader();
                        if (reader.readAsBinaryString) {
                            reader.onload = function (e) {
                                var workbook = XLSX.read(e.target.result, {
                                    type: 'binary'
                                });
                                var firstSheet = workbook.SheetNames[0];
                                var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[firstSheet]);
                                resultado = excelRows.map(function (item, index) {

                                    if (item["LocalizacionId"] == undefined) {
                                        utilidades.mensajeError("La columna LocalizacionId no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["LocalizacionId"])) {
                                        utilidades.mensajeError("El valor LocalizacionId " + item["Id"] + " no es númerico!");
                                        return false;
                                    }
                                    if (item["Valor"] == undefined) {
                                        utilidades.mensajeError("La columna 'Valor' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDecimal(item["Valor"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'Valor' " + item["Propios"] + ". La columna 'Valor' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }

                                    var valoresarchivo = {
                                        ProyectoId: vm.tramiteproyectoid,
                                        Bpin: vm.tramiteproyectoid,
                                        FuenteId: 0,
                                        PoliticaId: vm.PoliticaIdP,
                                        LocalizacionId: item["LocalizacionId"],
                                        PoliticaDependienteId: vm.PoliticaIdD,
                                        PeriodoProyectoId: 0,
                                        Vigencia: 0,
                                        ValorPoliticaPrincipal: item["Valor"],
                                        ValorCruceDependientePrincipal: 0,
                                        PersonaPoliticaPrincipal: $sessionStorage.idEntidad,
                                        PersonaCruce: 0,
                                        DimensionId: vm.DimensionPrincipalId
                                    }
                                    ValoresCruce.push(valoresarchivo);
                                });
                                if (resultado.indexOf(false) == -1) {
                                    vm.activarControles('validado');
                                    utilidades.mensajeSuccess("", false, false, false, "Validacion de Carga Exitosa.");
                                }
                                else {
                                    vm.activarControles('inicio');
                                    ValoresCruce = [];
                                }
                            };
                            reader.readAsBinaryString(file);
                        }
                    }
                }
            }
        }

        function ValidarDecimal(valor, decimals) {
            if (valor.toString().includes('.')) {
                var entero = valor.toString().split('.')[0];
                var decimal = valor.toString().split('.')[1];

                if (isNaN(entero)) {
                    return false;
                }

                if (isNaN(decimal)) {
                    return false;
                }

                if (decimal.length > decimals) {
                    return false;
                }
            }
            else {
                if (isNaN(valor)) {
                    return false;
                }
            }
            return true;
        }

        function ValidaSiEsNumero(valor) {
            if (valor === undefined)
                return false;
            else if (!isNaN(limpiaNumero(valor))) {
                return true;
            }
            else {
                return false;
            }
        }

        function limpiaNumero(valor) {
            return valor.toLocaleString().split(",")[0].toString().replaceAll(".", "");
        }

        vm.activarControles = function (evento) {
            switch (evento) {
                case "inicio":
                    $("#btnCruceLimpiarArchivo").attr('disabled', true);
                    $("#btnCruceArchivoSeleccionado").attr('disabled', true);
                    document.getElementById('filecruce').value = "";
                    vm.nombrearchivo = "";
                    break;
                case "cargaarchivo":
                    $("#btnCruceValidarArchivo").attr('disabled', false);
                    $("#btnCruceLimpiarArchivo").attr('disabled', false);
                    $("#btnCruceArchivoSeleccionado").attr('disabled', true);
                    break;
                case "validado":
                    $("#btnCruceValidarArchivo").attr('disabled', false);
                    $("#btnCruceLimpiarArchivo").attr('disabled', false);
                    $("#btnCruceArchivoSeleccionado").attr('disabled', false);

                    vm.HabilitaEditarBandera = true;
                    break;
                default:
            }
        }

        $scope.filefocalizacionNameChanged = function (input) {
            if (input.files.length == 1) {
                vm.nombrearchivo = input.files[0].name;
                vm.activarControles('cargaarchivo');
            }
            else {
                vm.activarControles('inicio');
            }
        }

        $scope.validaCruceNombreArchivo = function (nombre) {
            var regex = /^([\ \(a-zA-Z0-9\s_\\.\-:\ \)])+(.xls|.xlsx)$/;
            if (!regex.test(nombre.toLowerCase())) {
                utilidades.mensajeError("El archivo no es de tipo Excel!");
                $scope.files = [];
                $scope.nombreArchivo = '';
                return false;
            }
            else {
                return true;
            }
        }

        function RegistrarCargaCruce() {
            let Programacion = {};
            Programacion = ValoresCruce;
            return comunesServicio.guardarCrucePoliticasProgramacion(Programacion).then(
                function (respuesta) {
                    if (respuesta.data.Exito && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        utilidades.mensajeSuccess("Registro de carga de cruce de políticas para Programación exitosa", false, false, false, "Carga Exitosa.");
                        vm.activarControles('inicio');
                        ObtenerDatosCrucePoliticasDetalle(vm.cadena);
                        return;
                    }
                    else {
                        utilidades.mensajeError(respuesta.data.Mensaje);
                        var ErrorCruceMsgGuardar = document.getElementById("ErrorCruceMsgGuardar");
                        ErrorCruceMsgGuardar.innerHTML = '<span>' + respuesta.data.Mensaje + "</span>";
                        vm.activarControles('inicio');
                        return;
                    }
                },
                function (error) {
                    if (error) {
                        utilidades.mensajeError("El Archivo fue modificado en datos y/o estructura y no se puede procesar. Error: " + error.statusText);
                        console.log("codigoError: " + error.status + " - Error: " + error.statusText)
                    }
                    mostrarMensajeRespuesta();
                });
        }
        /*Fin Cruce*/
        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Cruce de Políticas");
            vm.limpiarErrores();
            var isValid = true;
            if (errores != undefined && errores.length > 0) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                if (vm.notificacionErrores != null && erroresJson != null) vm.notificacionErrores(erroresJson[vm.nombreComponente]);
                isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    erroresJson[vm.nombreComponente].forEach(p => {
                        if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                    });
                }

            }
            vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
        };

    }
    angular.module('backbone')
        .component('crucePoliticasTransversalesProgramacion', {
            templateUrl: 'src/app/formulario/ventanas/comun/programacionRecursos/politicas/detallePolitica/cruce/crucePoliticasTransversalesProgramacion.html',
            controller: 'crucePoliticasTransversalesProgramacion',
            controllerAs: 'vm',
            bindings: {
                callback: '&',
                notificacionvalidacion: '&',
                notificacionestado: '&',
                guardadoevent: '&',
                notificarrefresco: '&',
                notificacioncambios: '&',
                calendariopoliticastransversales: '@',
                tramiteproyectoid: '@',
                proyectoid: '@',
            },
        })
        .directive('stringToNumber', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (value) {

                        return '' + value;
                    });
                    ngModel.$formatters.push(function (value) {
                        return parseFloat(value);
                    });
                }
            };
        })
        .directive('nksOnlyNumber', function () {
            return {
                restrict: 'EA',
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                        var spiltArray = String(newValue).split("");

                        if (attrs.allowNegative == "false") {
                            if (spiltArray[0] == '-') {
                                newValue = newValue.replace("-", "");
                                ngModel.$setViewValue(newValue);
                                ngModel.$render();
                            }
                        }

                        if (attrs.allowDecimal == "false") {
                            newValue = parseInt(newValue);
                            ngModel.$setViewValue(newValue);
                            ngModel.$render();
                        }

                        if (attrs.allowDecimal != "false") {
                            if (attrs.decimalUpto) {
                                var n = String(newValue).split(".");
                                if (n[1]) {
                                    var n2 = n[1].slice(0, attrs.decimalUpto);
                                    newValue = [n[0], n2].join(".");
                                    ngModel.$setViewValue(newValue);
                                    ngModel.$render();
                                }
                            }
                        }


                        if (spiltArray.length === 0) return;
                        if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                        if (spiltArray.length === 2 && newValue === '-.') return;

                        /*Check it is number or not.*/
                        if (isNaN(newValue)) {
                            ngModel.$setViewValue(oldValue || '');
                            ngModel.$render();
                        }
                    });
                }
            };
        });
})();