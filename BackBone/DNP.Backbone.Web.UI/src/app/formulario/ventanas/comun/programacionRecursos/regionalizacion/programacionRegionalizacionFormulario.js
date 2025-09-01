(function () {
    'use strict';

    programacionRegionalizacionFormulario.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        '$uibModal',
        'comunesServicio'
    ];

    function programacionRegionalizacionFormulario(
        $scope,
        $sessionStorage,
        utilidades,
        $uibModal,
        comunesServicio
    ) {
        ///*Variables */
        var vm = this;
        vm.lang = "es";
        vm.EditarRegionaliza = false;
        vm.nombreComponente = "regionalizacionregionalizacion";
        vm.idNivel = $sessionStorage.idNivel;
        vm.seccionCapitulo = null;
        vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1
        /*declara metodos*/
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
        vm.RegistrarCargaRegionalizacion = RegistrarCargaRegionalizacion;
        let ValoresRegionalizacion = [];
        $("#btnRegionalizacionValidarArchivo-" + vm.tramiteproyectoid).attr('disabled', true);
        $("#btnRegionalizacionLimpiarArchivo-" + vm.tramiteproyectoid).attr('disabled', true);
        $("#btnRegionalizacionArchivoSeleccionado-" + vm.tramiteproyectoid).attr('disabled', true);
        $("#btn-agregar-localizacion").attr('disabled', true);
        vm.abrirModalAgregarLocalizacion = abrirModalAgregarLocalizacion;
        var DataJson = "";
        var arreglolistaLocalizaciones = [];
        vm.BPIN = "";
        vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1

        $scope.$watch('vm.tramiteproyectoid', function () {
            if (vm.tramiteproyectoid != '') {
                vm.BPIN = comunesServicio.getBpinCargado();
                ObtenerDatosProgramacionDetalleReg();
                vm.ObtenerLocalizacionProyecto();
                vm.mostrarregionalizacion = true;
            }
        });

        $scope.$watch('vm.calendarioregionalizacion', function () {
            if (vm.calendarioregionalizacion !== undefined && vm.calendarioregionalizacion !== '')
                vm.habilitaBotones = vm.calendarioregionalizacion === 'true' && !$sessionStorage.soloLectura ? true : false;

        });

        $scope.$watch('vm.modificardistribucion', function () {
            if (vm.modificardistribucion === '4') {
                ObtenerDatosProgramacionDetalle();
                vm.modificardistribucion = '0';
            }
        });

        vm.init = function () {

        };

        function ObtenerDatosProgramacionDetalleReg() {
            return comunesServicio.ObtenerDatosProgramacionDetalle(vm.tramiteproyectoid, vm.origen).then(
                function (respuesta) {
                    if (respuesta.data !== '') {
                        $scope.regionalizacion = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                    }
                    else {
                        $scope.regionalizacion = [];
                    }
                });
        }
        vm.ObtenerLocalizacionProyecto = function () {
            return comunesServicio.ObtenerLocalizacionProyecto(vm.BPIN).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        arreglolistaLocalizaciones = jQuery.parseJSON(respuesta.data);
                        DataJson = respuesta.data;
                    }
                }
            );
        }

        function CancelarValores(regionalizacion) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                vm.EditarRegionaliza = false;
                return comunesServicio.ObtenerDatosProgramacionDetalle(vm.tramiteproyectoid, vm.origen).then(
                    function (respuesta) {
                        if (respuesta.data !== '') {
                            $scope.regionalizacion = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                            utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                        }
                        else {
                            $scope.regionalizacion = [];
                        }
                        $("#btnExaminarArchivo").attr('disabled', true);
                        $("#btn-agregar-localizacion").attr('disabled', true);
                    });
            }, function funcionCancelar(reason) {
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function EditarValores(regionalizacion) {
            vm.EditarRegionaliza = true;
            $("#btnExaminarArchivo").attr('disabled', false);
            $("#btn-agregar-localizacion").attr('disabled', false);
            $("#BtnEliminarLocalizacion").attr('disabled', false);
            $("#btnRegionalizacionArchivoSeleccionado-" + vm.tramiteproyectoid).attr('disabled', true);
        }

        function GuardarValores(regionalizacion) {
            ValoresRegionalizacion = [];
            let Programacion = {};
            angular.forEach(regionalizacion.Recursos, function (series) {
                let valores = {
                    LocalizacionId: series.LocalizacionId,
                    NacionCSF: series.NacionCSF,
                    NacionSSF: series.NacionSSF,
                    Propios: series.Propios
                };

                ValoresRegionalizacion.push(valores);
            });

            ObtenerSeccionCapitulo();
            Programacion.TramiteProyectoId = regionalizacion.TramiteProyectoId;
            Programacion.NivelId = vm.idNivel;
            Programacion.SeccionCapitulo = vm.seccionCapitulo;
            Programacion.ValoresRegionalizar = ValoresRegionalizacion;

            return comunesServicio.GuardarProgramacionRegionalizacion(Programacion).then(
                function (respuesta) {
                    if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        if (respuesta.data.Exito) {
                            vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                            utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito. ");
                            vm.EditarRegionaliza = false;
                            vm.modificodatos = '2';
                            vm.init();
                        }
                        else {
                            utilidades.mensajeError(respuesta.data.Mensaje);
                            var ErrorRegionalizaMsgGuardar = document.getElementById("ErrorRegionalizaMsgGuardar");
                            ErrorRegionalizaMsgGuardar.innerHTML = '<span>' + respuesta.data.Mensaje + "</span>";
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

            var TNacionCSF = 0;
            var TNacionSSF = 0;
            var TPropios = 0;

            angular.forEach(regionalizacion.Recursos, function (series) {
                series.TotalTR = parseFloat(series.NacionCSF) + parseFloat(series.NacionSSF) + parseFloat(series.Propios);
                TNacionCSF += parseFloat(series.NacionCSF);
                TNacionSSF += parseFloat(series.NacionSSF);
                TPropios += parseFloat(series.Propios);
            });

            regionalizacion.TNacionCSF = TNacionCSF;
            regionalizacion.TNacionSSF = TNacionSSF;
            regionalizacion.TPropios = TPropios;

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

        //Inicio Plantilla Regionalizacion
        function descargarPlantila() {
            exportarRegionalizacionExcel();
        }

        function exportarRegionalizacionExcel() {
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
                        name: 'NacionCSF', title: 'NacionCSF'
                    },
                    {
                        name: 'NacionSSF', title: 'NacionSSF'
                    },
                    {
                        name: 'Propios', title: 'Propios'
                    }
                ];

                let colNames = columns.map(function (item) {
                    return item.title;
                })

                var wb = XLSX.utils.book_new();

                wb.Props = {
                    Title: "Plantilla regionalizacion programacion",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };

                wb.SheetNames.push("Regionalización");

                const header = colNames;
                const data = [];
                $scope.regionalizacion.Recursos.forEach(regionalizar => {
                    data.push({
                        LocalizacionId: regionalizar.LocalizacionId,
                        Localizacion: regionalizar.Localizacion,
                        NacionCSF: regionalizar.NacionCSF,
                        NacionSSF: regionalizar.NacionSSF,
                        Propios: regionalizar.Propios
                    });
                });

                const worksheet = XLSX.utils.json_to_sheet(data, {
                    header: [
                        "LocalizacionId",
                        "Localizacion",
                        "NacionCSF",
                        "NacionSSF",
                        "Propios"
                    ]
                });

                for (let col of [2]) {
                    formatColumn(worksheet, col, "#,##")
                }
                for (let col of [3]) {
                    formatColumn(worksheet, col, "#,##")
                }
                for (let col of [4]) {
                    formatColumn(worksheet, col, "#,##")
                }
                worksheet['!cols'] = [];
                worksheet['!cols'][0] = { hidden: true };
                wb.Sheets["Regionalización"] = worksheet;

                var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
                saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'PlantillaRegionalizacion.xlsx');
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

        function adjuntarArchivo(idProyecto) {
            document.getElementById('fileregionalizacion-' + idProyecto).value = "";
            document.getElementById('fileregionalizacion-' + idProyecto).click();
        }

        function limpiarArchivo(idProyecto) {
            $scope.fileregionalizacion = [];
            document.getElementById('fileregionalizacion-' + idProyecto).value = "";
            vm.activarControles('inicio', idProyecto);
            vm.nombrearchivo = "";
        }

        function validarArchivo(idProyecto) {
            var resultado = true;
            ValoresRegionalizacion = [];

            if (document.getElementById('fileregionalizacion-' + idProyecto).files.length > 0) {

                let file = document.getElementById("fileregionalizacion-" + idProyecto).files[0];
                if ($scope.validaRegionalizacionNombreArchivo(file.name)) {
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
                                    if (item["NacionCSF"] == undefined) {
                                        utilidades.mensajeError("La columna 'NacionCSF' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDecimal(item["NacionCSF"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'NacionCSF' " + item["NacionCSF"] + ". La columna 'NacionCSF' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    if (item["NacionSSF"] == undefined) {
                                        utilidades.mensajeError("La columna 'NacionSSF' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDecimal(item["NacionSSF"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'NacionSSF' " + item["NacionSSF"] + ". La columna 'NacionSSF' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    if (item["Propios"] == undefined) {
                                        utilidades.mensajeError("La columna 'Propios' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDecimal(item["Propios"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'Propios' " + item["Propios"] + ". La columna 'Propios' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }

                                    var valoresarchivo = {
                                        LocalizacionId: item["LocalizacionId"],
                                        NacionCSF: item["NacionCSF"],
                                        NacionSSF: item["NacionSSF"],
                                        Propios: item["Propios"]
                                    }
                                    ValoresRegionalizacion.push(valoresarchivo);
                                });
                                if (resultado.indexOf(false) == -1) {
                                    vm.activarControles('validado', idProyecto);
                                    utilidades.mensajeSuccess("", false, false, false, "Validacion de Carga Exitosa.");
                                }
                                else {
                                    vm.activarControles('inicio', idProyecto);
                                    ValoresRegionalizacion = [];
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

        vm.activarControles = function (evento, idProyecto) {
            switch (evento) {
                case "inicio":
                    $("#btnRegionalizacionLimpiarArchivo-" + idProyecto).attr('disabled', true);
                    $("#btnRegionalizacionArchivoSeleccionado-" + idProyecto).attr('disabled', true);
                    document.getElementById("fileregionalizacion-" + idProyecto).value = "";
                    vm.nombrearchivo = "";
                    break;
                case "cargaarchivo":
                    $("#btnRegionalizacionValidarArchivo-" + idProyecto).attr('disabled', false);
                    $("#btnRegionalizacionLimpiarArchivo-" + idProyecto).attr('disabled', false);
                    $("#btnRegionalizacionArchivoSeleccionado-" + idProyecto).attr('disabled', true);
                    break;
                case "validado":
                    $("#btnRegionalizacionValidarArchivo-" + idProyecto).attr('disabled', false);
                    $("#btnRegionalizacionLimpiarArchivo-" + idProyecto).attr('disabled', false);
                    $("#btnRegionalizacionArchivoSeleccionado-" + idProyecto).attr('disabled', false);

                    vm.HabilitaEditarBandera = true;
                    break;
                default:
            }
        }

        $scope.fileregionalizacionNameChanged = function (input) {
            let idProyecto = input.id.toString().substring(input.id.indexOf('-') + 1, input.id.length);
            if (input.files.length == 1) {
                //var nombreA = '#spNombrearchivo' + idProyectotramite;
                //$(nombreA).text(input.files[0].name);             
                vm.nombrearchivo = input.files[0].name;
                //document.getElementById('spNombrearchivo-' + idProyectotramite).textContent = input.files[0].name;
                vm.activarControles('cargaarchivo', idProyecto);
            }
            else {
                vm.activarControles('inicio', idProyecto);
            }
        }

        $scope.validaRegionalizacionNombreArchivo = function (nombre) {
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

        function RegistrarCargaRegionalizacion() {
            let Programacion = {};
            ObtenerSeccionCapitulo();
            Programacion.TramiteProyectoId = vm.tramiteproyectoid;
            Programacion.NivelId = vm.idNivel;
            Programacion.SeccionCapitulo = vm.seccionCapitulo;
            Programacion.ValoresRegionalizar = ValoresRegionalizacion;

            return comunesServicio.GuardarProgramacionRegionalizacion(Programacion).then(
                function (respuesta) {
                    if (respuesta.data.Exito && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        utilidades.mensajeSuccess("Registro de carga de regionalización para Programación exitosa", false, false, false, "Carga Exitosa.");
                        vm.activarControles('inicio', vm.proyectoid);
                        ObtenerDatosProgramacionDetalleReg();
                        return;
                    }
                    else {
                        utilidades.mensajeError(respuesta.data.Mensaje);
                        var ErrorRegionalizaMsgGuardar = document.getElementById("ErrorRegionalizaMsgGuardar");
                        ErrorRegionalizaMsgGuardar.innerHTML = '<span>' + respuesta.data.Mensaje + "</span>";
                        vm.activarControles('inicio', vm.proyectoid);
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

        function abrirModalAgregarLocalizacion(LocalizacionId) {
            $sessionStorage.LocalizacionId = LocalizacionId;
            $sessionStorage.listaObjLocalizacion = DataJson;
            $sessionStorage.proyectoId = vm.proyectoid;
            $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/comun/programacionRecursos/regionalizacion/modal/modalAgregarLocalizacionRegionalizacion.html',
                controller: 'modalAgregarLocalizacionRegionalizacionController',
            }).result.then(function (result) {
                ObtenerDatosProgramacionDetalleReg();
            }, function (reason) {

            }), err => {
                toastr.error("Ocurrió un error al consultar el idAplicacion");
            };
            ObtenerDatosProgramacionDetalleReg();
        }

        vm.eliminarLocalizacion = function (LocalizacionId) {
            var arreglo = arreglolistaLocalizaciones;
            var objJson = DataJson;
            var localizacionCambio = arreglo.NuevaLocalizacion.filter(x => x.LocalizacionId == LocalizacionId)[0];
            var munName = null;
            var depName = null;
            var agrName = null;
            var tipAgrName = null;

            if (localizacionCambio != undefined) {
                munName = localizacionCambio.Municipio;
            }
            if (localizacionCambio != undefined) {
                depName = localizacionCambio.Departamento;
            }
            if (localizacionCambio != undefined) {
                tipAgrName = localizacionCambio.TipoAgrupacion;
            }
            if (localizacionCambio != undefined) {
                agrName = localizacionCambio.Agrupacion;
            }

            let localizacion = [];
            let l = {
                LocalizacionId: LocalizacionId,
                RegionId: null,
                Region: null,
                DepartamentoId: null,
                Departamento: null,
                MunicipioId: null,
                Municipio: null,
                TipoAgrupacionId: null,
                TipoAgrupacion: null,
                AgrupacionId: null,
                Agrupacion: null,
            };
            localizacion.push(l);
            var seccionCapituloId = document.getElementById("id-capitulo-datosgeneraleslocalizaciones");
            vm.seccionCapituloId = seccionCapituloId != undefined && seccionCapituloId.innerHTML != '' ? seccionCapituloId.innerHTML : 657;
            var parametro = {
                ProyectoId: vm.proyectoid,
                BPIN: vm.proyectoid,
                Accion: "Delete",
                Justificacion: "Programacion",
                SeccionCapituloId: vm.seccionCapituloId,
                InstanciaId: vm.idInstancia,
                NuevaLocalizacion: localizacion,
            };
            if (depName == null) {
                depName = "N/A";
            }
            if (munName == null) {
                munName = "N/A";
            }
            if (tipAgrName == null) {
                tipAgrName = "N/A";
            }
            if (agrName == null) {
                agrName = "N/A";
            }
            utilidades.mensajeWarning("Se va a eliminar una localización", function funcionContinuar() {
                comunesServicio.guardarLocalizacion(parametro, usuarioDNP).then(function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {
                        if (response.data.Exito) {
                            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                            parent.postMessage("cerrarModal", window.location.origin);
                            if (depName == "N/A") {
                                utilidades.mensajeSuccess("Se eliminó la línea de información seleccionada. ", false, false, false, "Los datos fueron eliminados con éxito!");
                            }
                            else {
                                utilidades.mensajeSuccess("Se eliminó la línea de información: " + depName + " /" + munName + " /" + tipAgrName + " /" + agrName + " en la parte inferior de la tabla ''localizaciones''.", false, false, false, "Los datos fueron eliminados con éxito!");
                            }
                            ObtenerDatosProgramacionDetalleReg();
                            vm.ObtenerLocalizacionProyecto();
                        } else {
                            var mensajeError = JSON.parse(response.data.Mensaje);
                            var mensajeReturn = '';
                            console.log(mensajeError);
                            try {
                                for (var i = 0; i < mensajeError.ListaErrores.length; i++) {
                                    mensajeReturn = mensajeReturn + mensajeError.ListaErrores[i].Error + '\n';
                                }
                            }
                            catch {
                                mensajeReturn = mensajeError.Mensaje;
                            }
                            swal('', mensajeReturn, 'warning');
                        }
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                });
            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            });
        }


        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Regionalización");
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

        vm.ErrorRegionalizacion = function (errores) {
            var LocalizacionId = errores.split('|')[0];
            var MensajeError = errores.split('|')[1];;

            var ErrorRegionalizacion = document.getElementById("ErrorRegionalizacion-" + LocalizacionId);
            var ErrorRegionalizaMsg = document.getElementById("ErrorRegionalizaMsg-" + LocalizacionId);

            if (ErrorRegionalizacion != undefined) {
                if (ErrorRegionalizaMsg != undefined) {
                    ErrorRegionalizaMsg.innerHTML = '<span>' + MensajeError + "</span>";
                    ErrorRegionalizacion.classList.remove('hidden');
                }
            }
        }
        vm.errores = {
            'ErrorRegionalizacion-': vm.ErrorRegionalizacion
            //'': vm.limpiarErrores()
        }
    }

    angular.module('backbone').component('programacionRegionalizacionFormulario', {

        templateUrl: "/src/app/formulario/ventanas/comun/programacionRecursos/regionalizacion/programacionRegionalizacionFormulario.html",
        controller: programacionRegionalizacionFormulario,
        controllerAs: "vm",
        bindings: {
            tramiteproyectoid: '@',
            origen: '@',
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            nivel: '@',
            rol: '@',
            proyectoid: '@',
            CodigoBPIN: '@',
            actualizacomponentes: '@',
            calendarioregionalizacion: '@',
            modificardistribucion: '=',
        }
    })
})();
