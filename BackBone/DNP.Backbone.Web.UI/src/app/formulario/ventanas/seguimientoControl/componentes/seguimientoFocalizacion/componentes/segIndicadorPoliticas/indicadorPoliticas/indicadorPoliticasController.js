(function () {
    'use strict';

    indicadorPoliticasController.$inject = [
        '$scope',
        '$sessionStorage',
        '$uibModal',
        '$window',
        'utilidades',
        'desagregarEdtServicio',
        'indicadorPoliticasServicio',
        'utilsValidacionSeccionCapitulosServicio',
        '$timeout'
    ];

    function indicadorPoliticasController(
        $scope,
        $sessionStorage,
        $uibModal,
        $window,
        utilidades,
        desagregarEdtServicio,
        indicadorPoliticasServicio,
        utilsValidacionSeccionCapitulosServicio,
        $timeout

    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "segfocalizacionsegindipoliticas";
        vm.componentesRefresh = [
            "segfocalizacionsegfocpoliticas"
        ];
        vm.soloLectura = $sessionStorage.soloLectura;
        vm.habilitaEditarAvance = habilitaEditarAvance;
        vm.recorrerObjetivosNumber = recorrerObjetivosNumber;
        vm.obtenerIndicadores;
        vm.refreshIndicador;
        vm.listadoIndicadoresPolitica = [];
        vm.unidadesMedida = [];
        vm.periodos = [];
        vm.iFuenteS = 0;
        vm.iCategoriaS = 0;
        vm.iPoliticaS = 0;
        vm.iIndicadorS = 0;
        vm.iLocalizacionS = 0;
        vm.iVigenciaS = 0;
        vm.anteriorIdElemento = '';
        vm.anteriorIdElementoIndicador = '';
        vm.anteriorIdElementoLocalizacion = '';
        vm.anteriorIdElementoVigencia = '';
        vm.init = function () {
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.refreshComponente();
            vm.obtenerPeriodos();
        };

        vm.ConsultarDetallePolitica = function (politicaId) {
            var panelContainer = document.getElementsByClassName("panel-contenedor");
            for (var i = 0; i < panelContainer.length; i++) {
                if (!panelContainer[i].classList.contains("d-none")) {
                    panelContainer[i].classList.add("d-none");

                }
            }
            var politicaHT = document.getElementById("politica-panel-" + politicaId)
            if (politicaHT != undefined && politicaHT != null) {
                politicaHT.classList.remove("d-none");
                politicaHT.classList.remove("d-none");
            }
        }

        vm.refreshIndicador = function () {
            vm.obtenerIndicadores();
        }

        vm.abrirGantt = function () {
            $window.open(backboneURL + "/reporteGantt?BPIN=" + $sessionStorage.bpinProductos, '_blank');
        }

        vm.refreshComponente = function () {
            vm.obtenerIndicadores();
        }

        vm.reloadEdicion = function (dataSeleccionada) {
            vm.abrirArbol(dataSeleccionada.DataAgregarModal);
            vm.guardadoevent({ nombreComponenteHijo: this.nombreComponente })
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            console.log("vm.notificacionCambiosCapitulos Indicadores de Politicas", nombreCapituloHijo)
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                $timeout(function () {
                    var el = document.getElementById('refresh');
                    angular.element(el).triggerHandler('click');
                }, 0);

            }
        }

        vm.obtenerIndicadores = function () {
            return indicadorPoliticasServicio.obtenerIndicadores({ bpin: $sessionStorage.idObjetoNegocio })
                .then(resultado => {
                    console.log("desagregarEdtServicio de indicadorPoliticasController", resultado);
                    if (resultado.data != null) {
                        var data = resultado.data["Politicas"];
                        vm.listadoIndicadoresPolitica = data;
                        setTimeout(function () {
                            if (vm.listadoIndicadoresPolitica.length > 0) {
                                vm.ConsultarDetallePolitica(vm.listadoIndicadoresPolitica[0].PoliticaId);
                            }
                        }, 500);

                    }
                });
        }

        vm.obtenerPeriodos = function () {
            vm.periodos = [];
            indicadorPoliticasServicio.obtenerCalendarioPeriodo({ bpin: $sessionStorage.bpinProductos })
                .then(resultado => {
                    console.log(resultado);
                    vm.periodos = resultado.data;
                });
        }


        /*--------------------- Comportamientos collapse y contenido ---------------------*/

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
        }

        vm.abrilNivel = function (idElement, fuente, categoria, producto) {
            vm.iIndicadorS = 0;
            vm.iLocalizacionS = 0;
            vm.iVigenciaS = 0;

            if (vm.anteriorIdElemento != '' && vm.anteriorIdElemento != idElement) {
                vm.anteriorIdElemento = vm.anteriorIdElemento.replace("null", "");
                var elMasAnterior = document.getElementById(vm.anteriorIdElemento + '-mas');
                var elMenosAnterior = document.getElementById(vm.anteriorIdElemento + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            if (vm.anteriorIdElementoIndicador != '' && vm.anteriorIdElementoIndicador != idElement) {
                vm.anteriorIdElementoIndicador = vm.anteriorIdElementoIndicador.replace("null", "");
                var elMasAnterior = document.getElementById(vm.anteriorIdElementoIndicador + '-mas');
                var elMenosAnterior = document.getElementById(vm.anteriorIdElementoIndicador + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

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

            if (vm.anteriorIdElementoVigencia != '' && vm.anteriorIdElementoVigencia != idElement) {
                vm.anteriorIdElementoVigencia = vm.anteriorIdElementoVigencia.replace("null", "");
                var elMasAnterior = document.getElementById(vm.anteriorIdElementoVigencia + '-mas');
                var elMenosAnterior = document.getElementById(vm.anteriorIdElementoVigencia + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            vm.anteriorIdElemento = idElement;
            idElement = idElement.replace("null", "");
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.iFuenteS = 0;
                    vm.iCategoriaS = 0;
                    vm.iPoliticaS = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.iFuenteS = fuente;
                    vm.iCategoriaS = categoria;
                    vm.iPoliticaS = producto;
                }
            }
        }

        vm.abrilNiveliIndicador = function (idElement, indicador) {
            vm.iLocalizacionS = 0;
            vm.iVigenciaS = 0;

            if (vm.anteriorIdElementoIndicador != '' && vm.anteriorIdElementoIndicador != idElement) {
                vm.anteriorIdElementoIndicador = vm.anteriorIdElementoIndicador.replace("null", "");
                var elMasAnterior = document.getElementById(vm.anteriorIdElementoIndicador + '-mas');
                var elMenosAnterior = document.getElementById(vm.anteriorIdElementoIndicador + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

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

            if (vm.anteriorIdElementoVigencia != '' && vm.anteriorIdElementoVigencia != idElement) {
                vm.anteriorIdElementoVigencia = vm.anteriorIdElementoVigencia.replace("null", "");
                var elMasAnterior = document.getElementById(vm.anteriorIdElementoVigencia + '-mas');
                var elMenosAnterior = document.getElementById(vm.anteriorIdElementoVigencia + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            vm.anteriorIdElementoIndicador = idElement;
            idElement = idElement.replace("null", "");
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.iIndicadorS = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.iIndicadorS = indicador;
                }
            }
        }

        vm.abrilNiveliLocalizacion = function (idElement, localizacion) {
            vm.iVigenciaS = 0;

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

            if (vm.anteriorIdElementoVigencia != '' && vm.anteriorIdElementoVigencia != idElement) {
                vm.anteriorIdElementoVigencia = vm.anteriorIdElementoVigencia.replace("null", "");
                var elMasAnterior = document.getElementById(vm.anteriorIdElementoVigencia + '-mas');
                var elMenosAnterior = document.getElementById(vm.anteriorIdElementoVigencia + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            vm.anteriorIdElementoLocalizacion = idElement;
            idElement = idElement.replace("null", "");
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.iLocalizacionS = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.iLocalizacionS = localizacion;
                }
            }
        }

        vm.AbrilNivelResumen = function (idElement, vigencia) {
            if (vm.anteriorIdElementoVigencia != '' && vm.anteriorIdElementoVigencia != idElement) {
                vm.anteriorIdElementoVigencia = vm.anteriorIdElementoVigencia.replace("null", "");
                var elMasAnterior = document.getElementById(vm.anteriorIdElementoVigencia + '-mas');
                var elMenosAnterior = document.getElementById(vm.anteriorIdElementoVigencia + '-menos');
                if (elMasAnterior != null && elMenosAnterior != null) {
                    if (elMasAnterior.classList.contains('hidden')) {
                        elMenosAnterior.classList.add('hidden');
                        elMasAnterior.classList.remove('hidden');
                    }
                }
            }

            vm.anteriorIdElementoVigencia = idElement;
            idElement = idElement.replace("null", "");
            var elMas = document.getElementById(idElement + '-mas');
            var elMenos = document.getElementById(idElement + '-menos');

            if (elMas != null && elMenos != null) {
                if (elMas.classList.contains('hidden')) {
                    elMenos.classList.add('hidden');
                    elMas.classList.remove('hidden');
                    vm.iVigenciaS = 0;
                } else {
                    elMenos.classList.remove('hidden');
                    elMas.classList.add('hidden');
                    vm.iVigenciaS = vigencia;
                }
            }
        }

        vm.abrirMensajeQueEsEsto = function () {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <br /> <span class='tituhori' >Programar Actividades - Desglose de actividades</span>");
        }

        /*--------------------- Agregar - Eliminar niveles / actividades ---------------------*/

        vm.abrirArbol = function (dataModal) {
            var arbolCollapse = {
                productoId: "productos-nivel-1-" + dataModal["Producto"].ProductoId,
                productoIdCompuesto: dataModal["Producto"].IdCompuesto,
                entregablesNvl1: "items-" + dataModal["Nivel1CollapseId"],
                entregablesNvl2: "items-" + dataModal["Nivel2CollapseId"],
                entregablesNvl3: "items-" + dataModal["Nivel3CollapseId"],
            }
            vm.obtenerIndicadores(arbolCollapse);
        }

        /*--------------------- Validaciones ---------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            console.log("Validación  - Programar Actividades", errores);
            if (errores != undefined) {
                vm.erroresActivos = [];
                var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
                vm.erroresActivos = erroresFiltrados.erroresActivos;
                console.log("erroresActivos", vm.erroresActivos)
                vm.ejecutarErrores();
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: erroresFiltrados.isValid });
            }
        }

        vm.limpiarErrores = function () {
            var listadoErrores = document.getElementsByClassName("messagealerttableDNP");
            var listadoErroresContainer = document.getElementsByClassName("errores-contenedor");
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
        }

        vm.getErrorIndicador = function ({ error, descripcion, data }) {
            var dataObj = JSON.parse(descripcion)
            console.log("getErrorIndicador", dataObj);
            var imgError = '<span class="img iconadverdnp mr-2"><img src="Img/u4630.svg"></span>';

            dataObj.forEach((itemError) => {
                var subCategoriaHT = document.getElementById("categoria-error-button-" + itemError.DimensionId + "-" + itemError.PoliticaId)
                var indicadorHT = document.getElementById("indicadores-error-button-" + itemError.IndicadorId + "-" + itemError.DimensionId + "-" + itemError.PoliticaId)

                if (subCategoriaHT != undefined && subCategoriaHT != null)
                    subCategoriaHT.innerHTML = '<span class="img iconadverdnp"><img src="Img/u4630.svg"> </span><span class="messagealerttableDNP d-none" id="categoria-error-button-' + itemError.DimensionId + "-" + itemError.PoliticaId + '-txt"></span>';

                if (indicadorHT != undefined && indicadorHT != null)
                    indicadorHT.innerHTML = '<span class="img iconadverdnp"><img src="Img/u4630.svg"> </span><span class="messagealerttableDNP d-none" id="indicadores-error-button-' + + itemError.IndicadorId + "-" + itemError.DimensionId + "-" + itemError.PoliticaId + '-txt"></span>';
            })
            dataObj.forEach((itemError) => {

                var politicaHT = document.getElementById("politica-error-button-" + itemError.PoliticaId)
                var fuenteHT = document.getElementById("fuente-error-button-" + itemError.FuenteId)
                var subCategoriaHT = document.getElementById("categoria-error-button-" + itemError.DimensionId + "-" + itemError.PoliticaId)
                var localizacionHT = document.getElementById("localizacion-error-button-" + itemError.IndicadorId + "-" + itemError.LocalizacionId + "-" + itemError.DimensionId + "-" + itemError.PoliticaId)
                var indicadorHT = document.getElementById("indicadores-error-button-" + itemError.IndicadorId + "-" + itemError.DimensionId + "-" + itemError.PoliticaId)
                
                if (politicaHT != undefined && politicaHT != null) {
                    politicaHT.classList.remove("d-none");
                    politicaHT.innerHTML = imgError;
                }

                if (fuenteHT != undefined && fuenteHT != null) {
                    fuenteHT.classList.remove("d-none");
                    fuenteHT.innerHTML = imgError;
                }

                if (subCategoriaHT != undefined && subCategoriaHT != null) {
                    subCategoriaHT.classList.remove("d-none");
                    subCategoriaHT.innerHTML = subCategoriaHT.innerHTML + ' - ' + itemError.MensajeErrorDimension;
                }

                if (localizacionHT != undefined && localizacionHT != null) {
                    localizacionHT.classList.remove("d-none");
                    localizacionHT.innerHTML = imgError;
                }

                if (indicadorHT != undefined && indicadorHT != null) {
                    indicadorHT.classList.remove("d-none");
                    indicadorHT.innerHTML = indicadorHT.innerHTML + ' - ' + itemError.MensajeError;
                }

            })
        }

        vm.errores = {
            'INDICA001': vm.getErrorIndicador
        }

        function habilitaEditarAvance(indicadores, tipo) {
            if (tipo == 1) {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    indicadores.HabilitaEditar = false;
                    for (var i = 0; i < indicadores.ReporteIndicadores.length; i++) {
                        /*indicadores.ReporteIndicadores[i].ValorVigente = indicadores.ReporteIndicadores[i].ValorVigenteAnterior;*/
                        indicadores.ReporteIndicadores[i].ValorPagos = indicadores.ReporteIndicadores[i].ValorPagosAnterior;
                        indicadores.ReporteIndicadores[i].ValorObligaciones = indicadores.ReporteIndicadores[i].ValorObligacionesAnterior;
                        indicadores.ReporteIndicadores[i].ValorCompromisos = indicadores.ReporteIndicadores[i].ValorCompromisosAnterior;
                        indicadores.ReporteIndicadores[i].Observaciones = indicadores.ReporteIndicadores[i].ObservacionesAnterior;
                    }
                    OkCancelar();

                }, function funcionCancelar(reason) {
                    console.log("reason", reason);
                }, null, null, "Los posibles datos que haya diligenciado en la tabla 'Recursos indicador de política' se perderán.");
            }
            else {
                indicadores.HabilitaEditar = true;
            }
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.");
            }, 500);
        }

        vm.actualizarVigenciaAvance = function (indicadores, politicaId, fuenteId, dimensionId, localizacionId, indicadorId) {

            ///Se quita validación por implementación de los negativos, afecta esta validación el cual no aplicaría
            //for (var i = 0; i < indicadores.ReporteIndicadores.length; i++) {
            //    if (parseFloat(indicadores.ReporteIndicadores[i].ValorObligaciones) > parseFloat(indicadores.ReporteIndicadores[i].ValorCompromisos)) {
            //        utilidades.mensajeError("Las obligaciones no pueden ser superiores a los compromisos reportados.", false);
            //        return false;
            //    }
            //    if (parseFloat(indicadores.ReporteIndicadores[i].ValorPagos) > parseFloat(indicadores.ReporteIndicadores[i].ValorCompromisos)) {
            //        utilidades.mensajeError("Los pagos no pueden ser superiores a los compromisos reportados.", false);
            //        return false;
            //    }
            //    if (parseFloat(indicadores.ReporteIndicadores[i].ValorPagos) > parseFloat(indicadores.ReporteIndicadores[i].ValorObligaciones)) {
            //        utilidades.mensajeError("Los pagos no deben superar el valor de las obligaciones reportadas.", false);
            //        return false;
            //    }
            //}

            for (var i = 0; i < indicadores.ReporteIndicadores.length; i++) {
                indicadores.ReporteIndicadores[i].ValorCompromisos = indicadores.ReporteIndicadores[i].ValorCompromisos.toString().includes(",") ? indicadores.ReporteIndicadores[i].ValorCompromisos.replace(',', '.') : indicadores.ReporteIndicadores[i].ValorCompromisos;
                indicadores.ReporteIndicadores[i].ValorObligaciones = indicadores.ReporteIndicadores[i].ValorObligaciones.toString().includes(",") ? indicadores.ReporteIndicadores[i].ValorObligaciones.replace(',', '.') : indicadores.ReporteIndicadores[i].ValorObligaciones;
                indicadores.ReporteIndicadores[i].ValorPagos = indicadores.ReporteIndicadores[i].ValorPagos.toString().includes(",") ? indicadores.ReporteIndicadores[i].ValorPagos.replace(',', '.') : indicadores.ReporteIndicadores[i].ValorPagos;
            }


            var data = {
                ReporteIndicadores: indicadores.ReporteIndicadores,
                ProyectoId: $sessionStorage.proyectoId,
                PoliticaId: politicaId,
                FuenteId: fuenteId,
                DimensionId: dimensionId,
                LocalizacionId: localizacionId
            }

            indicadorPoliticasServicio.Guardar(data)
                .then(function (response) {
                    let exito = response.data;
                    if (response.data.Status) {
                        indicadorPoliticasServicio.obtenerIndicadores({ bpin: $sessionStorage.idObjetoNegocio })
                            .then(resultado => {
                                if (resultado.data != null) {
                                    var data = resultado.data["Politicas"];
                                    vm.listadoIndicadoresPolitica = data;
                                    utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                                    indicadores.HabilitaEditar = false;
                                    setTimeout(function () {
                                        vm.ConsultarDetallePolitica(politicaId);
                                        $("#progReporAct-indicador-" + dimensionId + '-' + politicaId).collapse("toggle");
                                        vm.abrilNivel('progReporAct-indicador-' + dimensionId + '-' + politicaId);
                                        $('#reporProgramar-localizaciones-' + indicadorId + '-' + dimensionId + '-' + politicaId).collapse("toggle");
                                        vm.abrilNivel('reporProgramar-localizaciones-' + indicadorId + '-' + dimensionId + '-' + politicaId);
                                        $('#reporProgramarindicadores-' + indicadorId + '-' + localizacionId + '-' + dimensionId + '-' + politicaId).collapse("toggle");
                                        vm.abrilNivel('reporProgramarindicadores-' + indicadorId + '-' + localizacionId + '-' + dimensionId + '-' + politicaId);
                                    }, 500)
                                    return;
                                }
                            });

                    }
                    else {
                        try {
                            utilidades.mensajeError(response.data.Message, false);
                        }
                        catch {
                            utilidades.mensajeError("Error al realizar la operación", false);
                        }

                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });
        }


        vm.validateFormat = function (event, cantidad) {
            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44 && event.keyCode != 46) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 12;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                vm.puntoDigitado = false;
                if (cantidad == 4)
                    tamanioPermitido = 17;
                else
                    tamanioPermitido = 15;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, cantidad);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > cantidad) {
                        tamanioPermitido = n[0].length + cantidad;
                        event.target.value = n[0] + '.' + n[1].slice(0, cantidad);
                        return;
                    }

                    if (cantidad == 2) {
                        if ((n[1].length == 1 && n[1] > 9) || (n[1].length > 1 && n[1] > 99)) {
                            event.preventDefault();
                        }
                    }
                    else {
                        if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                            event.preventDefault();
                        }
                    }

                }
            } else {
                if (tamanio > 13 && event.keyCode != 44 && event.keyCode != 46) {
                    event.preventDefault();
                }
            }

            if ((event.keyCode == 44 || event.keyCode == 46) && tamanio == 13) {
                vm.puntoDigitado = true;
            }
            else if (vm.puntoDigitado && tamanio == 13) {
                vm.puntoDigitado = false;
            }
            else {
                if (cantidad == 4) {
                    if (tamanio > tamanioPermitido || tamanio > 17) {
                        event.preventDefault();
                    }
                }
                else {
                    if (tamanio > tamanioPermitido || tamanio > 15) {
                        event.preventDefault();
                    }
                }

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
            var tamanioPermitido = 15;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 17;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 4);
                    newValue = [n[1], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1) return;
                    if (spiltArray.length === 2) return;

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
                if (tamanio > 17 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 17) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 17) {
                    event.preventDefault();
                }
            }
        };

        vm.validarTamanio = function (event, cantidad) {

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
                vm.puntoDigitado = false;
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > cantidad) {
                }
                if (cantidad == 4)
                    tamanioPermitido = 16;
                else
                    tamanioPermitido = 14;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, cantidad);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > cantidad) {
                        tamanioPermitido = n[0].length + cantidad;
                        event.target.value = n[0] + '.' + n[1].slice(0, cantidad);
                        return;
                    }

                    if (cantidad == 2) {
                        if ((n[1].length == 1 && n[1] > 9) || (n[1].length > 1 && n[1] > 99)) {
                            event.preventDefault();
                        }
                    }
                    else {
                        if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                            event.preventDefault();
                        }
                    }
                }
            }
        }

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
            var tamanioPermitido = 15;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > 4) {
                }
                tamanioPermitido = 17;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 4);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '.') return;

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

        function recorrerObjetivosNumber(event) {

            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
            }

            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
            }

            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 3, }).format(0.00);
            }

            event.target.value = parseFloat(event.target.value.replace(",", "."));

            //recorrerObjetivos(actividad, vigenciaInicial);
        }

    }

    angular.module('backbone').component('indicadorPoliticas', {
        templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/seguimientoFocalizacion/componentes/segIndicadorPoliticas/indicadorPoliticas/indicadorPoliticas.html",
        controller: indicadorPoliticasController,
        controllerAs: "vm",
        bindings: {
            bpin: '@',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacioncambios: '&',
            notificacionestado: '&'
        }
    });

})();