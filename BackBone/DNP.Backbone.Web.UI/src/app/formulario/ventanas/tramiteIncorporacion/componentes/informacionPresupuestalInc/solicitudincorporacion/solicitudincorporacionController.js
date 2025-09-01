(function () {
    'use strict';

    solicitudincorporacionController.$inject = [
        '$sessionStorage',
        '$scope',
        'solicitudincorporacionServicio',
        'utilidades',
        'justificacionCambiosServicio',
        'utilsValidacionSeccionCapitulosServicio',
        '$uibModal',
        'comunesServicio'

    ];

    function solicitudincorporacionController(
        $sessionStorage,
        $scope,
        solicitudincorporacionServicio,
        utilidades,
        justificacionCambiosServicio,
        utilsValidacionSeccionCapitulosServicio,
        $uibModal,
        comunesServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.extension = "";
        vm.filename = "";
        vm.numeroTramite = $sessionStorage.idObjetoNegocio;
        vm.etapa = "ej";
        vm.TramiteId = $sessionStorage.tramiteId;//906
        vm.usuario = $sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.instanciaId = $sessionStorage.idInstancia;
        vm.idTipoTramite = $sessionStorage.TipoTramiteId;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.erroresActivos = null;
        vm.idProyecto = 0;// $sessionStorage.proyectoId;
        vm.ObjetoVerMas = ObjetoVerMas;
        vm.nombreComponente = "informacionpresupuestalsolicitudincorporacion";
        vm.notificacionCambiosCapitulos = null;
        vm.abrirMensajeInformacion = abrirMensajeInformacion;
        vm.ConvertirNumero = ConvertirNumero;
        vm.CancelarAsociados = CancelarAsociados;
        vm.EditarAsociados = EditarAsociados;
        vm.GuardarAsociados = GuardarAsociados;
        vm.CancelarAportantes = CancelarAportantes;
        vm.EditarAportantes = EditarAportantes;
        vm.GuardarAportantes = GuardarAportantes;
        vm.handlerComponentes = [
        ];
        vm.handlerComponentesChecked = {};
        vm.habilitaBotones = $sessionStorage.nombreAccion.includes('Creación del trámite') && !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1
        $scope.$watch('vm.tramiteid', function () {
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                ObtenersolicitudincorporacionVigenciaFutura();
            }
        });

        /*$scope.$watch('vm.actualizacomponentes', function () {
            
            if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                vm.init();
            }
        });*/

        $scope.$watch(() => $sessionStorage.actualizadetalle
            , (newVal, oldVal) => {
                if (newVal) {
                    if (vm.tramiteid !== undefined && vm.tramiteid !== '')
                        ObtenersolicitudincorporacionVigenciaFutura();
                }
            }, true);

        vm.init = function () {
            vm.inicializarComponenteCheck();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        };

        function ObtenersolicitudincorporacionVigenciaFutura() {
            return solicitudincorporacionServicio.ObtenerPresupuestalProyectosAsociados(vm.tramiteid, vm.instanciaId).then(
                function (respuesta) {
                    console.log(respuesta.data);
                    $scope.datos = respuesta.data;
                });
        }

        $scope.myFilter = function (item) {
            return item.TipoOperacion === 'Credito';
        };

        $scope.myFilter2 = function (item) {
            return item.TipoOperacion === 'Contrato';
        };

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        function CancelarAsociados(asociados) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                asociados.EditarTramiteIncorporacion = false;

                angular.forEach(asociados.DetalleFuentes, function (series) {
                    series.ValorIncorporarCSF = series.ValorIncorporarCSFOriginal;
                    series.ValorIncorporarSSF = series.ValorIncorporarSSFOriginal;
                });

                var credito = $scope.datos.ResumenProyectos.findIndex(x => x.TipoOperacion == 'Credito');

                var TotalSolicitadoNacion = 0;
                angular.forEach(asociados.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Nación')), function (series) {
                    TotalSolicitadoNacion = TotalSolicitadoNacion + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
                });

                var proyecto = $scope.datos.ResumenProyectos[credito].Proyectos.findIndex(x => x.ProyectoId == asociados.ProyectoId);

                $scope.datos.ResumenProyectos[credito].Proyectos[proyecto].TotalSolicitadoNacion = TotalSolicitadoNacion;

                var TotalSolicitadoPropios = 0;
                angular.forEach(asociados.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Propios')), function (series) {
                    TotalSolicitadoPropios = TotalSolicitadoPropios + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
                });
                $scope.datos.ResumenProyectos[credito].Proyectos[proyecto].TotalSolicitadoPropios = TotalSolicitadoPropios;

                return solicitudincorporacionServicio.ObtenerPresupuestalProyectosAsociados(vm.tramiteid, vm.instanciaId).then(
                    function (respuesta) {
                        if (respuesta.data != null && respuesta.data != "") {
                            utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                        }
                    });

            }, function funcionCancelar(reason) {
                //poner aquí q pasa cuando cancela
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function EditarAsociados(asociados) {
            asociados.EditarTramiteIncorporacion = true;
        }

        function GuardarAsociados(asociados) {
            var valida = false;

            let fuentes = [];
            var TotalValorIncorporarCSF = 0;
            var TotalValorIncorporarSSF = 0;
            angular.forEach(asociados.DetalleFuentes, function (series) {
                let c = {
                    IdTramite: vm.TramiteId,
                    IdProyectoTramite: 0,
                    IdProyecto: asociados.ProyectoId,
                    EntidadId: $sessionStorage.EntidadId === undefined ? 0 : $sessionStorage.EntidadId,
                    IdFuente: series.TipoRecursoId,
                    Accion: 'C',
                    IdTipoValorContracreditoCSF: 19,
                    IdTipoValorContracreditoSSF: 20,
                    ValorContracreditoCSF: series.ValorIncorporarCSF,
                    ValorContracreditoSSF: series.ValorIncorporarSSF
                };
                fuentes.push(c);
                TotalValorIncorporarCSF = TotalValorIncorporarCSF + series.ValorIncorporarCSF;
                TotalValorIncorporarSSF = TotalValorIncorporarSSF + series.ValorIncorporarSSF;
            });

            if (TotalValorIncorporarCSF == 0 && TotalValorIncorporarSSF == 0) {
                utilidades.mensajeError("Todos los proyectos asociados deben tener como mínimo un valor digitado en el campo 'Valor a incorporar' para algún tipo de recurso.");
                return;
            }

            var TotalSolicitadoNacion = 0;
            angular.forEach(asociados.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Nación')), function (series) {
                TotalSolicitadoNacion = TotalSolicitadoNacion + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
            });

            var TotalSolicitadoPropios = 0;
            angular.forEach(asociados.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Propios')), function (series) {
                TotalSolicitadoPropios = TotalSolicitadoPropios + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
            });

            if ((TotalSolicitadoNacion > asociados.MontoTramiteNacion) || (TotalSolicitadoPropios > asociados.MontoTramitePropios)) {
                utilidades.mensajeError("El valor a ingresar NO debe ser superior, al valor ingresado en el campo de valor del tramité para el respectivo proyecto");
                return;
            }

            return comunesServicio.actualizarTramitesFuentesPresupuestales(fuentes).then(
                function (respuesta) {
                    if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        guardarCapituloModificado();
                        vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                        utilidades.mensajeSuccess("", false, false, false, "los datos fueron guardados con éxito");
                        asociados.EditarTramiteIncorporacion = false;

                        angular.forEach(asociados.DetalleFuentes, function (series) {
                            series.ValorIncorporarCSFOriginal = series.ValorIncorporarCSF;
                            series.ValorIncorporarSSFOriginal = series.ValorIncorporarSSF;
                        });

                        var credito = $scope.datos.ResumenProyectos.findIndex(x => x.TipoOperacion == 'Credito');

                        var TotalSolicitadoNacion = 0;
                        angular.forEach(asociados.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Nación')), function (series) {
                            TotalSolicitadoNacion = TotalSolicitadoNacion + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
                        });

                        var proyecto = $scope.datos.ResumenProyectos[credito].Proyectos.findIndex(x => x.ProyectoId == asociados.ProyectoId);

                        $scope.datos.ResumenProyectos[credito].Proyectos[proyecto].TotalSolicitadoNacion = TotalSolicitadoNacion;

                        var TotalSolicitadoPropios = 0;
                        angular.forEach(asociados.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Propios')), function (series) {
                            TotalSolicitadoPropios = TotalSolicitadoPropios + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
                        });
                        $scope.datos.ResumenProyectos[credito].Proyectos[proyecto].TotalSolicitadoPropios = TotalSolicitadoPropios;

                        //vm.init();
                    } else {
                        utilidades.mensajeError("Error al realizar la operación");
                    }
                });

        }

        function CancelarAportantes(aportantes) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                aportantes.EditarTramiteIncorporacion = false;

                angular.forEach(aportantes.DetalleFuentes, function (series) {
                    series.ValorIncorporarCSF = series.ValorIncorporarCSFOriginal;
                    series.ValorIncorporarSSF = series.ValorIncorporarSSFOriginal;
                });

                var contrato = $scope.datos.ResumenProyectos.findIndex(x => x.TipoOperacion == 'Contrato');

                var TotalSolicitadoNacion = 0;
                angular.forEach(aportantes.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Nación')), function (series) {
                    TotalSolicitadoNacion = TotalSolicitadoNacion + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
                });

                var proyecto = $scope.datos.ResumenProyectos[contrato].Proyectos.findIndex(x => x.ProyectoId == aportantes.ProyectoId);

                $scope.datos.ResumenProyectos[contrato].Proyectos[proyecto].TotalSolicitadoNacion = TotalSolicitadoNacion;

                var TotalSolicitadoPropios = 0;
                angular.forEach(aportantes.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Propios')), function (series) {
                    TotalSolicitadoPropios = TotalSolicitadoPropios + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
                });
                $scope.datos.ResumenProyectos[contrato].Proyectos[proyecto].TotalSolicitadoPropios = TotalSolicitadoPropios;

                return solicitudincorporacionServicio.ObtenerPresupuestalProyectosAsociados(vm.tramiteid, vm.instanciaId).then(
                    function (respuesta) {
                        if (respuesta.data != null && respuesta.data != "") {
                            utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
                        }
                    });

            }, function funcionCancelar(reason) {
                //poner aquí q pasa cuando cancela
            }, null, null, "Los posibles datos que haya diligenciado en la tabla se perderán.");
        }

        function EditarAportantes(aportantes) {
            aportantes.EditarTramiteIncorporacion = true;
        }

        function GuardarAportantes(aportantes) {
            var valida = false;

            let fuentes = [];
            var TotalValorIncorporarCSF = 0;
            var TotalValorIncorporarSSF = 0;
            angular.forEach(aportantes.DetalleFuentes, function (series) {
                let c = {
                    IdTramite: vm.TramiteId,
                    IdProyectoTramite: 0,
                    IdProyecto: aportantes.ProyectoId,
                    EntidadId: $sessionStorage.EntidadId === undefined ? 0 : $sessionStorage.EntidadId,
                    IdFuente: series.TipoRecursoId,
                    Accion: 'C',
                    IdTipoValorContracreditoCSF: 19,
                    IdTipoValorContracreditoSSF: 20,
                    ValorContracreditoCSF: series.ValorIncorporarCSF,
                    ValorContracreditoSSF: series.ValorIncorporarSSF
                };
                fuentes.push(c);
                TotalValorIncorporarCSF = TotalValorIncorporarCSF + series.ValorIncorporarCSF;
                TotalValorIncorporarSSF = TotalValorIncorporarSSF + series.ValorIncorporarSSF;
            });

            if (TotalValorIncorporarCSF == 0 && TotalValorIncorporarSSF == 0) {
                utilidades.mensajeError("Todos los proyectos aportantes deben tener como mínimo un valor digitado en el campo 'Valor a incorporar' para algún tipo de recurso.");
                return;
            }

            var TotalSolicitadoNacion = 0;
            angular.forEach(aportantes.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Nación')), function (series) {
                TotalSolicitadoNacion = TotalSolicitadoNacion + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
            });

            var TotalSolicitadoPropios = 0;
            angular.forEach(aportantes.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Propios')), function (series) {
                TotalSolicitadoPropios = TotalSolicitadoPropios + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
            });

            if ((TotalSolicitadoNacion > aportantes.MontoTramiteNacion) || (TotalSolicitadoPropios > aportantes.MontoTramitePropios)) {
                utilidades.mensajeError("El valor a ingresar NO debe ser superior, al valor ingresado en el campo de valor del tramite´' para el respectivo proyecto");
                return;
            }

            return comunesServicio.actualizarTramitesFuentesPresupuestales(fuentes).then(
                function (respuesta) {
                    if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        guardarCapituloModificado();
                        vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                        utilidades.mensajeSuccess("", false, false, false, "los datos fueron guardados con éxito");
                        aportantes.EditarTramiteIncorporacion = false;

                        angular.forEach(aportantes.DetalleFuentes, function (series) {
                            series.ValorIncorporarCSFOriginal = series.ValorIncorporarCSF;
                            series.ValorIncorporarSSFOriginal = series.ValorIncorporarSSF;
                        });

                        var contrato = $scope.datos.ResumenProyectos.findIndex(x => x.TipoOperacion == 'Contrato');

                        var TotalSolicitadoNacion = 0;
                        angular.forEach(aportantes.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Nación')), function (series) {
                            TotalSolicitadoNacion = TotalSolicitadoNacion + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
                        });

                        var proyecto = $scope.datos.ResumenProyectos[contrato].Proyectos.findIndex(x => x.ProyectoId == aportantes.ProyectoId);

                        $scope.datos.ResumenProyectos[contrato].Proyectos[proyecto].TotalSolicitadoNacion = TotalSolicitadoNacion;

                        var TotalSolicitadoPropios = 0;
                        angular.forEach(aportantes.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Propios')), function (series) {
                            TotalSolicitadoPropios = TotalSolicitadoPropios + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
                        });
                        $scope.datos.ResumenProyectos[contrato].Proyectos[proyecto].TotalSolicitadoPropios = TotalSolicitadoPropios;
                        //vm.init();
                    } else {
                        utilidades.mensajeError("Error al realizar la operación");
                    }
                });
        }

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

        vm.changeBotonAsociados = function (asociados) {
            if (asociados.LabelBoton == '+') {
                asociados.LabelBoton = '-'
            } else {
                asociados.LabelBoton = '+'
            }
            return asociados.LabelBoton;
        }

        vm.changeBotonAportantes = function (aportantes) {
            if (aportantes.LabelBoton == '+') {
                aportantes.LabelBoton = '-'
            } else {
                aportantes.LabelBoton = '+'
            }
            return aportantes.LabelBoton;
        }

        function abrirMensajeInformacion() {
            utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <span class='tituhori' > Objetivos específicos</span>");
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

        vm.actualizaFila = function (event, asociados) {
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

            var credito = $scope.datos.ResumenProyectos.findIndex(x => x.TipoOperacion == 'Credito');

            var TotalSolicitadoNacion = 0;
            angular.forEach(asociados.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Nación')), function (series) {
                TotalSolicitadoNacion = TotalSolicitadoNacion + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
            });

            var proyecto = $scope.datos.ResumenProyectos[credito].Proyectos.findIndex(x => x.ProyectoId == asociados.ProyectoId);

            $scope.datos.ResumenProyectos[credito].Proyectos[proyecto].TotalSolicitadoNacion = TotalSolicitadoNacion;

            var TotalSolicitadoPropios = 0;
            angular.forEach(asociados.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Propios')), function (series) {
                TotalSolicitadoPropios = TotalSolicitadoPropios + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
            });
            $scope.datos.ResumenProyectos[credito].Proyectos[proyecto].TotalSolicitadoPropios = TotalSolicitadoPropios;

            $scope.datos.ResumenProyectos[credito].TotalTipoOperacion = TotalSolicitadoNacion + TotalSolicitadoPropios;

            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
        }

        vm.actualizaFila2 = function (event, aportantes) {

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

            var contrato = $scope.datos.ResumenProyectos.findIndex(x => x.TipoOperacion == 'Contrato');

            var TotalSolicitadoNacion = 0;
            angular.forEach(aportantes.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Nación')), function (series) {
                TotalSolicitadoNacion = TotalSolicitadoNacion + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
            });

            var proyecto = $scope.datos.ResumenProyectos[contrato].Proyectos.findIndex(x => x.ProyectoId == aportantes.ProyectoId);

            $scope.datos.ResumenProyectos[contrato].Proyectos[proyecto].TotalSolicitadoNacion = TotalSolicitadoNacion;

            var TotalSolicitadoPropios = 0;
            angular.forEach(aportantes.DetalleFuentes.filter(x => x.NombreTipoRecurso.includes('Propios')), function (series) {
                TotalSolicitadoPropios = TotalSolicitadoPropios + parseFloat(series.ValorIncorporarCSF) + parseFloat(series.ValorIncorporarSSF);
            });
            $scope.datos.ResumenProyectos[contrato].Proyectos[proyecto].TotalSolicitadoPropios = TotalSolicitadoPropios;

            $scope.datos.ResumenProyectos[contrato].TotalTipoOperacion = TotalSolicitadoNacion + TotalSolicitadoPropios;

            const val = event.target.value;
            const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
            var total = event.target.value = decimalCnt && decimalCnt > 2 ? event.target.value : parseFloat(val).toFixed(2);
            return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
        }

        /*------------------------------------Validaciones-----------------------------------*/
        /**
       * Listado de componentes hijos, obligatorio para estructura de validación
       * */


        vm.changeArrow = function (nombreModificado) {
            var idSpanArrow = 'arrow-' + nombreModificado;
            var arrowCapitulo = document.getElementById(idSpanArrow);
            var arrowClasses = arrowCapitulo.classList;
            for (var i = 0; i < arrowClasses.length; i++) {
                if (arrowClasses[i] == vm.arrowIcoDown) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown);
                    break;
                } else if (arrowClasses[i] == vm.arrowIcoUp) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp);
                    break;
                }
            }
        }

        vm.guardado = function (nombreComponenteHijo, deshabilitarRegresar, devolver) {
            vm.callback();
            vm.guardadocomponent({ nombreComponente: vm.nombreComponente, nombreComponenteHijo: nombreComponenteHijo, deshabilitarRegresar: deshabilitarRegresar });

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

        vm.deshabilitarBotonDevolverAsociarProyectoVF = function () {
            vm.callback();

        }

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores(errores);
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


            var campoObligatorioJustificacion = document.getElementById("IPF001-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById("IPF004-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            if (vm.listaProyectos !== undefined) {
                vm.listaProyectos.map(function (proyecto) {
                    //Busca los proyectos y borra los errores

                    var campoObligatorioJustificacion = document.getElementById("IPF001-" + proyecto.ProyectoId);
                    if (campoObligatorioJustificacion != undefined) {
                        campoObligatorioJustificacion.innerHTML = "";
                        campoObligatorioJustificacion.classList.add('hidden');
                    }

                    if (proyecto.ListaFuentes !== undefined) {
                        proyecto.ListaFuentes.map(function (fuente) {
                            //Busca los proyectos y borra los errores

                            var campoObligatorioJustificacion = document.getElementById("IPF002-" + fuente.FuenteId + '-' + proyecto.ProyectoId);
                            if (campoObligatorioJustificacion != undefined) {
                                campoObligatorioJustificacion.innerHTML = "";
                                campoObligatorioJustificacion.classList.add('hidden');
                            }
                        });
                    }

                });
            }


            var campoObligatorioJustificacion = document.getElementById("IPF002-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
        }


        vm.validarIPF001 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("IPF001-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarIPF001Grill = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("IPF001-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarIPF002Grilla = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("IPF002-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarIPF002 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("IPF002-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarIPF002PGrilla = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("IPF001-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarIPF004Grilla = function (errores) {//Este maneja el error 2 y 4 de la fuente proyectopara que no se repitan los iconos
            var campoObligatorioJustificacion = document.getElementById("IPF002-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }


        vm.errores = {
            'IPF001-': vm.validarIPF001Grill,//Este sirve para el error del 1,2 y del 4 del proyecto para que no se repitan los iconos
            'IPF001': vm.validarIPF001,
            'IPF002-': vm.validarIPF002Grilla,
            'IPF002': vm.validarIPF002,
            'IPF004-': vm.validarIPF004Grilla,
            'IPF004': vm.validarIPF004,
        }

        /* --------------------------------- Validaciones ---------------------------*/

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
            //vm.showAlertError(nombreComponente, esValido, esValidoPaso4);
            vm.showAlertError(nombreComponente, esValido);
        }

        /**
         * Función que visualiza alerta de error tab de componente
         * @param {any} nombreComponente nombre configurado en cada uno de los componentes hijos vm.nombreComponente = 'datosgenerales'
         */
        //vm.showAlertError = function (nombreComponente, esValido, esValidoPaso4) {
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

        vm.capitulos = function (listadoCapitulos) {
            var listadoCapRecursos = listadoCapitulos.filter(p => p.SeccionModificado == vm.nombreComponente)
            listadoCapRecursos.forEach(function (item) {
                var el = document.getElementById("name-capitulo-" + item.nombreComponente);
                var elidSeccionCapitulo = document.getElementById("id-capitulo-" + item.nombreComponente);
                var elAccordion = document.getElementById("accordion-" + item.nombreComponente);
                if (el != undefined && el != null) {
                    el.innerHTML = item.Capitulo;
                }
                if (elAccordion != undefined && elAccordion != null) {
                    elAccordion.classList.remove("hidden");
                }
                if (elidSeccionCapitulo != undefined && elidSeccionCapitulo != null) {
                    elidSeccionCapitulo.innerHTML = item.SeccionCapituloId;
                }
            });
        };

        /*------------------------------------Fin Validaciones-----------------------------------*/

    }

    angular.module('backbone').component('solicitudincorporacion', {
        templateUrl: "src/app/formulario/ventanas/tramiteIncorporacion/componentes/informacionPresupuestalInc/solicitudincorporacion/solicitudincorporacion.html",
        controller: solicitudincorporacionController,
        controllerAs: "vm",
        bindings: {
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            tipotramiteid: '@',
            tramiteid: '@',
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