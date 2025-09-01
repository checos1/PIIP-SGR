(function () {
    'use strict';

    resumenReprogramacionVfFormulario.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        '$timeout',
        'justificacionCambiosServicio',
        'comunesServicio'
    ];

    function resumenReprogramacionVfFormulario(
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
        vm.ObjetoVerMas = ObjetoVerMas;
        vm.nombreComponente = "informacionpresupuestalresumenreprogramacion";
        vm.notificacionCambiosCapitulos = null;

        vm.Origen = 1;
        vm.OrigenProducto = 1;
        vm.pagina = 1;
        vm.paginaProducto = 1;
        vm.IndexProducto = 0;

        //Totales Constantes Vigencia
        
        vm.sumUtilizadoNacionConstanteVig = 0;
        vm.sumUtilizadoPropiosConstanteVig = 0;
        vm.sumReprogramadoNacionConstanteVig = 0;
        vm.sumReprogramadoPropiosConstanteVig = 0;
        vm.sumAprobadoNacionConstanteVig = 0;
        vm.sumAprobadoPropiosConstanteVig = 0;
        vm.sumModificadoNacionAprobadoConstanteVig = 0;
        vm.sumModificadoPropiosAprobadoConstanteVig = 0;
        
        vm.totalUtilizadoConstanteVig = 0;
        vm.totalReprogramadoConstanteVig = 0;
        vm.totalAprobadoConstanteVig = 0;
        vm.totalModificadoAprobadoConstanteVig = 0;

        //Totales Corrientes Vigencia        
        vm.sumUtilizadoNacionCorrienteVig = 0;
        vm.sumUtilizadoPropiosCorrienteVig = 0;
        vm.sumReprogramadoNacionCorrienteVig = 0;
        vm.sumReprogramadoPropiosCorrienteVig = 0;
        vm.sumAprobadoNacionCorrienteVig = 0;
        vm.sumAprobadoPropiosCorrienteVig = 0;
        vm.sumModificadoNacionAprobadoCorrienteVig = 0;
        vm.sumModificadoPropioAprobadosCorrienteVig = 0;
        
        vm.totalUtilizadoCorrienteVig = 0;
        vm.totalReprogramadoCorrienteVig = 0;
        vm.totalAprobadoCorrienteVig = 0;
        vm.totalModificadoAprobadoCorrienteVig = 0;

        vm.handlerComponentes = [];
        vm.handlerComponentesChecked = {};
        vm.habilitaBotones = true;
        vm.seccionCapitulo = null;

        vm.errores = {
            constante: false,
            corriente: false
        };

        /*declara metodos*/

        vm.CancelarValores = CancelarValores;
        vm.EditarValores = EditarValores;
        vm.GuardarValores = GuardarValores;
        vm.ConvertirNumero = ConvertirNumero;

        $scope.$watch('vm.rolanalista', function () {
            if (vm.rolanalista !== '' && vm.rolanalista !== undefined && vm.rolanalista !== null) {
                vm.habilitaBotones = vm.rolanalista.toLowerCase() === 'true' && !$sessionStorage.soloLectura ? true : false;
            }
        });

        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

        };

        $scope.$watch('vm.tramiteid', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                consultarResumenReprogramacionPorProductoVigencia();
            }
        });

        /*Funciones*/

        function consultarResumenReprogramacionPorProductoVigencia() {
            let instanciaId = $sessionStorage.idInstancia;
            let proyectoId = 0;
            let tramiteId = vm.tramiteid;

            return comunesServicio.obtenerResumenReprogramacionPorProductoVigencia(instanciaId, proyectoId, tramiteId)
                .then(respuesta => {                   

                    if (respuesta.data !== '') {
                        $scope.datos = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));

                        if ($scope.datos) {
                            if ($scope.datos.ResumenTramite) {

                                //Totales Vigencias constantes
                                if ($scope.datos.ResumenTramite[0].Valores) {
                                    let valoresConstantes = $scope.datos.ResumenTramite[0].Valores;

                                    vm.sumUtilizadoNacionConstanteVig = 0;
                                    vm.sumUtilizadoPropiosConstanteVig = 0;
                                    vm.sumReprogramadoNacionConstanteVig = 0;
                                    vm.sumReprogramadoPropiosConstanteVig = 0;
                                    vm.sumAprobadoNacionConstanteVig = 0;
                                    vm.sumAprobadoPropiosConstanteVig = 0;
                                    vm.sumModificadoNacionAprobadoConstanteVig = 0;
                                    vm.sumModificadoPropiosAprobadoConstanteVig = 0;

                                    valoresConstantes.forEach(item => {
                                        vm.sumUtilizadoNacionConstanteVig += item.UtilizadoNacion;
                                        vm.sumUtilizadoPropiosConstanteVig += item.UtilizadoPropios;
                                        vm.sumReprogramadoNacionConstanteVig += item.ReprogramadoNacion;
                                        vm.sumReprogramadoPropiosConstanteVig += item.ReprogramadoPropios;
                                        vm.sumAprobadoNacionConstanteVig += item.AprobadoNacion;
                                        vm.sumAprobadoPropiosConstanteVig += item.AprobadoPropios;
                                        vm.sumModificadoNacionAprobadoConstanteVig += item.ModificadoNacionAprobado;
                                        vm.sumModificadoPropiosAprobadoConstanteVig += item.ModificadoPropiosAprobado;
                                    });


                                    vm.totalUtilizadoConstanteVig = vm.sumUtilizadoNacionConstanteVig + vm.sumUtilizadoPropiosConstanteVig;
                                    vm.totalReprogramadoConstanteVig = vm.sumReprogramadoNacionConstanteVig + vm.sumReprogramadoPropiosConstanteVig;
                                    vm.totalAprobadoConstanteVig = vm.sumAprobadoNacionConstanteVig + vm.sumAprobadoPropiosConstanteVig;
                                    vm.totalModificadoAprobadoConstanteVig = vm.sumModificadoNacionAprobadoConstanteVig + vm.sumModificadoPropiosAprobadoConstanteVig;

                                }

                                //Totales Vigencias corrientes
                                if ($scope.datos.ResumenTramite[0].ValoresCorrientes) {
                                    let valoresCorrientes = $scope.datos.ResumenTramite[0].ValoresCorrientes;

                                    vm.sumUtilizadoNacionCorrienteVig = 0;
                                    vm.sumUtilizadoPropiosCorrienteVig = 0;
                                    vm.sumReprogramadoNacionCorrienteVig = 0;
                                    vm.sumReprogramadoPropiosCorrienteVig = 0;
                                    vm.sumAprobadoNacionCorrienteVig = 0;
                                    vm.sumAprobadoPropiosCorrienteVig = 0;
                                    vm.sumModificadoNacionAprobadoCorrienteVig = 0;
                                    vm.sumModificadoPropiosAprobadoCorrienteVig = 0;

                                    valoresCorrientes.forEach(item => {

                                        vm.sumUtilizadoNacionCorrienteVig += item.UtilizadoNacion;
                                        vm.sumUtilizadoPropiosCorrienteVig += item.UtilizadoPropios;
                                        vm.sumReprogramadoNacionCorrienteVig += item.ReprogramadoNacion;
                                        vm.sumReprogramadoPropiosCorrienteVig += item.ReprogramadoPropios;
                                        vm.sumAprobadoNacionCorrienteVig += item.AprobadoNacion;
                                        vm.sumAprobadoPropiosCorrienteVig += item.AprobadoPropios;
                                        vm.sumModificadoNacionAprobadoCorrienteVig += item.ModificadoNacionAprobado;
                                        vm.sumModificadoPropiosAprobadoCorrienteVig += item.ModificadoPropiosAprobado;
                                    });

                                    vm.totalUtilizadoCorrienteVig = vm.sumUtilizadoNacionCorrienteVig + vm.sumUtilizadoPropiosCorrienteVig;
                                    vm.totalReprogramadoCorrienteVig = vm.sumReprogramadoNacionCorrienteVig + vm.sumReprogramadoPropiosCorrienteVig;
                                    vm.totalAprobadoCorrienteVig = vm.sumAprobadoNacionCorrienteVig + vm.sumAprobadoPropiosCorrienteVig;
                                    vm.totalModificadoAprobadoCorrienteVig = vm.sumModificadoNacionAprobadoCorrienteVig + vm.sumModificadoPropiosAprobadoCorrienteVig;
                                }
                            }
                        }
                    }
                    else {
                        $scope.datos = [];
                    }                   
                })
                .catch(error => {
                    console.log(error);
                    utilidades.mensajeError("Hubo un error al cargar la tabla resumen de reprogramación");
                });
        }

        vm.AbrilNivel = function (objeto) {
            vm.paginaProducto = 1;
            vm.OrigenProducto = 1;

            var variable = $("#ico" + objeto).attr("src");

            if (variable === "Img/btnMas.svg") {
                $("#ico" + objeto).attr("src", "Img/btnMenos.svg");
            }
            else {
                $("#ico" + objeto).attr("src", "Img/btnMas.svg");
            }
        }

        vm.mostrarTab = function (origen) {
            vm.pagina = origen;
            vm.Origen = origen;

            setTimeout(function () {
            }, 200);
        }

        vm.mostrarTabProducto = function (rowIndexProd, origen) {
            vm.paginaProducto = origen;
            vm.OrigenProducto = origen
            vm.IndexProducto = rowIndexProd;

            setTimeout(function () {
            }, 200);
        }

        function CancelarValores(datos) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                datos.EditarAjusteConTramite = false;
                let instanciaId = $sessionStorage.idInstancia;
                let proyectoId = 0;
                let tramiteId = vm.tramiteid;

                return comunesServicio.obtenerResumenReprogramacionPorProductoVigencia(instanciaId, proyectoId, tramiteId)
                    .then(respuesta => {

                        if (respuesta.data !== '') {
                            $scope.datos = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));

                            if ($scope.datos) {
                                if ($scope.datos.ResumenTramite) {

                                    vm.sumAprobadoNacionConstanteVig = 0;
                                    vm.sumAprobadoPropiosConstanteVig = 0;
                                    vm.sumModificadoNacionAprobadoConstanteVig = 0;
                                    vm.sumModificadoPropiosAprobadoConstanteVig = 0;

                                    vm.totalAprobadoConstanteVig = 0;
                                    vm.totalModificadoAprobadoConstanteVig = 0;

                                    //Totales Vigencias constantes
                                    if ($scope.datos.ResumenTramite[0].Valores) {
                                        let valoresConstantes = $scope.datos.ResumenTramite[0].Valores;

                                        valoresConstantes.forEach(item => {
                                            vm.sumAprobadoNacionConstanteVig += item.AprobadoNacion;
                                            vm.sumAprobadoPropiosConstanteVig += item.AprobadoPropios;
                                            vm.sumModificadoNacionAprobadoConstanteVig += item.ModificadoNacionAprobado;
                                            vm.sumModificadoPropiosAprobadoConstanteVig += item.ModificadoPropiosAprobado;
                                        });

                                        vm.totalAprobadoConstanteVig = vm.sumAprobadoNacionConstanteVig + vm.sumAprobadoPropiosConstanteVig;
                                        vm.totalModificadoAprobadoConstanteVig = vm.sumModificadoNacionAprobadoConstanteVig + vm.sumModificadoPropiosAprobadoConstanteVig;
                                    }
                                }
                            }

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

        function GuardarValores(datos) {
            let Aprobacion = {};
            let ValoresAprobados = [];
            var listaDetalleVigencia = [];

            if (vm.Origen == 1) {
                listaDetalleVigencia = datos.ResumenTramite[0].Valores;
            }
            else {
                listaDetalleVigencia = datos.ResumenTramite[0].ValoresCorrientes;
            }

            angular.forEach(listaDetalleVigencia, function (series) {
                let constante = {
                    Vigencia: series.Vigencia,
                    ReprogramadoNacion: series.AprobadoNacion,
                    ReprogramadoPropios: series.AprobadoPropios,
                    ValoresReprogramado: false
                };

                ValoresAprobados.push(constante);
            });

            Aprobacion.TramiteId = datos.TramiteId;
            Aprobacion.ProyectoId = datos.ProyectoId;
            Aprobacion.ValoresReprogramacion = ValoresAprobados;

            return comunesServicio.GuardarDatosReprogramacion(Aprobacion).then(
                function (respuesta) {
                    if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        if (respuesta.data.Exito) {
                            guardarCapituloModificado();
                            vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                            utilidades.mensajeSuccess("", false, false, false, "Los valores de aprobación fueron guardados y con éxito.");
                            vm.init();
                            vm.limpiarErrores();
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

        vm.abrirTooltip = function () {
            utilidades.mensajeInformacion('Los valores solicitados pueden aprobarse inicialmente mediante el botón "Guardar".'
                + '<br>Si necesita modificarlo, edite y guarde.'
                , false, "Resumen Reprogramación")
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

        vm.actualizaFila = function (event, datos) {
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

            vm.sumAprobadoNacionConstanteVig = 0;
            vm.sumAprobadoPropiosConstanteVig = 0;
            vm.sumModificadoNacionAprobadoConstanteVig = 0;
            vm.sumModificadoPropiosAprobadoConstanteVig = 0;

            vm.totalAprobadoConstanteVig = 0;
            vm.totalModificadoAprobadoConstanteVig = 0;

            if (datos) {
                if (datos.ResumenTramite) {

                    //Totales Vigencias Constantes
                    if (datos.ResumenTramite[0].Valores) {
                        let valoresConstantes = datos.ResumenTramite[0].Valores;

                        valoresConstantes.forEach(item => {
                            vm.sumAprobadoNacionConstanteVig += parseFloat(item.AprobadoNacion);
                            vm.sumAprobadoPropiosConstanteVig += parseFloat(item.AprobadoPropios);
                            item.ModificadoNacionAprobado = item.UtilizadoNacion - item.AprobadoNacion;
                            item.ModificadoPropiosAprobado = item.UtilizadoPropios - item.AprobadoPropios;
                            vm.sumModificadoNacionAprobadoConstanteVig += parseFloat(item.ModificadoNacionAprobado);
                            vm.sumModificadoPropiosAprobadoConstanteVig += parseFloat(item.ModificadoPropiosAprobado);
                        });

                        vm.totalAprobadoConstanteVig = vm.sumAprobadoNacionConstanteVig + vm.sumAprobadoPropiosConstanteVig;
                        vm.totalModificadoAprobadoConstanteVig = vm.sumModificadoNacionAprobadoConstanteVig + vm.sumModificadoPropiosAprobadoConstanteVig;
                    }
                }
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
                var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                if (erroresJson != undefined) {
                    isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.limpiarErrores = function () {
            var campoObligatorioConstante = document.getElementById(vm.nombreComponente + "-error");
            if (campoObligatorioConstante != undefined) {
                campoObligatorioConstante.innerHTML = "";
                campoObligatorioConstante.classList.add('hidden');
            }
        }

        vm.validarConstantes = function (errores) {
            var campoObligatorioConstante = document.getElementById(vm.nombreComponente + "-error");
            if (campoObligatorioConstante != undefined) {
                campoObligatorioConstante.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span> " + errores + "</span>";
                campoObligatorioConstante.classList.remove('hidden');
            }
        }

        vm.errores = {
            'IPF001': vm.validarConstantes,

        }

        /* ------------------------ FIN Validaciones ---------------------------------*/
    }

    angular.module('backbone').component('resumenReprogramacionVfFormulario', {

        templateUrl: "src/app/formulario/ventanas/comun/InformacionPresupuestalResumenRvf/resumenReprogramacionVfFormulario.html",
        controller: resumenReprogramacionVfFormulario,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            //tipotramiteid: '@',
            tramiteid: '@',
            rolanalista: '@',
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
