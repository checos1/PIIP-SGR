
(function () {
    'use strict';

    regionalizacionFuentesSgpController.$inject = [
        '$sessionStorage',
        'gestionRecursosSGPServicio',
        'justificacionCambiosServicio',
        '$scope',
        'utilidades',
        'constantesBackbone',
        '$timeout'
    ];

    function regionalizacionFuentesSgpController(
        $sessionStorage,
        gestionRecursosSGPServicio,
        justificacionCambiosServicio,
        $scope,
        utilidades,
        constantesBackbone,
        $timeout
    ) {
        var vm = this;
        vm.lang = "es";
        vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.nombreComponente = "sgprecursosregionalizacionsgp";

        vm.ProyectoId = 0;
        vm.listaProductos = {};
        vm.listaProductosExcel = {};
        vm.BPIN = $sessionStorage.idObjetoNegocio;

        vm.datos = [];
        vm.listaProductosTemp = [];
        $scope.files = [];
        $scope.nombreArchivo = '';
        vm.disabled = true;
        vm.disabledProcesar = true;
        vm.mensaje = '';
        vm.permiteEditar = false;

        vm.exportExcel = exportExcel;
        vm.mostrarOcultarFlujo = mostrarOcultarFlujo;
        vm.archivoSeleccionado = archivoSeleccionado;
        vm.limpiarArchivo = limpiarArchivo;
        vm.validarArchivo = validarArchivo;
        vm.ConvertirNumero = ConvertirNumero;
        vm.ConvertirNumero4 = ConvertirNumero4;
        vm.fuenteConsultada = 0;
        vm.productoConsultado = 0;
        vm.localizacionConsultada = 0;
        vm.regionalizaAutomatico = false;
        vm.soloLectura = false;

        vm.componentesRefresh = [
            "sgprecursosfuentesfinanciacionsgp"
        ];

        vm.init = function () {
            vm.permiteEditar = false;
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.obtenerRegionalizacionFuentes(vm.BPIN);
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            ObtenerSeccionCapitulo();
        };

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-sgprecursosregionalizacionsgp');
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
            }

            gestionRecursosSGPServicio.guardarCambiosFirmeSGP(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                        //vm.callback();
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.init();
            }
        }

        vm.obtenerRegionalizacionFuentes = function (Bpin) {
            return gestionRecursosSGPServicio.obtenerDesagregarRegionalizacionSGP($sessionStorage.idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistas = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                        vm.ProyectoId = arreglolistas.ProyectoId;

                        var listaProductos = [];

                        const cantidadProductos = arreglolistas.Productos.length;

                        arreglolistas.Productos.forEach(producto => {

                            var listaFuente = [];
                            var regionaliza = false;

                            if (producto.Fuentes.length == 1) {
                                regionaliza = true;
                            }

                            const cantidadFuentes = producto.Fuentes.length;

                            producto.Fuentes.forEach(fuente => {

                                var listaLocalizacion = [];

                                const cantidadLocalizacion = fuente.Localizacion.length;

                                fuente.Localizacion.forEach(localizacion => {

                                    var listaMGA = [];
                                    var sumCostoMGA = 0;
                                    var sumRegionalizadoMGA = 0;
                                    var sumMetaMGA = 0;
                                    var sumValorFuente = 0;
                                    var sumTotalsolicitado = 0;

                                    localizacion.Vigencias.forEach(vigencia => {
                                        /*var totalsolicitado = regionaliza && vigencia.TotalSolicitado == 0 ? vigencia.RegionalizadoMGA : vigencia.TotalSolicitado*/
                                        vm.regionalizaAutomatico = (cantidadProductos == 1 && cantidadFuentes == 1 && cantidadLocalizacion == 1) && !Boolean(vigencia.ProductoFuenteLocalizacionProgramacionValoresId) && Boolean(fuente.FuenteId);
                                        vigencia.TotalSolicitado = vm.regionalizaAutomatico ? vigencia.ValorFuente : vigencia.TotalSolicitado;
                                        var totalsolicitado = regionaliza && vigencia.TotalSolicitado == 0 ? parseFloat(vigencia.TotalSolicitado).toFixed(2) : parseFloat(vigencia.TotalSolicitado).toFixed(2);
                                        listaMGA.push({
                                            Vigencia: vigencia.Vigencia,
                                            CostoMGA: vigencia.CostoMGA,
                                            RegionalizadoMGA: vigencia.RegionalizadoMGA,
                                            MetaMGA: vigencia.MetaMGA,
                                            ValorFuente: vigencia.ValorFuente,
                                            Totalsolicitado: totalsolicitado,
                                            Vigencia: vigencia.Vigencia,
                                            CostoMGAF: vigencia.CostoMGAF,
                                            RegionalizadoMGAF: vigencia.RegionalizadoMGAF,
                                            MetaMGAF: vigencia.MetaMGAF,
                                            ValorFuenteF: vigencia.ValorFuenteF

                                        })

                                        sumCostoMGA = sumCostoMGA + vigencia.CostoMGA;
                                        sumRegionalizadoMGA = sumRegionalizadoMGA + vigencia.RegionalizadoMGA;
                                        sumMetaMGA = sumMetaMGA + vigencia.MetaMGA;
                                        sumValorFuente = sumValorFuente + vigencia.ValorFuente;
                                        sumTotalsolicitado = sumTotalsolicitado + Number(totalsolicitado);
                                    });

                                    listaMGA.push({
                                        Vigencia: 'TOTAL',
                                        CostoMGA: sumCostoMGA,
                                        RegionalizadoMGA: sumRegionalizadoMGA > 0 ? sumRegionalizadoMGA : 0,
                                        MetaMGA: sumMetaMGA,
                                        ValorFuente: sumValorFuente,
                                        Totalsolicitado: sumTotalsolicitado > 0 ? sumTotalsolicitado : 0
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
                                    TotalFuenteF: fuente.TotalFuenteF,
                                    Localizacion: listaLocalizacion
                                });
                            });

                            listaProductos.push({
                                ProductoId: producto.ProductoId,
                                Producto: producto.Producto,
                                Fuentes: listaFuente
                            });
                        });

                        vm.listaProductos = angular.copy(listaProductos);
                        if (vm.regionalizaAutomatico && !vm.regionalizaAutomaticoDone) {
                            vm.actualizarRegionalizacionFuente(vm.listaProductos, true);
                            vm.regionalizaAutomaticoDone = true;
                        }
                        vm.listaProductosTemp = listaProductos;
                        vm.listaProductosExcel = arreglolistas.Productos;
                        vm.soloLectura = $sessionStorage.soloLectura;
                    }
                }
            );
        }

        vm.actualizarRegionalizacionFuente = function (response, isAutomatico = false) {
            var desagregarRegionalizacionDto = {};
            vm.mensaje = '';

            desagregarRegionalizacionDto = {
                ProyectoId: vm.ProyectoId,
                GuiMacroproceso: vm.guiMacroproceso,
                InstanciaId: vm.idInstancia,
                Productos: vm.listaProductos
            }

            gestionRecursosSGPServicio.actualizarDesagregarRegionalizacionSGP(desagregarRegionalizacionDto).then(function (response) {
                if (response.data && (response.statusText === "OK" || response.status === 200)) {
                    if (!isAutomatico) utilidades.mensajeSuccess('Guardado exitoso');
                    vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    guardarCapituloModificado();
                    vm.init();
                } else {
                    swal('', "Error al realizar la operación", 'error');
                }
            });

            $("#Editar").html("EDITAR");
            vm.disabled = true;
        }

        vm.ActivarEditar = function () {
            if (vm.disabled == true) {
                vm.permiteEditar = true;
                $("#Editar").html("CANCELAR");
                vm.disabled = false;
            }
            else {
                vm.listaProductos = [];
                vm.listaProductos = angular.copy(vm.listaProductosTemp);
                vm.permiteEditar = false;
                $("#Editar").html("EDITAR");
                vm.disabled = true;
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

                value = procesarNumero(value);


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
                            valSolicitado = vigencia.TotalSolicitado === '' ? 0 : parseFloat(procesarNumero(vigencia.Totalsolicitado));
                            vigencia.TotalSolicitado = valSolicitado;
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

        function limpiaNumero1(valor) {
            if (valor == "0.00" || valor == "0") return 0;
            if (`${valor.toLocaleString().split(",")[1]}` == 'undefined') return `${valor.toLocaleString().split(",")[0].toString()}`;
            return `${valor.toLocaleString().split(",")[0].toString().replaceAll(".", "")}.${valor.toLocaleString().split(",")[1].toString()}`;
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

            event.target.value = procesarNumero(event.target.value, false);

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 15;
            var tamanio = event.target.value.length;
            var decimal = false;
            decimal = event.target.value.toString().includes(".");
            if (decimal) {
                tamanioPermitido = 18;

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
                if (tamanio > tamanioPermitido && event.keyCode != 44 && event.keyCode != 188) {
                    event.target.value = event.target.value.slice(0, tamanioPermitido);
                    event.preventDefault();
                }
            }
        }

        function procesarNumero(value, convertFloat = true) {
            if (!Number(value)) {
                value = limpiaNumero1(value);

            } else if (!convertFloat) {
                value = value.replace(",", ".");
            } else {
                value = parseFloat(value.replace(",", "."));
            }

            return value;
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            //if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
            //    event.preventDefault();
            //}

            //var newValue = event.target.value;
            //var spiltArray = String(newValue).split("");
            //var tamanioPermitido = 11;
            //var tamanio = event.target.value.length;
            //var permitido = false;
            //permitido = event.target.value.toString().includes(".");
            //if (permitido) {
            //    tamanioPermitido = 16;

            //    var n = String(newValue).split(".");
            //    if (n[1]) {
            //        var n2 = n[1].slice(0, 2);
            //        newValue = [n[0], n2].join(".");
            //        if (spiltArray.length === 0) return;
            //        if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
            //        if (spiltArray.length === 2 && newValue === '-.') return;

            //        if (n[1].length > 4) {
            //            tamanioPermitido = n[0].length + 2;
            //            event.target.value = n[0] + '.' + n[1].slice(0, 2);
            //            return;
            //        }

            //        if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
            //            event.preventDefault();
            //        }
            //    }
            //} else {
            //    if (tamanio > 12 && event.keyCode != 44) {
            //        event.preventDefault();
            //    }
            //}

            //if (event.keyCode === 44 && tamanio == 12) {
            //}
            //else {
            //    if (tamanio > tamanioPermitido || tamanio > 16) {
            //        event.preventDefault();
            //    }
            //}
        }

        function ConvertirNumero(numero) {
            if (!numero) {
                numero = 0;
            }
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        function ConvertirNumero4(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 4,
            }).format(numero);
        }

        function mostrarOcultarFlujo(productoId, fuenteId, localizacionId) {

            var campo = productoId + '' + fuenteId + '' + localizacionId;
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

            if (vm.fuenteConsultada != 0 && vm.productoConsultado != 0 && vm.localizacionConsultada != 0) {
                if (vm.fuenteConsultada != fuenteId && vm.productoConsultado != productoId && vm.localizacionConsultada != localizacionId) {
                    var campo = vm.productoConsultado + '' + vm.fuenteConsultada + '' + vm.localizacionConsultada;
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
            }

            vm.fuenteConsultada = fuenteId;
            vm.productoConsultado = productoId;
            vm.localizacionConsultada = localizacionId;

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
                    name: 'TipoAgrupacion', title: 'Tipo agrupación'
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
                    name: 'ValorFuente', title: 'ValorFuente'
                },
                {
                    name: 'Totalsolicitado', title: 'Total solicitado'
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
                                    //Sector: fuente.Sector,
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
                                    //MetaMGA: vigencia.MetaMGA,
                                    ValorFuente: vigencia.ValorFuente,
                                    Totalsolicitado: regionaliza && vigencia.Totalsolicitado == 0 ? vigencia.RegionalizadoMGA : vigencia.Totalsolicitado
                                });
                            }
                        });
                    });
                });
            });

            const worksheet = XLSX.utils.json_to_sheet(data, {
                header: ["ProyectoId", "ProductoId", "Producto", "FuenteId", "Etapa", "TipoFinanciador", "Financiador", "Recurso", "TotalFuente",
                    "LocalizacionId", "Departamento", "Municipio", "TipoAgrupacion", "Agrupacion", "Vigencia", "CostoMGA", "RegionalizadoMGA", "ValorFuente", "Totalsolicitado"]
            });

            for (let col of [8, 15, 16, 17, 18]) { //Formatear columnas CostoMGA, RegionalizadoMGA,  ValorFuente y Totalsolicitado formato numero dos decimales
                formatColumn(worksheet, col, '#,##0.00')
            }

            //for (let col of [18]) { //Formatear columnas MetaMGA formato numero cuatro decimales
            //    formatColumn(worksheet, col, '#,##0.0000')
            //}

            /* hide second column */
            worksheet['!cols'] = [];
            worksheet['!cols'][0] = { hidden: true };
            worksheet['!cols'][1] = { hidden: true };
            worksheet['!cols'][3] = { hidden: true };
            worksheet['!cols'][9] = { hidden: true };

            wb.Sheets["Hoja Plantilla"] = worksheet;

            var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
            saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaRegionalizacion.xlsx');
        }

        function formatColumn(worksheet, col, fmtNumero) {

            //const fmtNumero2 = '#,##0.00';
            //const fmtNumero4 = '#,##0.0000';

            const range = XLSX.utils.decode_range(worksheet['!ref'])
            for (let row = range.s.r + 1; row <= range.e.r; ++row) {
                const ref = XLSX.utils.encode_cell({ r: row, c: col })

                if (ref != "K0" || ref != "L0" || ref != "N0" || ref != "O0") {
                    worksheet[ref].z = fmtNumero;
                    worksheet[ref].t = 'n';
                }
                if (ref === "S1") {
                    worksheet[ref].z = fmtNumero;
                    worksheet[ref].t = 'n';
                }
                if (ref === "T1") {
                    worksheet[ref].z = fmtNumero;
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
            if ($scope.files.length > 0) {
                $scope.files = [];
                $timeout(function () {
                    $scope.files = undefined;
                }, 100);
            }
        }

        function validarArchivo() {
            var resultado = true;
            var totalSolicitado = 0;
            var regionalzadoMGA = 0;
            var valorFuente = 0;

            if ($scope.files.length > 0) {
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
                                    utilidades.mensajeSuccess('', false, false, false, 'El archivo ha sido validado correctamente.');
                                    vm.disabledProcesar = false;
                                }
                                else {
                                    utilidades.mensajeError('Se han presentado errores en la validación.');
                                    vm.disabledProcesar = true;
                                }
                            };
                            reader.readAsBinaryString(file);
                        }
                    }
                }
            }
            else {
                utilidades.mensajeError("No se ha cargado ningún archivo. Por favor validar!");
                return false;
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
                                                vigencia.TotalSolicitado = item["Totalsolicitado"] === '' ? 0 : item["Totalsolicitado"];
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

                    gestionRecursosSGPServicio.actualizarDesagregarRegionalizacionSGP(desagregarRegionalizacionDto).then(function (response) {
                        if (response.data && (response.statusText === "OK" || response.status === 200)) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            //vm.limpiarArchivo();
                            $scope.files = [];
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
            /*console.log("Validación  - Regionalización Fuentes");*/
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
                                vm.validarValores(nameArr[1].toString(), nameArr.length == 3 ? nameArr[2].toString() : "", p.Descripcion, p.Data);
                            } else if (TipoError == 'GRVF') {
                                vm.validarValores("F" + nameArr[1].toString(), nameArr[2].toString(), p.Descripcion, null);
                            } else if (TipoError == 'GRFVR' || TipoError == 'GRFVP' || TipoError == 'GRLVR') {
                                vm.validarValoresRegionalizacion(TipoError, nameArr[1].toString(), nameArr[2].toString(), nameArr[3].toString(), nameArr[4].toString(), p.Descripcion);
                            } else if (TipoError == 'ErrorProducto') {
                                vm.ErrorProducto(nameArr[1].toString(), nameArr[2].toString(), p.Descripcion);
                            } else if (TipoError === 'SGPERRCAP') {
                                vm.validarCapitulo(TipoError, nameArr[1].toString(), p.Descripcion, false);
                            }
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        };

        vm.validarValores = function (producto, fuente, errores, data) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + producto + fuente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarCapitulo = function (tipoError, capitulo, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError + capitulo);
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
            if (vm.listaProductos != null) {
                if (vm.listaProductos.Fuentes != undefined) {
                    vm.listaProductos.Fuentes.forEach(f => {
                        f.Productos.forEach(p => {
                            var ErrorProducto = document.getElementById("ErrorProducto-" + f.FuenteId);
                            var ErrorProductomsn = document.getElementById("ErrorProductoErrorMsg-" + f.FuenteId);
                            if (ErrorProducto != undefined) {
                                if (ErrorProductomsn != undefined) {
                                    ErrorProductomsn.innerHTML = '';
                                    ErrorProducto.classList.add('hidden');
                                }
                            }
                        });
                    });
                }

                var campomensajeerror = document.getElementById('SGPERRCAP' + vm.nombreComponente);
                if (campomensajeerror != undefined) {

                    campomensajeerror.innerHTML = "";
                    campomensajeerror.classList.add('hidden');
                    campomensajeerror.classList.remove("ico-advertencia");
                }
            }
        }

        vm.ErrorProducto = function (FuenteId, ProductoId, descripcion) {
            var ErrorProducto = document.getElementById("ErrorProducto-" + FuenteId + "-" + ProductoId);
            var ErrorProductomsn = document.getElementById("ErrorProductoErrorMsg-" + FuenteId + "-" + ProductoId);

            if (ErrorProducto != undefined) {
                if (ErrorProductomsn != undefined) {
                    ErrorProductomsn.innerHTML = '<span>' + descripcion + "</span>";
                    ErrorProducto.classList.remove('hidden');
                }
            }
        }

        vm.errores = {
            'ErrorProducto': vm.ErrorProducto,
            '': vm.limpiarErrores()
        }

        vm.validarValoresRegionalizacion = function (TipoError, producto, fuente, localizacion, vigencia, errores) {
            var fila = producto + fuente + localizacion + vigencia;

            var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente + producto + fuente + localizacion);
            idSpanAlertComponent.classList.add("ico-advertencia");

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
    }

    angular.module('backbone').component('regionalizacionFuentesSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/recursos/regionalizacionFuentes/regionalizacionFuentesSgp.html",
        controller: regionalizacionFuentesSgpController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            callBack: '&',
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