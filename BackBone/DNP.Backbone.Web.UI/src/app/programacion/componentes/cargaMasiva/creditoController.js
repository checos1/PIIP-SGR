(function () {
    'use strict';

    creditoController.$inject = ['$scope',
        'constantesAutorizacion',
        '$uibModal',
        'FileSaver',
        'utilidades',
        'autorizacionServicios',
        'creditoServicio'];

    function creditoController($scope, constantesAutorizacion, $uibModal, FileSaver, utilidades, autorizacionServicios, creditoServicio) {
        var vm = this;

        vm.descargarPlantila = descargarPlantila;
        vm.listaCreditos = [];
        vm.listaCreditosArchivo = [];
        vm.listaCreditosError = [];
        vm.MostrarErrores = false;
        vm.archivoValido = true;

        vm.exportarCreditoCargaMasivaExcelErrores = exportarCreditoCargaMasivaExcelErrores;

        vm.HabilitaEditarBandera = false;
        vm.HabilitaEditar = HabilitaEditar;
        vm.limpiarArchivo = limpiarArchivo;
        vm.validarArchivo = validarArchivo;
        vm.RegistrarCargaMasivaCreditos = RegistrarCargaMasivaCreditos;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.nombrearchivo = "Seleccione Archivo";
        vm.CreditoCargaMasiva = null;
        vm.CreditoCargaMasivaArchivo = [];

        vm.init = function () {
            vm.CreditoCargaMasivaArchivo = [];
            vm.activarControles('inicio');
            vm.ObtenerCargaMasivaCreditos();
        };

        vm.ObtenerCargaMasivaCreditos = function () {
            return creditoServicio.ObtenerCargaMasivaCreditos().then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.listaCreditos = jQuery.parseJSON(respuesta.data);
                    }
                });
        }

        function ValidarRegistros(lista) {
            vm.listaCreditosError = [];

            return creditoServicio.ValidarCargaMasivaCreditos(JSON.stringify(lista)).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        vm.listaCreditosError = jQuery.parseJSON(respuesta.data.Result);
                        console.log(vm.listaCreditosError);

                        vm.archivoValido = true;
                        vm.MostrarErrores = false;

                        if (vm.listaCreditosError != undefined) {
                            vm.listaCreditosError.forEach(itemError => {
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

            exportarCreditoCargaMasivaExcel();
        }

        function exportarCreditoCargaMasivaExcel() {

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
                        name: 'Id', title: 'Id'
                    },
                    {
                        name: 'EntityTypeCatalogOptionId', title: 'EntityTypeCatalogOptionId'
                    },
                    {
                        name: 'EstadoId', title: 'EstadoId'
                    },
                    {
                        name: 'TipoId', title: 'TipoId'
                    },
                    {
                        name: 'Codigo', title: 'Codigo'
                    },
                    {
                        name: 'Contrato', title: 'Contrato'
                    },
                    {
                        name: 'codigoEntidad', title: 'codigoEntidad'
                    },
                    {
                        name: 'Razon', title: 'Razon'
                    },
                    {
                        name: 'Vigencia', title: 'Vigencia'
                    },
                    {
                        name: 'Monto', title: 'Monto'
                    },
                    {
                        name: 'EstadoFirma', title: 'EstadoFirma'
                    },
                    {
                        name: 'Tipo', title: 'Tipo'
                    }
                ];

                let colNames = columns.map(function (item) {
                    return item.title;
                })

                var wb = XLSX.utils.book_new();

                wb.Props = {
                    Title: "Plantilla cuota de créditos",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };

                wb.SheetNames.push("Créditos");

                const header = colNames;
                const data = [];


                vm.listaCreditos.forEach(credito => {

                    data.push({
                        Id: credito.Id,
                        EntityTypeCatalogOptionId: credito.EntityTypeCatalogOptionId,
                        EstadoId: credito.EstadoId,
                        TipoId: credito.TipoId,
                        Codigo: credito.Codigo,
                        Contrato: credito.NombreCredito,
                        codigoEntidad: credito.CodigoEntidad,
                        Razon: credito.Entidad,
                        Vigencia: credito.Vigencia,
                        Monto: credito.Monto,
                        EstadoFirma: credito.NombreEstadoCredito,
                        Tipo: credito.TipoCredito
                    });
                });

                const worksheet = XLSX.utils.json_to_sheet(data, {
                    header: [
                        "Id",
                        "EntityTypeCatalogOptionId",
                        "EstadoId",
                        "TipoId",
                        "Codigo",
                        "Contrato",
                        "codigoEntidad",
                        "Razon",
                        "Vigencia",
                        "Monto",
                        "EstadoFirma",
                        "Tipo"]
                });

                for (let col of [9]) {
                    formatColumn(worksheet, col, "#,##")
                }


                /* hide second column */
                worksheet['!cols'] = [];

                worksheet['!cols'][0] = { hidden: true };
                worksheet['!cols'][1] = { hidden: true };
                worksheet['!cols'][2] = { hidden: true };
                worksheet['!cols'][3] = { hidden: true };
                worksheet['!cols'][8] = { hidden: true };

                wb.Sheets["Créditos"] = worksheet;

                var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
                //saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaAjusteRegionalizacion.xlsx');
                saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'PlantillaCreditos.xlsx');
            }, function funcionCancelar(reason) {
            }, false, false, "Este archivo es compatible con Office 365");
        }

        function exportarCreditoCargaMasivaExcelErrores() {

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
                        name: 'Id', title: 'Id'
                    },
                    {
                        name: 'EntityTypeCatalogOptionId', title: 'EntityTypeCatalogOptionId'
                    },
                    {
                        name: 'EstadoId', title: 'EstadoId'
                    },
                    {
                        name: 'TipoId', title: 'TipoId'
                    },
                    {
                        name: 'Codigo', title: 'Codigo'
                    },
                    {
                        name: 'Contrato', title: 'Contrato'
                    },
                    {
                        name: 'codigoEntidad', title: 'codigoEntidad'
                    },
                    {
                        name: 'Razon', title: 'Razon'
                    },
                    {
                        name: 'Vigencia', title: 'Vigencia'
                    },
                    {
                        name: 'Monto', title: 'Monto'
                    },
                    {
                        name: 'EstadoFirma', title: 'EstadoFirma'
                    },
                    {
                        name: 'Tipo', title: 'Tipo'
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
                    Title: "Errores carga créditos",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };

                wb.SheetNames.push("Créditos");

                const header = colNames;
                const data = [];


                vm.listaCreditosError.forEach(credito => {

                    data.push({
                        Id: credito.Id,
                        EntityTypeCatalogOptionId: credito.EntityTypeCatalogOptionId,
                        EstadoId: credito.EstadoId,
                        TipoId: credito.TipoId,
                        Codigo: credito.Codigo,
                        Contrato: credito.NombreCredito,
                        codigoEntidad: credito.CodigoEntidad,
                        Razon: credito.Entidad,
                        Vigencia: credito.Vigencia,
                        Monto: credito.Monto,
                        EstadoFirma: credito.NombreEstadoCredito,
                        Tipo: credito.TipoCredito,
                        ValidacionError: credito.ValidacionError
                    });
                });

                const worksheet = XLSX.utils.json_to_sheet(data, {
                    header: [
                        "Id",
                        "EntityTypeCatalogOptionId",
                        "EstadoId",
                        "TipoId",
                        "Codigo",
                        "Contrato",
                        "codigoEntidad",
                        "Razon",
                        "Vigencia",
                        "Monto",
                        "EstadoFirma",
                        "Tipo",
                        "ValidacionError"]
                });

                for (let col of [9]) {
                    formatColumn(worksheet, col, "#,##")
                }


                /* hide second column */
                worksheet['!cols'] = [];

                worksheet['!cols'][0] = { hidden: true };
                worksheet['!cols'][1] = { hidden: true };
                worksheet['!cols'][2] = { hidden: true };
                worksheet['!cols'][3] = { hidden: true };
                worksheet['!cols'][8] = { hidden: true };

                wb.Sheets["Créditos"] = worksheet;

                var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
                //saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaAjusteRegionalizacion.xlsx');
                saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'ErrorCreditos.xlsx');
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
            document.getElementById('filecreditoCargaMasiva').value = "";
            document.getElementById('filecreditoCargaMasiva').click();
        }

        function limpiarArchivo() {
            $scope.filescreditoCargaMasiva = [];
            document.getElementById('filecreditoCargaMasiva').value = "";
            vm.activarControles('inicio');
        }

        function RegistrarCargaMasivaCreditos() {
            return creditoServicio.RegistrarCargaMasivaCreditos(JSON.stringify(vm.listaCreditosArchivo)).then(
                function (respuesta) {

                    if (respuesta.data.Status) {
                        utilidades.mensajeSuccess("Registro de carga masiva de créditos exitosa", false, false, false, "Carga Exitosa.");
                        vm.activarControles('inicio');

                        vm.ObtenerCargaMasivaCreditos();

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
            vm.listaCreditosArchivo = [];

            if (filecreditoCargaMasiva.files.length > 0) {

                let file = document.getElementById("filecreditoCargaMasiva").files[0];
                if ($scope.validaCreditoCargaMasivaNombreArchivo(file.name)) {
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

                                    let Id = item["Id"];
                                    if (Id == undefined || !ValidaSiEsNumero(Id)) {
                                        Id = 0;
                                    }

                                    let Vigencia = item["Vigencia"];
                                    if (Vigencia == undefined || !ValidaSiEsNumero(Vigencia)) {
                                        Vigencia = new Date().getFullYear() + 1;
                                    }

                                    if (item["Codigo"] == undefined) {
                                        utilidades.mensajeError("La columna Codigo no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["Codigo"])) {
                                        utilidades.mensajeError("El valor Codigo " + item["Codigo"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["Contrato"] == undefined) {
                                        utilidades.mensajeError("La columna Contrato no trae valor!");
                                        return false;
                                    }

                                    if (item["codigoEntidad"] == undefined) {
                                        utilidades.mensajeError("La columna codigoEntidad no trae valor!");
                                        return false;
                                    }

                                    if (item["Razon"] == undefined) {
                                        utilidades.mensajeError("La columna Razon no trae valor!");
                                        return false;
                                    }

                                    if (item["Monto"] == undefined) {
                                        utilidades.mensajeError("La columna 'Monto' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDicimal(item["Monto"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'MetaMonto' " + item["Monto"] + ". La columna 'Monto' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }

                                    if (item["EstadoFirma"] == undefined) {
                                        utilidades.mensajeError("La columna EstadoFirma no trae valor!");
                                        return false;
                                    }

                                    if (item["Tipo"] == undefined) {
                                        utilidades.mensajeError("La columna Tipo no trae valor!");
                                        return false;
                                    }

                                    var valoresarchivo = {
                                        Id: Id,
                                        EntityTypeCatalogOptionId: item["EntityTypeCatalogOptionId"],
                                        CodigoEntidad: item["codigoEntidad"],
                                        Entidad: item["Razon"],
                                        Codigo: item["Codigo"],
                                        EstadoId: item["EstadoId"],
                                        NombreEstadoCredito: item["EstadoFirma"],
                                        NombreCredito: item["Contrato"],
                                        Monto: item["Monto"],
                                        Vigencia: Vigencia,
                                        TipoId: item["TipoId"],
                                        TipoCredito: item["Tipo"]
                                    }

                                    vm.listaCreditosArchivo.push(valoresarchivo);

                                });

                                if (resultado.indexOf(false) == -1) {

                                    ValidarRegistros(vm.listaCreditosArchivo);


                                }
                                else {
                                    vm.activarControles('inicio');
                                    vm.listaCreditosArchivo = [];
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
                    //$("#btnCreditoCargaMasivaValidarArchivo").attr('disabled', true);
                    $("#btnCreditoCargaMasivaLimpiarArchivo").attr('disabled', true);
                    $("#btnCreditoCargaMasivaArchivoSeleccionado").attr('disabled', true);
                    document.getElementById('filecreditoCargaMasiva').value = "";
                    vm.nombrearchivo = "";
                    break;
                case "cargaarchivo":
                    $("#btnCreditoCargaMasivaValidarArchivo").attr('disabled', false);
                    $("#btnCreditoCargaMasivaLimpiarArchivo").attr('disabled', false);
                    $("#btnCreditoCargaMasivaArchivoSeleccionado").attr('disabled', true);
                    break;
                case "validado":
                    $("#btnCreditoCargaMasivaValidarArchivo").attr('disabled', false);
                    $("#btnCreditoCargaMasivaLimpiarArchivo").attr('disabled', false);
                    $("#btnCreditoCargaMasivaArchivoSeleccionado").attr('disabled', false);

                    vm.HabilitaEditarBandera = true;
                    break;
                default:
            }
        }

        $scope.filecreditoCargaMasivaNameChanged = function (input) {
            if (input.files.length == 1) {
                vm.nombrearchivo = input.files[0].name;
                vm.activarControles('cargaarchivo');
            }
            else {
                //vm.filename = input.files.length + " archivos"               
                vm.activarControles('inicio');
            }
        }

        $scope.validaCreditoCargaMasivaNombreArchivo = function (nombre) {
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

    angular.module('backbone').component('credito', {
        templateUrl: 'src/app/programacion/componentes/cargaMasiva/credito.html',
        controller: creditoController,
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