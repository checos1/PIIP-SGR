(function () {
    'use strict';

    reporteAvanceRegionalizacionCapController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModal',
        'utilidades',
        'reporteAvanceRegionalizacionCapServicio',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio'
    ];

    function reporteAvanceRegionalizacionCapController(
        $scope,
        $sessionStorage,
        $uibModal,
        utilidades,
        reporteAvanceRegionalizacionCapServicio,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio

    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "regionalizacionavanceregionaliza";
        vm.disabled = true;
        vm.habilitaBotones = true;
        vm.habilitarFinal = false;
        vm.registrosExcel = null;
        vm.listaDatosExcel = [];
        vm.fuentes = [];
        vm.Objetivos = [];
        vm.resumenrecursos = [];
        vm.verDatosExcel = false;
        vm.ConvertirNumero = ConvertirNumero;
        vm.ConvertirNumero4 = ConvertirNumero4;
        vm.SumaTotalVigencia = SumaTotalVigencia;
        vm.disabled = false;
        vm.disabled2 = false;
        vm.Guardar = Guardar;
        vm.Cancelar = Cancelar;
        vm.Editar = Editar;
        vm.copiaIndicador;
        vm.actualizaValoresAnteriores;
        vm.HabilitaGuardarIndicador = false;
        vm.soloLectura = $sessionStorage.soloLectura;
        vm.EsActualizacion = false;
        vm.listaDatos = "";
        vm.listaDatosOrigen = "";
        vm.mensajevalidacion = "";
        vm.FuenteSeleccionada = 0;
        vm.anteriorIdElementoFuente = "";
        vm.anteriorIdElementoObjetivo = "";
        vm.ObjetivoSeleccionadoReportAvance = 0;
        vm.ProductoSeleccionadoReportAvance = 0;
        vm.anteriorIdElementoLocalizacion = "";
        vm.localizacionReportAvanceSeleccionada = 0;
        vm.anteriorIdElementoRecursosMR = "";
        vm.VigenciaReportAvanceSeleccionada = 0;
        vm.VigenciaMRDDReportAvanceSelecionada = 0;
        vm.anteriorIdElementoRecursosMRDD = "";
        vm.abrirMensajeArchivoRegionalizacion = abrirMensajeArchivoRegionalizacion;
        vm.exportExcel = exportExcel;
        vm.ObtenerParaCargueMasivo = ObtenerParaCargueMasivo;
        vm.modelo = null;
        vm.adjuntarArchivo = adjuntarArchivo;
        vm.nombrearchivo = "";
        vm.validarArchivo = validarArchivo;
        vm.limpiarArchivo = limpiarArchivo;
        vm.GuardarArchivo = GuardarArchivo;
        vm.HabilitaCargue = false;

        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.refreshComponente(true, null);
        };

        $scope.$watch('vm.refreshregionalizacion', function () {
            if (vm.refreshregionalizacion === "true") {

                vm.refreshComponente(true, null);

                vm.refreshregionalizacion = "false";
            }

        });

        /*------------------------------AQUI INICIA PARA LOGICA DEL NEGOCIO ------------------------------------------------------*/
        vm.refreshComponente = function (tipoOperacion, localizacion) {
            vm.consultarAvanceMetaProducto(tipoOperacion, localizacion);
        };

        function Cancelar(localizacion, fuentes, fuenteid, objetivoespecificoid, productoid, localizacionid) {

            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                var indexrpa = 0;

                vm.DetalleRegionalizacion.RecursosPeriodosActivos.forEach(rpa => {

                    $("#imgcptj" + indexrpa + rpa.Vigencia + rpa.Mes + localizacionid + fuenteid + objetivoespecificoid + productoid).attr('disabled', true);
                    $("#imgcptj" + indexrpa + rpa.Vigencia + rpa.Mes + localizacionid + fuenteid + objetivoespecificoid + productoid).fadeOut();
                    $("#icocptj" + indexrpa + rpa.Vigencia + rpa.Mes + localizacionid + fuenteid + objetivoespecificoid + productoid).attr('disabled', true);
                    $("#icocptj" + indexrpa + rpa.Vigencia + rpa.Mes + localizacionid + fuenteid + objetivoespecificoid + productoid).fadeOut();

                    $("#errortottal" + localizacionid + fuenteid + objetivoespecificoid + productoid).attr('disabled', true);
                    $("#errortottal" + localizacionid + fuenteid + objetivoespecificoid + productoid).fadeOut();
                    $("#errortottalmsn" + localizacionid + fuenteid + objetivoespecificoid + productoid).attr('disabled', true);
                    $("#errortottalmsn" + localizacionid + fuenteid + objetivoespecificoid + productoid).fadeOut();

                    var Errormsn = document.getElementById("errortottalmsn" + localizacionid + fuenteid + objetivoespecificoid + productoid);
                    if (Errormsn != undefined) {
                        Errormsn.innerHTML = '<span></span>';
                    }
                    indexrpa++;
                });

                vm.HabilitaEditarIndicador = false;
                vm.DetalleRegionalizacion = JSON.parse(vm.listaDatosOrigen);
                calcularTotalesPorVigenciaYGeneral();

            }, function funcionCancelar(reason) {
                //poner aquí q pasa cuando cancela
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function Editar(localizacion, fuentes, fuenteid, objetivoespecificoid, productoid, localizacionid) {

            vm.listaDatosOrigen = "";
            vm.HabilitaEditarIndicador = true;
            vm.listaDatosOrigen = JSON.stringify(vm.DetalleRegionalizacion);
        }

        vm.consultarAvanceMetaProducto = function (tipoOperacion, localizacion) {
            const objetoParametros = {
                instanciaId: $sessionStorage.idInstanciaIframe,
                proyectoId: $sessionStorage.proyectoId,
                codigoBpin: $sessionStorage.idObjetoNegocio,
                vigencia: 0,
                periodoPeriodicidad: 0
            };

            vm.listaDatosExcel = [];
            vm.modelo = null;

            return reporteAvanceRegionalizacionCapServicio.consultarAvanceRegionalizacion(objetoParametros).then(
                function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {

                        var arreglolistas = jQuery.parseJSON(response.data);

                        var arreglo = jQuery.parseJSON(arreglolistas);

                        var listaDatos = jQuery.parseJSON(arreglolistas);

                        var fuentes = [];

                        arreglo.Fuentes.forEach(Indi => {

                            Indi.ComparativoFuentes.forEach(Indi1 => {
                                fuentes.push({ FuenteId: Indi.FuenteId, Mes: Indi1.Mes, Vigencia: Indi1.Vigencia, Comparativo: 'Valores de la fuente', Vigente: Indi1.Vigente, Compromisos: Indi1.Compromisos, Obligaciones: Indi1.Obligaciones, Pagos: Indi1.Pagos, NunMes: ObtenerMesNum(Indi1.Mes) });
                            });

                            Indi.ComparativoRegionalizados.forEach(Indi2 => {
                                fuentes.push({ FuenteId: Indi.FuenteId, Mes: Indi2.Mes, Vigencia: Indi2.Vigencia, Comparativo: ('Regionalizado Acumulado a ' + Indi2.Mes), Vigente: Indi2.Vigente, Compromisos: Indi2.Compromisos, Obligaciones: Indi2.Obligaciones, Pagos: Indi2.Pagos, NunMes: ObtenerMesNum(Indi2.Mes) });
                            });
                        });

                        vm.fuentes = fuentes;

                        $scope.datosavanceRegionalizacion = listaDatos;

                        if (listaDatos.ListaPeriodosActivos == null) {
                            vm.HabilitaCargue = true;
                            $scope.isBlue = false;
                            $scope.isGrey = true;
                        } else {
                            vm.HabilitaCargue = false;
                            $scope.isBlue = true;
                            $scope.isGrey = false;
                        }

                        vm.listaDatos = listaDatos;

                    } else {
                        swal('', "Error al obtener valores del avance de regionalización", 'error');
                    }
                },
                function (error) {
                    if (error) {
                        utilidades.mensajeError(error);
                    }
                }
            );
        };

        function SumaTotalVigencia(recursos) {
            return 0;
        }

        function ObtenerMesNum(expr) {

            if (expr == 'Enero') {
                return 1;
            } else if (expr == 'Febrero') {
                return 2;
            } else if (expr == 'Marzo') {
                return 3;
            } else if (expr == 'Abril') {
                return 4;
            } else if (expr == 'Mayo') {
                return 5;
            } else if (expr == 'Junio') {
                return 6;
            } else if (expr == 'Julio') {
                return 7;
            } else if (expr == 'Agosto') {
                return 8;
            } else if (expr == 'Septiembre') {
                return 9;
            } else if (expr == 'Octubre') {
                return 10;
            } else if (expr == 'Noviembre') {
                return 11;
            } else if (expr == 'Diciembre') {
                return 12;
            } else {
                return 1;
            }
        }

        function Guardar(localizacion) {
            if (vm.mensajevalidacion != "") {

                utilidades.mensajeError("Debe resolver los mensajes de errores antes de guardar. ");

            } else {

                const objetoParametros = {
                    instanciaId: $sessionStorage.idInstanciaIframe,
                    proyectoId: $sessionStorage.proyectoId,
                    FuenteId: vm.FuenteSeleccionada,
                    ObjetivoEspecificoId: vm.ObjetivoSeleccionadoReportAvance,
                    ProductoId: vm.ProductoSeleccionadoReportAvance,
                    localizacionId: vm.localizacionReportAvanceSeleccionada,
                    detalleRegionalizacion: vm.DetalleRegionalizacion
                };


                return reporteAvanceRegionalizacionCapServicio.guardarAvanceRegionalizacion(objetoParametros, $sessionStorage.proyectoId, $sessionStorage.BPIN).then(
                    function (respuesta) {

                        if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                            if (respuesta.data.Status) {

                                ObtenerDetalleLocalizacion();

                                guardarCapituloModificado();
                                vm.limpiarErrores();

                                utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito.");

                                vm.HabilitaEditarIndicador = false;

                                vm.refreshregionalizacion = "true";


                            } else {
                                utilidades.mensajeError(respuesta.data.Message);
                            }

                        } else {
                            utilidades.mensajeError("Error al realizar la operación");
                        }

                    });
            }
        }

        function GuardarMasivo(localizacion) {
            if (vm.mensajevalidacion != "") {

                utilidades.mensajeError("Debe resolver los mensajes de errores antes de guardar. ");

            } else {

                return reporteAvanceRegionalizacionCapServicio.guardarAvanceRegionalizacionMasivo(localizacion, $sessionStorage.proyectoId, $sessionStorage.BPIN).then(
                    function (respuesta) {

                        if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                            if (respuesta.data.Status) {

                                guardarCapituloModificado();
                                vm.limpiarErrores();

                                utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito.");

                                vm.HabilitaEditarIndicador = false;

                                vm.refreshregionalizacion = "true";


                            } else {
                                utilidades.mensajeError(respuesta.data.Message);
                            }

                        } else {
                            utilidades.mensajeError("Error al realizar la operación");
                        }

                    });
            }
        }

        function ObtenerDetalleLocalizacion() {
            const proyecto = {
                InstanciaId: $sessionStorage.idInstancia,
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Texto: 'DetalleLocalizacion',
                DetalleLocalizacion: [{
                    FuenteId: vm.FuenteSeleccionada,
                    ProductoId: vm.ProductoSeleccionadoReportAvance,
                    LocalizacionId: vm.localizacionReportAvanceSeleccionada
                }]
            };

            vm.DetalleRegionalizacion = null;
            var parametroConsulta = JSON.stringify(proyecto);
            return reporteAvanceRegionalizacionCapServicio.ObtenerDetalleRegionalizacionProgramacionSeguimiento(parametroConsulta, usuarioDNP).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.DetalleRegionalizacion = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                    }
                });
        }

        function ObtenerParaCargueMasivo(FuenteId) {
            const proyecto = {
                InstanciaId: $sessionStorage.idInstancia,
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Texto: 'CargueMasivo',
                DetalleLocalizacion: [{
                    FuenteId: FuenteId
                }]
            };

            var parametroConsulta = JSON.stringify(proyecto);
            return reporteAvanceRegionalizacionCapServicio.ObtenerDetalleRegionalizacionProgramacionSeguimiento(parametroConsulta, usuarioDNP).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        var arreglo = jQuery.parseJSON(arreglolistas);
                        vm.modelo = arreglo;
                        exportExcel();
                    }
                });
        }

        vm.actualizaFila = function (valoresPeriodosActivos, arregloRecursos, tab, index, vigencia, mes, localizacionid, fuenteid, objetivoid, productoid) {

            calcularTotales(valoresPeriodosActivos, arregloRecursos, tab, index, vigencia, mes, localizacionid, fuenteid, objetivoid, productoid);
        }

        function calcularTotales(valoresPeriodosActivos, arregloRecursos, tab, index, vigencia, mes, localizacionid, fuenteid, objetivoid, productoid) {
            var mensajevalidacion = "";
            var mensajefinal = "";

            var arregloResumen = arregloRecursos[0].DetalleVigencia;
            var arregloResumenGeneral = arregloRecursos;

            switch (tab) {
                case 1:
                    for (var i = 0; i < valoresPeriodosActivos.length; i++) {
                        mensajevalidacion = "";

                        //Recursos
                        var valorCompromisos = $("#inputCompromisos" + index + vigencia + mes + localizacionid + fuenteid + objetivoid + productoid).val();
                        var valorObligaciones = $("#inputObligaciones" + index + vigencia + mes + localizacionid + fuenteid + objetivoid + productoid).val();
                        var valorPagos = $("#txtPagosE" + index + vigencia + mes + localizacionid + fuenteid + objetivoid + productoid).val();
                        var observacionRecursos = $("#txtObservacionesRecursos" + index + vigencia + mes + localizacionid + fuenteid + objetivoid + productoid).val();

                        localStorage.setItem('Compromiso', valorCompromisos);
                        localStorage.setItem('DetalleCompromiso', vigencia + "-" + mes + "-" + fuenteid + "-" + localizacionid + "-" + objetivoid + "-" + productoid);
                        //Metas
                        var valorMetaAvance = $("#txtAvanceMes" + index + vigencia + mes + localizacionid + fuenteid + objetivoid + productoid).val();
                        var observacionMeta = $("#txtObservacionesMeta" + index + vigencia + mes + localizacionid + fuenteid + objetivoid + productoid).val();

                        //Detalle Recursos
                        $("#DetalleRecursosCompromisos-" + vigencia + "-" + mes + "-" + fuenteid + "-" + localizacionid + "-" + objetivoid + "-" + productoid).val(vm.ConvertirNumero((valorCompromisos)));
                        $("#DetalleRecursosObligaciones-" + vigencia + "-" + mes + "-" + fuenteid + "-" + localizacionid + "-" + objetivoid + "-" + productoid).val(vm.ConvertirNumero((valorObligaciones)));
                        $("#DetalleRecursosPagos-" + vigencia + "-" + mes + "-" + fuenteid + "-" + localizacionid + "-" + objetivoid + "-" + productoid).val(vm.ConvertirNumero((valorPagos)));
                        $("#DetalleRecursosObservacion-" + vigencia + "-" + mes + "-" + fuenteid + "-" + localizacionid + "-" + objetivoid + "-" + productoid).val(observacionRecursos);

                        //Metas
                        $("#Meta-" + vigencia + "-" + fuenteid + "-" + localizacionid + "-" + index + "-" + objetivoid + "-" + productoid).val(vm.ConvertirNumero4((valorMetaAvance)));

                        // Detalle Metas
                        $("#MetaDetalle-" + vigencia + "-" + mes + "-" + fuenteid + "-" + localizacionid + "-" + objetivoid + "-" + productoid).val(vm.ConvertirNumero4((valorMetaAvance)));
                        $("#DetalleMetasObservacion-" + vigencia + "-" + mes + "-" + fuenteid + "-" + localizacionid + "-" + objetivoid + "-" + productoid).val(observacionMeta);

                        if (mensajevalidacion != "") {
                            $("#imgcptj" + i + valoresPeriodosActivos[i].Vigencia + valoresPeriodosActivos[i].Mes + localizacionid + fuenteid + objetivoid + productoid).attr('disabled', false);
                            $("#imgcptj" + i + valoresPeriodosActivos[i].Vigencia + valoresPeriodosActivos[i].Mes + localizacionid + fuenteid + objetivoid + productoid).fadeIn();
                            $("#icocptj" + i + valoresPeriodosActivos[i].Vigencia + valoresPeriodosActivos[i].Mes + localizacionid + fuenteid + objetivoid + productoid).attr('disabled', false);
                            $("#icocptj" + i + valoresPeriodosActivos[i].Vigencia + valoresPeriodosActivos[i].Mes + localizacionid + fuenteid + objetivoid + productoid).fadeIn();
                            $("#txtPagosE" + i + valoresPeriodosActivos[i].Vigencia + valoresPeriodosActivos[i].Mes + localizacionid + fuenteid + objetivoid + productoid).css("border-color", "red");
                        }
                        else {
                            $("#imgcptj" + i + valoresPeriodosActivos[i].Vigencia + valoresPeriodosActivos[i].Mes + localizacionid + fuenteid + objetivoid + productoid).attr('disabled', true);
                            $("#imgcptj" + i + valoresPeriodosActivos[i].Vigencia + valoresPeriodosActivos[i].Mes + localizacionid + fuenteid + objetivoid + productoid).fadeOut();
                            $("#icocptj" + i + valoresPeriodosActivos[i].Vigencia + valoresPeriodosActivos[i].Mes + localizacionid + fuenteid + objetivoid + productoid).attr('disabled', true);
                            $("#icocptj" + i + valoresPeriodosActivos[i].Vigencia + valoresPeriodosActivos[i].Mes + localizacionid + fuenteid + objetivoid + productoid).fadeOut();
                        }
                        mensajefinal += mensajevalidacion;
                    }

                    if (mensajefinal != "") {
                        $("#errortottal" + localizacionid + fuenteid + objetivoid + productoid).attr('disabled', false);
                        $("#errortottal" + localizacionid + fuenteid + objetivoid + productoid).fadeIn();
                        $("#errortottalmsn" + localizacionid + fuenteid + objetivoid + productoid).attr('disabled', false);
                        $("#errortottalmsn" + localizacionid + fuenteid + objetivoid + productoid).fadeIn();
                    }
                    else {
                        $("#errortottal" + localizacionid + fuenteid + objetivoid + productoid).attr('disabled', true);
                        $("#errortottal" + localizacionid + fuenteid + objetivoid + productoid).fadeOut();
                        $("#errortottalmsn" + localizacionid + fuenteid + objetivoid + productoid).attr('disabled', true);
                        $("#errortottalmsn" + localizacionid + fuenteid + objetivoid + productoid).fadeOut();

                    }

                    var Errormsn = document.getElementById("errortottalmsn" + localizacionid + fuenteid + objetivoid + productoid);
                    if (Errormsn != undefined) {
                        Errormsn.innerHTML = '<span>' + mensajefinal + "</span>";

                    }
                    vm.mensajevalidacion = mensajefinal;
                    calcularTotalesPorVigenciaYGeneral();
                    break;
                case 2:

                    break;
                default:

            }
        }

        function calcularTotalesPorVigenciaYGeneral() {

            var arregloResumenGeneral = vm.DetalleRegionalizacion.ResumenRecursosMetas;
            var valoresPeriodosActivos = vm.DetalleRegionalizacion.RecursosPeriodosActivos;


            //Total para todas las vigencias
            var acumulaCompromisosGeneral = 0;
            var acumulaObligacionesGeneral = 0;
            var acumulaPagosGeneral = 0;

            //Total por cada vigencia
            var acumulaCompromisosVigencia = 0;
            var acumulaObligacionesVigencia = 0;
            var acumulaPagosVigencia = 0;
            var controlMesSumado = 0;
            var controlMesActivo = 0;

            arregloResumenGeneral.forEach(IndiV => {  ///recorrer cada vigencia
                var arregloResumenVigencia = IndiV.DetalleVigencia;
                acumulaCompromisosVigencia = 0;
                acumulaObligacionesVigencia = 0;
                acumulaPagosVigencia = 0;

                arregloResumenVigencia.forEach(Indi => { /// recorrer los meses por cada vigencia 
                    controlMesSumado = 0;
                    if (valoresPeriodosActivos != null) {

                        controlMesActivo = 0;
                        valoresPeriodosActivos.forEach(IndiMesActivo => {
                            if (IndiMesActivo.Mes === Indi.Mes) {
                                controlMesActivo = 1;
                            }
                        });
                        valoresPeriodosActivos.forEach(IndiMes => {

                            if (IndiV.Vigencia === IndiMes.Vigencia) {

                                if (IndiMes.Mes === Indi.Mes) {  /// si el mes y el año estan activos, se suma lo que se está editando en pantalla
                                    acumulaCompromisosVigencia = acumulaCompromisosVigencia + parseFloat(IndiMes.Compromisos);
                                    acumulaObligacionesVigencia = acumulaObligacionesVigencia + parseFloat(IndiMes.Obligaciones);
                                    acumulaPagosVigencia = acumulaPagosVigencia + parseFloat(IndiMes.Pagos);


                                    acumulaCompromisosGeneral = acumulaCompromisosGeneral + parseFloat(IndiMes.Compromisos);
                                    acumulaObligacionesGeneral = acumulaObligacionesGeneral + parseFloat(IndiMes.Obligaciones);
                                    acumulaPagosGeneral = acumulaPagosGeneral + parseFloat(IndiMes.Pagos);

                                    //Detalle Recursos
                                    $("#DetalleRecursosCompromisos-" + IndiV.Vigencia + "-" + IndiMes.Mes + "-" + vm.FuenteSeleccionada + "-" + vm.localizacionReportAvanceSeleccionada + "-" + vm.ObjetivoSeleccionadoReportAvance + "-" + vm.ProductoSeleccionadoReportAvance).val(vm.ConvertirNumero((IndiMes.Compromisos)));
                                    $("#DetalleRecursosObligaciones-" + IndiV.Vigencia + "-" + IndiMes.Mes + "-" + vm.FuenteSeleccionada + "-" + vm.localizacionReportAvanceSeleccionada + "-" + vm.ObjetivoSeleccionadoReportAvance + "-" + vm.ProductoSeleccionadoReportAvance).val(vm.ConvertirNumero((IndiMes.Obligaciones)));
                                    $("#DetalleRecursosPagos-" + IndiV.Vigencia + "-" + IndiMes.Mes + "-" + vm.FuenteSeleccionada + "-" + vm.localizacionReportAvanceSeleccionada + "-" + vm.ObjetivoSeleccionadoReportAvance + "-" + vm.ProductoSeleccionadoReportAvance).val(vm.ConvertirNumero((IndiMes.Pagos)));
                                    $("#DetalleRecursosObservacion-" + IndiV.Vigencia + "-" + IndiMes.Mes + "-" + vm.FuenteSeleccionada + "-" + vm.localizacionReportAvanceSeleccionada + "-" + vm.ObjetivoSeleccionadoReportAvance + "-" + vm.ProductoSeleccionadoReportAvance).val(IndiMes.ObservacionRecurso);

                                    controlMesSumado = 1;
                                }

                                if (IndiMes.Mes != Indi.Mes && controlMesSumado != 1 && controlMesActivo === 0) {  /// si NO ES el mes activo, se suma lo del resumen y no lo que está activo para editar.
                                    acumulaCompromisosVigencia = acumulaCompromisosVigencia + parseFloat(Indi.ValorCompromisos);
                                    acumulaObligacionesVigencia = acumulaObligacionesVigencia + parseFloat(Indi.ValorObligaciones);
                                    acumulaPagosVigencia = acumulaPagosVigencia + parseFloat(Indi.ValorPagos);


                                    acumulaCompromisosGeneral = acumulaCompromisosGeneral + parseFloat(Indi.ValorCompromisos);
                                    acumulaObligacionesGeneral = acumulaObligacionesGeneral + parseFloat(Indi.ValorObligaciones);
                                    acumulaPagosGeneral = acumulaPagosGeneral + parseFloat(Indi.ValorPagos);
                                    controlMesSumado = 1;
                                }

                            }
                            else {

                                if (controlMesSumado != 1) {
                                    acumulaCompromisosGeneral = acumulaCompromisosGeneral + parseFloat(Indi.ValorCompromisos);
                                    acumulaObligacionesGeneral = acumulaObligacionesGeneral + parseFloat(Indi.ValorObligaciones);
                                    acumulaPagosGeneral = acumulaPagosGeneral + parseFloat(Indi.ValorPagos);
                                    controlMesSumado = 1;
                                }
                            }
                        });
                    }
                    else {
                        acumulaCompromisosGeneral = acumulaCompromisosGeneral + parseFloat(Indi.ValorCompromisos);
                        acumulaObligacionesGeneral = acumulaObligacionesGeneral + parseFloat(Indi.ValorObligaciones);
                        acumulaPagosGeneral = acumulaPagosGeneral + parseFloat(Indi.ValorPagos);
                    }
                });

                // Total Recursos de la vigencia seleccionada sumando los meses de dicha vigencia
                $("#RecursosCompromisos-" + IndiV.Vigencia + "-" + vm.FuenteSeleccionada + "-" + vm.localizacionReportAvanceSeleccionada + "-" + vm.ObjetivoSeleccionadoReportAvance + "-" + vm.ProductoSeleccionadoReportAvance).val(vm.ConvertirNumero((acumulaCompromisosVigencia)));
                $("#RecursosObligaciones-" + IndiV.Vigencia + "-" + vm.FuenteSeleccionada + "-" + vm.localizacionReportAvanceSeleccionada + "-" + vm.ObjetivoSeleccionadoReportAvance + "-" + vm.ProductoSeleccionadoReportAvance).val(vm.ConvertirNumero((acumulaObligacionesVigencia)));
                $("#RecursosPagos-" + IndiV.Vigencia + "-" + vm.FuenteSeleccionada + "-" + vm.localizacionReportAvanceSeleccionada + "-" + vm.ObjetivoSeleccionadoReportAvance + "-" + vm.ProductoSeleccionadoReportAvance).val(vm.ConvertirNumero((acumulaPagosVigencia)));


                //Detalle Recursos de la vigencia seleccionada sumando los meses de dicha vigencia
                $("#DetalleRecursosTotalCompromisos-" + IndiV.Vigencia + "-" + vm.FuenteSeleccionada + "-" + vm.localizacionReportAvanceSeleccionada + "-" + vm.ObjetivoSeleccionadoReportAvance + "-" + vm.ProductoSeleccionadoReportAvance).val(vm.ConvertirNumero((acumulaCompromisosVigencia)));
                $("#DetalleRecursosTotalObligaciones-" + IndiV.Vigencia + "-" + vm.FuenteSeleccionada + "-" + vm.localizacionReportAvanceSeleccionada + "-" + vm.ObjetivoSeleccionadoReportAvance + "-" + vm.ProductoSeleccionadoReportAvance).val(vm.ConvertirNumero((acumulaObligacionesVigencia)));
                $("#DetalleRecursosTotalPagos-" + IndiV.Vigencia + "-" + vm.FuenteSeleccionada + "-" + vm.localizacionReportAvanceSeleccionada + "-" + vm.ObjetivoSeleccionadoReportAvance + "-" + vm.ProductoSeleccionadoReportAvance).val(vm.ConvertirNumero((acumulaPagosVigencia)));
            });



            //Total Recursos para todas las vigencias
            $("#TotaCompromisos-" + vm.FuenteSeleccionada + "-" + vm.localizacionReportAvanceSeleccionada + "-" + vm.ObjetivoSeleccionadoReportAvance + "-" + vm.ProductoSeleccionadoReportAvance).val(vm.ConvertirNumero((acumulaCompromisosGeneral)));
            $("#TotalObligaciones-" + vm.FuenteSeleccionada + "-" + vm.localizacionReportAvanceSeleccionada + "-" + vm.ObjetivoSeleccionadoReportAvance + "-" + vm.ProductoSeleccionadoReportAvance).val(vm.ConvertirNumero((acumulaObligacionesGeneral)));
            $("#TotalPagos-" + vm.FuenteSeleccionada + "-" + vm.localizacionReportAvanceSeleccionada + "-" + vm.ObjetivoSeleccionadoReportAvance + "-" + vm.ProductoSeleccionadoReportAvance).val(vm.ConvertirNumero((acumulaPagosGeneral)));


        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.InstanciaSeleccionada.IdInstancia,
                Modificado: false
            };
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

        vm.abrilNivel = function (idElement) {

            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                }
            }
        }

        vm.abrilNivelFuente = function (idElement, fuenteId) {
            vm.ObjetivoSeleccionadoReportAvance = 0;
            vm.ProductoSeleccionadoReportAvance = 0;
            vm.localizacionReportAvanceSeleccionada = 0;
            vm.VigenciaReportAvanceSeleccionada = 0;
            vm.VigenciaMRDDReportAvanceSelecionada = 0;
            if (vm.anteriorIdElementoFuente != '' && vm.anteriorIdElementoFuente != idElement) {
                vm.anteriorIdElementoFuente = vm.anteriorIdElementoFuente.replace("null", "");
                var elMasAnterior = document.getElementById(vm.anteriorIdElementoFuente + '-mas');
                var elMenosAnterior = document.getElementById(vm.anteriorIdElementoFuente + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            if (vm.anteriorIdElementoObjetivo != '') {
                vm.anteriorIdElementoObjetivo = vm.anteriorIdElementoObjetivo.replace("null", "");
                var elMasAnterior2 = document.getElementById(vm.anteriorIdElementoObjetivo + '-mas');
                var elMenosAnterior2 = document.getElementById(vm.anteriorIdElementoObjetivo + '-menos');
                if (elMasAnterior2 != null && elMenosAnterior2 != null) {
                    if (elMasAnterior2.classList.contains('hidden')) {
                        elMenosAnterior2.classList.add('hidden');
                        elMasAnterior2.classList.remove('hidden');
                    }
                }
            }

            if (vm.anteriorIdElementoLocalizacion != '') {
                vm.anteriorIdElementoLocalizacion = vm.anteriorIdElementoLocalizacion.replace("null", "");
                var elMasAnterior3 = document.getElementById(vm.anteriorIdElementoLocalizacion + '-mas');
                var elMenosAnterior3 = document.getElementById(vm.anteriorIdElementoLocalizacion + '-menos');
                if (elMasAnterior3 != null && elMenosAnterior3 != null) {
                    if (elMasAnterior3.classList.contains('hidden')) {
                        elMenosAnterior3.classList.add('hidden');
                        elMasAnterior3.classList.remove('hidden');
                    }
                }
            }

            if (vm.anteriorIdElementoRecursosMR != '') {
                vm.anteriorIdElementoRecursosMR = vm.anteriorIdElementoRecursosMR.replace("null", "");
                var elMasAnterior4 = document.getElementById(vm.anteriorIdElementoRecursosMR + '-mas');
                var elMenosAnterior4 = document.getElementById(vm.anteriorIdElementoRecursosMR + '-menos');
                if (elMasAnterior4 != null && elMenosAnterior4 != null) {
                    if (elMasAnterior4.classList.contains('hidden')) {
                        elMenosAnterior4.classList.add('hidden');
                        elMasAnterior4.classList.remove('hidden');
                    }
                }
            }

            if (vm.anteriorIdElementoRecursosMRDD != '') {
                vm.anteriorIdElementoRecursosMRDD = vm.anteriorIdElementoRecursosMRDD.replace("null", "");
                var elMasAnterior5 = document.getElementById(vm.anteriorIdElementoRecursosMRDD + '-mas');
                var elMenosAnterior5 = document.getElementById(vm.anteriorIdElementoRecursosMRDD + '-menos');
                if (elMasAnterior5 != null && elMenosAnterior5 != null) {
                    if (elMasAnterior5.classList.contains('hidden')) {
                        elMenosAnterior5.classList.add('hidden');
                        elMasAnterior5.classList.remove('hidden');
                    }
                }
            }

            vm.anteriorIdElementoFuente = idElement;
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.FuenteSeleccionada = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.FuenteSeleccionada = fuenteId;
                }
            }
        }

        vm.abrilNivelObjetivo = function (idElement, objetivoId, productoId) {
            vm.localizacionReportAvanceSeleccionada = 0;
            vm.VigenciaReportAvanceSeleccionada = 0;
            vm.VigenciaMRDDReportAvanceSelecionada = 0;
            if (vm.anteriorIdElementoObjetivo != '' && vm.anteriorIdElementoObjetivo != idElement) {
                vm.anteriorIdElementoObjetivo = vm.anteriorIdElementoObjetivo.replace("null", "");
                var elMasAnterior = document.getElementById(vm.anteriorIdElementoObjetivo + '-mas');
                var elMenosAnterior = document.getElementById(vm.anteriorIdElementoObjetivo + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            if (vm.anteriorIdElementoLocalizacion != '') {
                vm.anteriorIdElementoLocalizacion = vm.anteriorIdElementoLocalizacion.replace("null", "");
                var elMasAnterior3 = document.getElementById(vm.anteriorIdElementoLocalizacion + '-mas');
                var elMenosAnterior3 = document.getElementById(vm.anteriorIdElementoLocalizacion + '-menos');
                if (elMasAnterior3 != null && elMenosAnterior3 != null) {
                    if (elMasAnterior3.classList.contains('hidden')) {
                        elMenosAnterior3.classList.add('hidden');
                        elMasAnterior3.classList.remove('hidden');
                    }
                }
            }

            if (vm.anteriorIdElementoRecursosMR != '') {
                vm.anteriorIdElementoRecursosMR = vm.anteriorIdElementoRecursosMR.replace("null", "");
                var elMasAnterior4 = document.getElementById(vm.anteriorIdElementoRecursosMR + '-mas');
                var elMenosAnterior4 = document.getElementById(vm.anteriorIdElementoRecursosMR + '-menos');
                if (elMasAnterior4 != null && elMenosAnterior4 != null) {
                    if (elMasAnterior4.classList.contains('hidden')) {
                        elMenosAnterior4.classList.add('hidden');
                        elMasAnterior4.classList.remove('hidden');
                    }
                }
            }

            if (vm.anteriorIdElementoRecursosMRDD != '') {
                vm.anteriorIdElementoRecursosMRDD = vm.anteriorIdElementoRecursosMRDD.replace("null", "");
                var elMasAnterior5 = document.getElementById(vm.anteriorIdElementoRecursosMRDD + '-mas');
                var elMenosAnterior5 = document.getElementById(vm.anteriorIdElementoRecursosMRDD + '-menos');
                if (elMasAnterior5 != null && elMenosAnterior5 != null) {
                    if (elMasAnterior5.classList.contains('hidden')) {
                        elMenosAnterior5.classList.add('hidden');
                        elMasAnterior5.classList.remove('hidden');
                    }
                }
            }

            vm.anteriorIdElementoObjetivo = idElement;
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.ObjetivoSeleccionadoReportAvance = 0;
                    vm.ProductoSeleccionadoReportAvance = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.ObjetivoSeleccionadoReportAvance = objetivoId;
                    vm.ProductoSeleccionadoReportAvance = productoId;
                }
            }
        }

        vm.abrilNivelLocalizacion = function (idElement, fuenteId, productoId, localizacionId) {

            vm.VigenciaReportAvanceSeleccionada = 0;
            vm.VigenciaMRDDReportAvanceSelecionada = 0;
            if (vm.anteriorIdElementoLocalizacion != '' && vm.anteriorIdElementoLocalizacion != idElement) {
                vm.anteriorIdElementoLocalizacion = vm.anteriorIdElementoLocalizacion.replace("null", "");
                var elMasAnterior = document.getElementById(vm.anteriorIdElementoLocalizacion + '-mas');
                var elMenosAnterior = document.getElementById(vm.anteriorIdElementoLocalizacion + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            if (vm.anteriorIdElementoRecursosMR != '') {
                vm.anteriorIdElementoRecursosMR = vm.anteriorIdElementoRecursosMR.replace("null", "");
                var elMasAnterior4 = document.getElementById(vm.anteriorIdElementoRecursosMR + '-mas');
                var elMenosAnterior4 = document.getElementById(vm.anteriorIdElementoRecursosMR + '-menos');
                if (elMasAnterior4 != null && elMenosAnterior4 != null) {
                    if (elMasAnterior4.classList.contains('hidden')) {
                        elMenosAnterior4.classList.add('hidden');
                        elMasAnterior4.classList.remove('hidden');
                    }
                }
            }

            if (vm.anteriorIdElementoRecursosMRDD != '') {
                vm.anteriorIdElementoRecursosMRDD = vm.anteriorIdElementoRecursosMRDD.replace("null", "");
                var elMasAnterior5 = document.getElementById(vm.anteriorIdElementoRecursosMRDD + '-mas');
                var elMenosAnterior5 = document.getElementById(vm.anteriorIdElementoRecursosMRDD + '-menos');
                if (elMasAnterior5 != null && elMenosAnterior5 != null) {
                    if (elMasAnterior5.classList.contains('hidden')) {
                        elMenosAnterior5.classList.add('hidden');
                        elMasAnterior5.classList.remove('hidden');
                    }
                }
            }

            vm.anteriorIdElementoLocalizacion = idElement;
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.localizacionReportAvanceSeleccionada = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.localizacionReportAvanceSeleccionada = localizacionId;
                    ObtenerDetalleLocalizacion();
                }
            }

        };

        vm.abrilNivelRecursosMR = function (idElement, vigencia) {
            vm.VigenciaMRDDReportAvanceSelecionada = 0;
            if (vm.anteriorIdElementoRecursosMR != '' && vm.anteriorIdElementoRecursosMR != idElement) {
                vm.anteriorIdElementoRecursosMR = vm.anteriorIdElementoRecursosMR.replace("null", "");
                var elMasAnterior = document.getElementById(vm.anteriorIdElementoRecursosMR + '-mas');
                var elMenosAnterior = document.getElementById(vm.anteriorIdElementoRecursosMR + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            if (vm.anteriorIdElementoRecursosMRDD != '') {
                vm.anteriorIdElementoRecursosMRDD = vm.anteriorIdElementoRecursosMRDD.replace("null", "");
                var elMasAnterior5 = document.getElementById(vm.anteriorIdElementoRecursosMRDD + '-mas');
                var elMenosAnterior5 = document.getElementById(vm.anteriorIdElementoRecursosMRDD + '-menos');
                if (elMasAnterior5 != null && elMenosAnterior5 != null) {
                    if (elMasAnterior5.classList.contains('hidden')) {
                        elMenosAnterior5.classList.add('hidden');
                        elMasAnterior5.classList.remove('hidden');
                    }
                }
            }

            vm.anteriorIdElementoRecursosMR = idElement;
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.VigenciaReportAvanceSeleccionada = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.VigenciaReportAvanceSeleccionada = vigencia;
                }
            }

            var compromiso = localStorage.getItem('Compromiso');
            var DetallCompromiso = localStorage.getItem('DetalleCompromiso');


            $("#DetalleRecursosCompromisos-" + DetallCompromiso).val(vm.ConvertirNumero((compromiso)));
        }

        vm.abrilNivelRecursosMRDD = function (idElement, vigencia) {
            vm.VigenciaReportAvanceSeleccionada = 0;
            if (vm.anteriorIdElementoRecursosMRDD != '' && vm.anteriorIdElementoRecursosMRDD != idElement) {
                vm.anteriorIdElementoRecursosMRDD = vm.anteriorIdElementoRecursosMRDD.replace("null", "");
                var elMasAnterior = document.getElementById(vm.anteriorIdElementoRecursosMRDD + '-mas');
                var elMenosAnterior = document.getElementById(vm.anteriorIdElementoRecursosMRDD + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            if (vm.anteriorIdElementoRecursosMR != '') {
                vm.anteriorIdElementoRecursosMR = vm.anteriorIdElementoRecursosMR.replace("null", "");
                var elMasAnterior4 = document.getElementById(vm.anteriorIdElementoRecursosMR + '-mas');
                var elMenosAnterior4 = document.getElementById(vm.anteriorIdElementoRecursosMR + '-menos');
                if (elMasAnterior4 != null && elMenosAnterior4 != null) {
                    if (elMasAnterior4.classList.contains('hidden')) {
                        elMenosAnterior4.classList.add('hidden');
                        elMasAnterior4.classList.remove('hidden');
                    }
                }
            }

            vm.anteriorIdElementoRecursosMRDD = idElement;
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.VigenciaMRDDReportAvanceSelecionada = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.VigenciaMRDDReportAvanceSelecionada = vigencia;
                }
            }
        }

        vm.verNombreCompleto = function (idElement, indexElement) {
            var elValidacion = document.getElementById(idElement + indexElement + '-val');
            var elCorto = document.getElementById(idElement + indexElement + '-min');
            var elCompleto = document.getElementById(idElement + indexElement + '-max');

            if (elCompleto.classList.contains('hidden')) {
                elValidacion.innerHTML = 'VER MENOS';
                elCorto.classList.add('hidden');
                elCompleto.classList.remove('hidden');
            } else {
                elValidacion.innerHTML = 'VER MÁS';
                elCorto.classList.remove('hidden');
                elCompleto.classList.add('hidden');
            }
        };

        vm.abrirMensajeQueEsEsto1 = function (mensaje) {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <br /> <span class='tituhori' >Resumen recursos y metas</span>", mensaje);
        };

        vm.abrirMensajeQueEsEsto2 = function (mensaje) {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <br /> <span class='tituhori' >Recursos</span>", mensaje);
        };

        vm.abrirMensajeQueEsEsto3 = function (mensaje) {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <br /> <span class='tituhori' >Metas</span>", mensaje);
        };

        function abrirMensajeArchivoRegionalizacion() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > Plantilla Carga Masiva Regionalización, </span><br /> <span class='tituhori'><ul><li>No se permite texto en la columna de 'En ajuste $' y 'Meta en ajuste $'</li><li>La columna 'En ajuste $' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)</li><li>La columna 'Meta en ajuste' acepta valores numéricos sin separador de mil y y dos decimales con separador coma(,)</li></ul></span>");
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 13;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 15;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 1);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 1) {
                        tamanioPermitido = n[0].length + 1;
                        event.target.value = n[0] + '.' + n[1].slice(0, 1);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            } else {
                if (tamanio > 15 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 15) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 15) {
                    event.preventDefault();
                }
            }
        };

        vm.validateSignoNegativo = function (event) {

            var value = event.target.value;
            var count = (value.match(/-/g) || []).length;
            if (count > 1) {
                value = value.slice(0, -1); // Eliminar el último signo negativo adicional
                event.target.value = value; // Asignar el valor corregido al campo de entrada
            }
        }

        vm.validateFormatNegative = function (event) {

            if ((event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            if (event.key == '.') {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 13;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 15;

                var n = String(newValue).split(".");

                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[1], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1) return;
                    if (spiltArray.length === 2) return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 4;
                        event.target.value = n[0] + ',' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            } else {

                const regex = /^-?\d{0,14}(,\d{0,1})?$/; // Expresión regular para permitir máximo dos decimales

                if (!regex.test(newValue)) {
                    event.preventDefault(); // Evitar la entrada de caracteres no válidos
                }

                if (tamanio > 15 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 15) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 15) {
                    event.preventDefault();
                }
            }
        };

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
                if (decimales > 4) {
                }
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
            }
        };

        vm.validarTamanioNegative = function (event) {

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
                tamanioPermitido = 15;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '.') return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length + 4;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            }
        };

        vm.actualizaObservacion = function (indicador) {
            angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                angular.forEach(vigencia.DetalleVigencia, function (mes) {
                    angular.forEach(indicador.PeriodosActivos, function (series) {
                        if (series.PeriodosPeriodicidadId == mes.PeriodoPeriodicidadId && series.Vigencia == vigencia.Vigencia) {
                            mes.Observacion = series.Observacion;
                        }
                    });
                });
            });
        };

        vm.actualizaValoresAnteriores = function (indicador) {
            var acumula = 0;
            angular.forEach(indicador.PeriodosActivos, function (series) {
                acumula = acumula + parseFloat(series.CantidadEjecutada);
                indicador.TotalAvanceReportado = parseFloat(acumula.toFixed(4));
            });

            angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                angular.forEach(vigencia.DetalleVigencia, function (mes) {
                    angular.forEach(indicador.PeriodosActivos, function (series) {
                        acumula = acumula + parseFloat(series.CantidadEjecutada);
                        if (series.PeriodosPeriodicidadId == mes.PeriodoPeriodicidadId && series.Vigencia == vigencia.Vigencia) {
                            mes.AvanceEjecutadoMes = series.CantidadEjecutada;
                            mes.Observacion = series.Observacion;
                        }
                    });
                });
            });


            var cantidadperiodos = 0;
            angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                cantidadperiodos = 0;
                angular.forEach(indicador.PeriodosActivos, function (series) {
                    if (series.Vigencia == vigencia.Vigencia) {
                        cantidadperiodos = series.PeriodosPeriodicidadId;
                    }
                });
                vigencia.cantidadperiodos = cantidadperiodos;
            });



            var acumulameses = 0;
            if (indicador.UnidadMedidaId == 15) {
                angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                    acumulameses = 0;
                    angular.forEach(vigencia.DetalleVigencia, function (mes) {
                        acumulameses = acumulameses + parseFloat(mes.AvanceEjecutadoMes);
                        if (vigencia.cantidadperiodos !== 0) {
                            vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4) / vigencia.cantidadperiodos);
                            vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4) / vigencia.cantidadperiodos);
                        }
                    });
                });
            }
            else {
                angular.forEach(indicador.ResumenCantidades, function (vigencia) {
                    acumulameses = 0;
                    angular.forEach(vigencia.DetalleVigencia, function (mes) {
                        acumulameses = acumulameses + parseFloat(mes.AvanceEjecutadoMes);
                        vigencia.TotalMetaEjecutada = parseFloat(acumulameses.toFixed(4));
                        vigencia.AcumuladoVigencia = parseFloat(acumulameses.toFixed(4));
                    });
                });
            }


        };

        /*Archivo*/
        vm.activarControles = function (evento) {
            switch (evento) {
                case "inicio":
                    $("#btnRegionalizacionValidarArchivo" + vm.FuenteId).attr('disabled', true);
                    $("#btnRegionalizacionLimpiarArchivo" + vm.FuenteId).attr('disabled', true);
                    $("#btnRegionalizacionArchivoSeleccionado" + vm.FuenteId).attr('disabled', true);
                    document.getElementById('fileRegionalizacion' + vm.FuenteId).value = "";
                    vm.nombrearchivo = "";
                    break;
                case "cargaarchivo":
                    $("#btnRegionalizacionValidarArchivo" + vm.FuenteId).attr('disabled', false);
                    $("#btnRegionalizacionLimpiarArchivo" + vm.FuenteId).attr('disabled', false);
                    $("#btnRegionalizacionArchivoSeleccionado" + vm.FuenteId).attr('disabled', true);
                    break;
                case "validado":
                    $("#btnRegionalizacionValidarArchivo" + vm.FuenteId).attr('disabled', false);
                    $("#btnRegionalizacionLimpiarArchivo" + vm.FuenteId).attr('disabled', false);
                    $("#btnRegionalizacionArchivoSeleccionado" + vm.FuenteId).attr('disabled', false);
                    break;
                default:
            }
        }

        vm.activarControles2 = function (evento, FuenteId) {
            switch (evento) {
                case "inicio":
                    $("#btnRegionalizacionValidarArchivo" + FuenteId).attr('disabled', true);
                    $("#btnRegionalizacionLimpiarArchivo" + FuenteId).attr('disabled', true);
                    $("#btnRegionalizacionArchivoSeleccionado" + FuenteId).attr('disabled', true);
                    document.getElementById('fileRegionalizacion' + FuenteId).value = "";
                    vm.nombrearchivo = "";
                    break;
                case "cargaarchivo":
                    $("#btnRegionalizacionValidarArchivo" + FuenteId).attr('disabled', false);
                    $("#btnRegionalizacionLimpiarArchivo" + FuenteId).attr('disabled', false);
                    $("#btnRegionalizacionArchivoSeleccionado" + FuenteId).attr('disabled', true);
                    break;
                case "validado":
                    $("#btnRegionalizacionValidarArchivo" + FuenteId).attr('disabled', false);
                    $("#btnRegionalizacionLimpiarArchivo" + FuenteId).attr('disabled', false);
                    $("#btnRegionalizacionArchivoSeleccionado" + FuenteId).attr('disabled', false);
                    break;
                default:
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

        function exportExcel() {

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
                        name: 'FuenteId', title: 'Fuente Id'
                    },
                    {
                        name: 'IdObjetivo', title: 'Objetivo Id'
                    },
                    {
                        name: 'ProductoId', title: 'Producto Id'
                    },
                    {
                        name: 'LocalizacionId ', title: 'Localizacion Id'
                    },
                    {
                        name: 'PeriodoProyectoId', title: 'PeriodoProyecto Id'
                    },
                    {
                        name: 'PeriodosPeriodicidadId', title: 'Periodos Periodicidad Id'
                    },
                    {
                        name: 'Fuente', title: 'Fuente'
                    },
                    {
                        name: 'Etapa', title: 'Etapa'
                    },
                    {
                        name: 'Objetivo', title: 'Objetivo'
                    },
                    {
                        name: 'Producto', title: 'Producto'
                    },
                    {
                        name: 'CostoProductoVigencia', title: 'Costo Producto Vigencia $'
                    },
                    {
                        name: 'Localizacion', title: 'Localizacion'
                    },
                    {
                        name: 'Vigencia', title: 'Vigencia'
                    },
                    {
                        name: 'PeriodoProyecto', title: 'PeriodoProyecto'
                    },
                    {
                        name: 'Compromisos', title: 'Compromisos $'
                    },
                    {
                        name: 'Obligaciones', title: 'Obligaciones $'
                    },
                    {
                        name: 'Pagos', title: 'Pagos $'
                    },
                    {
                        name: 'ObservacionesRecursos', title: 'Observaciones Recursos'
                    },
                    {
                        name: 'AvanceMeta', title: 'Avance Meta'
                    },
                    {
                        name: 'ObservacionesMeta', title: 'Observaciones Meta'
                    }
                ];

                let colNames = columns.map(function (item) {
                    return item.title;
                })

                var wb = XLSX.utils.book_new();

                wb.Props = {
                    Title: "Plantilla Seguimiento Regionalizacion",
                    Subject: "PIIP",
                    Author: "PIIP",
                    CreatedDate: new Date().getDate()
                };

                wb.SheetNames.push("Hoja Plantilla");

                const header = colNames;
                const data = [];

                vm.modelo.Fuentes.forEach(fuente => {
                    fuente.Objetivos.forEach(objetivo => {
                        objetivo.Productos.forEach(producto => {

                            producto.Localizaciones.forEach(localizacion => {

                                var localiza = "";
                                if (localizacion.Municipio == null) {
                                    localiza = localizacion.Departamento;
                                }
                                else {
                                    localiza = localizacion.Departamento + " - " + localizacion.Municipio;
                                }

                                localizacion.RecursosPeriodosActivos.forEach(vigencia => {
                                    data.push({
                                        FuenteId: fuente.FuenteId,
                                        IdObjetivo: objetivo.ObjetivoEspecificoId,
                                        ProductoId: producto.ProductoId,
                                        LocalizacionId: localizacion.LocalizacionId,
                                        PeriodoProyectoId: vigencia.PeriodoProyectoId,
                                        PeriodosPeriodicidadId: vigencia.PeriodosPeriodicidadId,
                                        Fuente: fuente.NombreFuente,
                                        Etapa: producto.Etapa,
                                        Objetivo: objetivo.ObjetivoEspecifico,
                                        NumeroProducto: producto.NumeroProducto,
                                        Producto: producto.NombreProducto,
                                        CostoProductoVigencia: producto.CostoProducto,
                                        Localizacion: localiza,
                                        Vigencia: vigencia.Vigencia,
                                        PeriodoProyecto: vigencia.Mes,
                                        Compromisos: vigencia.Compromisos,
                                        Obligaciones: vigencia.Obligaciones,
                                        Pagos: vigencia.Pagos,
                                        ObservacionesRecursos: vigencia.ObservacionRecurso,
                                        AvanceMes: localizacion.MetasPeriodosActivos.find(item => item.Vigencia === vigencia.Vigencia && item.Mes === vigencia.Mes).AvanceMes,
                                        ObservacionesMetas: localizacion.MetasPeriodosActivos.find(item => item.Vigencia === vigencia.Vigencia && item.Mes === vigencia.Mes).ObservacionMeta
                                    });
                                });
                            });
                        });
                    });
                });

                const worksheet = XLSX.utils.json_to_sheet(data, {
                    header: ["FuenteId", "IdObjetivo", "ProductoId", "LocalizacionId", "PeriodoProyectoId", "PeriodosPeriodicidadId", "Fuente", "Etapa", "Objetivo",
                        "NumeroProducto", "Producto", "CostoProductoVigencia", "Localizacion", "Vigencia",
                        "PeriodoProyecto", "Compromisos", "Obligaciones", "Pagos", "ObservacionesRecursos", "AvanceMes", "ObservacionesMetas"]
                });

                for (let col of [11]) {
                    formatColumn(worksheet, col, "#,##")
                }
                for (let col of [15]) {
                    formatColumn(worksheet, col, "#,##")
                }
                for (let col of [16]) {
                    formatColumn(worksheet, col, "#,##")
                }
                for (let col of [17]) {
                    formatColumn(worksheet, col, "#,##")
                }
                for (let col of [19]) {
                    formatColumn2(worksheet, col, "#,####")
                }

                /* hide second column */
                worksheet['!cols'] = [];
                worksheet['!cols'][0] = { hidden: true };
                worksheet['!cols'][1] = { hidden: true };
                worksheet['!cols'][2] = { hidden: true };
                worksheet['!cols'][3] = { hidden: true };
                worksheet['!cols'][4] = { hidden: true };
                worksheet['!cols'][5] = { hidden: true };

                wb.Sheets["Hoja Plantilla"] = worksheet;

                var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
                //saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaAjusteRegionalizacion.xlsx');
                saveAs(new Blob([s2ab(wbout)], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8" }), 'PlantillaSeguimientoRegionalizacion.xlsx');
            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            }, false, false, "Este archivo es compatible con Office 365");
        }

        function formatColumn(worksheet, col) {
            var fmtnumero2 = "#,##0.00";// "##,##";
            var fmtnumero4 = "#,##0.0000";//"#,####";
            const range = XLSX.utils.decode_range(worksheet['!ref'])
            for (let row = range.s.r + 1; row <= range.e.r; ++row) {
                const ref = XLSX.utils.encode_cell({ r: row, c: col })
                if (ref != "K0" || ref != "L0" || ref != "N0" || ref != "O0" /*|| ref != "R0" || ref != "S0"*/) {
                    worksheet[ref].z = fmtnumero2;
                    worksheet[ref].t = 'n';
                }
                if (ref === "R1") {
                    worksheet[ref].z = fmtnumero2;
                    worksheet[ref].t = 'n';
                }
                if (ref === "S1") {
                    worksheet[ref].z = fmtnumero4;
                    worksheet[ref].t = 'n';
                }
            }
        }

        function formatColumn2(worksheet, col) {
            var fmtnumero2 = "#,##0.0000";// "##,##";
            var fmtnumero4 = "#,##0.0000";//"#,####";
            const range = XLSX.utils.decode_range(worksheet['!ref'])
            for (let row = range.s.r + 1; row <= range.e.r; ++row) {
                const ref = XLSX.utils.encode_cell({ r: row, c: col })
                if (ref != "K0" || ref != "L0" || ref != "N0" || ref != "O0" /*|| ref != "R0" || ref != "S0"*/) {
                    worksheet[ref].z = fmtnumero2;
                    worksheet[ref].t = 'n';
                }
                if (ref === "R1") {
                    worksheet[ref].z = fmtnumero2;
                    worksheet[ref].t = 'n';
                }
                if (ref === "S1") {
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

        $scope.fileNameChanged = function (input) {
            if (input.files.length == 1) {
                vm.nombrearchivo = input.files[0].name;
                document.getElementById('regionalizacionnombrearchivo2' + vm.FuenteId).textContent = input.files[0].name;
                vm.activarControles('cargaarchivo');
            }
            else {
                vm.filename = input.files.length + " archivos"
                vm.activarControles('inicio');
            }
        }

        $scope.ChangeSet = function () {
            if (vm.nombrearchivo == "") {
                vm.activarControles('inicio');
            }
        };

        function adjuntarArchivo(FuenteId) {
            vm.FuenteId = FuenteId;
            document.getElementById('fileRegionalizacion' + FuenteId).value = "";
            document.getElementById('fileRegionalizacion' + FuenteId).click();
        }

        function limpiarArchivo(FuenteId) {
            $scope.files = [];
            document.getElementById('fileRegionalizacion' + FuenteId).value = "";
            vm.activarControles2('inicio', FuenteId);
            vm.nombrearchivo = "";
            document.getElementById('regionalizacionnombrearchivo2' + FuenteId).textContent = "";
        }

        function limpiaNumero(valor) {
            return valor.toLocaleString().split(",")[0].toString().replaceAll(".", "");
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

        function validarArchivo() {

            var resultado = true;
            var enajuste = 0;
            var metaEnAjuste = 0;
            vm.FuenteArchivo = [];
            if (document.getElementById("fileRegionalizacion" + vm.FuenteId).files.length > 0) {

                let file = document.getElementById("fileRegionalizacion" + vm.FuenteId).files[0];
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

                                    if (item["PeriodoProyectoId"] == undefined) {
                                        utilidades.mensajeError("La columna PeriodoProyectoId no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["PeriodoProyectoId"])) {
                                        utilidades.mensajeError("El valor PeriodoProyectoId " + item["PeriodoProyectoId"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["PeriodoProyecto"] == undefined) {
                                        utilidades.mensajeError("La columna PeriodoProyecto no trae valor!");
                                        return false;
                                    }

                                    if (item["AvanceMes"] == undefined) {
                                        utilidades.mensajeError("La columna 'Avance Mes' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else if (!ValidarDecimal(item["AvanceMes"].toString(), 4)) {
                                        utilidades.mensajeError("Valor no valido 'AvanceMes' " + item["AvanceMes"] + ". La columna 'Avance Mes' acepta valores numéricos sin separador de mil y y cuatro decimales con separador coma(,)");
                                        return false;
                                    }
                                    else {
                                        metaEnAjuste = item["AvanceMes"];
                                    }

                                    if (item["Compromisos"] == undefined) {
                                        utilidades.mensajeError("La columna Compromisos no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["Compromisos"])) {
                                        utilidades.mensajeError("No se permite texto en la columna de Compromisos $ y Obligaciones $, Pagos $. La columna Compromisos $ acepta valores numéricos sin separador de mil y dos decimales con separador coma(,). El valor de Compromisos " + item["Compromisos"] + " no es númerico!");
                                        return false;
                                    }


                                    if (item["Obligaciones"] == undefined) {
                                        utilidades.mensajeError("La columna Obligaciones no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["Obligaciones"])) {
                                        utilidades.mensajeError("No se permite texto en la columna de Compromisos $ y Obligaciones $, Pagos $. La columna Obligaciones $ acepta valores numéricos sin separador de mil y dos decimales con separador coma(,). El valor de Obligaciones " + item["Obligaciones"] + " no es númerico!");
                                        return false;
                                    }

                                    if (item["Pagos"] == undefined) {
                                        utilidades.mensajeError("La columna Pagos no trae valor!");
                                        return false;
                                    }
                                    else if (!ValidaSiEsNumero(item["Pagos"])) {
                                        utilidades.mensajeError("No se permite texto en la columna de Compromisos $ y Obligaciones $, Pagos $. La columna Pagos $ acepta valores numéricos sin separador de mil y dos decimales con separador coma(,). El valor de Pagos " + item["Pagos"] + " no es númerico!");
                                        return false;
                                    }

                                    var valoresarchivo = {
                                        FuenteId: item["FuenteId"],
                                        Fuente: item["Fuente"],
                                        Etapa: item["Etapa"],
                                        IdObjetivo: item["IdObjetivo"],
                                        Objetivo: item["Objetivo"],
                                        ProductoId: item["ProductoId"],
                                        NumeroProducto: item["NumeroProducto"],
                                        Producto: item["Producto"],
                                        CostoProductoVigencia: item["CostoProductoVigencia"],
                                        LocalizacionId: item["LocalizacionId"],
                                        Localizacion: item["Localizacion"],
                                        Vigencia: item["Vigencia"],
                                        PeriodoProyectoId: item["PeriodoProyectoId"],
                                        PeriodosPeriodicidadId: item["PeriodosPeriodicidadId"],
                                        PeriodoProyecto: item["PeriodoProyecto"],
                                        Compromisos: item["Compromisos"],
                                        Obligaciones: item["Obligaciones"],
                                        Pagos: item["Pagos"],
                                        ObservacionesRecursos: item["ObservacionesRecursos"],
                                        AvanceMes: item["AvanceMes"],
                                        ObservacionesMetas: item["ObservacionesMetas"]
                                    };
                                    vm.FuenteArchivo.push(valoresarchivo);

                                });

                                if (resultado.indexOf(false) == -1) {
                                    if (!ValidarRegistros()) {
                                        utilidades.mensajeError("El Archivo fue modificado en datos y/o estructura y no se puede procesar.");
                                        vm.activarControles('inicio');
                                    }
                                    else {
                                        vm.activarControles('validado');
                                        utilidades.mensajeSuccess("Proceda a cargar el archivo para que quede registrado en el sistema", false, false, false, "Validación de carga exitosa.");
                                    }
                                }
                                else {
                                    vm.activarControles('inicio');
                                    vm.FuenteArchivo = [];
                                }
                            };
                            reader.readAsBinaryString(file);
                        }
                    }
                }
            }
        }

        function ValidarRegistros() {
            var aFuentes = [];
            var aObjetivos = [];
            var aProductos = [];
            var aLocalizaciones = [];
            var aPeriodoProyecto = [];
            var aVigencias = [];
            var contFuente = [];


            var existeFuente = 0;
            var existeProducto = 0;
            var existeLocaliacion = 0;
            var existeVigencia = 0;
            var existePeriodoProyecto = 0;
            var CantidadRegistros = 0;

            vm.listaDatos.Fuentes.forEach(fuente => {
                aFuentes.push(fuente.FuenteId);
                fuente.Objetivos.forEach(Objetivo => {
                    aObjetivos.push(Objetivo.ObjetivoEspecificoId);
                    Objetivo.Productos.forEach(producto => {
                        aProductos.push(producto.ProductoId);
                    });
                });

                CantidadRegistros = CantidadRegistros + 1;
            });

            vm.listaDatos.ListaLocalizacion.forEach(lo => {
                aLocalizaciones.push(lo.LocalizacionId);
            });

            vm.listaDatos.ListaPeriodosActivos.forEach(lpa => {
                aPeriodoProyecto.push(lpa.PeriodosPeriodicidadId);
                aVigencias.push(lpa.Vigencia);
            });

            vm.FuenteArchivo.forEach(fa => {

                if (aFuentes.indexOf(fa.FuenteId) == -1) {
                    existeFuente = existeFuente + 1;
                }
                if (aProductos.indexOf(fa.ProductoId) == -1) {
                    existeProducto = existeProducto + 1;
                }
                if (aLocalizaciones.indexOf(fa.LocalizacionId) == -1) {
                    existeLocaliacion = existeLocaliacion + 1;
                }
                if (aPeriodoProyecto.indexOf(fa.PeriodosPeriodicidadId) == -1) {
                    existePeriodoProyecto = existePeriodoProyecto + 1;
                }
                if (aVigencias.indexOf(fa.Vigencia) == -1) {
                    existeVigencia = existeVigencia + 1;
                }
            });

            if (existeFuente > 0 || existeProducto > 0 || existeLocaliacion > 0 || existeVigencia > 0 || existePeriodoProyecto > 0) {
                return false;
            }
            else {
                if (CantidadRegistros != aFuentes.length) {
                    return false;
                }
                else {
                    return true;
                }
            }
        }

        function GuardarArchivo(cargamasiva) {
            if (cargamasiva)
                vm.activarControles('inicio');

            var _Objetivos = [];
            var _localizacion = [];
            var _local = [];
            var _fuente = [];
            var _recursosPeriodosActivos = [];
            var _metasPeriodosActivos = [];
            var _metasPeriodosActivos = [];
            var _DetalleVigencia = [];
            var _Productos = [];
            var _Prod = [];
            var _obj = [];

            ////INICIO DEL FOREACH
            vm.FuenteArchivo.forEach(fa => {

                const existeObjetivo = _obj.some(elemento => elemento.ObjetivoEspecificoId === fa.IdObjetivo && elemento.FuenteId === fa.FuenteId);

                if (!existeObjetivo) {
                    _obj.push({
                        FuenteId: fa.FuenteId,
                        ObjetivoEspecificoId: fa.IdObjetivo
                    }
                    );
                }

                const existeElementoMes = _recursosPeriodosActivos.some(elemento => elemento.Mes === fa.PeriodoProyecto && elemento.ProductoId === fa.ProductoId && elemento.FuenteId === fa.FuenteId && elemento.LocalizacionId === fa.LocalizacionId);
                const existeProd = _Productos.some(elemento => elemento.ProductoId === fa.ProductoId);

                if (!existeElementoMes) {

                    _recursosPeriodosActivos.push({
                        Vigencia: fa.Vigencia,
                        PeriodoProyectoId: fa.PeriodoProyectoId,
                        PeriodosPeriodicidadId: fa.PeriodosPeriodicidadId,
                        Mes: fa.PeriodoProyecto,
                        FechaDesde: "",
                        FechaHasta: "",
                        ObservacionRecurso: fa.ObservacionesRecursos === undefined ? "" : fa.ObservacionesRecursos,
                        InicialF: "0,00",
                        ValorVigenteF: "0,00",
                        VigenteDelMesF: "0,00",
                        CompromisosF: "0,00",
                        ObligacionesF: "0,00",
                        PagosF: "0,00",
                        Inicial: 0,
                        ValorVigente: 0,
                        VigenteDelMes: 0,
                        Compromisos: fa.Compromisos,
                        Obligaciones: fa.Obligaciones,
                        Pagos: fa.Pagos,
                        ProductoId: fa.ProductoId,
                        LocalizacionId: fa.LocalizacionId,
                        FuenteId: fa.FuenteId
                    });
                }

                const existeElementop = _metasPeriodosActivos.some(elemento => elemento.Mes === fa.PeriodoProyecto && elemento.ProductoId === fa.ProductoId && elemento.FuenteId === fa.FuenteId && elemento.LocalizacionId === fa.LocalizacionId);

                if (!existeElementop) {
                    _metasPeriodosActivos.push({
                        Vigencia: fa.Vigencia,
                        PeriodoProyectoId: fa.PeriodoProyectoId,
                        PeriodosPeriodicidadId: fa.PeriodosPeriodicidadId,
                        Mes: fa.PeriodoProyecto,
                        FechaDesde: "",
                        FechaHasta: "",
                        ObservacionMeta: fa.ObservacionesMetas === undefined ? "" : fa.ObservacionesMetas,
                        AcumuladoMesAnteriorF: "0,0000",
                        AvanceMesF: "0,0000",
                        AcumuladoMesAnterior: 0,
                        AvanceMes: fa.AvanceMes,
                        ProductoId: fa.ProductoId,
                        LocalizacionId: fa.LocalizacionId,
                        FuenteId: fa.FuenteId
                    });
                }

                const existeElementov = _DetalleVigencia.some(elemento => elemento.Mes === fa.PeriodoProyecto && elemento.ProductoId === fa.ProductoId);

                if (!existeElementov) {
                    _DetalleVigencia.push(
                        {
                            PeriodoPeriodicidadId: fa.PeriodosPeriodicidadId,
                            Mes: fa.PeriodoProyecto,
                            ValorVigenteFirmeF: "0,00",
                            ValorVigenteMesF: "0,00",
                            ValorCompromisosF: "0,00",
                            ValorObligacionesF: "0,00",
                            ValorPagosF: "0,00",
                            AvanceMesF: "0,00",
                            ValorVigenteFirme: 0,
                            ValorVigenteMes: 0,
                            ValorCompromisos: fa.Compromisos,
                            ValorObligaciones: fa.Obligaciones,
                            ValorPagos: fa.Pagos,
                            AvanceMes: fa.AvanceMes,
                            ObservacionRecurso: fa.ObservacionesRecursos === undefined ? "" : fa.ObservacionesRecursos,
                            ObservacionMeta: fa.ObservacionesMetas === undefined ? "" : fa.ObservacionesMetas,
                            ProductoId: fa.ProductoId,
                            LocalizacionId: fa.LocalizacionId,
                            FuenteId: fa.FuenteId
                        });
                }

                const existeElementol = _local.some(elemento => elemento.localId === fa.LocalizacionId && elemento.ProductoId === fa.ProductoId && elemento.FuenteId === fa.FuenteId);

                if (!existeElementol) {
                    _local.push({
                        localId: fa.LocalizacionId,
                        ProductoId: fa.ProductoId,
                        Departamento: fa.Localizacion,
                        FuenteId: fa.FuenteId
                    }
                    );
                }

                const existeElementopx = _Prod.some(elemento => elemento.ProductoId === fa.ProductoId && elemento.ObjetivoEspecificoId === fa.IdObjetivo && elemento.FuenteId === fa.FuenteId);

                if (!existeElementopx) {
                    _Prod.push({
                        FuenteId: fa.FuenteId,
                        ObjetivoEspecificoId: fa.IdObjetivo,
                        ProductoId: fa.ProductoId,
                        NumeroProducto: fa.NumeroProducto
                    }
                    );
                }
            });
            ///FIN DEL FOREACH

            _local.forEach(fa => {
                _localizacion.push({
                    LocalizacionId: fa.localId,
                    Departamento: fa.Localizacion,
                    Municipio: fa.Departamento,
                    TipoAgrupacion: null,
                    Agrupacion: null,
                    RecursosPeriodosActivos: _recursosPeriodosActivos.filter(function (registro) { return registro.ProductoId === fa.ProductoId && registro.LocalizacionId === fa.localId && registro.FuenteId === fa.FuenteId; }),
                    MetasPeriodosActivos: _metasPeriodosActivos.filter(function (registro) { return registro.ProductoId === fa.ProductoId && registro.LocalizacionId === fa.localId && registro.FuenteId === fa.FuenteId; }),
                    ProductoId: fa.ProductoId,
                    FuenteId: fa.FuenteId
                }
                );
            });

            _Prod.forEach(pp => {
                _Productos.push(
                    {
                        FuenteId: pp.FuenteId,
                        ObjetivoEspecificoId: pp.ObjetivoEspecificoId,
                        ProductoId: pp.ProductoId,
                        NumeroProducto: pp.NumeroProducto,
                        Localizaciones: _localizacion.filter(function (registro) { return registro.ProductoId === pp.ProductoId && registro.FuenteId === pp.FuenteId; })
                    }
                );
            });

            _obj.forEach(ob => {
                _Objetivos.push(
                    {
                        FuenteId: ob.FuenteId,
                        ObjetivoEspecificoId: ob.ObjetivoEspecificoId,
                        Productos: _Productos.filter(function (registro) { return registro.ObjetivoEspecificoId === ob.ObjetivoEspecificoId && registro.FuenteId === ob.FuenteId; })
                    }
                );
            });

            var _fuenteId = vm.FuenteArchivo.map(item => item.FuenteId).filter((value, index, self) => self.indexOf(value) === index);
            var _nombreFuente = vm.FuenteArchivo.map(item => item.Fuente).filter((value, index, self) => self.indexOf(value) === index);

            for (var i = 0; i < _fuenteId.length; i++) {
                _fuente.push({
                    FuenteId: _fuenteId[i],
                    NombreFuente: _nombreFuente[i],
                    Objetivos: _Objetivos.filter(function (registro) { return registro.FuenteId === _fuenteId[i]; }),
                    HabilitaEditarIndicador: false
                });
            }

            //Guarda la información del archivo
            GuardarMasivo(_fuente);
        }


        /*Fin Archivo*/



        ///*Fin Archivo*/

        /*------------------------------AQUI TERMINA PARA LOGICA DEL NEGOCIO ------------------------------------------------------*/

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
                'regionalizacionavanceregionaliza': true
            };
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.refreshComponente(true, null);
            }
        };

        /*--------------------- Validaciones ---------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            //debugger;
            if (errores != undefined) {
                var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
                vm.erroresActivos = erroresFiltrados.erroresActivos;

                vm.ejecutarErrores();

                var isValid = (vm.erroresActivos.length <= 0);
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            } else {
                vm.limpiarErrores();
            }
        };

        vm.getErrorDES001 = function ({ error, descripcion, data }) {

            var fuente = data.substring(0, 4);
            var divisiones = data.split("-");

            $("#imgcptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).attr('disabled', false);
            $("#imgcptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).fadeIn();
            $("#icocptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).attr('disabled', false);
            $("#icocptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).fadeIn();

            $("#errortottalmsn-" + fuente).attr('disabled', false);
            $("#errortottal-" + fuente).fadeIn();
            $("#errortottalmsn-" + fuente).fadeIn();

            var Errormsn = document.getElementById("errortottalmsn-" + fuente);
            if (Errormsn != undefined) {
                if (!Errormsn.innerHTML.includes(divisiones[3])) {
                    Errormsn.innerHTML += '<span>' + divisiones[3] + "; " + "</span>";
                }
            }
        }

        vm.getErrorDES002 = function ({ error, descripcion, data }) {

            var fuente = data.substring(0, 4);
            var divisiones = data.split("-");

            $("#imgcptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).attr('disabled', false);
            $("#imgcptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).fadeIn();
            $("#icocptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).attr('disabled', false);
            $("#icocptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).fadeIn();

            $("#errortottalmsn-" + fuente).attr('disabled', false);
            $("#errortottal-" + fuente).fadeIn();
            $("#errortottalmsn-" + fuente).fadeIn();

            var Errormsn = document.getElementById("errortottalmsn-" + fuente);
            if (Errormsn != undefined) {
                if (!Errormsn.innerHTML.includes(divisiones[3])) {
                    Errormsn.innerHTML += '<span>' + divisiones[3] + "; " + "</span>";
                }
            }
        }

        vm.getErrorDES003 = function ({ error, descripcion, data }) {

            var fuente = data.substring(0, 4);
            var divisiones = data.split("-");

            $("#imgcptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).attr('disabled', false);
            $("#imgcptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).fadeIn();
            $("#icocptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).attr('disabled', false);
            $("#icocptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).fadeIn();

            $("#errortottalmsn-" + fuente).attr('disabled', false);
            $("#errortottal-" + fuente).fadeIn();
            $("#errortottalmsn-" + fuente).fadeIn();

            var Errormsn = document.getElementById("errortottalmsn-" + fuente);
            if (Errormsn != undefined) {
                if (!Errormsn.innerHTML.includes(divisiones[3])) {
                    Errormsn.innerHTML += '<span>' + divisiones[3] + "; " + "</span>";
                }
            }
        }

        vm.getErrorDES004 = function ({ error, descripcion, data }) {

            var fuente = data.substring(0, 4);
            var divisiones = data.split("-");

            $("#imgcptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).attr('disabled', false);
            $("#imgcptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).fadeIn();
            $("#icocptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).attr('disabled', false);
            $("#icocptj-" + divisiones[0] + "-" + divisiones[1] + "-" + divisiones[2]).fadeIn();

            $("#errortottalmsn-" + fuente).attr('disabled', false);
            $("#errortottal-" + fuente).fadeIn();
            $("#errortottalmsn-" + fuente).fadeIn();

            var Errormsn = document.getElementById("errortottalmsn-" + fuente);
            if (Errormsn != undefined) {
                if (!Errormsn.innerHTML.includes(divisiones[3])) {
                    Errormsn.innerHTML += '<span>' + divisiones[3] + "; " + "</span>";
                }
            }

        }

        vm.getErrorDES005 = function ({ error, descripcion, data }) {

            var fuente = data.substring(0, 4);
            var divisiones = data.split("-");

            $("#imgcptj-" + divisiones[0] + "-" + divisiones[3] + "-" + divisiones[4]).attr('disabled', false);
            $("#imgcptj-" + divisiones[0] + "-" + divisiones[3] + "-" + divisiones[4]).fadeIn();
            $("#icocptj-" + divisiones[0] + "-" + divisiones[3] + "-" + divisiones[4]).attr('disabled', false);
            $("#icocptj-" + divisiones[0] + "-" + divisiones[4] + "-" + divisiones[4]).fadeIn();

            $("#errortottalmsn-" + fuente).attr('disabled', false);
            $("#errortottal-" + fuente).fadeIn();
            $("#errortottalmsn-" + fuente).fadeIn();

            var Errormsn = document.getElementById("errortottalmsn-" + fuente);
            if (Errormsn != undefined) {
                if (!Errormsn.innerHTML.includes(divisiones[5].substring(0, 10))) {
                    Errormsn.innerHTML += '<span>' + divisiones[5] + "; " + "</span>";
                }
            }

        }

        vm.ejecutarErrores = function () {
            vm.limpiarErrores();
            vm.erroresActivos.forEach(p => {
                if (vm.errores[p.Error] != undefined) {
                    vm.errores[p.Error]({
                        error: p.Error,
                        descripcion: p.Descripcion,
                        data: p.Data
                    });
                }
            });
        };

        vm.errores = {
            'AVREG001': vm.getErrorDES001,
            'AVREG002': vm.getErrorDES002,
            'AVREG003': vm.getErrorDES003,
            'AVREG004': vm.getErrorDES004,
            'AVREG005': vm.getErrorDES005
        }

        vm.ejecutarErrores = function () {
            vm.limpiarErrores();
            vm.erroresActivos.forEach(p => {
                if (vm.errores[p.Error] != undefined) {
                    vm.errores[p.Error]({
                        error: p.Error,
                        descripcion: '',
                        data: p.Descripcion
                    });
                }
            });
        }

        vm.limpiarErrores = function () {
            var listadoErrores = document.getElementsByClassName("messagealerttableDNP");
            var listadoErroresContainer = document.getElementsByClassName("errores-contenedor");
            vm.mensajevalidacion = "";

            $(".iconoErrorDNPError").attr('disabled', true);
            $(".iconoErrorDNPError").fadeOut();

            var idSpanAlertComponent = document.getElementById("alert-regionalizacion");
            var idSpanAlertComponent1 = document.getElementById("alert-regionalizacionavanceregionaliza");

            idSpanAlertComponent.classList.remove("ico-advertencia");
            idSpanAlertComponent1.classList.remove("ico-advertencia");

            for (var i = 0; i < listadoErroresContainer.length; i++) {
                if (!listadoErroresContainer[i].classList.contains("d-none")) {
                    listadoErroresContainer[i].classList.add("d-none");

                }
            }

            for (var i = 0; i < listadoErrores.length; i++) {
                if (!listadoErrores[i].classList.contains("d-none")) {
                    listadoErrores[i].classList.add("d-none")
                    listadoErrores[i].innerHTML = ''
                }
            }
        }

        // TODO: Validar que se esté usando
        vm.notificarRefrescoFuentes = null;
        // TODO: Validar que se esté usando
        //vm.notificarRefresco = function (handler, nombreComponente) {
        //	if (nombreComponente == "regionalizacionresumenregionaliza") {
        //		vm.notificarRefrescoFuentes = handler;
        //	}
        //};
    }

    angular.module('backbone').component('reporteAvanceRegionalizacion', {
        templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/reporteRegionalizacion/componentes/reporteAvanceRegionalizacion/reporteAvanceRegionalizacionCap.html",
        controller: reporteAvanceRegionalizacionCapController,
        controllerAs: "vm",
        bindings: {
            bpin: '@',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacioncambios: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            refreshregionalizacion: '=',
        }
    });

})();