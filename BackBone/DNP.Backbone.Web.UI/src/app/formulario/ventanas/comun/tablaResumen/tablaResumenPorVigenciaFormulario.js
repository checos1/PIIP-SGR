(function () {
    'use strict';

    tablaResumenPorVigenciaFormulario.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        '$timeout',
        'justificacionCambiosServicio',
        'comunesServicio'
    ];

    function tablaResumenPorVigenciaFormulario(
        $scope,
        $sessionStorage,
        utilidades,
        $timeout,
        justificacionCambiosServicio,
        comunesServicio
    ) {
        /*Varibales */

        var vm = this;
        vm.lang = "es";
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.tramiteid = $sessionStorage.TramiteId;
        vm.Origen = 0;
        vm.ObjetoVerMas = ObjetoVerMas;
        vm.nombreComponente = "reprogramacionvfreprogramacionporvigencia";
        vm.notificacionCambiosCapitulos = null;



        vm.handlerComponentes = [];
        vm.handlerComponentesChecked = {};
        vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1
        vm.seccionCapitulo = null;

        vm.errores = {
            constante: false,
            corriente: false
        };

        vm.pagina = 1;

        /*declara metodos*/

        vm.CancelarValores = CancelarValores;
        vm.EditarValores = EditarValores;
        vm.GuardarValores = GuardarValores;
        vm.AgregarVigencia = AgregarVigencia;
        vm.EliminarVigencia = EliminarVigencia;
        vm.ConvertirNumero = ConvertirNumero;

        ////Ajustar para que traiga el tramiteid
        //$scope.$watch('vm.tramiteid', function () {
        //    if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
        //        ObtenerResumenReprogramacionPorVigencia();
        //    }
        //});

        $scope.$watch(() => $sessionStorage.actualizaresumen
            , (newVal, oldVal) => {
                if (newVal != oldVal) {
                    if (vm.tramiteid !== undefined && vm.tramiteid !== '')
                        ObtenerResumenReprogramacionPorVigencia();
                }
            }, true);

        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });

            //eliminar la siguiente funcion
            ObtenerResumenReprogramacionPorVigencia();
        };

        /*Funciones*/

        function ObtenerResumenReprogramacionPorVigencia() {
            return comunesServicio.ObtenerResumenReprogramacionPorVigencia(vm.instanciaId, vm.proyectoId, vm.tramiteid).then(
                function (respuesta) {
                    if (respuesta.data !== '') {
                        var arreglolistas = jQuery.parseJSON(respuesta.data);
                        $scope.datos = jQuery.parseJSON(arreglolistas);

                        vm.Origen = 1;
                    }
                    else {
                        $scope.datos = [];
                    }
                });
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

        function CancelarValores(datos) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                datos.EditarAjusteConTramite = false;

                return comunesServicio.ObtenerResumenReprogramacionPorVigencia(vm.instanciaId, vm.proyectoId, vm.tramiteid).then(
                    function (respuesta) {
                        if (respuesta.data !== '') {
                            var arreglolistas = jQuery.parseJSON(respuesta.data);
                            $scope.datos = jQuery.parseJSON(arreglolistas);
                            utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                        }
                        else {
                            $scope.datos = [];
                        }

                    });

            }, function funcionCancelar(reason) {
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function EditarValores(datos) {
            datos.EditarAjusteConTramite = true;
        }

        function AgregarVigencia(datos) {
            var ultimaVigencia = 0;
            var listaDetalleVigencia = [];

            if (vm.Origen == 1) {
                listaDetalleVigencia = datos.Totales[0].TotalesTipoRecurso[0].DetalleVigencia;
            }
            else {
                listaDetalleVigencia = datos.TotalesCorrientes[0].TotalesTipoRecurso[0].DetalleVigencia;
            }

            listaDetalleVigencia.forEach(vigencia => {
                ultimaVigencia = vigencia.Vigencia;
            });


            if (datos.VigenciaFinal > ultimaVigencia) {
                listaDetalleVigencia.push({
                    PeriodoProyectoId: 0,
                    Vigencia: ultimaVigencia + 1,
                    Deflactor: 0.0000,
                    AutorizadoNacion: 0.00,
                    AutorizadoPropios: 0.00,
                    UtilizadoNacion: 0.00,
                    UtilizadoPropios: 0.00,
                    ReprogramadoNacion: 0.00,
                    ReprogramadoPropios: 0.00,
                    ReprogramadoNacionOriginal: 0.00,
                    ReprogramadoPropiosOriginal: 0.00,
                    ModificadoNacion: 0.00,
                    ModificadoPropios: 0.00,
                    Eliminar: true
                });

                return listaDetalleVigencia;
            }
            else {
                utilidades.mensajeError("", null, "No se pueden agregar vigencias, porque se excede el horizonte del proyecto.");
            }
        }

        //Se elimina a nivel de vista; se eliminan nuevos y existentes que no existan en vigencias futuras
        function EliminarVigencia(datos, vigencia) {
            var msg = "";
            var ultimaVigencia = 0;
            var listaDetalleVigencia = [];

            if (vm.Origen == 1) {
                listaDetalleVigencia = datos.Totales[0].TotalesTipoRecurso[0].DetalleVigencia;
            }
            else {
                listaDetalleVigencia = datos.TotalesCorrientes[0].TotalesTipoRecurso[0].DetalleVigencia;
            }

            listaDetalleVigencia.forEach(vigencia => {
                ultimaVigencia = vigencia.Vigencia;
            });

            msg = "Se van a eliminar los datos de la vigencia " + vigencia + "."

            if (vigencia == ultimaVigencia) {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                    listaDetalleVigencia.forEach(function (item, index, object) {
                        if (item.Vigencia === vigencia) {
                            object.splice(index, 1);
                        }
                    });

                    var TotalReprogramadoNacion = 0;
                    var TotalReprogramadoPropios = 0;
                    var TotalModificadoNacion = 0;
                    var TotalModificadoPropios = 0;

                    angular.forEach(listaDetalleVigencia, function (series) {
                        TotalReprogramadoNacion = TotalReprogramadoNacion + parseFloat(series.ReprogramadoNacion);
                        TotalReprogramadoPropios = TotalReprogramadoPropios + parseFloat(series.ReprogramadoPropios);

                        //Este campo se calcula a partir de la resta del Valor Utilizado menos el Valor Reprogramado
                        series.ModificadoNacion = series.UtilizadoNacion - series.ReprogramadoNacion;
                        series.ModificadoPropios = series.UtilizadoPropios - series.ReprogramadoPropios;

                        TotalModificadoNacion = TotalModificadoNacion + parseFloat(series.ModificadoNacion);
                        TotalModificadoPropios = TotalModificadoPropios + parseFloat(series.ModificadoPropios);
                    });

                    if (vm.Origen == 1) {
                        $scope.datos.Totales[0].TotalesTipoRecurso[0].TotalReprogramadoNacion = TotalReprogramadoNacion;
                        $scope.datos.Totales[0].TotalesTipoRecurso[0].TotalReprogramadoPropios = TotalReprogramadoPropios;
                        $scope.datos.Totales[0].TotalReprogramado = TotalReprogramadoNacion + TotalReprogramadoPropios;

                        $scope.datos.Totales[0].TotalesTipoRecurso[0].TotalModificadoNacion = TotalModificadoNacion;
                        $scope.datos.Totales[0].TotalesTipoRecurso[0].TotalModificadoPropios = TotalModificadoPropios;
                        $scope.datos.Totales[0].TotalModificado = TotalModificadoNacion + TotalModificadoPropios;
                    }
                    else {
                        $scope.datos.TotalesCorrientes[0].TotalesTipoRecurso[0].TotalReprogramadoNacion = TotalReprogramadoNacion;
                        $scope.datos.TotalesCorrientes[0].TotalesTipoRecurso[0].TotalReprogramadoPropios = TotalReprogramadoPropios;
                        $scope.datos.TotalesCorrientes[0].TotalReprogramado = TotalReprogramadoNacion + TotalReprogramadoPropios;

                        $scope.datos.TotalesCorrientes[0].TotalesTipoRecurso[0].TotalModificadoNacion = TotalModificadoNacion;
                        $scope.datos.TotalesCorrientes[0].TotalesTipoRecurso[0].TotalModificadoPropios = TotalModificadoPropios;
                        $scope.datos.TotalesCorrientes[0].TotalModificado = TotalModificadoNacion + TotalModificadoPropios;
                    }

                    return comunesServicio.ObtenerResumenReprogramacionPorVigencia(vm.instanciaId, vm.proyectoId, vm.tramiteid).then(
                        function (respuesta) {
                            utilidades.mensajeSuccess("", false, false, false, "Los datos de la vigencia " + vigencia + " se han eliminado con éxito.");
                        });

                }, function funcionCancelar(reason) {
                }, null, null, msg);
            }
            else {
                utilidades.mensajeError("", null, "Existe una vigencia posterior asociada al proceso");
            }
        }

        function GuardarValores(datos) {
            let Reprogramacion = {};
            let ValoresReprogramacion = [];
            var listaDetalleVigencia = [];

            if (vm.Origen == 1) {
                listaDetalleVigencia = datos.Totales[0].TotalesTipoRecurso[0].DetalleVigencia;
            }
            else {
                listaDetalleVigencia = datos.TotalesCorrientes[0].TotalesTipoRecurso[0].DetalleVigencia;
            }

            angular.forEach(listaDetalleVigencia, function (series) {
                let constante = {
                    Vigencia: series.Vigencia,
                    ReprogramadoNacion: series.ReprogramadoNacion,
                    ReprogramadoPropios: series.ReprogramadoPropios,
                    ValoresReprogramado: true
                };

                ValoresReprogramacion.push(constante);
            });

            Reprogramacion.TramiteId = datos.TramiteId;
            Reprogramacion.ProyectoId = datos.ProyectoId;
            Reprogramacion.ValoresReprogramacion = ValoresReprogramacion;

            return comunesServicio.GuardarDatosReprogramacion(Reprogramacion).then(
                function (respuesta) {
                    if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        if (respuesta.data.Exito) {
                            guardarCapituloModificado();
                            vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                            utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito");
                            datos.EditarAjusteConTramite = false;
                            ObtenerResumenReprogramacionPorVigencia();
                            vm.init();
                            vm.limpiarErrores();
                            vm.vigenciaadicionada = '1';

                        }
                        else {
                            utilidades.mensajeError(respuesta.data.Mensaje);
                        }
                    } else {
                        utilidades.mensajeError("", null, "Error al realizar la operación");
                    }
                });
        }

        //para guardar los capitulos modificados y que se llenen las lunas

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.proyectoId,
                Justificacion: "",
                //SeccionCapituloId: vm.SeccionCapituloId,
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: false,
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
                ProyectoId: $sessionStorage.proyectoId,
                Justificacion: "",
                //SeccionCapituloId: vm.SeccionCapituloId,
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

        function ObjetoVerMas(resumen) {
            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/formulario/ventanas/ajustes/componentes/datosgenerales/indicadores/modal/objetivosIndicadorModal.html',
                controller: 'objetivosIndicadorModalController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "entidad-modal-adherencia",
                resolve: {
                    Objetivo: function () {
                        return resumen.NombreProyecto;
                    },
                    IdObjetivo: function () {
                        return '';
                    },
                    Tipo: function () {
                        return 'Objeto';
                    },
                    Titulo: function () {
                        return 'Liberación Vigencias Futuras';
                    }
                },
            });
        }

        $scope.$watchCollection("vm.handlerComponentesChecked", function (newValue, oldValue) {
            var estado = true;
            var listHijos = Object.keys(vm.handlerComponentesChecked);
            if (listHijos.length == 0 || newValue === oldValue) {
                return;
            }
            listHijos.forEach(p => {
                if (vm.handlerComponentesChecked[p] == false) {
                    estado = false;
                }
            });
            vm.notificacionestado({ estado: estado, nombreComponente: vm.nombreComponente });
        });

        vm.inicializarComponenteCheck = function () {
            vm.handlerComponentesChecked = {
            };
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 12;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
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
            } else {
                if (tamanio > 12 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 12) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 15) {
                    event.preventDefault();
                }
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

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
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

        vm.actualizaFila = function (event, datos, tiporecurso) {
            if (Number.isNaN(event.target.value)) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == null) {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            if (event.target.value == "") {
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(0.00);
            }

            event.target.value = parseFloat(event.target.value.replace(",", "."));

            var listaDetalleVigencia = [];

            if (vm.Origen == 1) {
                listaDetalleVigencia = datos.Totales[0].TotalesTipoRecurso[0].DetalleVigencia;
            }
            else {
                listaDetalleVigencia = datos.TotalesCorrientes[0].TotalesTipoRecurso[0].DetalleVigencia;
            }

            var TotalReprogramadoNacion = 0;
            var TotalReprogramadoPropios = 0;
            var TotalModificadoNacion = 0;
            var TotalModificadoPropios = 0;

            angular.forEach(listaDetalleVigencia, function (series) {
                TotalReprogramadoNacion = TotalReprogramadoNacion + parseFloat(series.ReprogramadoNacion);
                TotalReprogramadoPropios = TotalReprogramadoPropios + parseFloat(series.ReprogramadoPropios);

                //Este campo se calcula a partir de la resta del Valor Utilizado menos el Valor Reprogramado
                series.ModificadoNacion = series.UtilizadoNacion - series.ReprogramadoNacion;
                series.ModificadoPropios = series.UtilizadoPropios - series.ReprogramadoPropios;

                TotalModificadoNacion = TotalModificadoNacion + parseFloat(series.ModificadoNacion);
                TotalModificadoPropios = TotalModificadoPropios + parseFloat(series.ModificadoPropios);
            });

            if (vm.Origen == 1) {
                $scope.datos.Totales[0].TotalesTipoRecurso[0].TotalReprogramadoNacion = TotalReprogramadoNacion;
                $scope.datos.Totales[0].TotalesTipoRecurso[0].TotalReprogramadoPropios = TotalReprogramadoPropios;
                $scope.datos.Totales[0].TotalReprogramado = TotalReprogramadoNacion + TotalReprogramadoPropios;

                $scope.datos.Totales[0].TotalesTipoRecurso[0].TotalModificadoNacion = TotalModificadoNacion;
                $scope.datos.Totales[0].TotalesTipoRecurso[0].TotalModificadoPropios = TotalModificadoPropios;
                $scope.datos.Totales[0].TotalModificado = TotalModificadoNacion + TotalModificadoPropios;
            }
            else {
                $scope.datos.TotalesCorrientes[0].TotalesTipoRecurso[0].TotalReprogramadoNacion = TotalReprogramadoNacion;
                $scope.datos.TotalesCorrientes[0].TotalesTipoRecurso[0].TotalReprogramadoPropios = TotalReprogramadoPropios;
                $scope.datos.TotalesCorrientes[0].TotalReprogramado = TotalReprogramadoNacion + TotalReprogramadoPropios;

                $scope.datos.TotalesCorrientes[0].TotalesTipoRecurso[0].TotalModificadoNacion = TotalModificadoNacion;
                $scope.datos.TotalesCorrientes[0].TotalesTipoRecurso[0].TotalModificadoPropios = TotalModificadoPropios;
                $scope.datos.TotalesCorrientes[0].TotalModificado = TotalModificadoNacion + TotalModificadoPropios;
            }

            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        /* ------------------------ Validaciones ---------------------------------*/

        /**
        * Función que recibe los estados de los componentes hijos
        * @param {any} esValido true: valido, false: invalido
        * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
        */
        vm.notificacionEstado = function (nombreComponente, esValido) {
            var indx = vm.handlerComponentes.findIndex(p => p.componente == nombreComponente);
            vm.esValido = !esValido ? false : vm.esValido;
            vm.handlerComponentes[indx].esValido = esValido;
            vm.handlerComponentesChecked[nombreComponente] = esValido;
            vm.showAlertError(nombreComponente, esValido);
        }

        /**
         * Función que visualiza alerta de error tab de componente
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        vm.showAlertError = function (nombreComponente, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + nombreComponente);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    if (erroresJson != undefined) {
                        isValid = (erroresJson == null || erroresJson.length == 0);
                        if (!isValid) {
                            erroresJson[vm.nombreComponente].forEach(p => {

                                if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                            });
                        }
                    }
                }

                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.limpiarErrores = function () {
            vm.errores.constante = false;
            vm.errores.corriente = false;
            var campoObligatorioConstante = document.getElementById(vm.nombreComponente + "-error");
            if (campoObligatorioConstante != undefined) {
                campoObligatorioConstante.innerHTML = "";
                campoObligatorioConstante.classList.add('hidden');
            }
        }

        vm.validarConstantes = function (errores) {
            vm.errores.constante = true;
            var campoObligatorioConstante = document.getElementById(vm.nombreComponente + "-error");
            if (campoObligatorioConstante != undefined) {
                campoObligatorioConstante.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioConstante.classList.remove('hidden');
            }
        }

        vm.validarCorrientes = function (errores) {
            vm.errores.corriente = true;
            var campoObligatorioCorriente = document.getElementById(vm.nombreComponente + "-error");
            if (campoObligatorioCorriente != undefined) {
                campoObligatorioCorriente.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioCorriente.classList.remove('hidden');
            }
        }

        vm.errores = {
            'ATRV001': vm.validarConstantes,
            'ATRV002': vm.validarCorrientes,
        }

        /* ------------------------ FIN Validaciones ---------------------------------*/
    }

    angular.module('backbone').component('tablaResumenPorVigenciaFormulario', {

        templateUrl: "src/app/formulario/ventanas/comun/tablaResumen/tablaResumenPorVigenciaFormulario.html",
        controller: tablaResumenPorVigenciaFormulario,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            vigenciaadicionada: "=",
            //tipotramiteid: '@',
            //tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            actualizacomponentes: '@'
        }
    }).directive('stringToNumber', function () {
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
