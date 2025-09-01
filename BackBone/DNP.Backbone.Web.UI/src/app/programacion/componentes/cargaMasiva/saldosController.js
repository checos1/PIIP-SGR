(function () {
    'use strict';

    saldosController.$inject = ['$scope',
        'utilidades',
        'calendarioServicio',
        '$sessionStorage',
        'creditoServicio',
        'saldosServicio'
    ];

    function saldosController($scope,
        utilidades,
        calendarioServicio,
        $sessionStorage,
        creditoServicio,
        saldosServicio
    ) {
        var vm = this;

        vm.descargarPlantila = descargarPlantila;

        vm.listaSaldodArchivo = [];
        vm.listaDetalleSaldodArchivo = [];
        vm.listaSaldosError = [];
        vm.SaldodArchivo = {};
        vm.MostrarErrores = false;
        vm.archivoValido = true;
        vm.carguesIntegracionId = undefined;
        vm.MostrarDetalle = false;
        vm.DetalleMostrado = undefined;

        vm.exportarSaldosCargaMasivaExcelErrores = exportarSaldosCargaMasivaExcelErrores;

        vm.HabilitaEditarBandera = false;
        vm.HabilitaEditar = HabilitaEditar;
        vm.limpiarArchivo = limpiarArchivo;
        vm.validarArchivo = validarArchivo;
        vm.RegistrarCargaMasivaSaldos = RegistrarCargaMasivaSaldos;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.nombrearchivo = "Seleccione Archivo";
        vm.CreditoCargaMasiva = null;
        vm.CreditoCargaMasivaArchivo = [];
        vm.mostrarDetalleCargueSaldos = mostrarDetalleCargueSaldos;
        vm.descargarCargueSaldos = descargarCargueSaldos;
        vm.stylecolor = {
            "color": "#004884",
            "font-weight": "600"
        };
        vm.stylecolorgris = {
            "color": "#4B4B4B"
        };

        vm.listaTipoArchivos = [];
        vm.idTipoArchivoSeleccionado = undefined;


        //     add key = "uriObtenerLogErrorCargaMasivaSaldos" value = "api/Programacion/ObtenerLogErrorCargaMasivaSaldos" />
        //<add key="uriObtenerCargaMasivaSaldos" value="api/Programacion/ObtenerCargaMasivaSaldos"/>
        //<add key="uriObtenerTipoCargaMasiva" value="api/Programacion/ObtenerTipoCargaMasiva"/>

        vm.initSaldos = function () {

            saldosServicio.obtenerTipoCargaMasiva('saldos')
                .then(function (rta) {
                    if (rta != undefined && rta.data != null) {
                        var lista = JSON.parse(rta.data);
                        lista.forEach(function (dato, index) {
                            vm.listaTipoArchivos.push({
                                Id: dato.Id,
                                TipoArchivo: dato.TipoCargue
                            });
                        });

                    }

                });


            vm.activarControles('inicio');


            vm.ObtenerCargaMasivaSaldos();
        };

        vm.CambiarTipoArchivo = function () {
            if (vm.idTipoArchivoSeleccionado !== '' && vm.idTipoArchivoSeleccionado !== undefined) {
                vm.activarControles("seleccionoArchivo");
            }
            else {
                vm.idTipoArchivoSeleccionado = undefined;
                vm.activarControles("inicio");
            }
        }

        vm.ObtenerCargaMasivaSaldos = function () {
            var idtipoarchivo = vm.idTipoArchivoSeleccionado === undefined ? 0 : vm.idTipoArchivoSeleccionado;
            return saldosServicio.obtenerCargaMasivaSaldos(idtipoarchivo).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.listaCarguesSaldos = jQuery.parseJSON(respuesta.data);
                    }
                });
        }

        vm.abrirTooltip = function (value) {
            var texto = '';
            if (value === 'Resumen')
                texto = 'Esta es la explicación del resumen de las cargas de saldos....';
            else if (value === 'Detalle')
                texto = 'Esta es la explicación del detalle de las cargas de saldos....';
            else if (value === 'Plantilla')
                texto = 'Esta es la explicación de la plantilla de las cargas de saldos....';
            else if (value === 'Errores')
                texto = 'Esta es la explicación de descarga de errores de las cargas de saldos....';
            utilidades.mensajeInformacion(texto, false, "Archivos en proceso actual");
        }

        function mostrarDetalleCargueSaldos(Id) {
            vm.MostrarDetalle = true;
            if (!validaDetalleMostrado(Id)) {
                vm.DetalleMostrado = Id;
                vm.listaDetalleSaldodArchivo = [];
                return saldosServicio.obtenerDetalleCargaMasivaSaldos(Id).then(
                    function (respuesta) {
                        if (respuesta.data != null && respuesta.data != "") {
                            vm.listaDetalleSaldodArchivo = jQuery.parseJSON(respuesta.data);
                            obtenerDetalleSaldos(Id);
                        }
                    });
            }

        }

        function descargarCargueSaldos(Id) {
            obtenerDetalleSaldos(Id).then(function (rta) {
                /*region cabecera archivo*/
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
                        name: 'CodigoUnidadEjecutora', title: 'CodigoUnidadEjecutora'
                    },
                    {
                        name: 'UnidadEjecutora', title: 'UnidadEjecutora'
                    },
                    {
                        name: 'TipoGasto', title: 'TipoGasto'
                    },
                    {
                        name: 'Programa', title: 'Programa'
                    },
                    {
                        name: 'Subprograma', title: 'Subprograma'
                    },
                    {
                        name: 'SOrd', title: 'SOrd'
                    },
                    {
                        name: 'Fuente', title: 'Fuente'
                    },
                    {
                        name: 'TipoRecurso', title: 'TipoRecurso'
                    },
                    {
                        name: 'SituacionFondo', title: 'SituacionFondo'
                    },
                    {
                        name: 'Rubro', title: 'Rubro'
                    },
                    {
                        name: 'Valor', title: 'Valor'
                    },
                    {
                        name: 'Estado', title: 'Estado'
                    },

                    {
                        name: 'DescripcionProceso', title: 'DescripcionProceso'
                    },
                    {
                        name: 'Fecha', title: 'Fecha'
                    }
                ];


                let colNames = columns.map(function (item) {
                    return item.title;
                })

                var wb = XLSX.utils.book_new();

                wb.Props = {
                    Title: "Detalle cargue saldos",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };

                wb.SheetNames.push("Saldos");

                const header = colNames;
                if (vm.listaDetalleSaldodArchivo.length > 0) {
                    const worksheet = XLSX.utils.json_to_sheet(vm.listaDetalleSaldodArchivo, {
                        header: [
                            "CodigoUnidadEjecutora",
                            "UnidadEjecutora",
                            "TipoGasto",
                            "Programa",
                            "Subprograma",
                            "SOrd",
                            "Fuente",
                            "TipoRecurso",
                            "SituacionFondo",
                            "Rubro",
                            "Valor",
                            "Estado",
                            "DescripcionProceso",
                            "Fecha"]
                    });

                    worksheet['!cols'] = [];

                    wb.Sheets["Saldos"] = worksheet;

                    var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
                    //saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaAjusteRegionalizacion.xlsx');
                    saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'DetalleCargueSaldos.xlsx');


                };
            });
        }

        function obtenerDetalleSaldos(value) {
            return saldosServicio.obtenerDetalleCargaMasivaSaldos(value).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.listaDetalleSaldodArchivo = jQuery.parseJSON(respuesta.data);
                    }
                });
        }
        function validaDetalleMostrado(Id) {
            if (vm.DetalleMostrado === Id)
                return true;
            else
                return false;
        }

        vm.ocultarDetalle = function () {
            vm.MostrarDetalle = false;
        }


        function descargarPlantila() {

            exportarSaldosCargaMasivaExcel();
        }

        function exportarSaldosCargaMasivaExcel() {

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
                        name: 'Codigo', title: 'Código'
                    },
                    {
                        name: 'NombreUnidadEjecutora', title: 'Nombre Unidad Ejecutora'
                    },
                    {
                        name: 'TipoGasto', title: 'Tipo Gasto'
                    },
                    {
                        name: 'Cta_Prog', title: 'Cta.Prog'
                    },
                    {
                        name: 'ObjG_Proy', title: 'ObjG Proy'
                    },
                    {
                        name: 'Ord_SubP_Gasto', title: 'Ord SubP.Gasto'
                    },
                    {
                        name: 'SOrd', title: 'SOrd'
                    },
                    {
                        name: 'Fuente', title: 'Fuente'
                    },
                    {
                        name: 'Rec', title: 'Rec'
                    },
                    {
                        name: 'Sit', title: 'Sit'
                    },
                    {
                        name: 'Nombre_del_rubro', title: 'Nombre del rubro'
                    },
                    {
                        name: 'Proyecto', title: 'Proyecto'
                    }
                ];

                let colNames = columns.map(function (item) {
                    return item.title;
                })

                var wb = XLSX.utils.book_new();

                wb.Props = {
                    Title: "Plantilla saldos",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };

                wb.SheetNames.push("Saldos");

                const header = colNames;
                const data = [];


                //vm.listaCreditos.forEach(credito => {

                data.push({
                    Codigo: '01-01-01',
                    NombreUnidadEjecutora: 'SENADO DE LA REPÚBLICA',
                    TipoGasto: 'C',
                    Cta_Prog: '0199',
                    ObjG_Proy: '1000',
                    Ord_SubP_Gasto: 6,
                    SOrd: 0,
                    Fuente: 1,
                    Rec: 11,
                    Sit: 'C',
                    Nombre_del_rubro: 'MEJORAMIENTO DE LAS CONDICIONES DE SEGURIDAD Y OPORTUNIDAD EN LOS DESPLAZAMIENTOS DE LOS SERVIDORES PÚBLICOS DEL SENADO DE LA REPÚBLICA  NACIONAL',
                    Proyecto: '45.876.166.683,00'.toLocaleString("es-ES")
                });
                //});

                const worksheet = XLSX.utils.json_to_sheet(data, {
                    header: [
                        'Codigo',
                        'NombreUnidadEjecutora',
                        'TipoGasto',
                        'Cta_Prog',
                        'ObjG_Proy',
                        'Ord_SubP_Gasto',
                        'SOrd',
                        'Fuente',
                        'Rec',
                        'Sit',
                        'Nombre_del_rubro',
                        'Proyecto',
                    ]
                });


                //for (let col of [11]) {
                //    formatColumn(worksheet, col, "{0:0,0.00}")
                //}


                /* hide second column */
                worksheet['!cols'] = [];

                //worksheet['!cols'][0] = { hidden: true };
                //worksheet['!cols'][1] = { hidden: true };
                //worksheet['!cols'][2] = { hidden: true };
                //worksheet['!cols'][3] = { hidden: true };
                //worksheet['!cols'][8] = { hidden: true };

                wb.Sheets["Saldos"] = worksheet;

                var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
                //saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaAjusteRegionalizacion.xlsx');
                saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'PlantillaSaldos.xlsx');
            }, function funcionCancelar(reason) {
            }, false, false, "Este archivo es compatible con Office 365");
        }

        function exportarSaldosCargaMasivaExcelErrores() {
            const data = [];

            //vm.MostrarErrores = true;
            utilidades.mensajeWarning("Si ocurren inconvenientes de descarga o visualización, es necesario actualizar la aplicación.", function funcionContinuar() {

                /*region cabecera archivo*/
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
                        name: 'Codigo', title: 'Código'
                    },
                    {
                        name: 'NombreUnidadEjecutora', title: 'Nombre Unidad Ejecutora'
                    },
                    {
                        name: 'TipoGasto', title: 'Tipo Gasto'
                    },
                    {
                        name: 'CodigoPrograma', title: 'Cta.Prog'
                    },
                    {
                        name: 'CodigoSubprograma', title: 'ObjG Proy'
                    },
                    {
                        name: 'Ord_SubP_Gasto', title: 'Ord SubP.Gasto'
                    },
                    {
                        name: 'SOrd', title: 'SOrd'
                    },
                    {
                        name: 'Fuente', title: 'Fuente'
                    },
                    {
                        name: 'TipoRecursos', title: 'Rec'
                    },
                    {
                        name: 'SituacionFondos', title: 'Sit'
                    },
                    {
                        name: 'Rubro', title: 'Nombre del rubro'
                    },
                    {
                        name: 'ValorProyecto', title: 'Proyecto'
                    },

                    {
                        name: 'Obervacion', title: 'ValidacionError'
                    }
                ];


                let colNames = columns.map(function (item) {
                    return item.title;
                })

                var wb = XLSX.utils.book_new();

                wb.Props = {
                    Title: "Errores carga saldo",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };

                wb.SheetNames.push("Saldos");

                const header = colNames;
                if (vm.listaSaldosError.length > 0) {
                    const worksheet = XLSX.utils.json_to_sheet(vm.listaSaldosError, {
                        header: [
                            "NombreProceso",
                            "CodigoUnidadEjecutora",
                            "UnidadEjecutora",
                            "Fuente",
                            "Situacion",
                            "Recurso",
                            "NombreRubro",
                            "CodigoPresupuestal",
                            "Obervacion"]
                    });

                    worksheet['!cols'] = [];

                    wb.Sheets["Saldos"] = worksheet;

                    var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
                    //saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaAjusteRegionalizacion.xlsx');
                    saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'ErrorSaldos.xlsx');


                    /*endregion cabecera archivo*/


                }
                /* hide second column */

            }, function funcionCancelar(reason) {
            }, false, false, "Este archivo es compatible con Office 365");
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
            document.getElementById('archivoCargaMasiva').value = "";
            document.getElementById('archivoCargaMasiva').click();
        }

        function limpiarArchivo() {
            $scope.archivosCargaMasiva = [];
            document.getElementById('archivoCargaMasiva').value = "";
            vm.activarControles('limpiarArchivo');
        }

        function RegistrarCargaMasivaSaldos() {
            vm.listaSaldosError = [];
            return saldosServicio.registrarCargaMasivaSaldos(vm.idTipoArchivoSeleccionado).then(
                function (respuesta) {

                    if (respuesta.status === 200 || reapuesta.statusText === "OK") {
                        utilidades.mensajeSuccess("Registro de carga masiva de saldos exitosa", false, false, false, "Carga Exitosa.");
                        $("#btnBuscarArchivo").removeClass('colorDeshabilitaBuscarArchivo');
                        $("#btnBuscarArchivo").addClass("btncargarch");
                        $("#btnBuscarArchivo").attr('disabled', false);
                        vm.activarControles('limpiarArchivo');


                        vm.ObtenerCargaMasivaSaldos();

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
            vm.SaldodArchivo.DatosExcel.map(function (item) {
                item.ValorProyecto = item.ValorProyecto.toString().replaceAll('.', '').replaceAll(',', '.');
            });
            var listaEnvio = JSON.stringify(vm.SaldodArchivo);
            saldosServicio.validarCargaMasiva(listaEnvio)
                .then(function (respuesta) {
                    if (respuesta.data != null && respuesta.data.Result && respuesta.data.Result.Exito) {
                        vm.carguesIntegracionId = respuesta.data.Result.Mensaje;
                        utilidades.mensajeSuccess('Si se presentan inconsistencias en el diligenciamiento de los datos, identifiquelos descargando el archivo de errores. Si requiere cargar el archivo, hágalo mediante el botón "Procesar".', false, false, false, "Se ha generado la validación del archivo.");
                        saldosServicio.obtenerLogErrorCargaMasivaSaldos(vm.idTipoArchivoSeleccionado, vm.carguesIntegracionId)
                            .then(function (respuesta) {
                                var datos = jQuery.parseJSON(respuesta.data);
                                if (datos.CarguePorEntidad !== null) {
                                    datos.CarguePorEntidad.map(function (itemCabecera) {
                                        if (itemCabecera.DetalleCargue !== null) {
                                            itemCabecera.DetalleCargue.map(function (detalle) {
                                                vm.listaSaldosError.push({
                                                    NombreProceso: detalle.NombreProceso,
                                                    CodigoUnidadEjecutora: detalle.CodigoUnidadEjecutora,
                                                    UnidadEjecutora: detalle.UnidadEjecutora,
                                                    Fuente: detalle.Fuente,
                                                    Situacion: detalle.Situacion,
                                                    Recurso: detalle.Recurso,
                                                    NombreRubro: detalle.NombreRubro,
                                                    CodigoPresupuestal: detalle.CodigoPresupuestal,
                                                    Obervacion: detalle.Obervacion
                                                });
                                            });

                                        }
                                        if (vm.listaSaldosError.length > 0)
                                            vm.MostrarErrores = true;

                                    });
                                }

                            });




                        vm.activarControles('validado');
                        // vm.listaCargues = jQuery.parseJSON(respuesta.data);
                    }
                });
        }

        function mensajerror() {
            utilidades.mensajeError("Verifique que el archivo cumple con las condiciones de tipo archivo 'XLSX', peso máximo 5 (MGB), estructura de filas, columnas y celdas de acuerdo a la plantilla de carga  masiva y el total diligenciamiento.", false, "Las condiciones del archivo presentan iconsistencias");

        }

        function validarLen(valor, maximo, minimo) {
            if (valor.length > maximo || valor.length < minimo)
                return false;
            else
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
                case "limpiarArchivo":
                    $("#btnCancelarArchivo").attr('disabled', true);
                    $("#btnCancelarArchivo").removeClass('btncancearch');
                    $("#btnCancelarArchivo").addClass("btncancearchDisabled");

                    $("#btnCargaMasivaValidarArchivo").attr('disabled', true);
                    $("#btnCargaMasivaLimpiarArchivo").attr('disabled', true);
                    $("#btnCargaMasivaArchivoSeleccionado").attr('disabled', true);
                    document.getElementById('archivoCargaMasiva').value = "";
                    vm.nombrearchivo = "";
                    if (evento === "limpiarArchivo") {
                        $("#btnBuscarArchivo").removeClass('colorDeshabilitaBuscarArchivo');
                        $("#btnBuscarArchivo").addClass("btncargarch");
                        $("#btnBuscarArchivo").attr('disabled', false);
                    }
                    else {
                        $("#btnBuscarArchivo").addClass('colorDeshabilitaBuscarArchivo');
                        $("#btnBuscarArchivo").removeClass("btncargarch");
                        $("#btnBuscarArchivo").attr('disabled', true);
                    }
                    break;

                case "seleccionoArchivo":
                    $("#btnBuscarArchivo").removeClass('colorDeshabilitaBuscarArchivo');
                    $("#btnBuscarArchivo").addClass("btncargarch");
                    $("#btnBuscarArchivo").attr('disabled', false);

                    break;
                case "cargaarchivo":
                    $("#btnCancelarArchivo").attr('disabled', false);
                    $("#btnCancelarArchivo").removeClass('btncancearchDisabled');
                    $("#btnCancelarArchivo").addClass("btncancearch");

                    $("#btnCargaMasivaValidarArchivo").attr('disabled', false);
                    $("#btnCargaMasivaLimpiarArchivo").attr('disabled', false);
                    $("#btnCargaMasivaArchivoSeleccionado").attr('disabled', true);
                    break;
                case "validado":
                    $("#btnCargaMasivaValidarArchivo").attr('disabled', true);
                    $("#btnCargaMasivaArchivoSeleccionado").attr('disabled', false);
                    vm.HabilitaEditarBandera = true;
                    break;
                default:
            }
        }

        $scope.archivoCargaMasivaNameChanged = function (input) {
            var resultado = true;
            vm.SaldodArchivo = {};
            vm.SaldodArchivo.TipoCargueId = vm.idTipoArchivoSeleccionado;
            vm.carguesIntegracionId = undefined;
            vm.listaSaldodArchivo = [];
            vm.MostrarErrores = false;
            var mensaje = '';
            if (input.files.length == 1) {
                vm.nombrearchivo = input.files[0].name;


                if (archivoCargaMasiva.files.length > 0) {

                    let file = document.getElementById("archivoCargaMasiva").files[0];
                    if (!$scope.validaarchivoCargaMasivanombrearchivo(file.name)) {
                        mensajerror();
                        //return;
                    }
                    else {
                        if (typeof (FileReader) != "undefined") {
                            var reader = new FileReader();
                            if (reader.readAsBinaryString) {
                                reader.onload = function (e) {
                                    var workbook = XLSX.read(e.target.result, {
                                        type: 'binary'
                                    });
                                    var firstSheet = workbook.SheetNames[0];
                                    var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[firstSheet]);
                                    var ctaColumnas = 0;
                                    resultado = excelRows.map(function (item, index) {

                                        var cta = Object.values(item);

                                        if (cta.length !== 12) {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false;
                                        }

                                        if (item["Codigo"] == undefined) {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false;
                                        }
                                        else if (!validarLen(item["Codigo"].trim(), 9, 6)) {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false
                                        }
                                        else {
                                            var tmp = item["Codigo"].split("-")
                                            tmp.map(function (itemcodigo) {
                                                if (itemcodigo === undefined || !validarLen(itemcodigo, 2, 1) || !ValidaSiEsNumero(itemcodigo)) {
                                                    mensajerror();
                                                    vm.activarControles('inicio');
                                                    return false
                                                }
                                            });
                                        }

                                        if (item["NombreUnidadEjecutora"] === undefined || item["NombreUnidadEjecutora"] === '') {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false
                                        }

                                        if (item["TipoGasto"] == undefined) {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false;
                                        }
                                        else if (item["TipoGasto"] !== 'C' && item["TipoGasto"] !== 'c') {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false;
                                        }

                                        if (item["Cta_Prog"] == undefined) {
                                            mensajerror();
                                            return false;
                                        }
                                        else if (!validarLen(item["Cta_Prog"], 4, 1)) {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false;
                                        }


                                        if (item["ObjG_Proy"] == undefined) {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false;
                                        }
                                        else if (!validarLen(item["ObjG_Proy"], 4, 1)) {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false;
                                        }

                                        if (item["Ord_SubP_Gasto"] == undefined) {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false;
                                        }
                                        else if (!ValidaSiEsNumero(item["Ord_SubP_Gasto"])) {
                                            vm.activarControles('inicio');
                                            mensajerror();
                                            return false;
                                        }

                                        if (item["SOrd"] == undefined) {
                                            vm.activarControles('inicio');
                                            mensajerror();
                                            return false;
                                        }
                                        else if (item["SOrd"] !== '0' && item["SOrd"] !== 0) {
                                            utilidades.mensajeError("El valor SOrd  no es igual '0'!");
                                            vm.activarControles('inicio');
                                            return false;
                                        }

                                        if (item["Fuente"] == undefined) {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false;
                                        }
                                        else if (item["Fuente"] !== '1' && item["Fuente"] !== 1
                                            && item["Fuente"] !== '2' && item["Fuente"] !== 2) {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false;
                                        }

                                        if (item["Rec"] == undefined) {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false;
                                        }
                                        else if (!ValidaSiEsNumero(item["Rec"])) {
                                            mensajerror();
                                            return false;
                                        }
                                        else if (!validarLen(item["Rec"], 6, 1)) {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false
                                        }
                                        else {
                                            if (!ValidaSiEsNumero(item["Rec"])) {
                                                mensajerror();
                                                vm.activarControles('inicio');
                                                return false
                                            }

                                        }

                                        
                                        if (item["Sit"] == undefined) {
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            //utilidades.mensajeError("La columna SituacionFondos no trae valor!");
                                            return false;
                                        }
                                        let SituacionFondos = item["Sit"].trim();
                                        if ((SituacionFondos !== 'C' && SituacionFondos !== 'c') && (SituacionFondos !== 'S' && SituacionFondos !== 's')) {
                                            //utilidades.mensajeError("El valor SituacionFondos  no es igual 'C' o igual 'S'!");
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false;
                                        }



                                        if (item["Nombre_del_rubro"] === undefined || item["Nombre_del_rubro"] === '') {
                                            //utilidades.mensajeError("La columna Rubro no trae valor!");
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false
                                        }

                                        if (item["Proyecto"] == undefined) {
                                            //utilidades.mensajeError("La columna ValorProyecto no trae valor!");
                                            mensajerror();
                                            vm.activarControles('inicio');
                                            return false;
                                        }
                                        else {
                                            let ValorProyecto = item["Proyecto"].toString().trim().replaceAll(".", "");
                                            if (ValorProyecto.includes(",")) {
                                                let arrayNumero = ValorProyecto.trim().split(",");
                                                if (!validarLen(arrayNumero[0], 14, 1)) {
                                                    //utilidades.mensajeError("La columna ValorProyecto no cumple!");
                                                    mensajerror();
                                                    vm.activarControles('inicio');
                                                    return false;
                                                }
                                                if (!validarLen(arrayNumero[1], 2, 0)) {
                                                    //utilidades.mensajeError("La columna ValorProyecto no cumple!");
                                                    mensajerror();
                                                    vm.activarControles('inicio');
                                                    return false;
                                                }
                                            }
                                            else {
                                                if (!validarLen(ValorProyecto, 14, 1)) {
                                                    //utilidades.mensajeError("La columna ValorProyecto no cumple!");
                                                    mensajerror();
                                                    vm.activarControles('inicio');
                                                    return false;
                                                }
                                            }

                                        }

                                        var valoresarchivo = {
                                            Codigo: item["Codigo"],
                                            NombreUnidadEjecutora: item["NombreUnidadEjecutora"],
                                            TipoGasto: item["TipoGasto"],
                                            CodigoPrograma: item["Cta_Prog"],
                                            CodigoSubprograma: item["ObjG_Proy"],
                                            Ord_SubP_Gasto: item["Ord_SubP_Gasto"],
                                            SOrd: item["SOrd"],
                                            Fuente: item["Fuente"],
                                            TipoRecurso: item["Rec"],
                                            SituacionFondo: item["Sit"],
                                            Rubro: item["Nombre_del_rubro"],
                                            ValorProyecto: item["Proyecto"]
                                        }

                                        vm.listaSaldodArchivo.push(valoresarchivo);

                                    });
                                    vm.SaldodArchivo.DatosExcel = vm.listaSaldodArchivo;
                                    if (resultado.indexOf(false) == -1) {

                                        //ValidarRegistros(vm.listaSaldodArchivo);
                                        utilidades.mensajeSuccess("Proceda a validar el diligenciamiento de los datos", false, false, false, "Las condiciones del archivo han sido comprobadas con exito.");
                                        vm.activarControles('cargaarchivo');
                                    }
                                    else {
                                        utilidades.mensajeError("Verifique que el archivo cumple con las condiciones de tipo archivo 'XLSX', peso máximo 5 (MGB), estructura de filas, columnas y celdas de acuerdo a la plantilla de carga  masiva y el total diligenciamiento.", false, "Las condiciones del archivo presentan iconsistencias.");
                                        vm.activarControles('limpiarArchivo');
                                        vm.listaSaldodArchivo = [];
                                    }
                                }
                                reader.readAsBinaryString(file);
                            }
                        }
                    }
                }
            }
            else {
                //vm.filename = input.files.length + " archivos"               
                vm.activarControles('inicio');
            }


        }

        $scope.validaarchivoCargaMasivanombrearchivo = function (nombre) {
            var regex = /^([\ \(a-zA-Z0-9\s_\\.\-:\ \)])+(.xls|.xlsx)$/;
            if (!regex.test(nombre.toLowerCase())) {
                //utilidades.mensajeError("El archivo no es de tipo Excel!");
                $scope.files = [];
                $scope.nombreArchivo = '';
                return false;
            }
            else {
                return true;
            }
        }
    }

    angular.module('backbone').component('saldos', {
        templateUrl: 'src/app/programacion/componentes/cargaMasiva/saldos.html',
        controller: saldosController,
        controllerAs: "vm",
    });
})()