(function () {
    'use strict';

    cuotaController.$inject = ['$scope',
        'constantesAutorizacion',
        '$uibModal',
        'FileSaver',
        'utilidades',
        'autorizacionServicios',
        'cuotaServicio'];

    function cuotaController($scope, constantesAutorizacion, $uibModal, FileSaver, utilidades, autorizacionServicios, cuotaServicio) {
        var vm = this;

        vm.descargarPlantila = descargarPlantila;
        vm.listaCuotas = [];
        vm.listaCuotasArchivo = {};
        vm.listaCuotasArchivo.ValoresCuotaEntidad = [];
        vm.listaCuotasError = [];
        vm.MostrarErrores = false;
        vm.archivoValido = true;

        vm.MostrarBuscar = false;
        vm.MostrarSinResultados = false;

        vm.exportarCuotaCargaMasivaExcelErrores = exportarCuotaCargaMasivaExcelErrores;

        vm.HabilitaEditarBandera = false;
        vm.HabilitaEditar = HabilitaEditar;
        vm.limpiarArchivo = limpiarArchivo;
        vm.validarArchivo = validarArchivo;
        vm.RegistrarCargaMasivaCuotas = RegistrarCargaMasivaCuotas;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.nombrearchivo = "Seleccione Archivo";
        vm.CuotaCargaMasiva = null;

        vm.init = function () {
            vm.activarControles('inicio');
            vm.ObtenerCargaMasivaCuotas();
        };

        vm.ObtenerCargaMasivaCuotas = function () {
            return cuotaServicio.ObtenerCargaMasivaCuotas().then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        console.log(respuesta.data);

                        vm.listaCuotas = jQuery.parseJSON(respuesta.data);

                        console.log(vm.listaCuotas);
                    }
                });
        }

        function ValidarRegistros(lista) {
            vm.listaCuotasError = [];

            return cuotaServicio.ValidarCargaMasivaCuotas(JSON.stringify(lista)).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        vm.listaCuotasError = jQuery.parseJSON(respuesta.data.Result);
                        console.log(vm.listaCuotasError);

                        vm.archivoValido = true;
                        vm.MostrarErrores = false;

                        if (vm.listaCuotasError != undefined) {

                            vm.listaCuotasError.forEach(itemError => {
                                if (itemError.ValidacionError != undefined && itemError.ValidacionError != '') {
                                    vm.archivoValido = false;
                                    vm.MostrarErrores = true;
                                }
                            })
                        }

                        if (vm.archivoValido) {
                            vm.activarControles('validado');
                            utilidades.mensajeSuccess("", false, false, false, "Validacion de Carga Exitosa.");
                        }
                        else {
                            utilidades.mensajeError("El Archivo fue modificado en datos y/o estructura y no se puede procesar.");
                            vm.activarControles('inicio');
                        }
                    }
                    else {
                        utilidades.mensajeError("El Archivo fue modificado en datos y/o estructura y no se puede procesar.");
                        vm.activarControles('inicio');
                    }
                });
        }

        function descargarPlantila() {

            exportarCuotaCargaMasivaExcel();
        }

        function exportarCuotaCargaMasivaExcel() {

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
                        name: 'EntityTypeCatalogOptionId', title: 'EntityTypeCatalogOptionId'
                    },
                    {
                        name: 'Codigo', title: 'Codigo'
                    },
                    {
                        name: 'Entidad', title: 'Entidad'
                    },
                    {
                        name: 'NacionCSF', title: 'NACION CSF'
                    },
                    {
                        name: 'NacionSSF', title: 'NACION SFF'
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
                    Title: "Plantilla de cuotas",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };

                wb.SheetNames.push("Cuotas");

                const header = colNames;
                const data = [];


                vm.listaCuotas.ValoresCuotaEntidad.forEach(cuota => {

                    data.push({
                        EntityTypeCatalogOptionId: cuota.EntityTypeCatalogOptionId,
                        Codigo: cuota.Codigo,
                        Entidad: cuota.Entidad,
                        NacionCSF: cuota.NacionCSF,
                        NacionSSF: cuota.NacionSSF,
                        Propios: cuota.Propios
                    });
                });

                const worksheet = XLSX.utils.json_to_sheet(data, {
                    header: [
                        "EntityTypeCatalogOptionId",
                        "Codigo",
                        "Entidad",
                        "NacionCSF",
                        "NacionSSF",
                        "Propios"]
                });

                for (let col of [3]) {
                    formatColumn(worksheet, col, "#,##")
                }
                for (let col of [4]) {
                    formatColumn(worksheet, col, "#,##")
                }
                for (let col of [5]) {
                    formatColumn(worksheet, col, "#,##")
                }


                /* hide second column */
                worksheet['!cols'] = [];

                worksheet['!cols'][0] = { hidden: true };

                wb.Sheets["Cuotas"] = worksheet;

                var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
                //saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaAjusteRegionalizacion.xlsx');
                saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'PlantillaCuotas.xlsx');
            }, function funcionCancelar(reason) {
            }, false, false, "Este archivo es compatible con Office 365");
        }

        function exportarCuotaCargaMasivaExcelErrores() {

            vm.MostrarErrores = true;
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
                        name: 'EntityTypeCatalogOptionId', title: 'EntityTypeCatalogOptionId'
                    },
                    {
                        name: 'Codigo', title: 'Codigo'
                    },
                    {
                        name: 'Entidad', title: 'Entidad'
                    },
                    {
                        name: 'NacionCSF', title: 'NACION CSF'
                    },
                    {
                        name: 'NacionSSF', title: 'NACION SFF'
                    },
                    {
                        name: 'Propios', title: 'Propios'
                    },
                    {
                        name: 'ValidacionError', title: 'ValidacionError'
                    }
                ];

                let colNames = columns.map(function (item) {
                    return item.title;
                })

                var wb = XLSX.utils.book_new();

                wb.Props = {
                    Title: "Errores de cuotas",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };

                wb.SheetNames.push("Cuotas");

                const header = colNames;
                const data = [];


                vm.listaCuotasError.forEach(cuota => {

                    data.push({
                        EntityTypeCatalogOptionId: cuota.EntityTypeCatalogOptionId,
                        Codigo: cuota.Codigo,
                        Entidad: cuota.Entidad,
                        NacionCSF: cuota.NacionCSF,
                        NacionSSF: cuota.NacionSSF,
                        Propios: cuota.Propios,
                        ValidacionError: cuota.ValidacionError
                    });
                });

                const worksheet = XLSX.utils.json_to_sheet(data, {
                    header: [
                        "EntityTypeCatalogOptionId",
                        "Codigo",
                        "Entidad",
                        "NacionCSF",
                        "NacionSSF",
                        "Propios",
                        "ValidacionError"]
                });

                for (let col of [3]) {
                    formatColumn(worksheet, col, "#,##")
                }
                for (let col of [4]) {
                    formatColumn(worksheet, col, "#,##")
                }
                for (let col of [5]) {
                    formatColumn(worksheet, col, "#,##")
                }


                /* hide second column */
                worksheet['!cols'] = [];

                worksheet['!cols'][0] = { hidden: true };

                wb.Sheets["Cuotas"] = worksheet;

                var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
                //saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaAjusteRegionalizacion.xlsx');
                saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'ErrorCuotas.xlsx');
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
            document.getElementById('filecuotaCargaMasiva').value = "";
            document.getElementById('filecuotaCargaMasiva').click();
        }

        function limpiarArchivo() {
            $scope.filescuotaCargaMasiva = [];
            document.getElementById('filecuotaCargaMasiva').value = "";
            vm.activarControles('inicio');
        }

        function RegistrarCargaMasivaCuotas() {
            return cuotaServicio.RegistrarCargaMasivaCuotas(JSON.stringify(vm.listaCuotasArchivo.ValoresCuotaEntidad)).then(
                function (respuesta) {

                    if (respuesta.data.Status) {
                        utilidades.mensajeSuccess("Registro de carga masiva de cuotas exitosa", false, false, false, "Carga Exitosa.");
                        vm.activarControles('inicio');

                        vm.ObtenerCargaMasivaCuotas();
                        return;
                    }
                    else {
                        utilidades.mensajeError("El Archivo fue modificado en datos y/o estructura y no se puede procesar.");
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
        function validarArchivo() {
            var resultado = true;
            vm.listaCuotasArchivo = {};

            vm.listaCuotasArchivo.totalPropios = vm.listaCuotas.totalPropios;
            vm.listaCuotasArchivo.TotalNacionSSF = vm.listaCuotas.TotalNacionSSF;
            vm.listaCuotasArchivo.TotalNacionCSF = vm.listaCuotas.TotalNacionCSF;
            vm.listaCuotasArchivo.ValoresCuotaEntidad = [];

            if (filecuotaCargaMasiva.files.length > 0) {

                let file = document.getElementById("filecuotaCargaMasiva").files[0];
                if ($scope.validaCuotaCargaMasivaNombreArchivo(file.name)) {
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

                                    let EntityTypeCatalogOptionId = item["EntityTypeCatalogOptionId"];
                                    if (EntityTypeCatalogOptionId == undefined || !ValidaSiEsNumero(EntityTypeCatalogOptionId)) {
                                        EntityTypeCatalogOptionId = 0;
                                    }

                                    if (item["Codigo"] == undefined) {
                                        utilidades.mensajeError("La columna Codigo no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["Codigo"])) {
                                        utilidades.mensajeError("El valor Codigo " + item["Codigo"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["Entidad"] == undefined) {
                                        utilidades.mensajeError("La columna Entidad no trae valor!");
                                        return false;
                                    }

                                    if (item["NacionCSF"] == undefined) {
                                        utilidades.mensajeError("La columna 'NacionCSF' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDicimal(item["NacionCSF"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'NacionCSF' " + item["NacionCSF"] + ". La columna 'NacionCSF' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }

                                    if (item["NacionSSF"] == undefined) {
                                        utilidades.mensajeError("La columna 'NacionSSF' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDicimal(item["NacionSSF"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'NacionSSF' " + item["NacionSSF"] + ". La columna 'NacionSSF' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }

                                    if (item["Propios"] == undefined) {
                                        utilidades.mensajeError("La columna 'Propios' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDicimal(item["Propios"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'Propios' " + item["Propios"] + ". La columna 'Propios' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }

                                    var valoresarchivo = {
                                        EntityTypeCatalogOptionId: EntityTypeCatalogOptionId,
                                        Codigo: item["Codigo"],
                                        Entidad: item["Entidad"],
                                        Sector: "",
                                        Propios: item["Propios"],
                                        NacionSSF: item["NacionSSF"],
                                        NacionCSF: item["NacionCSF"],
                                        Vigencia: new Date().getFullYear()
                                    }

                                    vm.listaCuotasArchivo.ValoresCuotaEntidad.push(valoresarchivo);

                                });

                                if (resultado.indexOf(false) == -1) {

                                    vm.listaCuotasArchivo.totalPropios = vm.listaCuotas.totalPropios;
                                    vm.listaCuotasArchivo.TotalNacionSSF = vm.listaCuotas.TotalNacionSSF;
                                    vm.listaCuotasArchivo.TotalNacionCSF = vm.listaCuotas.TotalNacionCSF;

                                    ValidarRegistros(vm.listaCuotasArchivo.ValoresCuotaEntidad);


                                }
                                else {
                                    vm.activarControles('inicio');
                                    vm.listaCuotasArchivo = {};

                                    vm.listaCuotasArchivo.totalPropios = vm.listaCuotas.totalPropios;
                                    vm.listaCuotasArchivo.TotalNacionSSF = vm.listaCuotas.TotalNacionSSF;
                                    vm.listaCuotasArchivo.TotalNacionCSF = vm.listaCuotas.TotalNacionCSF;
                                    vm.listaCuotasArchivo.ValoresCuotaEntidad = [];
                                }
                            };
                            reader.readAsBinaryString(file);
                        }
                    }
                }
            }
        }

        function ValidarDicimal(valor, decimals) {

            //if (valor.toString().includes(',')) {
            //    return false;
            //}
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
                    //$("#btnCuotaCargaMasivaValidarArchivo").attr('disabled', true);
                    $("#btnCuotaCargaMasivaLimpiarArchivo").attr('disabled', true);
                    $("#btnCuotaCargaMasivaArchivoSeleccionado").attr('disabled', true);
                    document.getElementById('filecuotaCargaMasiva').value = "";
                    vm.nombrearchivo = "";
                    break;
                case "cargaarchivo":
                    $("#btnCuotaCargaMasivaValidarArchivo").attr('disabled', false);
                    $("#btnCuotaCargaMasivaLimpiarArchivo").attr('disabled', false);
                    $("#btnCuotaCargaMasivaArchivoSeleccionado").attr('disabled', true);
                    break;
                case "validado":
                    $("#btnCuotaCargaMasivaValidarArchivo").attr('disabled', false);
                    $("#btnCuotaCargaMasivaLimpiarArchivo").attr('disabled', false);
                    $("#btnCuotaCargaMasivaArchivoSeleccionado").attr('disabled', false);

                    vm.HabilitaEditarBandera = true;
                    break;
                default:
            }
        }

        $scope.filecuotaCargaMasivaNameChanged = function (input) {
            if (input.files.length == 1) {
                vm.nombrearchivo = input.files[0].name;
                vm.activarControles('cargaarchivo');
            }
            else {
                //vm.filename = input.files.length + " archivos"               
                vm.activarControles('inicio');
            }
        }

        $scope.validaCuotaCargaMasivaNombreArchivo = function (nombre) {
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
    }

    angular.module('backbone').component('cuota', {
        templateUrl: 'src/app/programacion/componentes/cargaMasiva/cuota.html',
        controller: cuotaController,
        controllerAs: "vm",
        //bindings: {
        //    callback: '&',
        //    notificacionvalidacion: '&',
        //    notificacionestado: '&',
        //    guardadoevent: '&',
        //    notificarrefresco: '&',
        //    notificacioncambios: '&'
        //}
    });
})()