(function () {
    'use strict';

    listaProyectosRegistradosController.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        'comunesServicio',
        'appSettings'
    ];

    function listaProyectosRegistradosController(
        $scope,
        $sessionStorage,
        utilidades,
        comunesServicio,
        appSettings
    ) {
        /*Varibales */

        var vm = this;
        vm.lang = "es";
        vm.EntidadDestinoId = $sessionStorage.InstanciaSeleccionada.entidadId;
        vm.tramiteid = $sessionStorage.InstanciaSeleccionada.tramiteId;
        vm.idNivel = $sessionStorage.idNivel;
        vm.ObjetoVerMas = ObjetoVerMas;
        vm.nombreComponente = "distribuciondistribucionrecursos";
        vm.notificacionCambiosCapitulos = null;
        $sessionStorage.calendarioCerrado = false;
        vm.handlerComponentes = [];
        vm.handlerComponentesChecked = {};
        vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1
        vm.seccionCapitulo = null;
        vm.mostrardistribucion = false;
        vm.mostrarfuentes = false;
        vm.mostrarregionalizacion = false;
        vm.mostrarpoliticas = false;
        vm.mostrariniciativas = false;
        vm.TramiteProyectoId = undefined;
        vm.erroresJson = [];

        /*declara metodos*/

        vm.CancelarValores = CancelarValores;
        vm.EditarValores = EditarValores;
        vm.GuardarValores = GuardarValores;
        vm.ConvertirNumero = ConvertirNumero;

        $scope.datos = [];
        $scope.currentPage = 0;
        $scope.pageSize = appSettings.topePaginacionConsultaAplicaciones;;
        $scope.pages = [];

        $scope.$watch('$sessionStorage.TramiteId', function () {
            if ($sessionStorage.TramiteId !== '' && $sessionStorage.TramiteId !== undefined && $sessionStorage.TramiteId !== null) {
                vm.tramiteid = $sessionStorage.TramiteId;
                cargaVariables();
            }

        });
        
        
        $scope.$watch('vm.calendariodistribucioncuotas', function () {
            if (vm.calendariodistribucioncuotas !== undefined && vm.calendariodistribucioncuotas !== '')
                vm.habilitaBotones = vm.calendariodistribucioncuotas === 'true' && !$sessionStorage.soloLectura ? true : false;
          
        });

       
        $scope.$watch('vm.modificardistribucion', function () {
            if (vm.modificardistribucion === '2') {
                ObtenerProgramacionDistribucion();
                vm.modificardistribucion = '0';
            }
            
        });       

        $scope.$watch('vm.section', function () {

            switch (vm.section) {
                case 'distribucion':
                    vm.nombreComponente = "distribuciondistribucionrecursos";
                    vm.mostrardistribucion = true;
                    vm.mostrarfuentes = false;
                    vm.mostrarregionalizacion = false;
                    vm.mostrariniciativas = false;
                    vm.mostrarpoliticas = false;
                    break;
                case 'fuentes':
                    vm.nombreComponente = "fuentesfuentesdefinanc";
                    vm.mostrardistribucion = false;
                    vm.mostrarfuentes = true;
                    vm.mostrarregionalizacion = false;
                    vm.mostrariniciativas = false;
                    vm.mostrarpoliticas = false;
                    break;
                case 'regionalizacion':
                    vm.nombreComponente = "regionalizacionregionalizacion";
                    vm.mostrardistribucion = false;
                    vm.mostrarfuentes = false;
                    vm.mostrarregionalizacion = true;
                    vm.mostrariniciativas = false;
                    vm.mostrarpoliticas = false;
                    break;
                case 'iniciativas':
                    vm.mostrardistribucion = false;
                    vm.mostrarfuentes = false;
                    vm.mostrarregionalizacion = false;
                    vm.mostrariniciativas = true;
                    vm.mostrarpoliticas = false;
                    break;
                case 'politicastransversales':
                    vm.nombreComponente = "politicastransversalespoliticastransv";
                    vm.mostrardistribucion = false;
                    vm.mostrarfuentes = false;
                    vm.mostrarregionalizacion = false;
                    vm.mostrariniciativas = false;
                    vm.mostrarpoliticas = true;
                    break;
                default:
                    break;
            }

            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            ObtenerProgramacionDistribucion();
        });

       

        $scope.configPages = function () {
            if ($scope.datos !== undefined && $scope.datos.Proyectos !== undefined) {
                $scope.pages.length = 0;
                var ini = $scope.currentPage - 3;
                var fin = $scope.currentPage + 5;
                if (ini < 1) {
                    ini = 1;
                    //if (Math.ceil($scope.datos.Proyectos.length / $scope.pageSize) > 0)
                    //    fin = 1;
                    //else
                        fin = Math.ceil($scope.datos.Proyectos.length / $scope.pageSize);
                } else {
                    if (ini >= Math.ceil($scope.datos.Proyectos.length / $scope.pageSize) - 6) {
                        ini = Math.ceil($scope.datos.Proyectos.length / $scope.pageSize) - 6;
                        fin = Math.ceil($scope.datos.Proyectos.length / $scope.pageSize);
                    }
                }
                if (ini < 1) ini = 1;
                for (var i = ini; i <= fin; i++) {
                    $scope.pages.push({
                        no: i
                    });
                }

                if ($scope.currentPage >= $scope.pages.length)
                    $scope.currentPage = $scope.pages.length - 1;
            }
        };

        $scope.setPage = function (index) {
            $scope.currentPage = index - 1;
            setTimeout(function () {
                chgPage();
            }, 500);
        };

        function chgPage() {
            var isValid = true;
            isValid = (vm.erroresJson == null || vm.erroresJson.length == 0);
            if (!isValid) {
                vm.erroresJson[vm.nombreComponente].forEach(p => {

                    if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                });
            }
        }

        $scope.startFromGrid = function () {
            return function (input, start) {
                start = +start;
                return input.slice(start);
            }
        }

        vm.init = function () {           
            ObtenerProgramacionDistribucion();
        };

        /*Funciones*/

        function ObtenerProgramacionDistribucion() {
            if ($sessionStorage.TramiteId !== '' && $sessionStorage.TramiteId !== undefined && $sessionStorage.TramiteId !== null)
                return comunesServicio.ObtenerDatosProgramacionEncabezado(vm.EntidadDestinoId, vm.tramiteid, vm.section).then(
                    function (respuesta) {
                        if (respuesta.data !== '') {
                            $scope.datos = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                        }
                        else {
                            $scope.datos = [];
                        }
                        $scope.configPages();
                    });
        }

        function CancelarValores(datos) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                datos.EAjCT = false;

                return comunesServicio.ObtenerDatosProgramacionEncabezado(vm.EntidadDestinoId, vm.tramiteid, vm.section)
                    .then(respuesta => {
                        if (respuesta.data !== '') {
                            $scope.datos = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
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
            datos.EAjCT = true;
        }

        function GuardarValores(datos) {
            let Programacion = {};
            let ValoresProgramacion = [];
            let cambioCero = 0;

            angular.forEach(datos.Proyectos, function (series) {
                if ((series.NacionCSFC > 0 && series.NacionCSF == 0) || (series.NacionSSFC > 0 && series.NacionSSF == 0) || (series.DCCC > 0 && series.DCC == 0)) {
                    if (series.NacionCSF == 0 && series.NacionSSF == 0 && series.DCC == 0) {
                        cambioCero++;
                    }
                }
                let valores = {
                    ProyectoId: series.ProyectoId,
                    DistribucionCuotaComunicadaNacionCSF: series.NacionCSF,
                    DistribucionCuotaComunicadaNacionSSF: series.NacionSSF,
                    DistribucionCuotaComunicadaPropios: series.DCC
                };

                ValoresProgramacion.push(valores);
            });

            ObtenerSeccionCapitulo();
            Programacion.TramiteId = vm.tramiteid;
            Programacion.NivelId = vm.idNivel;
            Programacion.EntidadDestinoId = vm.EntidadDestinoId;
            Programacion.ValoresProgramacion = ValoresProgramacion;
            Programacion.seccionCapitulo = vm.seccionCapitulo;

            if (cambioCero > 0) {
                utilidades.mensajeWarning("¿Está seguro Continuar?", function funcionContinuar() {
                    comunesServicio.GuardarDatosProgramacionDistribucion(Programacion).then(
                        function (respuesta) {
                            if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                                if (respuesta.data.Exito) {
                                    guardarCapituloModificado();
                                    vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                                    utilidades.mensajeSuccess("Verifique que los datos no presenten inconsistencias:<br/>" +
                                        " En la tabla de resumen, los saldos deben quedar en ceros.<br/> " +
                                        " En la tabla de proyectos, la distribución de cuota por proyecto debe ser mayor o igual a su vigencia futura.", false, false, false, "Los datos han sido guardados con éxito.");
                                    datos.EAjCT = false;
                                    vm.modificodatos = '1';
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
                }, function funcionCancelar(reason) {
                    console.log("reason", reason);
                }, "Aceptar",
                    "Cancelar",
                    "Si se modifican los valores del proyecto a ceros, se perderá la información del proyecto diligenciada en otros campos del formulario.");

            }
            else {
                return comunesServicio.GuardarDatosProgramacionDistribucion(Programacion).then(
                    function (respuesta) {
                        if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                            if (respuesta.data.Exito) {
                                guardarCapituloModificado();
                                vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                                utilidades.mensajeSuccess("Verifique que los datos no presenten inconsistencias:<br/>" +
                                    " En la tabla de resumen, los saldos deben quedar en ceros.<br/> " +
                                    " En la tabla de proyectos, la distribución de cuota por proyecto debe ser mayor o igual a su vigencia futura.", false, false, false, "Los datos han sido guardados con éxito.");
                                datos.EAjCT = false;
                                vm.modificodatos = '1';
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
        }

        vm.mostrarOcultarFuentes = function (objeto, tramiteProyectoid,bpin) {
            var variable = $("#ico" + vm.section + objeto).attr("src");

            angular.forEach($scope.datos.Proyectos, function (series) {
                $("#ico" + vm.section + series.ProyectoId).attr("src", "Img/btnMas.svg");
                if (series.ProyectoId != objeto) {
                    var campofuente = document.getElementById(vm.section + series.ProyectoId);
                    if (campofuente != null) { 
                        campofuente.classList.remove('in');
                    }                    
                }
            });

            if (variable === "Img/btnMas.svg") {
                $("#ico" + vm.section + objeto).attr("src", "Img/btnMenos.svg");
                comunesServicio.setProyectoCargado(objeto);
                comunesServicio.setBpinCargado(bpin);
                vm.TramiteProyectoId = tramiteProyectoid;
            }
        }

        //para guardar los capitulos modificados y que se llenen las lunas

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        function guardarCapituloModificado() {
            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
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

        vm.actualizaFila = function (event, datos, campo, proyectoId) {
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

            if (campo === 'NacionCSF') datos.Proyectos.find(p => p.ProyectoId == proyectoId).NacionCSF = event.target.value;
            if (campo === 'NacionSSF') datos.Proyectos.find(p => p.ProyectoId == proyectoId).NacionSSF = event.target.value;
            if (campo === 'DCC') datos.Proyectos.find(p => p.ProyectoId == proyectoId).DCC = event.target.value;

            angular.forEach(datos.Proyectos, function (series) {

                series.TDCC = parseFloat(series.NacionCSF) + parseFloat(series.NacionSSF) + parseFloat(series.DCC);

            });

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
                            vm.erroresJson = erroresJson;
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
            var campoObligatorioprogramacion1 = document.getElementById(vm.nombreComponente + "-error1");
            if (campoObligatorioprogramacion1 != undefined) {
                campoObligatorioprogramacion1.innerHTML = "";
                campoObligatorioprogramacion1.classList.add('hidden');
            }

            var campoObligatorioprogramacion2 = document.getElementById(vm.nombreComponente + "-error2");
            if (campoObligatorioprogramacion2 != undefined) {
                campoObligatorioprogramacion2.innerHTML = "";
                campoObligatorioprogramacion2.classList.add('hidden');
            }

            var campoObligatorioprogramacion3 = document.getElementById(vm.nombreComponente + "-error3");
            if (campoObligatorioprogramacion3 != undefined) {
                campoObligatorioprogramacion3.innerHTML = "";
                campoObligatorioprogramacion3.classList.add('hidden');
            }

            var campoObligatorioprogramacion4 = document.getElementById(vm.nombreComponente + "-error4");
            if (campoObligatorioprogramacion4 != undefined) {
                campoObligatorioprogramacion4.innerHTML = "";
                campoObligatorioprogramacion4.classList.add('hidden');
            }

            if ($scope.datos.Proyectos !== undefined && $scope.datos.Proyectos !== null)
                $scope.datos.Proyectos.forEach(p => {
                    var campoObligatorioJustificacion2Detalle = document.getElementById(vm.nombreComponente + "-" + vm.section + p.ProyectoId);
                    if (campoObligatorioJustificacion2Detalle != undefined) {
                        campoObligatorioJustificacion2Detalle.innerHTML = "";
                        campoObligatorioJustificacion2Detalle.classList.add('hidden');
                    }
                }
                );
        }

        vm.validarValoresProyectosRegistrados1 = function (errores) {
            var campoObligatorioprogramacion1 = document.getElementById(vm.nombreComponente + "-error1");
            if (campoObligatorioprogramacion1 != undefined) {
                campoObligatorioprogramacion1.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span><br />";
                campoObligatorioprogramacion1.classList.remove('hidden');
            }
        }

        vm.validarValoresProyectosRegistrados2 = function (errores) {
            var campoObligatorioprogramacion2 = document.getElementById(vm.nombreComponente + "-error2");
            if (campoObligatorioprogramacion2 != undefined) {
                campoObligatorioprogramacion2.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span><br />";
                campoObligatorioprogramacion2.classList.remove('hidden');
            }
        }

        vm.validarValoresProyectosRegistrados3 = function (errores) {
            var campoObligatorioprogramacion3 = document.getElementById(vm.nombreComponente + "-error3");
            if (campoObligatorioprogramacion3 != undefined) {
                campoObligatorioprogramacion3.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span><br />";
                campoObligatorioprogramacion3.classList.remove('hidden');
            }
        }

        vm.validarValoresProyectosRegistrados4 = function (errores) {
            var campoObligatorioprogramacion4 = document.getElementById(vm.nombreComponente + "-error4");
            if (campoObligatorioprogramacion4 != undefined) {
                campoObligatorioprogramacion4.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span><br />";
                campoObligatorioprogramacion4.classList.remove('hidden');
            }
        }

        vm.validarValoresProyectosRegistrados2Detalle = function (errores) {
            var campoObligatorioJustificacion2Detalle = document.getElementById(vm.nombreComponente + "-" + vm.section + errores);
            if (campoObligatorioJustificacion2Detalle != undefined) {
                campoObligatorioJustificacion2Detalle.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion2Detalle.classList.remove('hidden');
            }
        }

        vm.validarValoresPoliticas = function (errores) {
            var campoObligatorioPoliticas = document.getElementById("Politicas" + "-" + errores);
            if (campoObligatorioPoliticas != undefined) {
                campoObligatorioPoliticas.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioPoliticas.classList.remove('hidden');
            }
        }

        vm.validarValoresCrucePoliticas = function (errores) {
            var campoObligatorioCrucePoliticas = document.getElementById("Cruce" + "-" + errores);
            if (campoObligatorioCrucePoliticas != undefined) {
                campoObligatorioCrucePoliticas.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioCrucePoliticas.classList.remove('hidden');
            }
        }

        vm.errores = {
            'PDI001': vm.validarValoresProyectosRegistrados1,
            'PDI002': vm.validarValoresProyectosRegistrados2,
            'PDI002--': vm.validarValoresPoliticas,
            'PDI003': vm.validarValoresProyectosRegistrados3,
            'PDI004': vm.validarValoresProyectosRegistrados4,
            'PDI002-': vm.validarValoresProyectosRegistrados2Detalle,
            'PDI003-': vm.validarValoresProyectosRegistrados2Detalle,
            'PDI004-': vm.validarValoresProyectosRegistrados2Detalle,
            'PDI004--': vm.validarValoresCrucePoliticas,
        }

        /* ------------------------ FIN Validaciones ---------------------------------*/
    }

    angular.module('backbone').component('listaProyectosRegistrados', {

        templateUrl: "src/app/formulario/ventanas/comun/programacionRecursos/listaProyectosRegistrados.html",
        controller: listaProyectosRegistradosController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            nivel: '@',
            rol: '@',
            section: '@',
            actualizacomponentes: '@',
            calendariodistribucioncuotas: '@',
            calendariofuentes: '@',
            calendarioiniciativas: '@',
            calendarioregionalizacion: '@',
            modificodatos: '=',
            modificardistribucion: '=',
            calendariopoliticastransversales: '@',

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
        .filter('startFromGrid', function () {
            return function (input, start) {
                start = +start;
                if (input !== undefined && input !==  null)
                    return input.slice(start);
            }
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
