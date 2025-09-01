(function () {
    'use strict';

    regionalizacionSgrSgrController.$inject = [
        '$sessionStorage',
        'previosSgrServicio',
        'justificacionCambiosServicio',
        '$scope',
        'utilidades',
        '$timeout'
    ];

    function regionalizacionSgrSgrController(
        $sessionStorage,
        previosSgrServicio,
        justificacionCambiosServicio,
        $scope,
        utilidades,
        $timeout
    ) {
        var vm = this;
        vm.lang = "es";
        vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.nombreComponente = "sgrviabilidadpreviosregionalizacionregionalizacion";

        vm.ProyectoId = 0;
        vm.TieneDeMasUnaFuente = true;
        vm.listaProductos = [];
        vm.listaProductosExcel = [];
        vm.BPIN = $sessionStorage.idObjetoNegocio;

        vm.datos = [];
        $scope.files = [];
        $scope.nombreArchivo = '';
        vm.disabled = true;
        vm.disabledProcesar = true;
        vm.activar = true;

        vm.mensaje = '';
        vm.permiteEditar = false;

        vm.exportExcel = exportExcel;
        vm.mostrarOcultarFlujo = mostrarOcultarFlujo;
        vm.archivoSeleccionado = archivoSeleccionado;
        vm.limpiarArchivo = limpiarArchivo;
        vm.validarArchivo = validarArchivo;
        vm.ConvertirNumero = ConvertirNumero;
        vm.ConvertirNumero4 = ConvertirNumero4;
        vm.erroresComponente = [];


        vm.componentesRefresh = [
            "sgrviabilidadpreviosrecursosfuentessgr",
            "sgrviabilidadpreviosrecursosfuentesnosgr"
        ];

        vm.init = function () {

            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacioninicio({ handlerInicio: vm.notificacionInicioPadre, nombreComponente: vm.nombreComponente, esValido: true });


        };
        vm.notificacionInicioPadre = function (nombreModificado, errores) {
            if (nombreModificado == vm.nombreComponente) {

                if (errores != null && errores != undefined) {
                    vm.erroresComponente = errores;
                }

                vm.permiteEditar = false;
                vm.disabled = $sessionStorage.soloLectura;
                vm.obtenerRegionalizacionFuentes(vm.idInstancia);
                ObtenerSeccionCapitulo();

                vm.flujoaprobacion = utilidades.obtenerParametroTransversal('FlujoAprobacion1');

                if (vm.flujoaprobacion === $sessionStorage.InstanciaSeleccionada.FlujoId.toUpperCase()) {
                    bloquearControles();
                }

                if ($scope.$parent !== undefined && $scope.$parent.$parent !== undefined && $scope.$parent.$parent.vm !== undefined && $scope.$parent.$parent.vm.HabilitarGuardarPaso !== undefined && !$scope.$parent.$parent.vm.HabilitarGuardarPaso) {
                    bloquearControles();
                }
            }
        }
        $scope.$on("BloquearPorCTUSPendiente", function (evt, data) {
            bloquearControles();
        });

        function bloquearControles() {
            vm.disabled = true;
            vm.activar = true;
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-sgrviabilidadpreviosregionalizacionregionalizacion');
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
            }

            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia
            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.obtenerRegionalizacionFuentes(vm.idInstancia);
                eliminarCapitulosModificados();
            }
        }

        vm.guardado = function (nombreComponenteHijo) {
            vm.guardadoevent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo });
        }

        vm.obtenerRegionalizacionFuentes = function (Bpin) {
            var idInstancia = $sessionStorage.idInstancia;
            return previosSgrServicio.obtenerDesagregarRegionalizacion(idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistas = respuesta.data;
                        vm.ProyectoId = arreglolistas.ProyectoId;
                        vm.TieneDeMasUnaFuente = arreglolistas.TieneDeMasUnaFuente;

                        var listaProductos = [];
                        arreglolistas.Productos.forEach(producto => {

                            var listaFuente = [];
                            var regionaliza = false;

                            if (producto.Fuentes.length == 1) {
                                regionaliza = true;
                            }
                            producto.Fuentes.forEach(fuente => {
                                var listaLocalizacion = [];
                                fuente.Localizacion.forEach(localizacion => {
                                    var listaMGA = [];
                                    var sumCostoMGA = 0;
                                    var sumRegionalizadoMGA = 0;
                                    var sumMetaMGA = 0;
                                    var sumValorFuente = 0;
                                    var sumTotalsolicitado = 0;

                                    localizacion.Vigencias.forEach(vigencia => {
                                        var totalsolicitado = regionaliza && vigencia.Totalsolicitado == 0 ? vigencia.RegionalizadoMGA : vigencia.Totalsolicitado
                                        listaMGA.push({
                                            Vigencia: vigencia.Vigencia,
                                            CostoMGA: vigencia.CostoMGA,
                                            RegionalizadoMGA: vigencia.RegionalizadoMGA,
                                            MetaMGA: vigencia.MetaMGA,
                                            ValorFuente: vigencia.ValorFuente,
                                            Totalsolicitado: totalsolicitado

                                        })

                                        sumCostoMGA = sumCostoMGA + vigencia.CostoMGA;
                                        sumRegionalizadoMGA = sumRegionalizadoMGA + vigencia.RegionalizadoMGA;
                                        sumMetaMGA = sumMetaMGA + vigencia.MetaMGA;
                                        sumValorFuente = sumValorFuente + vigencia.ValorFuente;
                                        sumTotalsolicitado = sumTotalsolicitado + totalsolicitado;
                                    });

                                    listaMGA.push({
                                        Vigencia: 'TOTAL',
                                        CostoMGA: sumCostoMGA,
                                        RegionalizadoMGA: sumRegionalizadoMGA,
                                        MetaMGA: sumMetaMGA,
                                        ValorFuente: sumValorFuente,
                                        Totalsolicitado: sumTotalsolicitado
                                    })

                                    listaLocalizacion.push({
                                        LocalizacionId: localizacion.LocalizacionId,
                                        Region: localizacion.Region,
                                        Departamento: localizacion.Departamento,
                                        Municipio: localizacion.Municipio,
                                        TipoAgrupacion: localizacion.TipoAgrupacion,
                                        Agrupacion: localizacion.Agrupacion,
                                        Vigencias: listaMGA
                                    });
                                });

                                listaFuente.push({
                                    FuenteId: fuente.FuenteId,
                                    Etapa: fuente.Etapa,
                                    TipoFinanciador: fuente.TipoFinanciador,
                                    Financiador: fuente.Financiador,
                                    Recurso: fuente.Recurso,
                                    TotalFuente: fuente.TotalFuente,
                                    Localizacion: listaLocalizacion
                                });
                            });

                            listaProductos.push({
                                ProductoId: producto.ProductoId,
                                Producto: producto.Producto,
                                Fuentes: listaFuente
                            });
                        });

                        vm.listaProductos = listaProductos;
                        vm.listaProductosExcel = arreglolistas.Productos;


                        if (!vm.TieneDeMasUnaFuente) {
                            guardarCapituloModificado();
                        }

                        if (vm.erroresComponente != null && vm.erroresComponente != undefined) {
                            $timeout(function () {
                                vm.notificacionValidacionPadre(vm.erroresComponente);
                            }, 600);
                        }
                    }
                }
            );
        }

        vm.actualizarRegionalizacionFuente = function (response) {
            var desagregarRegionalizacionDto = {};
            vm.mensaje = '';

            desagregarRegionalizacionDto = {
                ProyectoId: vm.ProyectoId,
                GuiMacroproceso: vm.guiMacroproceso,
                InstanciaId: vm.idInstancia,
                Productos: vm.listaProductos
            }

            response.forEach(producto => {
                producto.Fuentes.forEach(fuente => {

                    fuente.Localizacion.forEach(localizacion => {
                        localizacion.Vigencias.forEach(vigencia => {
                            vigencia.Totalsolicitado = vigencia.Totalsolicitado === 0 ? 0 : typeof vigencia.Totalsolicitado == "string" ? parseFloat(vigencia.Totalsolicitado.replace(",", ".")) : vigencia.Totalsolicitado;
                        });
                    });
                });
            });

            previosSgrServicio.actualizarDesagregarRegionalizacion(desagregarRegionalizacionDto).then(function (response) {
                if (response.data || response.statusText === "OK") {
                    utilidades.mensajeSuccess('Guardado exitoso');
                    guardarCapituloModificado();
                    vm.permiteEditar = false;
                } else {
                    swal('', "Error al realizar la operación", 'error');
                }
            });

            $("#EditarReg").html("EDITAR");
            vm.activar = true;

            vm.limpiarErrores();
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                vm.permiteEditar = true;
                $("#EditarReg").html("CANCELAR");
                vm.activar = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    vm.permiteEditar = false;
                    $("#EditarReg").html("EDITAR");
                    vm.activar = true;
                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán");
            }
        }

        //Métodos       

        vm.actualizaFila = function (event, fila, productosIndex, fuenteIndex, localizacionIndex) {
            var sum = 0;
            var valSolicitado = 0;

            $(event.target).val(function (index, value) {

                if (Number.isNaN(value)) {
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                }

                if (value == null) {
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                }

                if (value == "") {
                    return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
                }

                value = parseFloat(value.replace(",", "."));

                vm.listaProductos[productosIndex].Fuentes[fuenteIndex].Localizacion[localizacionIndex].Vigencias.forEach(vigencia => {
                    if (vigencia.Vigencia !== 'TOTAL') {

                        var fila2 = fila + vigencia.Vigencia;
                        $("#input" + fila2).css("border-color", "");
                        $("#img" + fila2).hide();

                        if (vigencia.Totalsolicitado == undefined) {
                            $("#input" + fila2).css("border-color", "red");
                            $("#img" + fila2).removeAttr("style");
                        }
                        else {
                            valSolicitado = vigencia.Totalsolicitado === '' ? 0 : parseFloat(vigencia.Totalsolicitado);
                            sum = sum + valSolicitado;
                        }
                    }
                });

                $("#label" + fila).html(ConvertirNumero(sum));

                const val = value.toString();
                const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
                var total = value = decimalCnt && decimalCnt > 2 ? value : parseFloat(val).toFixed(2);
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
            });
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
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

            event.target.value = event.target.value.replace(",", ".");

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 16;
            var tamanio = event.target.value.length;
            var decimal = false;
            decimal = event.target.value.toString().includes(".");
            if (decimal) {
                tamanioPermitido = 19;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    event.target.value = newValue;
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
                else {
                    var n2 = "";
                    newValue = [n[0], n2].join(".");
                    event.target.value = newValue;
                }
            }
            else {
                if (tamanio > tamanioPermitido && event.keyCode != 44) {
                    event.target.value = event.target.value.slice(0, tamanioPermitido);
                    event.preventDefault();
                }
            }
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        function ConvertirNumero4(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 4,
            }).format(numero);
        }

        function mostrarOcultarFlujo(campo) {
            var variable = $("#ico" + campo)[0].innerText;
            var imgmas = document.getElementById("imgmas" + campo);
            var imgmenos = document.getElementById("imgmenos" + campo);
            if (variable === "+") {
                $("#ico" + campo).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';
            }
            else {
                $("#ico" + campo).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }

        function formatearNumero(n, sep, decimals) {
            sep = sep || ","; // Default to period as decimal separator
            decimals = decimals || 2; // Default to 2 decimals

            var unidad = n.toLocaleString().split(sep)[0];
            var decimal = n.toFixed == undefined || n.toFixed(decimals).split(".")[1] == undefined ? 0 : n.toFixed(decimals).split(".")[1];

            return unidad
                + sep
                + decimal;
        }

        function exportExcel() {
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
                    name: 'ProyectoId', title: 'Proyecto Id'
                },
                {
                    name: 'ProductoId', title: 'Producto Id'
                },
                {
                    name: 'Producto', title: 'Producto'
                },
                {
                    name: 'FuenteId', title: 'Fuente Id'
                },
                {
                    name: 'Etapa', title: 'Etapa'
                },
                {
                    name: 'TipoFinanciador', title: 'Tipo Financiador'
                },
                {
                    name: 'Sector', title: 'Sector'
                },
                {
                    name: 'Financiador', title: 'Financiador'
                },
                {
                    name: 'Recurso', title: 'Recurso'
                },
                {
                    name: 'TotalFuente', title: 'Total Fuente'
                },
                {
                    name: 'LocalizacionId ', title: 'Localizacion Id'
                },
                {
                    name: 'Departamento', title: 'Departamento'
                },
                {
                    name: 'Municipio', title: 'Municipio'
                },
                {
                    name: 'TipoAgrupacion', title: 'Tipo Agrupación'
                },
                {
                    name: 'Agrupacion ', title: 'Agrupación'
                },
                {
                    name: 'Vigencia', title: 'Vigencia'
                },
                {
                    name: 'CostoMGA', title: 'CostoMGA'
                },
                {
                    name: 'RegionalizadoMGA', title: 'RegionalizadoMGA'
                },
                {
                    name: 'MetaMGA', title: 'MetaMGA'
                },
                {
                    name: 'ValorFuente', title: 'ValorFuente'
                },
                {
                    name: 'Totalsolicitado', title: 'Total Solicitado'
                }
            ];

            let colNames = columns.map(function (item) {
                return item.title;
            })

            var wb = XLSX.utils.book_new();

            wb.Props = {
                Title: "Plantilla Regionalizacion",
                Subject: "PIIP",
                Author: "PIIP",
                CreatedDate: new Date().getDate()
            };

            wb.SheetNames.push("Hoja Plantilla");

            const header = colNames;
            const data = [];

            vm.listaProductos.forEach(producto => {
                var regionaliza = false;
                if (producto.Fuentes.length == 1) {
                    regionaliza = true;
                }
                producto.Fuentes.forEach(fuente => {
                    fuente.Localizacion.forEach(localizacion => {
                        localizacion.Vigencias.forEach(vigencia => {
                            if (vigencia.Vigencia !== 'TOTAL') {
                                data.push({
                                    ProyectoId: vm.ProyectoId,
                                    ProductoId: producto.ProductoId,
                                    Producto: producto.Producto,
                                    FuenteId: fuente.FuenteId,
                                    Etapa: fuente.Etapa,
                                    TipoFinanciador: fuente.TipoFinanciador,
                                    Sector: fuente.Sector,
                                    Financiador: fuente.Financiador,
                                    Recurso: fuente.Recurso,
                                    TotalFuente: fuente.TotalFuente,
                                    LocalizacionId: localizacion.LocalizacionId,
                                    Departamento: localizacion.Departamento,
                                    Municipio: localizacion.Municipio,
                                    TipoAgrupacion: localizacion.TipoAgrupacion,
                                    Agrupacion: localizacion.Agrupacion,
                                    Vigencia: vigencia.Vigencia,
                                    CostoMGA: vigencia.CostoMGA,
                                    RegionalizadoMGA: vigencia.RegionalizadoMGA,
                                    MetaMGA: vigencia.MetaMGA,
                                    ValorFuente: vigencia.ValorFuente,
                                    Totalsolicitado: regionaliza && vigencia.Totalsolicitado == 0 ? vigencia.RegionalizadoMGA : vigencia.Totalsolicitado
                                });
                            }
                        });
                    });
                });
            });

            const worksheet = XLSX.utils.json_to_sheet(data, {
                header: ["ProyectoId", "ProductoId", "Producto", "FuenteId", "Etapa", "TipoFinanciador", "Sector", "Financiador", "Recurso", "TotalFuente",
                    "LocalizacionId", "Departamento", "Municipio", "TipoAgrupacion", "Agrupacion", "Vigencia", "CostoMGA", "RegionalizadoMGA", "MetaMGA", "ValorFuente", "Totalsolicitado"]
            });
            for (let col of [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17]) {
                formatColumn(worksheet, col, "#.###")
            }

            /* hide second column */
            worksheet['!cols'] = [];
            worksheet['!cols'][0] = { hidden: true };
            worksheet['!cols'][1] = { hidden: true };
            worksheet['!cols'][3] = { hidden: true };
            worksheet['!cols'][10] = { hidden: true };

            wb.Sheets["Hoja Plantilla"] = worksheet;

            var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
            saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaRegionalizacion.xlsx');
        }

        function formatColumn(worksheet, col) {
            var fmtnumero = "#.###";
            var fmtfecha = "dd/MM/yyyy";
            const range = XLSX.utils.decode_range(worksheet['!ref'])
            for (let row = range.s.r + 1; row <= range.e.r; ++row) {
                const ref = XLSX.utils.encode_cell({ r: row, c: col })

                if (worksheet[ref] && worksheet[ref].t === 'n') {
                    if (ref === "J2" || ref === "Q2" || ref === "R2" || ref === "S2" || ref === "T2" || ref === "U2") {
                        worksheet[ref].z = fmtnumero;
                        worksheet[ref].t = 'n';
                    }

                    //else if (ref === "C2")
                    //    worksheet[ref].z = fmtfecha
                }
            }
        }

        function s2ab(s) {
            var buf = new ArrayBuffer(s.length); //convert s to arrayBuffer
            var view = new Uint8Array(buf);  //create uint8array as viewer
            for (var i = 0; i < s.length; i++) view[i] = s.charCodeAt(i) & 0xFF; //convert to octet
            return buf;
        }

        function archivoSeleccionado() {
            let file = $scope.files[0].arhivo; //evt.target.files[0]; //$scope.files; //
            if ($scope.validaNombreArchivo(file.name)) {
                if (typeof (FileReader) != "undefined") {
                    var reader = new FileReader();
                    //For Browsers other than IE.
                    if (reader.readAsBinaryString) {
                        reader.onload = function (e) {
                            $scope.ProcessExcel(e.target.result);
                        };
                        reader.readAsBinaryString(file);
                    } else {
                        //For IE Browser.
                        reader.onload = function (e) {
                            var data = "";
                            var bytes = new Uint8Array(e.target.result);
                            for (var i = 0; i < bytes.byteLength; i++) {
                                data += String.fromCharCode(bytes[i]);
                            }
                            $scope.ProcessExcel(data);
                        };
                        reader.readAsArrayBuffer(file);
                    }
                } else {
                    $window.alert(".");
                }
            }
        }

        function limpiarArchivo() {
            $scope.files = [];
            document.getElementById("control").value = "";
            $timeout(function () {
                $scope.data.file = undefined;
            }, 100);
        }

        function validarArchivo() {
            var resultado = true;
            var totalSolicitado = 0;
            var regionalzadoMGA = 0;
            var valorFuente = 0;

            let file = $scope.files[0].arhivo;
            if ($scope.validaNombreArchivo(file.name)) {
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

                                if (item["ProyectoId"] == undefined) {
                                    utilidades.mensajeError("La columna ProyectoId no trae valor!");
                                    return false;
                                }
                                else if (!ValidaSiEsNumero(item["ProyectoId"])) {
                                    utilidades.mensajeError("El valor ProyectoId " + item["ProyectoId"] + " no es númerico!");
                                    return false;
                                }

                                if (item["ProductoId"] == undefined) {
                                    utilidades.mensajeError("La columna ProductoId no trae valor!");
                                    return false;
                                }
                                else if (!ValidaSiEsNumero(item["ProductoId"])) {
                                    utilidades.mensajeError("El valor ProductoId " + item["ProductoId"] + " no es númerico!");
                                    return false;
                                }

                                if (item["FuenteId"] == undefined) {
                                    utilidades.mensajeError("La columna FuenteId no trae valor!");
                                    return false;
                                }
                                else if (!ValidaSiEsNumero(item["FuenteId"])) {
                                    utilidades.mensajeError("El valor FuenteId " + item["FuenteId"] + " no es númerico!");
                                    return false;
                                }

                                if (item["LocalizacionId"] == undefined) {
                                    utilidades.mensajeError("La columna LocalizacionId no trae valor!");
                                    return false;
                                }
                                else if (!ValidaSiEsNumero(item["LocalizacionId"])) {
                                    utilidades.mensajeError("El valor LocalizacionId " + item["LocalizacionId"] + " no es númerico!");
                                    return false;
                                }

                                if (item["Vigencia"] == undefined) {
                                    utilidades.mensajeError("La columna Vigencia no trae valor!");
                                    return false;
                                }
                                else if (!ValidaSiEsNumero(item["Vigencia"])) {
                                    utilidades.mensajeError("El valor de la Vigencia " + item["Vigencia"] + " no es númerico!");
                                    return false;
                                }

                                if (item["RegionalizadoMGA"] == undefined) {
                                    utilidades.mensajeError("La columna RegionalizadoMGA no trae valor!");
                                    return false;
                                }
                                else if (ValidaSiEsNumero(item["RegionalizadoMGA"])) {
                                    regionalzadoMGA = parseInt(limpiaNumero(item["RegionalizadoMGA"]));
                                }
                                else {
                                    utilidades.mensajeError("El valor Regionalizado MGA " + item["RegionalizadoMGA"] + " no es númerico!");
                                    return false;
                                }

                                if (item["ValorFuente"] == undefined) {
                                    utilidades.mensajeError("La columna ValorFuente no trae valor!");
                                    return false;
                                }
                                else if (ValidaSiEsNumero(item["ValorFuente"])) {
                                    valorFuente = parseInt(limpiaNumero(item["ValorFuente"]));
                                }
                                else {
                                    utilidades.mensajeError("El valor programado para la fuente " + item["ValorFuente"] + " no es númerico!");
                                    return false;
                                }

                                if (item["Totalsolicitado"] == undefined) {
                                    utilidades.mensajeError("La columna Totalsolicitado no trae valor!");
                                    return false;
                                }
                                else if (ValidaSiEsNumero(item["Totalsolicitado"])) {
                                    totalSolicitado = parseInt(limpiaNumero(item["Totalsolicitado"]));
                                }
                                else {
                                    utilidades.mensajeError("El valor Total Solicitado   " + item["Totalsolicitado"] + " no es númerico!");
                                    return false;
                                }
                            });

                            if (resultado.indexOf(false) == -1) {
                                vm.disabledProcesar = false;
                            }
                            else {
                                vm.disabledProcesar = true;
                            }
                        };
                        reader.readAsBinaryString(file);
                    }
                }
            }
        }

        $scope.SelectFile = function (event) {

            $scope.files = [];
            var files = [];

            if (event != null) {
                event.preventDefault();
            }

            files.push(event.currentTarget.files);

            files.forEach(function (item) {
                var reader = new FileReader();
                reader.readAsDataURL(item[0]);
                if ($scope.validaNombreArchivo(item[0].name))
                    $scope.files.push({ nombreArchivo: item[0].name, size: item[0].size, arhivo: item[0] });
            });
        };

        $scope.ProcessExcel = function (data) {
            var workbook = XLSX.read(data, {
                type: 'binary'
            });
            var firstSheet = workbook.SheetNames[0];

            var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[firstSheet]);

            $scope.$apply(function () {
                try {
                    var desagregarRegionalizacionDto = {};

                    excelRows.map(function (item, index) {

                        vm.listaProductosExcel.forEach(producto => {
                            producto.Fuentes.forEach(fuente => {
                                fuente.Localizacion.forEach(localizacion => {
                                    localizacion.Vigencias.forEach(vigencia => {

                                        if (item["ProyectoId"] === vm.ProyectoId &&
                                            item["ProductoId"] === producto.ProductoId &&
                                            item["FuenteId"] === fuente.FuenteId &&
                                            item["LocalizacionId"] === localizacion.LocalizacionId &&
                                            item["Vigencia"] === vigencia.Vigencia) {

                                            if (item["Totalsolicitado"] != vigencia.Totalsolicitado) {
                                                vigencia.Totalsolicitado = item["Totalsolicitado"] === '' ? 0 : item["Totalsolicitado"];
                                            }
                                        }
                                    });
                                });
                            });
                        });

                    });

                    desagregarRegionalizacionDto = {
                        ProyectoId: vm.ProyectoId,
                        GuiMacroproceso: vm.guiMacroproceso,
                        InstanciaId: vm.idInstancia,
                        Productos: vm.listaProductosExcel
                    }

                    previosSgrServicio.actualizarDesagregarRegionalizacion(desagregarRegionalizacionDto).then(function (response) {
                        if (response.data || response.statusText === "OK") {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            vm.obtenerRegionalizacionFuentes(vm.BPIN);
                            vm.disabledProcesar = true;
                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
                    });
                }
                catch (ex) {
                    utilidades.mensajeError("Debe validar que el archivo corresponda a la plantilla!");
                }
            });
        };

        $scope.validaNombreArchivo = function (nombre) {
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


        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Regionalización");
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var isValid = true;
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            var nameArr = p.Error.split('-');
                            var TipoError = nameArr[0].toString();
                            if (TipoError == 'GRP' || TipoError == 'GRF') {
                                vm.validarValores(nameArr[1].toString(), nameArr.length == 3 ? nameArr[2].toString() : "", p.Descripcion, false);
                            } else if (TipoError == 'GRVF') {
                                vm.validarValores("F" + nameArr[1].toString(), nameArr[2].toString(), p.Descripcion, false);
                            } else if (TipoError == 'GRFVR' || TipoError == 'GRFVP' || TipoError == 'GRLVR' || TipoError == 'GRFVPR' || TipoError == 'GRRVF') {
                                vm.validarValoresRegionalizacion(TipoError, nameArr[1].toString(), nameArr[2].toString(), nameArr[3].toString(), nameArr[4].toString(), p.Descripcion);
                            } else if (TipoError == 'SGRERRSEC') {
                                vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                            }
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        };

        vm.validarValores = function (producto, fuente, errores, esValido) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + producto + fuente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }

            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarValoresRegionalizacion = function (TipoError, producto, fuente, localizacion, vigencia, errores) {
            var fila = producto + fuente + localizacion + vigencia;

            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + producto + fuente + localizacion);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.add("ico-advertencia");
            }
            
            var campoObligatorioJustificacion = document.getElementById(TipoError + producto + fuente + localizacion);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = errores;
                campoObligatorioJustificacion.classList.remove('hidden');
            }

            $("#input" + fila).css("border-color", "#A80521 !important");
            $("#input" + fila).css("border-width", "2px !important");
            $("#input" + fila).css("border-style", "dotted");
            $("#input" + fila).removeClass('editInputDNP');
            $("#input" + fila).addClass('editInputErrorDNP');
            $("#Divin" + fila).addClass('divInconsistencia');
            $("#img" + fila).removeAttr("style");

        }

        vm.validarSeccion = function (tipoError, seccion, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError + seccion);
            if (campomensajeerror != undefined) {
                if (!esValido) {
                    campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                    campomensajeerror.classList.remove('hidden');
                } else {
                    campomensajeerror.classList.remove("ico-advertencia");
                }
            }
        }

        vm.limpiarErrores = function () {
            try {
                var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
                if (idSpanAlertComponent != undefined) {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }

                var elementos = document.querySelectorAll('[id^="SGRERRSEC"]');
                if (elementos != undefined) {
                elementos.forEach(function (elemento) {
                    elemento.classList.add('hidden');
                });
                }
                if (vm.listaProductos != null && vm.listaProductos != undefined) {

                vm.listaProductos.forEach(producto => {
                    producto.Fuentes.forEach(fuente => {
                        fuente.Localizacion.forEach(localizacion => {
                            localizacion.Vigencias.forEach(vigencia => {
                                var fila = producto.ProductoId.toString() + fuente.FuenteId.toString() + localizacion.LocalizacionId.toString() + vigencia.Vigencia.toString();

                                /*Limpia validarValores */

                                var campo0 = document.getElementById(vm.nombreComponente + producto.ProductoId);
                                if (campo0 != undefined) {
                                    campo0.innerHTML = "";
                                    campo0.classList.add('hidden');
                                }

                                var campo1 = document.getElementById(vm.nombreComponente + producto.ProductoId + fuente.FuenteId);
                                if (campo1 != undefined) {
                                    campo1.innerHTML = "";
                                    campo1.classList.add('hidden');
                                }

                                var campoF = document.getElementById(vm.nombreComponente + 'F' + producto.ProductoId + fuente.FuenteId);
                                if (campoF != undefined) {
                                    campoF.innerHTML = "";
                                    campoF.classList.add('hidden');
                                }

                                var idSpanAlertComponent1 = document.getElementById("alert-" + vm.nombreComponente);
                                if (idSpanAlertComponent1 != undefined) {
                                    idSpanAlertComponent1.classList.remove("ico-advertencia");
                                }

                                /*Limpia validarValoresRegionalizacion TipoError == 'GRFVR' || TipoError == 'GRFVP' || TipoError == 'GRLVR' || TipoError == 'GRFVPR' || TipoError == 'GRRVF'*/

                                var idSpanAlertComponent2 = document.getElementById("alert-" + vm.nombreComponente + producto.ProductoId + fuente.FuenteId + localizacion.LocalizacionId);
                                idSpanAlertComponent2.classList.remove("ico-advertencia");

                                var campo2 = document.getElementById('GRFVR' + producto.ProductoId + fuente.FuenteId + localizacion.LocalizacionId);
                                if (campo2 != undefined) {
                                    campo2.innerHTML = "";
                                    campo2.classList.add('hidden');
                                }

                                var campo3 = document.getElementById('GRFVP' + producto.ProductoId + fuente.FuenteId + localizacion.LocalizacionId);
                                if (campo3 != undefined) {
                                    campo3.innerHTML = "";
                                    campo3.classList.add('hidden');
                                }

                                var campo4 = document.getElementById('GRLVR' + producto.ProductoId + fuente.FuenteId + localizacion.LocalizacionId);
                                if (campo4 != undefined) {
                                    campo4.innerHTML = "";
                                    campo4.classList.add('hidden');
                                }

                                var campo5 = document.getElementById('GRFVPR' + producto.ProductoId + fuente.FuenteId + localizacion.LocalizacionId);
                                if (campo5 != undefined) {
                                    campo5.innerHTML = "";
                                    campo5.classList.add('hidden');
                                }

                                var campo6 = document.getElementById('GRRVF' + producto.ProductoId + fuente.FuenteId + localizacion.LocalizacionId);
                                if (campo6 != undefined) {
                                    campo6.innerHTML = "";
                                    campo6.classList.add('hidden');
                                }

                                $("#input" + fila).css("border-color", "");
                                $("#input" + fila).css("border-style", "");
                                $("#input" + fila).css({ 'border-color': '', 'border-style': '' });
                                $("#input" + fila).addClass('editInputDNP');
                                $("#input" + fila).removeClass('editInputErrorDNP');
                                $("#Divin" + fila).removeClass('divInconsistencia');
                                $("#img" + fila).attr('style', 'display:none;');

                                /*Limpia validarSeccion */

                                var campomensajeerror = document.getElementById('SGRERRSEC' + producto.ProductoId);
                                if (campomensajeerror != undefined) {

                                    campomensajeerror.innerHTML = "";
                                    campomensajeerror.classList.add('hidden');
                                    campomensajeerror.classList.remove("ico-advertencia");
                                }
                            });
                        });
                    });
                });
                }
            }
            catch (ex) {
                console.error(ex);
            }
        }
    }

    angular.module('backbone').component('regionalizacionSgrSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/viabilidadSGR/previos/regionalizacion/regionalizacionSgr/regionalizacionSgrSgr.html",
        controller: regionalizacionSgrSgrController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            callBack: '&',
            notificacioninicio: '&',
            notificacioncambios: '&'
        }
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
        });

})();