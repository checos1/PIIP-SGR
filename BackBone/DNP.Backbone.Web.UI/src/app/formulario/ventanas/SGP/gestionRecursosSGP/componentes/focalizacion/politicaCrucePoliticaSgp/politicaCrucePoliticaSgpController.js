(function () {
    'use strict';
    politicaCrucePoliticaSgpController.$inject = [
        '$sessionStorage',
        'gestionRecursosSGPServicio',
        '$scope',
        '$anchorScroll',
        '$location',
        'utilidades'
    ];

    function politicaCrucePoliticaSgpController(
        $sessionStorage,
        gestionRecursosSGPServicio,
        $scope,
        $anchorScroll,
        $location,
        utilidades
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.NombreUsuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.Bpin = $sessionStorage.idObjetoNegocio;
        vm.elementoOrigen = $sessionStorage.nombreElementoOrigenPCP;
        vm.IdFuente = sessionStorage.getItem("fuenteId");
        vm.politicaId = sessionStorage.getItem("politicaId");
        vm.PoliticasTCrucePoliticas = {};
        vm.IdInstancia = $sessionStorage.idInstancia;
        vm.IdProyecto = $sessionStorage.IdProyecto;
        $scope.datos = [];
        vm.permiteEditar = false;
        vm.disabled = true;
        vm.soloLectura = false;
        vm.mensaje = '';
        $scope.files = [];

        vm.mensajeSinDatosLoc = '¡Sin datos de localizaciones!';
        vm.nombreComponente = "sgpsolicitudrecursosfocalizacionpoliticassgp";

        vm.exportarFocalizacionExcel = exportarFocalizacionExcel;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.validarArchivo = validarArchivo;
        vm.limpiarArchivo = limpiarArchivo;
        vm.GuardarArchivoCruce = GuardarArchivoCruce;

        /* metodos*/
        vm.ConvertirNumero = ConvertirNumero;
        vm.obtenerPoliticasTransversalesCrucePoliticas = obtenerPoliticasTransversalesCrucePoliticas;
        //vm.actualizarPoliticasTransversalesCrucePoliticas = actualizarPoliticasTransversalesCrucePoliticas;

        ///*definiciion DTO*/
        vm.PoliticasTCrucePoliticas = [{
            ProyectoId: 0,
            BPIN: "",
            FuenteId: 0,
            PoliticaPrincipal: [{
                PoliticaId: 0,
                Politica: "",
                Localizaciones: [{
                    LocalizacionId: 0,
                    Localizacion: "",
                    RelacionPoliticas: [{
                        OrdenId: 0,
                        TituloPoliticaPrincipal: "",
                        PoliticaDependienteId: 0,
                        PoliticaDependiente: "",
                        CrucePoliticasVigencias: [{
                            PeriodoProyectoId: 0,
                            Vigencia: 0,
                            ValorPoliticaPrincipal: 0,
                            ValorPoliticaDependiente: 0,
                            ValorCruceDependientePrincipal: 0,
                        }],
                    }],
                }],
            }],
        }];


        //Inicio
        vm.init = function () {
            vm.permiteEditar = false;
            vm.cargarCss('/src/app/formulario/ventanas/gestionRecursos/componentes/indicadoresPolitica/indicadoresPolitica.css',
                'indicadoresPoliticaCSS');
            obtenerPoliticasTransversalesCrucePoliticas();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        };

        /* ------------------------ lunas ---------------------------------*/

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-focalizaciongrcapitulo1');
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
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            vm.init();
        }

        /* -----------------------------------------------------------------*/

        function obtenerPoliticasTransversalesCrucePoliticas() {

            if (vm.IdFuente != null)
                gestionRecursosSGPServicio.obtenerPoliticasTransversalesCrucePoliticasSGP($sessionStorage.idInstancia, vm.IdFuente)
                    .then(resultado => {
                        if (resultado != undefined && resultado.data.length > 0) {
                            var datos = jQuery.parseJSON(resultado.data);
                            vm.PoliticasTCrucePoliticas = jQuery.parseJSON(resultado.data);

                            var loc = [];
                            var politicaPrincipal = datos.PoliticaPrincipal.filter(e => e.PoliticaId.toString() === vm.politicaId.toString());

                            var politica = politicaPrincipal[0].Politica;

                            var listaLocalizacion = [];
                            politicaPrincipal[0].Localizaciones.forEach(Localizaciones => {
                                var listadoRelacionPoliticas = [];
                                Localizaciones.RelacionPoliticas.forEach(RelacionPoliticas => {
                                    if (RelacionPoliticas.TituloPoliticaPrincipal == politica) {

                                        var listaCrucePoliticas = [];
                                        var sumValorPoliticaPrincipal = 0;
                                        var sumValorPoliticaDependiente = 0;
                                        var sumValorCruceDependientePrincipal = 0;

                                        RelacionPoliticas.CrucePoliticasVigencias.forEach(CrucePoliticasVigencias => {

                                            listaCrucePoliticas.push({
                                                PeriodoProyectoId: CrucePoliticasVigencias.PeriodoProyectoId,
                                                Vigencia: CrucePoliticasVigencias.Vigencia,
                                                ValorPoliticaPrincipal: CrucePoliticasVigencias.ValorPoliticaPrincipal,
                                                ValorPoliticaDependiente: CrucePoliticasVigencias.ValorPoliticaDependiente,
                                                ValorCruceDependientePrincipal: CrucePoliticasVigencias.ValorCruceDependientePrincipal
                                            });
                                            sumValorPoliticaPrincipal = sumValorPoliticaPrincipal + CrucePoliticasVigencias.ValorPoliticaPrincipal;
                                            sumValorPoliticaDependiente = sumValorPoliticaDependiente + CrucePoliticasVigencias.ValorPoliticaDependiente;
                                            sumValorCruceDependientePrincipal = sumValorCruceDependientePrincipal + CrucePoliticasVigencias.ValorCruceDependientePrincipal;

                                        });

                                        listaCrucePoliticas.push({
                                            Vigencia: 'TOTAL',
                                            ValorPoliticaPrincipal: sumValorPoliticaPrincipal,
                                            ValorPoliticaDependiente: sumValorPoliticaDependiente,
                                            ValorCruceDependientePrincipal: sumValorCruceDependientePrincipal

                                        })

                                        listadoRelacionPoliticas.push({
                                            TituloPoliticaPrincipal: RelacionPoliticas.TituloPoliticaPrincipal,
                                            PoliticaDependienteId: RelacionPoliticas.PoliticaDependienteId,
                                            PoliticaDependiente: RelacionPoliticas.PoliticaDependiente,
                                            CrucePoliticasVigencias: listaCrucePoliticas
                                        })
                                    }
                                });

                                listaLocalizacion.push({
                                    LocalizacionId: Localizaciones.LocalizacionId,
                                    Localizacion: Localizaciones.Localizacion.replace(/-*$/g, ''),
                                    RelacionPoliticas: listadoRelacionPoliticas
                                })
                                vm.soloLectura = $sessionStorage.soloLectura;
                            })

                        }

                        politicaPrincipal[0].Localizaciones = listaLocalizacion;
                        vm.PoliticasTCrucePoliticas.PoliticaPrincipal = politicaPrincipal;
                        vm.PoliticasTCrucePoliticas.FuenteId = vm.IdFuente;


                    })
        };

        vm.actualizarCrucePolitica = function (response) {
            var PoliticasTCrucePoliticasDTO = {};
            vm.mensaje = '';
            PoliticasTCrucePoliticasDTO = {
                ProyectoId: vm.ProyectoId,
                FuenteId: vm.IdFuente,
                politicaId: vm.politicaId,
                Bpin: vm.Bpin


            }
            //var acumuladoTotalValor = 0.00;
            //vm.PoliticasTCrucePoliticas.PoliticaPrincipal.forEach(politicaPrincipal => {
            //    politicaPrincipal.Localizaciones.forEach(localizaciones => {
            //        localizaciones.RelacionPoliticas.forEach(relacionPoliticas => {
            //            acumuladoTotalValor = 0.00;
            //            relacionPoliticas.CrucePoliticasVigencias.forEach(crucePoliticasVigencias => {
            //                if (crucePoliticasVigencias.Vigencia != 'TOTAL') {
            //                    acumuladoTotalValor = acumuladoTotalValor + parseFloat(crucePoliticasVigencias.ValorCruceDependientePrincipal);
            //                }
            //                if (crucePoliticasVigencias.Vigencia == 'TOTAL') {
            //                    crucePoliticasVigencias.ValorCruceDependientePrincipal = acumuladoTotalValor;
            //                    document.getElementById('label' + politicaPrincipal.PoliticaId + localizaciones.LocalizacionId + relacionPoliticas.PoliticaDependienteId).value = acumuladoTotalValor;
            //                }
            //            })
            //        })

            //    })
            //})
            gestionRecursosSGPServicio.actualizarPoliticasTransversalesCrucePoliticasSGP(vm.PoliticasTCrucePoliticas).then(function (response) {
                if (response.data && (response.statusText === "OK" || response.status === 200)) {

                    if (response.data.Exito) {
                        parent.postMessage("cerrarModal", window.location.origin);
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        guardarCapituloModificado();
                        vm.init();
                    } else {
                        swal('', response.data.Mensaje, 'warning');
                    }

                } else {
                    swal('', "Error al realizar la operación", 'error');
                }

            });

            $("#Editar").html("EDITAR");
            vm.disabled = true;
        }

        vm.scrollToCrucePolitica = function (id) {
            var old = $location.hash();
            $location.hash(id);
            $anchorScroll();
            //reset to old to keep any additional routing logic from kicking in
            $location.hash(old);
        };

        vm.ocultarPoliticaCrucePolitica = function (e) {
            var e2 = e.closest("cDivMainPolCrucPol");
            if (e2) {
                var p = e2.parentNode;
                var p2 = p.parentNode.parentNode;

                if (p) {
                    $(p).css("display", "none");
                }

                if (p2) {
                    $(p2).css("display", "none");
                }
            }

        };

        vm.retornar = function () {
            var nombreElementoOrigen = '';
            var e = event.currentTarget;
            if (e) {
                var att = e.getAttribute("data-attribute");
                if (att) {
                    nombreElementoOrigen = att;
                }
            }

            if (nombreElementoOrigen)
                vm.ocultarPoliticaCrucePolitica(nombreElementoOrigen);

            vm.ocultarPoliticaCrucePolitica(e);
        };

        /*Carga Masiva*/

        function exportarFocalizacionExcel(politicaId, localizacionId, politicaDependienteId) {

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
                        name: 'ProyectoId', title: 'Proyecto Id'
                    },

                    {
                        name: 'Bpin', title: 'Bpin'
                    },

                    {
                        name: 'PoliticaId', title: 'Politica Id'
                    },

                    {
                        name: 'Politica', title: 'Politica'
                    },

                    {
                        name: 'LocalizacionId', title: 'Localizacion Id'
                    },

                    {
                        name: 'Localizacion', title: 'Localizacion'
                    },

                    {
                        name: 'TituloPoliticaPrincipal', title: 'Titulo Politica Principal'
                    },

                    {
                        name: 'PoliticaDependienteId', title: 'Politica Dependiente Id'
                    },

                    {
                        name: 'PoliticaDependiente', title: 'Politica Dependiente'
                    },

                    {
                        name: 'PeriodoProyectoId', title: 'PeriodoProyecto Id'
                    },

                    {
                        name: 'Vigencia', title: 'Vigencia'
                    },

                    {
                        name: 'ValorPoliticaPrincipal', title: 'Valor Politica Principal'
                    },

                    {
                        name: 'ValorPoliticaDependiente', title: 'Valor Politica Dependiente'
                    },

                    {
                        name: 'ValorCruceDependientePrincipal', title: 'Valor Cruce Dependiente Principal'
                    }
                ];

                let colNames = columns.map(function (item) {
                    return item.title;
                })

                var wb = XLSX.utils.book_new();

                wb.Props = {
                    Title: "Plantilla Politicas Cruce",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };

                wb.SheetNames.push("Politicas Cruce");

                const header = colNames;
                const data = [];

                vm.PoliticasTCrucePoliticas.PoliticaPrincipal.forEach(PoliticaPrincipal => {
                    PoliticaPrincipal.Localizaciones.forEach(Localizaciones => {
                        Localizaciones.RelacionPoliticas.forEach(RelacionPoliticas => {
                            RelacionPoliticas.CrucePoliticasVigencias.forEach(CrucePoliticasVigencias => {
                                //vm.PoliticasTCrucePoliticas.PoliticaPrincipal[0].Localizaciones[0].RelacionPoliticas[0].CrucePoliticasVigencias[4].Vigencia
                                if (politicaId == PoliticaPrincipal.PoliticaId && localizacionId == Localizaciones.LocalizacionId && politicaDependienteId == RelacionPoliticas.PoliticaDependienteId && CrucePoliticasVigencias.Vigencia != "TOTAL")
                                    data.push({

                                        ProyectoId: vm.PoliticasTCrucePoliticas.ProyectoId,
                                        Bpin: vm.PoliticasTCrucePoliticas.BPIN,
                                        FuenteId: vm.PoliticasTCrucePoliticas.FuenteId,

                                        PoliticaId: PoliticaPrincipal.PoliticaId,
                                        Politica: PoliticaPrincipal.Politica,

                                        LocalizacionId: Localizaciones.LocalizacionId,
                                        Localizacion: Localizaciones.Localizacion,

                                        TituloPoliticaPrincipal: RelacionPoliticas.TituloPoliticaPrincipal,

                                        PoliticaDependiente: RelacionPoliticas.PoliticaDependiente,
                                        PoliticaDependienteId: RelacionPoliticas.PoliticaDependienteId,

                                        PeriodoProyectoId: CrucePoliticasVigencias.PeriodoProyectoId,
                                        Vigencia: CrucePoliticasVigencias.Vigencia,

                                        ValorPoliticaPrincipal: CrucePoliticasVigencias.ValorPoliticaPrincipal,
                                        ValorPoliticaDependiente: CrucePoliticasVigencias.ValorPoliticaDependiente,

                                        ValorCruceDependientePrincipal: CrucePoliticasVigencias.ValorCruceDependientePrincipal


                                    });
                            });
                        });
                    });
                });

                const worksheet = XLSX.utils.json_to_sheet(data, {
                    header: [

                        "ProyectoId",
                        "Bpin",
                        "FuenteId",

                        "PoliticaId",
                        "Politica",

                        "LocalizacionId",
                        "Localizacion",

                        "TituloPoliticaPrincipal",

                        "PoliticaDependiente",
                        "PoliticaDependienteId",

                        "PeriodoProyectoId",
                        "Vigencia",

                        "ValorPoliticaPrincipal",
                        "ValorPoliticaDependiente",

                        "ValorCruceDependientePrincipal"
                    ]
                });

                /* hide second column */
                worksheet['!cols'] = [];
                worksheet['!cols'][0] = { hidden: true };
                worksheet['!cols'][1] = { hidden: true };
                worksheet['!cols'][3] = { hidden: true };
                worksheet['!cols'][2] = { hidden: true };
                worksheet['!cols'][4] = { hidden: true };
                worksheet['!cols'][8] = { hidden: true };
                worksheet['!cols'][9] = { hidden: true };

                wb.Sheets["Politicas Cruce"] = worksheet;

                var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
                saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'PlantillaPoliticasCruce.xlsx');

            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            }, false, false, "Este archivo es compatible con Office 365");
        }

        function formatColumn(worksheet, col) {

            var fmtnumero2 = "#,##";
            const range = XLSX.utils.decode_range(worksheet['!ref'])

            for (let row = range.s.r + 1; row <= range.e.r; ++row) {

                const ref = XLSX.utils.encode_cell({ r: row, c: col })

                if (ref != "L0" || ref != "M0" || ref != "N0") {
                    worksheet[ref].z = fmtnumero2;
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

        function adjuntarArchivo(PoliticaId, IdFuente, LocalizacionId, PoliticaDependienteId) {
            document.getElementById('filefocalizacion-' + PoliticaId + '-' + IdFuente + '-' + LocalizacionId + '-' + PoliticaDependienteId).value = "";
            document.getElementById('filefocalizacion-' + PoliticaId + '-' + IdFuente + '-' + LocalizacionId + '-' + PoliticaDependienteId).click();
        }

        $scope.filefocalizacionNameChanged = function (input) {
            if (input.files.length == 1) {
                var idInput = input.id.substring(16, input.id.length);
                var nombreA = '#spNombrearchivo' + idInput;
                $(nombreA).text(input.files[0].name);
                vm.activarControles('cargaarchivo', input.id);
                vm.nombreArchivo = input.files[0].name;
            }
            else {
                //vm.filename = input.files.length + " archivos"               
                vm.activarControles('inicio', input.id);
            }
        }

        $scope.ChangeFocalizacionSet = function () {
            if (vm.nombrearchivo == "") {

            }
        };

        function validarArchivo(PoliticaId, IdFuente, LocalizacionId, PoliticaDependienteId) {
            var resultado = true;
            var totalSolicitado = 0;
            var regionalzadoMGA = 0;
            var valorFuente = 0;

            var fileId = 'filefocalizacion-' + PoliticaId + '-' + IdFuente + '-' + LocalizacionId + '-' + PoliticaDependienteId
            let file = document.getElementById(fileId).files[0];

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

                                if (item["PoliticaId"] == undefined) {
                                    utilidades.mensajeError("La columna PoliticaId no trae valor!");
                                    return false;
                                }
                                else if (!ValidaSiEsNumero(item["PoliticaId"])) {
                                    utilidades.mensajeError("El valor PoliticaId " + item["PoliticaId"] + " no es númerico!");
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

                                if (item["PoliticaDependienteId"] == undefined) {
                                    utilidades.mensajeError("La columna PoliticaDependienteId no trae valor!");
                                    return false;
                                }
                                else if (!ValidaSiEsNumero(item["PoliticaDependienteId"])) {
                                    utilidades.mensajeError("El valor PoliticaDependienteId " + item["PoliticaDependienteId"] + " no es númerico!");
                                    return false;
                                }

                                if (item["PeriodoProyectoId"] == undefined) {
                                    utilidades.mensajeError("La columna PeriodoProyectoId no trae valor!");
                                    return false;
                                }
                                else if (!ValidaSiEsNumero(item["PeriodoProyectoId"])) {
                                    utilidades.mensajeError("El valor PeriodoProyectoId " + item["PeriodoProyectoId"] + " no es númerico!");
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

                                if (item["ValorPoliticaPrincipal"] == undefined) {
                                    utilidades.mensajeError("La columna Valor Politica Principal no trae valor!");
                                    return false;
                                }
                                else if (ValidaSiEsNumero(item["ValorPoliticaPrincipal"])) {
                                    regionalzadoMGA = parseInt(limpiaNumero(item["ValorPoliticaPrincipal"]));
                                }
                                else {
                                    utilidades.mensajeError("El Valor Politica Principal " + item["ValorPoliticaPrincipal"] + " no es númerico!");
                                    return false;
                                }

                                if (item["ValorPoliticaDependiente"] == undefined) {
                                    utilidades.mensajeError("La columna Valor Politica Dependiente no trae valor!");
                                    return false;
                                }
                                else if (ValidaSiEsNumero(item["ValorPoliticaDependiente"])) {
                                    valorFuente = parseInt(limpiaNumero(item["ValorPoliticaDependiente"]));
                                }
                                else {
                                    utilidades.mensajeError("El Valor Politica Dependiente " + item["ValorPoliticaDependiente"] + " no es númerico!");
                                    return false;
                                }

                                if (item["ValorCruceDependientePrincipal"] == undefined) {
                                    utilidades.mensajeError("La columna Valor Cruce Dependiente Principal no trae valor!");
                                    return false;
                                }
                                else if (ValidaSiEsNumero(item["ValorCruceDependientePrincipal"])) {
                                    valorFuente = parseInt(limpiaNumero(item["ValorCruceDependientePrincipal"]));
                                }
                                else {
                                    utilidades.mensajeError("El valor Valor Cruce Dependiente Principal " + item["ValorCruceDependientePrincipal"] + " no es númerico!");
                                    return false;
                                }
                            });

                            if (resultado.indexOf(false) == -1) {

                                vm.activarControles('validado', fileId);
                                utilidades.mensajeSuccess("Proceda a cargar el archivo para que quede registrado en el sistema", false, false, false, "Validación de carga exitosa.");
                            }

                        };
                        reader.readAsBinaryString(file);
                    }
                }
            }
        }

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

        vm.activarControles = function (evento, id) {
            id = id.substring(16, id.length);
            switch (evento) {
                case "inicio":
                    $("#btnFocalizacionValidarArchivo" + id).attr('disabled', true);
                    $("#btnFocalizacionLimpiarArchivo" + id).attr('disabled', true);
                    $("#btnFocalizacionArchivoSeleccionado" + id).attr('disabled', true);

                    document.getElementById('filefocalizacion' + id).value = "";
                    vm.nombrearchivo = "";
                    break;
                case "cargaarchivo":
                    $("#btnFocalizacionValidarArchivo" + id).attr('disabled', false);
                    $("#btnFocalizacionLimpiarArchivo" + id).attr('disabled', false);
                    $("#btnFocalizacionArchivoSeleccionado" + id).attr('disabled', true);
                    break;
                case "validado":
                    $("#btnFocalizacionValidarArchivo" + id).attr('disabled', false);
                    $("#btnFocalizacionLimpiarArchivo" + id).attr('disabled', false);
                    $("#btnFocalizacionArchivoSeleccionado" + id).attr('disabled', false);
                    break;
                default:
            }
        }

        function limpiarArchivo(PoliticaId, IdFuente, LocalizacionId, PoliticaDependienteId) {

            var fileId = PoliticaId + '-' + IdFuente + '-' + LocalizacionId + '-' + PoliticaDependienteId
            document.getElementById('spNombrearchivo-' + fileId).textContent = "";
            document.getElementById('filefocalizacion-' + fileId).value = "";

            fileId = 'filefocalizacion-' + fileId
            vm.activarControles('inicio', fileId);
            vm.nombrearchivo = "";
        }

        function ValidarRegistros() {
            var aFuentes = [];
            var aProductos = [];
            var aLocalizaciones = [];
            var aPeriodoProyecto = [];
            var aVigencias = [];

            var existeBpin = 0;
            var existeFuente = 0;
            var existeProducto = 0;
            var existeLocaliacion = 0;
            var existeVigencia = 0;
            var existePeriodoProyecto = 0;
            var CantidadRegistros = 0;

            vm.modelo.Fuentes.forEach(fuente => {
                aFuentes.push(fuente.FuenteId);
                fuente.Productos.forEach(producto => {
                    aProductos.push(producto.ProductoId);
                    producto.Localizaciones.forEach(localizacion => {
                        aLocalizaciones.push(localizacion.LocalizacionId);
                        localizacion.Vigencias.forEach(vigencia => {
                            aPeriodoProyecto.push(vigencia.PeriodoProyectoId);
                            aVigencias.push(vigencia.Vigencia);
                            CantidadRegistros = CantidadRegistros + 1;
                        });
                    });
                });
            });

            vm.FuenteArchivo.forEach(fa => {

                if (fa.Bpin != vm.modelo.BPIN) {
                    existeBpin++;
                }

                if (aFuentes.indexOf(fa.FuenteId) == -1) {
                    existeFuente = existeFuente + 1;
                }
                if (aProductos.indexOf(fa.ProductoId) == -1) {
                    existeProducto = existeProducto + 1;
                }
                if (aLocalizaciones.indexOf(fa.LocalizacionId) == -1) {
                    existeLocaliacion = existeLocaliacion + 1;
                }
                if (aPeriodoProyecto.indexOf(fa.PeriodoProyectoId) == -1) {
                    existePeriodoProyecto = existePeriodoProyecto + 1;
                }
                if (aVigencias.indexOf(fa.Vigencia) == -1) {
                    existeVigencia = existeVigencia + 1;
                }
            });

            if (existeBpin > 0 || existeFuente > 0 || existeProducto > 0 || existeLocaliacion > 0 || existeVigencia > 0 || existePeriodoProyecto > 0) {
                return false;
            }
            else {
                if (CantidadRegistros != vm.FuenteArchivo.length) {
                    return false;
                }
                else {
                    return true;
                }
            }
        }

        function GuardarArchivoCruce(PoliticaId, IdFuente, LocalizacionId, PoliticaDependienteId) {
            var fileId = 'filefocalizacion-' + PoliticaId + '-' + IdFuente + '-' + LocalizacionId + '-' + PoliticaDependienteId
            var numeroErrores = 0;
            var numeroGuardado = 0;
            var guardado = false;

            let file = document.getElementById(fileId).files[0];

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

                            ///*definicion DTO*/
                            vm.PoliticasTCrucePoliticas = [{
                                ProyectoId: item["ProyectoId"],
                                BPIN: item["bpin"],
                                FuenteId: item["FuenteId"],
                                PoliticaPrincipal: [{
                                    PoliticaId: item["PoliticaId"],
                                    Politica: item["Politica"],
                                    Localizaciones: [{
                                        LocalizacionId: item["LocalizacionId"],
                                        Localizacion: item["Localizacion"],
                                        RelacionPoliticas: [{
                                            OrdenId: 0,
                                            TituloPoliticaPrincipal: "TituloPoliticaPrincipal",
                                            PoliticaDependienteId: item["PoliticaDependienteId"],
                                            PoliticaDependiente: "PoliticaDependiente",
                                            CrucePoliticasVigencias: [{
                                                PeriodoProyectoId: item["PeriodoProyectoId"],
                                                Vigencia: item["Vigencia"],
                                                ValorPoliticaPrincipal: item["ValorPoliticaPrincipal"],
                                                ValorPoliticaDependiente: item["ValorPoliticaDependiente"],
                                                ValorCruceDependientePrincipal: item["ValorCruceDependientePrincipal"],
                                            }],
                                        }],
                                    }],
                                }],
                            }];

                            gestionRecursosSGPServicio.actualizarPoliticasTransversalesCrucePoliticasSGP(vm.PoliticasTCrucePoliticas).then(function (response) {
                                if (response.data && (response.statusText === "OK" || response.status === 200)) {
                                    if (response.data.Exito) {
                                        numeroGuardado = numeroGuardado + 1;
                                    } else {
                                        numeroErrores = numeroErrores + 1;
                                    }

                                } else {
                                    numeroErrores = numeroErrores + 1;

                                }
                            });
                        });
                    };
                    reader.readAsBinaryString(file);
                }
                if (numeroErrores == 0) {
                    utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                    guardarCapituloModificado();
                    vm.init();
                    vm.activarControles('inicio', fileId);

                }
                else if (numeroErrores > 0)
                    swal('', 'Se presentaron:' + numeroErrores + 'en el cargue', 'warning');
            }
        }

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

                        vm.PoliticasTCrucePoliticas.forEach(politicaPrincipal => {

                            politicaPrincipal.Localizaciones.forEach(localizaciones => {

                                localizaciones.RelacionPoliticas.forEach(relacionPoliticas => {

                                    relacionPoliticas.CrucePoliticasVigencias.forEach(crucePoliticasVigencias => {

                                        if (item["ProyectoId"] === vm.ProyectoId &&
                                            item["PoliticaId"] === politicaPrincipal.PoliticaId &&
                                            item["LocalizacionId"] === localizaciones.LocalizacionId &&
                                            item["PoliticaDependienteId"] === relacionPoliticas.PoliticaDependienteId &&
                                            item["PeriodoProyectoId"] === crucePoliticasVigencias.PeriodoProyectoId &&
                                            item["Vigencia"] === crucePoliticasVigencias.Vigencia)
                                            crucePoliticasVigencias.ValorCruceDependientePrincipal = item["ValorCruceDependientePrincipal"] === '' ? 0 : item["ValorCruceDependientePrincipal"];
                                    });
                                });
                            });
                        });

                        gestionRecursosSGPServicio.actualizarPoliticasTransversalesCrucePoliticasSGP(vm.PoliticasTCrucePoliticas).then(function (response) {
                            if (response.data && (response.statusText === "OK" || response.status === 200)) {

                                if (response.data.Exito) {
                                    parent.postMessage("cerrarModal", window.location.origin);
                                    utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                                    guardarCapituloModificado();
                                    vm.init();
                                } else {
                                    swal('', response.data.Mensaje, 'warning');
                                }

                            } else {
                                swal('', "Error al realizar la operación", 'error');
                            }

                        });

                    });
                }
                catch (ex) {
                    utilidades.mensajeError("Debe validar que el archivo corresponda a la plantilla!");
                }
            });
        };


        //Métodos       

        vm.actualizaFila = function (event, fila, crucepoliticasIndex, localizacionIndex, relacionarpoliticaIndex) {
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

                vm.PoliticasTCrucePoliticas.PoliticaPrincipal[crucepoliticasIndex].Localizaciones[localizacionIndex].RelacionPoliticas[relacionarpoliticaIndex].CrucePoliticasVigencias.forEach(CrucePoliticasVigencias => {
                    if (CrucePoliticasVigencias.Vigencia !== 'TOTAL') {

                        var fila2 = fila + CrucePoliticasVigencias.Vigencia;
                        $("#input" + fila2).css("border-color", "");
                        $("#img" + fila2).hide();

                        if (CrucePoliticasVigencias.ValorPoliticaPrincipal < CrucePoliticasVigencias.ValorCruceDependientePrincipal) {
                            $("#input" + fila2).css("border-color", "red");
                            $("#img" + fila2).removeAttr("style");
                        }
                        valSolicitado = CrucePoliticasVigencias.ValorCruceDependientePrincipal === '' ? 0 : parseFloat(CrucePoliticasVigencias.ValorCruceDependientePrincipal);
                        sum = sum + valSolicitado;
                        //if (vigencia.Totalsolicitado == undefined) {
                        //    $("#input" + fila2).css("border-color", "red");
                        //    $("#img" + fila2).removeAttr("style");
                        //}
                        //else {

                        //}
                    }
                });

                $("#label" + fila).html(ConvertirNumero(sum));

                const val = value;
                const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
                var total = value = decimalCnt && decimalCnt > 2 ? value : parseFloat(val).toFixed(2);
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);

            });
        }

        vm.filtrarDatosResult = function (politicaId) {
            /// Filtrar vista de datos.
            var secPoliticaCrucePolitica = document.getElementById('secPoliticaCrucePolitica');
            if (secPoliticaCrucePolitica && politicaId) {
                var s = secPoliticaCrucePolitica.getElementsByClassName('cDivMainPolCrucPol');
                if (s && s.length > 0) {
                    var encontro = false;
                    for (var i = 0; i < s.length; i++) {
                        var d = s[i];
                        var att = d.getAttribute("data-attribute");
                        if (att && att !== politicaId.toString()) {
                            d.style.display = 'none';
                        }
                        else {
                            d.style.display = 'block';
                            encontro = true;
                        }
                    }

                    /// No tiene datos.
                    if (!encontro) {
                        var cMensajePoliticacrucePolitca = secPoliticaCrucePolitica.getElementsByClassName('cMensajePoliticacrucePolitca');
                        if (cMensajePoliticacrucePolitca) {
                            cMensajePoliticacrucePolitca[0].innerHTML = '¡Sin datos de consulta!.';
                        }
                    }
                }
            }
        };

        vm.cargarCss = function (url, id) {
            var old = document.getElementById(id);
            if (old !== null) {
                return;
            }

            var link = document.createElement("link");
            link.id = id;
            link.async = true;
            link.type = "text/css";
            link.rel = "stylesheet";
            link.href = url;
            document.getElementsByTagName("head")[0].appendChild(link);
        };

        vm.activarDescripcionCrucePolitica = function (_this) {
            if (_this) {
                var element = _this.currentTarget;
                if (element) {
                    var DescripcionCrucePolitica = element;
                    if (DescripcionCrucePolitica) {
                        DescripcionCrucePolitica.classList.toggle("fa-plus-square");
                        DescripcionCrucePolitica.classList.toggle("fa-minus-square");
                    }

                    var dDescripcionpcp;
                    var dRepeat = DescripcionCrucePolitica.parentNode.parentNode;
                    if (dRepeat) {
                        var d = dRepeat.getElementsByClassName('dcrucepolitica');
                        if (d && d.length > 0) {
                            dDescripcionpcp = d[0];
                        }
                    }

                    var cConectorDiv;
                    var cConectorDivAux = dRepeat.getElementsByClassName('cConectorDiv');
                    if (cConectorDivAux && cConectorDivAux.length > 0) {
                        cConectorDiv = cConectorDivAux[0];
                    }

                    if (DescripcionCrucePolitica.classList.contains("fa-plus-square")) {
                        if (dDescripcionpcp) {
                            dDescripcionpcp.style.display = 'none';
                        }

                        if (cConectorDiv) {
                            cConectorDiv.style.display = 'none';
                        }
                    }
                    else if (DescripcionCrucePolitica.classList.contains("fa-minus-square")) {
                        if (dDescripcionpcp) {
                            dDescripcionpcp.style.display = 'block';
                        }

                        if (cConectorDiv) {
                            cConectorDiv.style.display = 'block';
                        }
                    }

                }
            }
        };

        /// Formatear titulo localizacion.
        vm.tituloLocalizacion = function (obj) {
            var titulo = '';
            if (obj) {

                if (obj.Departamento) {
                    titulo = obj.Departamento;
                }

                if (obj.Municipio) {
                    titulo += ' - ' + obj.Municipio;
                }

                if (obj.TipoAgrupacion) {
                    titulo += ' - ' + obj.TipoAgrupacion;
                }

                if (obj.Agrupacion) {
                    titulo += ' - ' + obj.Agrupacion;
                }

                if (titulo && titulo.indexOf('-') > 0 && titulo.indexOf('-') <= 1) {
                    titulo = titulo.substring(2);
                }
            }

            return titulo;
        };

        //validacion formato
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
                if (decimales > 2) {
                }
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

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 4) {
                        tamanioPermitido = n[0].length + 2;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
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
                if (tamanio > tamanioPermitido || tamanio > 16) {
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

        vm.ActivarEditar = function () {
            if (vm.disabled == true) {
                vm.permiteEditar = true;
                $("#EditarCruce").html("CANCELAR");
                vm.disabled = false;
            }
            else {
                vm.permiteEditar = false;
                $("#EditarCruce").html("EDITAR");
                vm.disabled = true;
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
            vm.limpiarErrores();
            if (errores != undefined) {

                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl !== undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {

                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }


                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

            }
        };

        vm.validarPCP001 = function (errores) {
            let fuente = errores.substring(0, errores.indexOf("-", 0));
            let mensaje = errores.substring(fuente.length + 1, errores.length);
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + '-' + fuente + "-pregunta-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span> " + mensaje + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }

        }

        vm.validarERR_DETALLE_POLITICA_001 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-POLITICA-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById("idCruce-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.classList.add('divInconsistencia');
            }
        }

        vm.validarERR_DETALLE_FUENTE_001 = function (errores) {
            var idSpanAlertComponent = document.getElementById("alert-focalizaciongrcapitulo1-" + errores);
            if (idSpanAlertComponent != undefined) {

                idSpanAlertComponent.classList.add("ico-advertencia");

            }
        }

        vm.validarCAT_FUENTE_001 = function (errores) {
            var idSpanAlertComponent = document.getElementById("alert-focalizaciongrcapitulo1-" + errores);
            if (idSpanAlertComponent != undefined) {

                idSpanAlertComponent.classList.add("ico-advertencia");

            }
            var idSpanAlertComponent = document.getElementById("alert-focalizaciongr");
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.add("ico-advertencia");
            }
        }

        vm.validarERR_DETALLE_LOCALIZACION_001 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-LOCALIZACION-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarERR_DETALLE_POLITCA_DEPENDIENTE_001 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-POLITICADETALLE-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarERR_DETALLE_VIGENCIA_001 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-VIGENCIA-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }


            var Arreglo = errores.split('-');
            var campoObligatorioJustificacion = document.getElementById("input" + Arreglo[0] + Arreglo[2] + Arreglo[3] + Arreglo[4]);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.classList.add('editInputDNP');
            }

            var campoObligatorioJustificacion = document.getElementById("idinputde-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.classList.add('divInconsistencia');



            }
            //}
        }

        vm.validarCAT_POLITICA_001 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-POLITICA-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
            var campoObligatorioJustificacion = document.getElementById("idCategoria-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.classList.add('divInconsistencia');
            }
        }

        vm.limpiarErrores = function () {

            var idSpanAlertComponent = document.getElementById("alert-focalizaciongrcapitulo1-" + vm.IdFuente);
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.remove("ico-advertencia");
            }
            var idSpanAlertComponent = document.getElementById("alert-focalizaciongr");
            if (idSpanAlertComponent != undefined) {
                idSpanAlertComponent.classList.remove("ico-advertencia");
            }


            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-" + vm.IdFuente + "-pregunta-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }


            if (vm.PoliticasTCrucePoliticas.PoliticaPrincipal !== undefined)
                vm.PoliticasTCrucePoliticas.PoliticaPrincipal.forEach(p => {


                    var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-POLITICA-" + p.PoliticaId);
                    if (campoObligatorioProyectos != undefined) {
                        campoObligatorioProyectos.innerHTML = "";
                        campoObligatorioProyectos.classList.add('hidden');
                    }

                    var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-POLITICA-" + vm.IdFuente + '-' + p.PoliticaId);
                    if (campoObligatorioJustificacion != undefined) {
                        campoObligatorioJustificacion.innerHTML = "";
                        campoObligatorioJustificacion.classList.add('hidden');
                    }
                    var campoObligatorioJustificacion = document.getElementById("idCategoria-" + vm.IdFuente + '-' + p.PoliticaId);
                    if (campoObligatorioJustificacion != undefined) {
                        campoObligatorioJustificacion.classList.remove('divInconsistencia');
                    }
                    var campoObligatorioJustificacion = document.getElementById("idCruce-" + vm.IdFuente + '-' + p.PoliticaId);
                    if (campoObligatorioJustificacion != undefined) {
                        campoObligatorioJustificacion.classList.remove('divInconsistencia');
                    }


                    p.Localizaciones.forEach(l => {

                        var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-LOCALIZACION-" + p.PoliticaId + "-" + vm.IdFuente + "-" + l.LocalizacionId);
                        if (campoObligatorioProyectos != undefined) {
                            campoObligatorioProyectos.innerHTML = "";
                            campoObligatorioProyectos.classList.add('hidden');
                        }
                        l.RelacionPoliticas.forEach(d => {
                            var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-POLITICADETALLE-" + p.PoliticaId + "-" + vm.IdFuente + "-" + l.LocalizacionId + "-" + d.PoliticaDependienteId);
                            if (campoObligatorioProyectos != undefined) {
                                campoObligatorioProyectos.innerHTML = "";
                                campoObligatorioProyectos.classList.add('hidden');
                            }
                            d.CrucePoliticasVigencias.forEach(v => {
                                var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-VIGENCIA-" + p.PoliticaId + "-" + vm.IdFuente + "-" + l.LocalizacionId + "-" + d.PoliticaDependienteId + "-" + v.Vigencia);
                                if (campoObligatorioProyectos != undefined) {
                                    campoObligatorioProyectos.innerHTML = "";
                                    campoObligatorioProyectos.classList.add('hidden');
                                }
                                var campoObligatorioJustificacion = document.getElementById("idinput-" + p.PoliticaId + "-" + vm.IdFuente + "-" + l.LocalizacionId + "-" + d.PoliticaDependienteId + "-" + v.Vigencia);
                                if (campoObligatorioJustificacion != undefined) {
                                    campoObligatorioJustificacion.classList.remove('divInconsistencia');
                                }
                            })

                        })
                    })

                }
                );
        }

        vm.errores = {
            'PCP001': vm.validarPCP001,

            'ERR_DETALLE_POLITICA_001': vm.validarERR_DETALLE_POLITICA_001,
            'ERR_DETALLE_FUENTE_001': vm.validarERR_DETALLE_FUENTE_001,
            'ERR_DETALLE_LOCALIZACION_001': vm.validarERR_DETALLE_LOCALIZACION_001,
            'ERR_DETALLE_POLITCA_DEPENDIENTE_001': vm.validarERR_DETALLE_POLITCA_DEPENDIENTE_001,
            'ERR_DETALLE_VIGENCIA_001': vm.validarERR_DETALLE_VIGENCIA_001,

            'CAT_POLITICA_001': vm.validarCAT_POLITICA_001,
            'CAT_FUENTE_001': vm.validarCAT_FUENTE_001,
        }

        vm.validarValores = function (producto, fuente, errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + producto + fuente);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        $scope.$watch('vm.cargado', function () {
            if (vm.valido === "true") {
                setTimeout(() => vm.notificacionValidacionPadre(JSON.parse(vm.listaerrores)), 2500);
                //vm.notificacionValidacionPadre(JSON.parse(vm.listaerrores));
            }
        });

    }

    angular.module('backbone').component('politicaCrucePoliticaSgp', {

        templateUrl: "src/app/formulario/ventanas/SGP/gestionRecursosSGP/componentes/focalizacion/politicaCrucePoliticaSgp/politicaCrucePoliticaSgp.html",        
        controller: politicaCrucePoliticaSgpController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            callBack: '&',
            valido: '@',
            listaerrores: '@',
            cargado: '@',
        }
    });



})();