(function () {
    'use strict';

    tablaVigenciaFuturaProductoFormulario.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        'trasladosServicio',
        '$timeout',
        'justificacionCambiosServicio',
        'comunesServicio'
    ];

    function tablaVigenciaFuturaProductoFormulario(
        $scope,
        $sessionStorage,
        utilidades,
        trasladosServicio,
        $timeout,
        justificacionCambiosServicio,
        comunesServicio
    ) {

        var vm = this;

        /*Varibales */
        vm.instanciaId = $sessionStorage.instanciaId;
        vm.proyectoId = undefined;
        vm.tramiteid = undefined;
        vm.nombreComponente = "reprogramacionproductovigfutura";
        vm.pagina = 1;
        vm.Origen = 0;

        /*declara metodos*/
        vm.Guardar = Guardar;
        vm.habilitaEditar = habilitaEditar;
        vm.ProductoVerMas = ProductoVerMas;
        vm.Cancelar = Cancelar;
        vm.ConvertirNumero = ConvertirNumero;
        vm.initTablaVigenciaFuturaProducto = initTablaVigenciaFuturaProducto;


        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;


        /*Funciones*/

        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
                // ObtenerSeccionCapitulo();
            }
        });


        $scope.$watch('$sessionStorage.TramiteId', function () {
            if ($sessionStorage.TramiteId !== '' && $sessionStorage.TramiteId !== undefined && $sessionStorage.TramiteId !== null) {
                vm.tramiteid = $sessionStorage.TramiteId;
                cargaVariables();
            }

        });

        $scope.$watch('$sessionStorage.proyectoId', function () {
            if ($sessionStorage.proyectoId !== '' && $sessionStorage.proyectoId !== undefined && $sessionStorage.proyectoId !== null) {
                vm.proyectoId = $sessionStorage.proyectoId;
                cargaVariables();
            }

        });

        $scope.$watch('$sessionStorage.idInstancia', function () {
            if ($sessionStorage.idInstancia !== '' && $sessionStorage.idInstancia !== undefined && $sessionStorage.idInstancia !== null) {
                vm.instanciaId = $sessionStorage.idInstancia;
                cargaVariables();
            }

        });


        $scope.$watch('vm.vigenciaadicionada', function () {
            if (vm.vigenciaadicionada === '1') {
                cargaVariables();
                vm.vigenciaadicionada = '0';
            }

        });

        function cargaVariables() {
            if (vm.proyectoId !== undefined && vm.tramiteid !== undefined && vm.instanciaId !== undefined)
                ObtenerResumenReprogramacionPorVigencia();
        }

        function ObtenerResumenReprogramacionPorVigencia() {
            return comunesServicio.obtenerResumenReprogramacionPorProductoVigencia(vm.instanciaId, vm.proyectoId, vm.tramiteid).then(
                function (respuesta) {
                    if (respuesta.data !== '' && respuesta.data !== 'null' && respuesta.data !== null && respuesta.data !== undefined) {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        vm.datosProducto = jQuery.parseJSON(arreglolistas);
                        vm.Origen = 1;
                        if (!vm.datosProducto.EsConstante) {
                            vm.datosProducto.ResumenTramite[0].ValoresCorrientes = vm.datosProducto.ResumenTramite[0].Valores;
                            vm.Origen = 2;
                        }

                        cargaObjetivos();
                    }
                    else {
                        vm.datosProducto = {};
                    }
                });
        }


        function cargaObjetivos() {
            if (vm.datosProducto !== undefined && vm.datosProducto !== null) {
                var i = 0;
                var index = 0;
                vm.datosProducto.Objetivos.map(function (itemObjetivo) {
                    index++;
                    itemObjetivo.index = index;
                    itemObjetivo.NombreCorto = mapearNombreObjetivoEspecifico(itemObjetivo.ObjetivoEspecifico);
                    itemObjetivo.LabelBotonObjetivo = '+';
                    itemObjetivo.Productos.map(function (itemProducto) {
                        itemProducto.LabelBotonProducto = '+';
                        itemProducto.HabilitaEditarProducto = false;
                        itemProducto.Valores.map(function (itemVigencia) {
                            itemVigencia.HabilitarEditarVigencia = false;
                        });
                    });
                });
                vm.datosProducto.Objetivos.map(function (itemObjetivo) {
                    var indexproducto = 0;
                    itemObjetivo.Productos.map(function (itemProducto, index) {
                        indexproducto++;
                        itemProducto.indexProducto = indexproducto;
                        
                        if (vm.datosProducto.EsConstante) {
                            if (itemProducto.valoresTotalesVigencia === undefined) {
                                itemProducto.valoresTotalesVigencia = {
                                    ValorUtilizadoNacion: 0,
                                    ValorUtilizadoPropios: 0,
                                    ValorReprogramadoNacion: 0,
                                    ValorReprogramadoPropios: 0,
                                    TotalValorUtilizado: 0,
                                    TotalValorReprogramado: 0

                                }
                            }
                        }
                        if (itemProducto.valoresTotalesCorrientesVigencia === undefined) {
                            itemProducto.valoresTotalesCorrientesVigencia = {
                                ValorUtilizadoNacion: 0,
                                ValorUtilizadoPropios: 0,
                                ValorReprogramadoNacion: 0,
                                ValorReprogramadoPropios: 0,
                                TotalValorUtilizado: 0,
                                TotalValorReprogramado: 0

                            }
                        }
                        
                        if (vm.datosProducto.EsConstante) {
                            itemProducto.Valores.map(function (itemValores) {
                                itemProducto.valoresTotalesVigencia = limpiaValores(itemProducto.valoresTotalesVigencia);
                            });
                            itemProducto.Valores.map(function (itemValores) {
                                itemProducto.valoresTotalesVigencia.ValorUtilizadoNacion += itemValores.UtilizadoNacion;
                                itemProducto.valoresTotalesVigencia.ValorUtilizadoPropios += itemValores.UtilizadoPropios;
                                itemProducto.valoresTotalesVigencia.ValorReprogramadoNacion += itemValores.ReprogramadoNacion;
                                itemProducto.valoresTotalesVigencia.ValorReprogramadoPropios += itemValores.ReprogramadoPropios;
                                itemProducto.valoresTotalesVigencia.TotalValorUtilizado += (itemValores.UtilizadoNacion + itemValores.UtilizadoPropios);
                                itemProducto.valoresTotalesVigencia.TotalValorReprogramado += (itemValores.ReprogramadoNacion + itemValores.ReprogramadoPropios);
                            });
                        }
                        else {
                            itemProducto.ValoresCorrientes = itemProducto.Valores;
                        }
                        itemProducto.ValoresCorrientes.map(function (itemValores) {
                            itemProducto.valoresTotalesCorrientesVigencia = limpiaValores(itemProducto.valoresTotalesCorrientesVigencia);
                        });
                        itemProducto.ValoresCorrientes.map(function (itemValores) {
                            itemProducto.valoresTotalesCorrientesVigencia.ValorUtilizadoNacion += itemValores.UtilizadoNacion;
                            itemProducto.valoresTotalesCorrientesVigencia.ValorUtilizadoPropios += itemValores.UtilizadoPropios;
                            itemProducto.valoresTotalesCorrientesVigencia.ValorReprogramadoNacion += itemValores.ReprogramadoNacion;
                            itemProducto.valoresTotalesCorrientesVigencia.ValorReprogramadoPropios += itemValores.ReprogramadoPropios;
                            itemProducto.valoresTotalesCorrientesVigencia.TotalValorUtilizado += (itemValores.UtilizadoNacion + itemValores.UtilizadoPropios);
                            itemProducto.valoresTotalesCorrientesVigencia.TotalValorReprogramado += (itemValores.ReprogramadoNacion + itemValores.ReprogramadoPropios);
                        });

                    });
                });


            }
        }

        function limpiaValores(item) {
            item.ValorUtilizadoNacion = 0;
            item.ValorUtilizadoPropios = 0;
            item.ValorReprogramadoNacion = 0;
            item.ValorReprogramadoPropios = 0;
            item.TotalValorUtilizado = 0;
            item.TotalValorReprogramado = 0;
            return item;
        }

        function initTablaVigenciaFuturaProducto() {
            vm.vigenciaActual = new Date().getFullYear();



            //Validaciones
            //vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponenteDatosIniciales, esValido: true });

        }

        vm.mostrarTab = function (origen) {
            vm.pagina = origen;

            switch (origen) {
                case 1:
                    {
                        //Constante
                        vm.Origen = 1;
                        break;
                    }
                case 2:
                    {
                        //Corriente
                        vm.Origen = 2;
                        break;
                    }
            }

            setTimeout(function () {
            }, 200);
        }

        function Guardar(producto) {
            producto.HabilitaEditarProducto = true;

            var productosValores = [];
            if (vm.datosProducto.EsConstante) {
                producto.Valores.map(function (item) {
                    var ReprogramacionValores = {};
                    ReprogramacionValores.ProyectoId = vm.datosProducto.ProyectoId;
                    ReprogramacionValores.TramiteId = vm.datosProducto.TramiteId;
                    ReprogramacionValores.ProductoId = producto.ProductoId;
                    ReprogramacionValores.PeriodoProyectoId = item.PeriodoProyectoId;
                    ReprogramacionValores.ReprogramadoNacion = item.ReprogramadoNacion;
                    ReprogramacionValores.ReprogramadoPropios = item.ReprogramadoPropios
                    productosValores.push(ReprogramacionValores);
                });
            }
            else {
                producto.ValoresCorrientes.map(function (item) {
                    var ReprogramacionValores = {};
                    ReprogramacionValores.ProyectoId = vm.datosProducto.ProyectoId;
                    ReprogramacionValores.TramiteId = vm.datosProducto.TramiteId;
                    ReprogramacionValores.ProductoId = producto.ProductoId;
                    ReprogramacionValores.PeriodoProyectoId = item.PeriodoProyectoId;
                    ReprogramacionValores.ReprogramadoNacion = item.ReprogramadoNacion;
                    ReprogramacionValores.ReprogramadoPropios = item.ReprogramadoPropios
                    productosValores.push(ReprogramacionValores);
                });
            }



            return comunesServicio.guardarReprogramacionPorProductoVigencia(productosValores).then(
                function (response) {
                    if (response.data.Mensaje === "OK") {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                        producto.HabilitaEditarProducto = false;
                        guardarCapituloModificado();
                        utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito");
                        //angular.forEach(indicador.Vigencias, function (series) {
                        //    series.MetaVigencialIndicadorAjusteOriginal = parseFloat(series.MetaVigencialIndicadorAjuste).toFixed(4);
                        //});
                        vm.initTablaVigenciaFuturaProducto();
                        ObtenerResumenReprogramacionPorVigencia();
                        vm.modificodatos = '1';
                    } else {
                        var mensajeError = JSON.parse(response.data.Message);
                        var mensajeReturn = '';

                        try {
                            for (var i = 0; i < mensajeError.ListaErrores.length; i++) {
                                mensajeReturn = mensajeReturn + mensajeError.ListaErrores[i].Error + '\n';
                            }

                        }
                        catch {
                            mensajeReturn = mensajeError.Mensaje;
                        }
                        utilidades.mensajeError(mensajeReturn, false);
                    }

                }).catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });
        }

        function Cancelar(producto, productoId, ObjetivoId) {
            producto.HabilitaEditarProducto = false;

            //var objetivoOriginal = $scope.datosOriginales.ObjetivosEspecificos.find(element => element.ObjetivoId == ObjetivoId);
            //var productoOriginal = objetivoOriginal.Productos.find(element => element.ProductoId == productoId);
            //var indicadorOriginal = productoOriginal.Indicadores.find(element => element.IndicadorId == indicador.IndicadorId);

            //indicador.Vigencias = indicadorOriginal.Vigencias;
            //indicador.IndicadorAcumula = indicador.IndicadorAcumulaOriginal;
            //indicador.IndicadorAcumulaAjustado = indicador.IndicadorAcumulaAjustadoOriginal;
            //indicador.MetaTotalFirmeAjustado = indicador.MetaTotalFirmeAjustadoOriginal;
            //angular.forEach(indicador.Vigencias, function (series) {
            //    series.MetaVigencialIndicadorAjuste = parseFloat(series.MetaVigencialIndicadorAjusteOriginal).toFixed(4);
            //});
        }

        vm.actualizaFila = function (event, vig) {

            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.0000);
            }

            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.0000);
            }

            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.0000);
            }

            event.target.value = parseFloat(event.target.value.replace(",", "."));

            //if (vig.IndicadorAcumula) {
            //    var acumula = 0;
            //    angular.forEach(indicador.Vigencias, function (series) {
            //        acumula = acumula + parseFloat(series.MetaVigencialIndicadorAjuste);
            //        indicador.MetaTotalFirmeAjustado = parseFloat(acumula.toFixed(4));
            //    });
            //} else {
            //    indicador.MetaTotalFirmeAjustado = Math.max.apply(Math, indicador.Vigencias.map(function (item) { return item.MetaVigencialIndicadorAjuste; }));
            //}

            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
        }


        function ProductoVerMas(prod) {
            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModal.html',
                controller: 'objetivosIndicadorModalController',
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

        function mapearNombreObjetivoEspecifico(nombreObjetivoEspecifico) {
            if (nombreObjetivoEspecifico !== undefined && nombreObjetivoEspecifico.length > 110)
                return nombreObjetivoEspecifico.substring(0, 110);
            else
                return nombreObjetivoEspecifico;

        }

        vm.verNombreCompleto = function (idVerMas, ObjetivoEspecificoId) {
            if (document.getElementById(idVerMas).classList.contains("proyecto-nombreFP")) {
                document.getElementById(idVerMas).classList.remove("proyecto-nombreFP");
                document.getElementById(idVerMas).classList.add("proyecto-nombreFP-completo");
                document.getElementById(idVerMas).innerText = vm.datosProducto.Objetivos.find(w => w.ObjetivoEspecificoId == ObjetivoEspecificoId).ObjetivoEspecifico;
                document.getElementById("btnVerMasNombre-" + ObjetivoEspecificoId).innerText = "Ver menos"
            } else {
                document.getElementById(idVerMas).classList.remove("proyecto-nombreFP-completo");
                document.getElementById(idVerMas).classList.add("proyecto-nombreFP");
                document.getElementById(idVerMas).innerText = vm.datosProducto.Objetivos.find(w => w.ObjetivoEspecificoId == ObjetivoEspecificoId).NombreCorto;
                document.getElementById("btnVerMasNombre-" + ObjetivoEspecificoId).innerText = "Ver más"
            }
        }

        vm.BtnObjetivos = function (objetivo) {
            if (objetivo.LabelBotonObjetivo == '+') {
                objetivo.LabelBotonObjetivo = '-'
            } else {
                objetivo.LabelBotonObjetivo = '+'
            }
            return objetivo.LabelBotonObjetivo;
        }

        vm.BtnProductos = function (prod) {
            if (prod.LabelBotonProducto == '+') {
                prod.LabelBotonProducto = '-'
            } else {
                prod.LabelBotonProducto = '+'
            }
            return prod.LabelBotonProducto;
        }

        function habilitaEditar(producto) {

            producto.HabilitaEditarProducto = true;

            //angular.forEach(indicador.Vigencias, function (series) {
            //    series.MetaVigencialIndicadorAjuste = parseFloat(series.MetaVigencialIndicadorAjuste).toFixed(4);
            //});
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
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
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

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



        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function () {

            var campogeneral = document.getElementById('RVFP-');
            if (campogeneral != undefined) {
                campogeneral.innerHTML = "";
                campogeneral.classList.add('hidden');
            }



            vm.datosProducto.ResumenTramite[0].ValoresCorrientes.map(function (item) {
                var campogeneral1 = document.getElementById('RVFP-' + item.Vigencia);
                if (campogeneral1 != undefined) {
                    campogeneral1.innerHTML = "";
                    campogeneral1.classList.add('hidden');
                }
            });

            vm.datosProducto.ResumenTramite[0].ValoresCorrientes.map(function (item) {
                var campogeneral1 = document.getElementById('RVFC-' + item.Vigencia);
                if (campogeneral1 != undefined) {
                    campogeneral1.innerHTML = "";
                    campogeneral1.classList.add('hidden');
                }
            });


        }

        vm.notificacionValidacionPadre = function (errores) {
            //console.log("Validación  - CD Pvigencias futuras");
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == null ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {
                            var error = p.Error.substring(0, 5);
                            if (vm.errores[error] != undefined) vm.errores[error](p.Error, p.Descripcion, nombreerror);
                        });

                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });

            }
        }



        vm.validarReprogramacionProducto = function (error, errores, nombreerror) {
            var campoObligatorioJustificacion = document.getElementById(error);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span>" +
                    "<span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
            var campogeneral = document.getElementById(nombreerror);
            if (campogeneral != undefined) {
                campogeneral.innerHTML = "<span class='d-inline-block ico-advertencia'></span>" +
                    "<span>" + errores + "</span>";
                campogeneral.classList.remove('hidden');
            }
        }


        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + 'reprogramacionvfreprogramacionporproducto');
            vm.seccionCapitulo = span.textContent;


        }


        vm.errores = {
            'RVFP-': vm.validarReprogramacionProducto,
            'RVFPC-': vm.validarReprogramacionProducto



        }

        /* ------------------------ FIN Validaciones ---------------------------------*/



    }

    angular.module('backbone').component('tablaVigenciaFuturaProductoFormulario', {

        templateUrl: "src/app/formulario/ventanas/comun/tablaVigenciaFuturaProducto/tablaVigenciaFuturaProductoFormulario.html",
        controller: tablaVigenciaFuturaProductoFormulario,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
            modificodatos: '=',
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            nombrecomponentepaso: '@',
            vigenciaadicionada: '='

        }
    });
})();
