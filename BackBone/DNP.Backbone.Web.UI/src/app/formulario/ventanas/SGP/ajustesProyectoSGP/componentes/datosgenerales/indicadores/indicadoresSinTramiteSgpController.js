
(function () {
    'use strict';

    indicadoresSinTramiteSgpController.$inject = [
        '$scope',
        '$sessionStorage',
        'constantesBackbone',
        'indicadoresSinTramiteSgpServicio',
        'utilidades',
        '$uibModal',
        'utilsValidacionSeccionCapitulosServicio',
        'justificacionCambiosServicio',
        'justificacionIndicadoresServicio',
        'transversalSgpServicio'
    ];



    function indicadoresSinTramiteSgpController(
        $scope,
        $sessionStorage,
        constantesBackbone,
        indicadoresSinTramiteSgpServicio,
        utilidades,
        $uibModal,
        utilsValidacionSeccionCapitulosServicio,
        justificacionCambiosServicio,
        justificacionIndicadoresServicio,
        transversalSgpServicio

    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "datosgeneralessgpindicadoresdeprsintramitesgp";
        vm.idProyecto = $sessionStorage.proyectoId;
        vm.codigoBpin = $sessionStorage.idObjetoNegocio;
        vm.habilitaGuardar = false;
        vm.anioInicio;
        vm.anioFinal;
        vm.estadoProyecto;
        vm.habilitar = false;
        vm.habilitarFinal = false;
        vm.verBotones = false;
        vm.anioInicioOriginal;
        vm.anioFinalOriginal;
        vm.evaluarVerBotones = evaluarVerBotones;
        vm.habilitarInicio = false;
        vm.Guardar = Guardar;
        vm.Obtenerindicadores = Obtenerindicadores;
        vm.EsMover = false;
        vm.Usuario = usuarioDNP;
        vm.vigenciaActual = new Date().getFullYear();
        vm.BtnIndicador = BtnIndicador;
        vm.editarIndicador = false;
        vm.AgregarIndSecundario = AgregarIndSecundario;
        vm.habilitaEditar = habilitaEditar;
        vm.ObjetivoVerMas = ObjetivoVerMas;
        vm.ProductoVerMas = ProductoVerMas;
        vm.EliminarIndicador = EliminarIndicador;
        vm.cambiaAcumulativo = cambiaAcumulativo;
        vm.Cancelar = Cancelar;
        vm.erroresActivos = null;
        vm.ConvertirNumero = ConvertirNumero;
        vm.SeccionCapituloId = 0;
        vm.abrirMensajeInformacionObjetivo = abrirMensajeInformacionObjetivo;
        vm.validacionGuardado = null;
        vm.ContraerIndicadorObjetivo = ContraerIndicadorObjetivo;
        vm.ContraerIndicadorProductos = ContraerIndicadorProductos;
        vm.soloLectura = false;
        //Inicio

        vm.parametros = {

            idInstancia: $sessionStorage.idInstancia,
            idFlujo: $sessionStorage.idFlujoIframe,
            idNivel: $sessionStorage.idNivel,
            idProyecto: vm.idProyecto,
            idProyectoStr: $sessionStorage.idObjetoNegocio,
            Bpin: vm.Bpin

        };

        vm.init = function () {
            Obtenerindicadores();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificarrefresco({ handler: vm.refrescarIndicadoresModificados, nombreComponente: vm.nombreComponente });
            vm.vigenciaActual = new Date().getFullYear();
            transversalSgpServicio.registrarObservador(function (datos) {
                if (datos.actualizarIndicadores === true) {
                    console.log("Actulizando Indicadores");
                    vm.init();
                }
            });
        };

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 4,
            }).format(numero);
        }

        function Guardar(indicador) {

            angular.forEach(indicador.Vigencias, function (series) {

                if (series.MetaVigencialIndicadorAjuste == "" || series.MetaVigencialIndicadorAjuste == null) {
                    series.MetaVigencialIndicadorAjuste = 0.0000;
                }
            });

            return indicadoresSinTramiteSgpServicio.ActualizarMetaAjusteIndicador(indicador).then(
                function (respuesta) {
                    if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                        guardarCapituloModificado();
                        utilidades.mensajeSuccess("Los datos diligenciados se reflejarán en la tabla de indicadores dentro de la celda Meta ajuste del indicador intervenido.", false, false, false, "Los datos fueron guardados con éxito");
                        indicador.HabilitaEditarIndicador = false;
                        indicador.IndicadorAcumulaOriginal = indicador.IndicadorAcumula;
                        indicador.IndicadorAcumulaAjustadoOriginal = indicador.IndicadorAcumulaAjustado;
                        indicador.MetaTotalFirmeAjustadoOriginal = indicador.MetaTotalFirmeAjustado;
                        angular.forEach(indicador.Vigencias, function (series) {
                            series.MetaVigencialIndicadorAjusteOriginal = parseFloat(series.MetaVigencialIndicadorAjuste).toFixed(4);
                        });
                        vm.init();
                    } else {
                        utilidades.mensajeError("Error al realizar la operación");
                    }

                });
        }
        function abrirMensajeInformacionObjetivo() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' > Objetivos específicos</span>");

        }
        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }


        function Cancelar(indicador, productoId, ObjetivoId, Bandera) {

            if (Bandera == 1) {

                utilidades.mensajeWarning("Los posibles datos que haya diligenciado se perderan. ¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();
                    indicador.HabilitaEditarIndicador = false;
                    indicador.IndicadorAcumula = indicador.IndicadorAcumulaOriginal;
                    indicador.IndicadorAcumulaAjustado = indicador.IndicadorAcumulaAjustadoOriginal;
                    indicador.MetaTotalFirmeAjustado = indicador.MetaTotalFirmeAjustadoOriginal;
                    angular.forEach(indicador.Vigencias, function (series) {
                        series.MetaVigencialIndicadorAjuste = parseFloat(series.MetaVigencialIndicadorAjusteOriginal).toFixed(4);
                    });

                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Advertencia");
            }
            else {
                indicador.HabilitaEditarIndicador = false;
                indicador.IndicadorAcumula = indicador.IndicadorAcumulaOriginal;
                indicador.IndicadorAcumulaAjustado = indicador.IndicadorAcumulaAjustadoOriginal;
                indicador.MetaTotalFirmeAjustado = indicador.MetaTotalFirmeAjustadoOriginal;
                angular.forEach(indicador.Vigencias, function (series) {
                    series.MetaVigencialIndicadorAjuste = parseFloat(series.MetaVigencialIndicadorAjusteOriginal).toFixed(4);
                });
            }


        }

        function Obtenerindicadores() {
            return indicadoresSinTramiteSgpServicio.ObtenerIndicadoresProducto($sessionStorage.idInstancia).then(
                function (respuesta) {
                    vm.verBotones = false;
                    if (respuesta.data != null && respuesta.data != "") {
                        respuesta.data.ObjetivosEspecificos.forEach(obj => {
                            obj.visible = false;
                            obj.Productos.forEach(pro => {
                                pro.visible = false;
                                pro.Indicadores.forEach(ind => {
                                    ind.visible = false;
                                });
                            });
                        });
                        $scope.datos = respuesta.data;
                        vm.soloLectura = $sessionStorage.soloLectura;
                    }
                });
        }

        function evaluarVerBotones() {
            vm.verBotones = false;
            if (vm.anioInicioOriginal != vm.anioInicio) {
                vm.verBotones = true;
            }
        }

        function habilitaEditar(indicador) {

            indicador.HabilitaEditarIndicador = true;

            angular.forEach(indicador.Vigencias, function (series) {
                series.MetaVigencialIndicadorAjuste = parseFloat(series.MetaVigencialIndicadorAjuste).toFixed(4);
            });
        }

        function BtnIndicador(indicador) {
            vm.editarIndicador = true;// con true despliega las vigencias
            ContraerIndicadores(indicador);
            return indicador.LabelBotonIndicador;
        }

        function AgregarIndSecundario(prod, ObjetivoId) {

            angular.forEach(prod.CatalogoIndicadoresSecundarios, function (series) {

                var result = prod.Indicadores.find(element => element.CodigoIndicador > series.CodigoIndicador);

                if (series.Marcado == 1) {
                    series.Marcado = 2;
                }

            });
            var cantidadIndSec = prod.CatalogoIndicadoresSecundarios.length;
            if (cantidadIndSec == 1 && (prod.CatalogoIndicadoresSecundarios[0].CodigoIndicador == "" || prod.CatalogoIndicadoresSecundarios[0].CodigoIndicador == null)) {
                const mensaje1 = "El producto no tiene indicadores secundarios asociados.";
                //utilidades.mensajeInformacionN(mensaje1, null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' > Indicadores Secundarios</span>");
                utilidades.mensajeError(mensaje1);
            }
            else {
                let modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: 'src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/indicadores/modal/agregarIndicadorSecModalSinTramiteSgp.html',
                    controller: 'agregarIndicadorSecModalSinTramiteSgpController',
                    controllerAs: "vm",
                    size: 'lg',
                    openedClass: "entidad-modal-adherencia",
                    resolve: {
                        Programacion: function () {
                            return 1;
                        },
                        ProductoId: function () {
                            return prod.ProductoId;
                        },
                        IndicadoresSec: function () {
                            return prod.CatalogoIndicadoresSecundarios;
                        },
                        ObjetivoId: function () {
                            return ObjetivoId;
                        }
                    },
                });
                modalInstance.result.then(data => {
                    if (data != null) {

                        var paramsIndSec = {
                            IdProducto: prod.ProductoId,
                            Lista: data
                        };

                        indicadoresSinTramiteSgpServicio.agregarIndicadorSecundario(paramsIndSec).then(function (response) {

                            if (response.data && (response.statusText === "OK" || response.status === 200)) {
                                guardarCapituloModificado();
                                const mensaje2 = "Los indicadores secundarios seleccionados fueron agregados correctamente al producto.";
                                new utilidades.mensajeSuccess(mensaje2, false, false, false);
                                vm.init();
                            } else {
                                new utilidades.mensajeError("Error al realizar la operación");
                            }

                        });
                    }
                });
            }
        }

        vm.actualizaFila = function (event, indicador) {

            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
            }

            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
            }

            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
            }

            event.target.value = procesarNumero(event.target.value, 4);

            if (indicador.IndicadorAcumula) {
                var acumula = 0;
                angular.forEach(indicador.Vigencias, function (series) {
                    acumula = acumula + parseFloat(procesarNumero(series.MetaVigencialIndicadorAjuste, 4));
                    indicador.MetaTotalFirmeAjustado = parseFloat(acumula).toFixed(4);
                });
            } else {
                indicador.MetaTotalFirmeAjustado = parseFloat(Math.max.apply(Math, indicador.Vigencias.map(function (item) { return procesarNumero(item.MetaVigencialIndicadorAjuste, 4); }))).toFixed(4);
            }

            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 4 ? event.target.value : parseFloat(val).toFixed(4);
            event.target.value = new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(total);
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

            event.target.value = procesarNumero(event.target.value, null, false);

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 15;
            var tamanio = event.target.value.length;
            var decimal = false;
            decimal = event.target.value.toString().includes(".");
            if (decimal) {
                tamanioPermitido = 20;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 4);
                    newValue = [n[0], n2].join(".");
                    event.target.value = newValue;
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

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }
        }

        function procesarNumero(value, cantidadDecimales, convertFloat = true) {
            if (!Number(value)) {
                value = limpiaNumero(value);

            } else if (!convertFloat) {
                value = value.replace(",", ".");
            } else {
                if (cantidadDecimales != undefined)
                    value = parseFloat(value.replace(",", ".")).toFixed(cantidadDecimales);
            }

            return value;
        }

        function limpiaNumero(valor) {
            if (`${valor.toLocaleString().split(",")[1]}` == 'undefined') return `${valor.toLocaleString().split(",")[0].toString()}`;
            return `${valor.toLocaleString().split(",")[0].toString().replaceAll(".", "")}.${valor.toLocaleString().split(",")[1].toString()}`;
        }

        function formatearNumero(value) {
            var numerotmp = value.toString().replaceAll('.', '');
            return parseInt(numerotmp).toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ".");
        }

        function ObjetivoVerMas(objetivo) {
            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModalSinTramiteSgp.html',
                controller: 'objetivosIndicadorModalSinTramiteSgpController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    Objetivo: function () {
                        return objetivo.ObjetivoEspecifico;
                    },
                    IdObjetivo: function () {
                        return objetivo.ObjetivoId;
                    },
                    Tipo: function () {
                        return 'Objetivo';
                    },
                    Titulo: function () {
                        return 'Indicadores';
                    }
                },
            });
        }

        function ProductoVerMas(prod) {
            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModalSinTramiteSgp.html',
                controller: 'objetivosIndicadorModalSinTramiteSgpController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    Objetivo: function () {
                        return prod.NombreProducto;
                    },
                    IdObjetivo: function () {
                        return prod.ProductoId;
                    },
                    Tipo: function () {
                        return 'Producto';
                    }
                },
            });
        }

        function EliminarIndicador(IndicadorId, CodigoIndicador) {
            utilidades.mensajeWarning("La línea de información del indicador con código  " + CodigoIndicador + " se perderá.¿Está seguro de continuar ?", function funcionContinuar() {
                const mensaje3 = "El indicador ha sido eliminado correctamente.";
                return indicadoresSinTramiteSgpServicio.EliminarIndicadorProducto(IndicadorId).then(
                    function (respuesta) {
                        if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                            guardarCapituloModificado();
                            new utilidades.mensajeSuccess(mensaje3, false, false, false);
                            vm.init();
                        } else {
                            new utilidades.mensajeError("Error al realizar la operación");
                        }
                    });
            }, function funcionCancelar(reason) {
                //console.log("reason", reason);
            }, null, null, "Los datos serán eliminados.");
        }

        vm.BtnObjetivos = function (objetivo) {
            ContraerIndicadorObjetivo(objetivo);
            return objetivo.LabelBotonObjetivo;
        }

        vm.BtnProductos = function (prod) {
            ContraerIndicadorProductos(prod);
            return prod.LabelBotonProducto;
        }

        function cambiaAcumulativo(indicador) {
            if (indicador.IndicadorAcumula) {

                var ajustado = 0;
                var mga = 0;
                var firme = 0;
                angular.forEach(indicador.Vigencias, function (series) {
                    ajustado = ajustado + Number(series.MetaVigencialIndicadorAjuste);
                    mga = mga + Number(series.MetaVigenciaIndicadorMga);
                    firme = firme + Number(series.MetaVigenciaIndicadorFirme);

                });
                indicador.MetaTotalFirmeAjustado = ajustado;
                indicador.MetaTotalIndicadorMga = mga;
                indicador.MetaTotalFirme = firme;
                indicador.IndicadorAcumulaAjustado = "SI";
                return;
            } else {

                var ajustado = Math.max.apply(Math, indicador.Vigencias.map(function (item) { return item.MetaVigencialIndicadorAjuste; }));
                var mga = Math.max.apply(Math, indicador.Vigencias.map(function (item) { return item.MetaVigenciaIndicadorMga; }));
                var firme = Math.max.apply(Math, indicador.Vigencias.map(function (item) { return item.MetaVigenciaIndicadorFirme; }));

                indicador.MetaTotalFirmeAjustado = ajustado;
                indicador.MetaTotalIndicadorMga = mga;
                indicador.MetaTotalFirme = firme;
                indicador.IndicadorAcumulaAjustado = "NO";
                return;
            }
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (nombreCapituloHijo == 'datosgeneraleshorizonte') {
                vm.init();
            }
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
            var isValid = true;
            if (erroresRelacionconlapl != undefined) {
                var erroresJson = erroresRelacionconlapl.Errores == null ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    erroresJson[vm.nombreComponente].forEach(p => {
                        if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                    });
                    var idSpanAlertComponent = document.getElementById("alert-" + vm.nombreComponente);
                    if (idSpanAlertComponent != undefined) {
                        idSpanAlertComponent.classList.add("ico-advertencia");
                    }
                }
            }

            if (errores != undefined) {
                var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
                vm.erroresActivos = erroresFiltrados.erroresActivos;
                vm.ejecutarErrores();
            }
            vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: erroresFiltrados.isValid });
        };

        vm.ejecutarErrores = function () {
            vm.limpiarErrores();
            $scope.errores = vm.erroresActivos;
            vm.erroresActivos.forEach(p => {
                if (vm.errores[p.Error] != undefined) {
                    vm.errores[p.Error](p.Error, p.Descripcion, p.Data);
                }
            });
        }

        vm.limpiarErrores = function () {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "ValidaIndicador1");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
            var campoObligatorioJustificacion2 = document.getElementById(vm.nombreComponente + "ValidaIndicador2");
            if (campoObligatorioJustificacion2 != undefined) {
                campoObligatorioJustificacion2.innerHTML = "";
                campoObligatorioJustificacion2.classList.add('hidden');
            }
            var campoObligatorioJustificacion3 = document.getElementById(vm.nombreComponente + "ValidaIndicador3");
            if (campoObligatorioJustificacion3 != undefined) {
                campoObligatorioJustificacion3.innerHTML = "";
                campoObligatorioJustificacion3.classList.add('hidden');
            }
            var campoObligatorioJustificacion4 = document.getElementById(vm.nombreComponente + "ValidaIndicador4");
            if (campoObligatorioJustificacion4 != undefined) {
                campoObligatorioJustificacion4.innerHTML = "";
                campoObligatorioJustificacion4.classList.add('hidden');
            }
        }

        vm.validacionAJIND001 = function (errores, descripcion, data) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "ValidaIndicador1");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + descripcion + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }
        vm.validacionAJIND002 = function (errores, descripcion, data) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "ValidaIndicador2");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + descripcion + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }
        vm.validacionAJIND003 = function (errores, descripcion, data) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "ValidaIndicador3");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + descripcion + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }
        vm.validacionAJIND004 = function (errores, descripcion, data) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "ValidaIndicador4");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + descripcion + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }

        vm.errores = {
            'AJIND001': vm.validacionAJIND001,
            'AJIND002': vm.validacionAJIND002,
            'AJIND003': vm.validacionAJIND003,
            'AJIND004': vm.validacionAJIND004
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
                //SeccionCapituloId: vm.SeccionCapituloId,
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.InstanciaSeleccionada.IdInstancia,
                Modificado: true,
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

        vm.refrescarIndicadoresModificados = function () {

            return justificacionIndicadoresServicio.IndicadoresValidarCapituloModificado(vm.codigoBpin).then(
                function (resp) {
                    if (resp.data != null && resp.data != "") {
                        vm.validacionGuardado();
                    }
                }
            );
        }

        function ContraerIndicadorObjetivo(objetivo) {

            $scope.datos.ObjetivosEspecificos.forEach(obj => {
                //Validar el collapse de los objetivos
                if (obj.ObjetivoId === objetivo.ObjetivoId) {
                    obj.visible = !obj.visible;
                    if (obj.visible) {
                        obj.LabelBotonObjetivo = '-';
                    }
                    else {
                        obj.LabelBotonObjetivo = '+';
                    }
                } else {
                    obj.visible = false;
                    obj.LabelBotonObjetivo = '+';
                }
                //Contraer todos los productos
                obj.Productos.forEach(pro => {
                    pro.visible = false;
                    pro.LabelBotonProducto = '+';

                    //Cancelar la edición los indicadores
                    pro.Indicadores.forEach(ind => {
                        Cancelar(ind);
                    });
                });
            });
        }

        function ContraerIndicadorProductos(producto) {
            $scope.datos.ObjetivosEspecificos.forEach(obj => {

                //Validar el collapse de los productos
                obj.Productos.forEach(pro => {

                    if (pro.ProductoId === producto.ProductoId) {
                        pro.visible = !pro.visible;
                        if (pro.visible) {
                            pro.LabelBotonProducto = '-';
                        }
                        else {
                            pro.LabelBotonProducto = '+';
                        }
                    }
                    else {
                        pro.visible = false;
                        pro.LabelBotonProducto = '+';
                    }

                    //Contraer todos los indicadores
                    pro.Indicadores.forEach(ind => {
                        ind.visible = false;
                        ind.LabelBotonIndicador = '+';
                        Cancelar(ind);

                    });
                });
            });
        }

        function ContraerIndicadores(indicador) {
            $scope.datos.ObjetivosEspecificos.forEach(obj => {

                //Validar el collapse de los indicadores
                obj.Productos.forEach(pro => {
                    pro.Indicadores.forEach(ind => {
                        if (ind.IndicadorId === indicador.IndicadorId) {
                            ind.visible = !ind.visible;
                            if (ind.visible) {
                                ind.LabelBotonIndicador = '-';
                            }
                            else {
                                ind.LabelBotonIndicador = '+';
                                Cancelar(ind);
                            }
                        }
                        else {
                            ind.visible = false;
                            ind.LabelBotonIndicador = '+';
                            Cancelar(ind);
                        }
                    });
                });
            });
        }
    }

    angular.module('backbone').component('indicadoresSinTramiteSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/datosgenerales/indicadores/indicadoresSinTramiteSgp.html",
        controller: indicadoresSinTramiteSgpController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&'
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