(function () {
    'use strict';

    resumenInformacionPresupuestalAprobadosController.$inject = [
        '$scope',
        'justificacionCambiosServicio',
        'tramiteVigenciaFuturaServicio',        
        '$sessionStorage',
        'utilidades',
        'constantesBackbone',

    ];

    function resumenInformacionPresupuestalAprobadosController(
        $scope,
        justificacionCambiosServicio,
        tramiteVigenciaFuturaServicio,        
        $sessionStorage,
        utilidades,
        constantesBackbone,

    ) {
        var vm = this;
        vm.lang = "es";
        vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
        vm.idInstancia = $sessionStorage.idInstancia;

        vm.mensajetabla = "El sistema no ha actualizado los datos de esta tabla desde el formulario de ajustes.";
        vm.mensajeEncabezado = "Vigencias futuras con valores constantes";

        vm.TramiteId = $sessionStorage.TramiteId;
        vm.informacionPresupuestal = {};

        vm.EsConstante = true;
        vm.disabled = false; 
        vm.SeleccionCosto = "aprobado";
        vm.DisableEdit = false;
        vm.disabledProcesar = true;
        vm.mensaje = '';
        vm.permiteEditar = false;
        vm.PermisoEdicion = false;

        vm.ConvertirNumero = ConvertirNumero;
        vm.ConvertirNumero4 = ConvertirNumero4;

        //Validaciones
        vm.nombreComponente = "informacionpresupuestalresumen";

        vm.init = function () {
            vm.permiteEditar = false;
            $scope.$watch('vm.tramiteid', function () {
                if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                    vm.obtenerInformacionPresupuestal(vm.tramiteid);
                }
            });
            PermisosEdicion();

            //Validaciones
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
        };

        vm.obtenerInformacionPresupuestal = function (TramiteId) {

            tramiteVigenciaFuturaServicio.ObtenerInformacionPresupuestalValores(TramiteId).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        if (response.data.ProyectoId != null) {

                            vm.mensajetabla = "El sistema ha actualizado los datos de esta tabla. Desde ajustes se incluyeron los datos";

                            var infoPresupuestal = {};

                            var listaVigencias = [];
                            vm.EsConstante = response.data.AplicaConstante;

                            if (vm.EsConstante) {
                                vm.mensajeEncabezado = "Vigencias futuras con valores constantes";
                            }
                            else {
                                vm.mensajeEncabezado = "Vigencias futuras con valores corrientes";
                            }

                            //var sumValorDeflactor = 0;
                            var sumNacionConstante = 0;
                            var sumPropiosConstante = 0;
                            var sumNacionCorriente = 0;
                            var sumPropiosCorriente = 0;
                            var sumNacionAprobadosConstante = 0;
                            var sumPropiosAprobadosConstante = 0;
                            var sumNacionAprobadosCorriente = 0;
                            var sumPropiosAprobadosCorriente = 0;

                            response.data.ResumensolicitadoFuentesVigenciaFutura.forEach(vigencia => {

                                listaVigencias.push({
                                    Vigencia: vigencia.Vigencia,
                                    Deflactor: vigencia.Deflactor,
                                    ValorFuentesNacion: vigencia.ValorFuentesNacion,
                                    ValorFuentesPropios: vigencia.ValorFuentesPropios,
                                    NacionCorriente: vigencia.ValorFuentesNacion * (vm.EsConstante ? vigencia.Deflactor : 1),
                                    PropiosCorriente: vigencia.ValorFuentesPropios * (vm.EsConstante ? vigencia.Deflactor : 1),
                                    ValorAprobadoNacion: vigencia.ValorAprobadoNacion,
                                    ValorAprobadoPropios: vigencia.ValorAprobadoPropios,
                                    AprobadoNacionCorriente: vigencia.ValorAprobadoNacion * (vm.EsConstante ? vigencia.Deflactor : 1),
                                    AprobadoPropiosCorriente: vigencia.ValorAprobadoPropios * (vm.EsConstante ? vigencia.Deflactor : 1)
                                });

                                //sumValorDeflactor = sumValorDeflactor + vigencia.Deflactor;
                                sumNacionConstante = sumNacionConstante + vigencia.ValorFuentesNacion;
                                sumPropiosConstante = sumPropiosConstante + vigencia.ValorFuentesPropios;
                                sumNacionCorriente = sumNacionCorriente + (vigencia.ValorFuentesNacion * (vm.EsConstante ? vigencia.Deflactor : 1));
                                sumPropiosCorriente = sumPropiosCorriente + (vigencia.ValorFuentesPropios * (vm.EsConstante ? vigencia.Deflactor : 1));
                                sumNacionAprobadosConstante = sumNacionAprobadosConstante + vigencia.ValorAprobadoNacion;
                                sumPropiosAprobadosConstante = sumPropiosAprobadosConstante + vigencia.ValorAprobadoPropios;
                                sumNacionAprobadosCorriente = sumNacionAprobadosCorriente + (vigencia.ValorAprobadoNacion * (vm.EsConstante ? vigencia.Deflactor : 1));
                                sumPropiosAprobadosCorriente = sumPropiosAprobadosCorriente + (vigencia.ValorAprobadoPropios * (vm.EsConstante ? vigencia.Deflactor : 1));


                            });

                            listaVigencias.push({
                                Vigencia: 'Total',
                                //Deflactor: sumValorDeflactor,
                                Deflactor: 'N/A',
                                ValorFuentesNacion: sumNacionConstante,
                                ValorFuentesPropios: sumPropiosConstante,
                                NacionCorriente: sumNacionCorriente,
                                PropiosCorriente: sumPropiosCorriente,
                                ValorAprobadoNacion: sumNacionAprobadosConstante,
                                ValorAprobadoPropios: sumPropiosAprobadosConstante,
                                AprobadoNacionCorriente: sumNacionAprobadosCorriente,
                                AprobadoPropiosCorriente: sumPropiosAprobadosCorriente
                            });

                            var listaObjetivos = [];

                            var numObjetivo = 1;
                            response.data.DetalleProductosVigenciaFutura.forEach(objetivo => {

                                var listaProductos = [];

                                var numProducto = 1;
                                var conteoNumProducto = numObjetivo + '.' + numProducto

                                objetivo.Productos.forEach(producto => {
                                    var listaValores = [];
                                    var sumValorSolicitado = 0;
                                    var sumValorSolicitadoCorriente = 0;

                                    producto.Vigencias.forEach(valor => {

                                        listaValores.push({
                                            Vigencia: valor.Vigencia,
                                            ValorSolicitado: valor.ValorSolicitadoVF,
                                            ValorSolicitadoCorriente: valor.ValorSolicitadoVF * (vm.EsConstante ? valor.Deflactor : 1),
                                        })

                                        sumValorSolicitado = sumValorSolicitado + valor.ValorSolicitadoVF;
                                        sumValorSolicitadoCorriente = sumValorSolicitadoCorriente + (valor.ValorSolicitadoVF * (vm.EsConstante ? valor.Deflactor : 1));
                                    });

                                    listaValores.push({
                                        Vigencia: 'Total por valores',
                                        ValorSolicitado: sumValorSolicitado,
                                        ValorSolicitadoCorriente: sumValorSolicitadoCorriente
                                    })

                                    if (producto.TotalValores > 0) {
                                        listaProductos.push({
                                            ProductoId: producto.ProductoId,
                                            NumeroProducto: 'Producto ' + conteoNumProducto + '.',
                                            //NumeroProducto: 'Producto ' + numProducto + '.',
                                            NombreProducto: producto.NombreProducto,
                                            TotalValores: producto.TotalValores,
                                            TotalValoresCorrientes: sumValorSolicitadoCorriente,
                                            labelEncabezado: vm.mensajeEncabezado,
                                            Vigencias: listaValores
                                        });

                                        numProducto++;
                                    }
                                });

                                if (listaProductos.length > 0) {
                                    listaObjetivos.push({
                                        ObjetivoEspecificoId: objetivo.ObjetivoEspecificoId,
                                        NumeroObjetivo: 'Objetivo ' + numObjetivo + '.',
                                        ObjetivoEspecifico: objetivo.ObjetivoEspecifico,
                                        Productos: listaProductos
                                    });

                                    numObjetivo++;
                                }
                            });

                            infoPresupuestal.ProyectoId = response.data.ProyectoId;
                            infoPresupuestal.BPIN = response.data.BPIN;
                            infoPresupuestal.TramiteId = TramiteId;
                            infoPresupuestal.AplicaConstante = response.data.AplicaConstante;
                            infoPresupuestal.AnoBase = response.data.AñoBase,
                                infoPresupuestal.labeltable = vm.mensajetabla,
                                infoPresupuestal.ResumensolicitadoFuentesVigenciaFutura = listaVigencias;
                            infoPresupuestal.DetalleProductosVigenciaFutura = listaObjetivos;

                            vm.informacionPresupuestal = infoPresupuestal;
                        }
                        else {
                            vm.informacionPresupuestal = {
                                ProyectoId: '-',
                                AnoBase: '-',
                                labeltable: vm.mensajetabla,
                                ResumensolicitadoFuentesVigenciaFutura: [
                                    {
                                        Vigencia: '-',
                                        Deflactor: 'Dato pendiente',
                                        ValorFuentesNacion: 'Dato pendiente',
                                        ValorFuentesPropios: 'Dato pendiente',
                                        NacionCorriente: 'Dato pendiente',
                                        PropiosCorriente: 'Dato pendiente'
                                    },
                                    {
                                        Vigencia: 'Total',
                                        Deflactor: 'Dato pendiente',
                                        ValorFuentesNacion: 'Dato pendiente',
                                        ValorFuentesPropios: 'Dato pendiente',
                                        NacionCorriente: 'Dato pendiente',
                                        PropiosCorriente: 'Dato pendiente'
                                    }
                                ],
                                DetalleProductosVigenciaFutura: [
                                    {
                                        ObjetivoEspecificoId: 1,
                                        NumeroObjetivo: "Objetivo 1",
                                        ObjetivoEspecifico: '',
                                        Productos: [
                                            {
                                                ProductoId: 1,
                                                NumeroProducto: "Producto 1.",
                                                NombreProducto: "",
                                                TotalValores: 0,
                                                TotalValoresCorrientes: 0,
                                                labelEncabezado: vm.mensajeEncabezado,
                                                Vigencias: [
                                                    {
                                                        Vigencia: '-',
                                                        ValorSolicitado: 'Dato pendiente',
                                                        ValorSolicitadoCorriente: 'Dato pendiente'
                                                    },
                                                    {
                                                        Vigencia: '-',
                                                        ValorSolicitado: 'Dato pendiente',
                                                        ValorSolicitadoCorriente: 'Dato pendiente'
                                                    },
                                                    {
                                                        Vigencia: 'Total por valores',
                                                        ValorSolicitado: 'Dato pendiente',
                                                        ValorSolicitadoCorriente: 'Dato pendiente'
                                                    }
                                                ]
                                            },
                                            {
                                                ProductoId: 2,
                                                NumeroProducto: "Producto 2.",
                                                NombreProducto: "",
                                                TotalValores: 0,
                                                TotalValoresCorrientes: 0,
                                                labelEncabezado: vm.mensajeEncabezado,
                                                Vigencias: [
                                                    {
                                                        Vigencia: '-',
                                                        ValorSolicitado: 'Dato pendiente',
                                                        ValorSolicitadoCorriente: 'Dato pendiente'
                                                    },
                                                    {
                                                        Vigencia: '-',
                                                        ValorSolicitado: 'Dato pendiente',
                                                        ValorSolicitadoCorriente: 'Dato pendiente'
                                                    },
                                                    {
                                                        Vigencia: 'Total por valores',
                                                        ValorSolicitado: 'Dato pendiente',
                                                        ValorSolicitadoCorriente: 'Dato pendiente'
                                                    }
                                                ]
                                            }
                                        ]
                                    },
                                    {
                                        ObjetivoEspecificoId: 2,
                                        NumeroObjetivo: "Objetivo 2",
                                        ObjetivoEspecifico: '',
                                        Productos: [
                                            {
                                                ProductoId: 3,
                                                NumeroProducto: "Producto 3.",
                                                NombreProducto: "",
                                                TotalValores: 0,
                                                TotalValoresCorrientes: 0,
                                                labelEncabezado: vm.mensajeEncabezado,
                                                Vigencias: [
                                                    {
                                                        Vigencia: '-',
                                                        ValorSolicitado: 'Dato pendiente',
                                                        ValorSolicitadoCorriente: 'Dato pendiente'
                                                    },
                                                    {
                                                        Vigencia: '-',
                                                        ValorSolicitado: 'Dato pendiente',
                                                        ValorSolicitadoCorriente: 'Dato pendiente'
                                                    },
                                                    {
                                                        Vigencia: 'Total por valores',
                                                        ValorSolicitado: 'Dato pendiente',
                                                        ValorSolicitadoCorriente: 'Dato pendiente'
                                                    }
                                                ]
                                            },
                                            {
                                                ProductoId: 4,
                                                NumeroProducto: "Producto 4.",
                                                NombreProducto: "",
                                                TotalValores: 0,
                                                TotalValoresCorrientes: 0,
                                                labelEncabezado: vm.mensajeEncabezado,
                                                Vigencias: [
                                                    {
                                                        Vigencia: '-',
                                                        ValorSolicitado: 'Dato pendiente',
                                                        ValorSolicitadoCorriente: 'Dato pendiente'
                                                    },
                                                    {
                                                        Vigencia: '-',
                                                        ValorSolicitado: 'Dato pendiente',
                                                        ValorSolicitadoCorriente: 'Dato pendiente'
                                                    },
                                                    {
                                                        Vigencia: 'Total por valores',
                                                        ValorSolicitado: 'Dato pendiente',
                                                        ValorSolicitadoCorriente: 'Dato pendiente'
                                                    }
                                                ]
                                            }
                                        ]
                                    }
                                ]
                            };
                        }
                    }
                }, function (error) {
                    utilidades.mensajeError('No fue posible consultar la información presupuestal');
                });
        }

        function PermisosEdicion() {

            if ($sessionStorage.idNivel.toUpperCase() == constantesBackbone.idNivelElaboracionConcepto) {
                vm.PermisoEdicion = true;
            }
        }

        vm.BtnObjetivos = function (campo) {
            var variable = $("#img" + campo)[0].outerHTML;
            if (variable.includes('Img/btnMas.svg')) {
                $("#img" + campo).attr('src', 'Img/btnMenos.svg');
            }
            else {
                $("#img" + campo).attr('src', 'Img/btnMas.svg');
            }
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2,
            }).format(numero);
        }

        function ConvertirNumero4(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 4,
            }).format(numero);
        }

        vm.abrirTooltip = function () {
            utilidades.mensajeInformacion('Si no ha guardado por primera vez, los valores aprobados se muestran con los valores solicitados como valor sugerido'
                + ' , una vez guarde por primera vez se mostraran los valores aprobados almacenados.'
                , false, "Información presupuestal")
        }

        vm.actualizaFilaNac = function (event, fila, valorIndex) {
            var sumApNac = 0;
            var valAprobadoNacionConstante = 0;
            var multlipNacion = 0;
            var sumAprobadoCorrienteNacion = 0;

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

                vm.informacionPresupuestal.ResumensolicitadoFuentesVigenciaFutura.forEach(vigencia => {
                    if (vigencia.Vigencia !== 'Total') {

                        valAprobadoNacionConstante = vigencia.ValorAprobadoNacion === '' ? 0 : parseFloat(vigencia.ValorAprobadoNacion);
                        sumApNac = sumApNac + valAprobadoNacionConstante;

                        if (vm.EsConstante) {
                            multlipNacion = vigencia.Deflactor * valAprobadoNacionConstante;
                            $("#aprobadoNacionCorriente" + vigencia.Vigencia).html(ConvertirNumero(multlipNacion));
                            sumAprobadoCorrienteNacion = sumAprobadoCorrienteNacion + multlipNacion;
                        }

                    }
                });

                $("#totalAprobadoNacion").html(ConvertirNumero(sumApNac));
                if (vm.EsConstante) {
                    $("#sumAprobadoNacionCorriente").html(ConvertirNumero(sumAprobadoCorrienteNacion));
                }

                const val = value;
                const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
                var total = value = decimalCnt && decimalCnt > 2 ? value : parseFloat(val).toFixed(2);
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
            });
        }

        vm.actualizaFilaProp = function (event, fila, valorIndex) {
            var sumApProp = 0;
            var valAprobadoPropiosConstante = 0;
            var multlipPropios = 0;
            var sumAprobadoCorrientePropios = 0;

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

                vm.informacionPresupuestal.ResumensolicitadoFuentesVigenciaFutura.forEach(vigencia => {
                    if (vigencia.Vigencia !== 'Total') {

                        valAprobadoPropiosConstante = vigencia.ValorAprobadoPropios === '' ? 0 : parseFloat(vigencia.ValorAprobadoPropios);
                        sumApProp = sumApProp + valAprobadoPropiosConstante;

                        if (vm.EsConstante) {
                            multlipPropios = vigencia.Deflactor * valAprobadoPropiosConstante;
                            $("#aprobadoPropiosCorriente" + vigencia.Vigencia).html(ConvertirNumero(multlipPropios));
                            sumAprobadoCorrientePropios = sumAprobadoCorrientePropios + multlipPropios;
                        }

                    }
                });

                $("#totalAprobadoPropios").html(ConvertirNumero(sumApProp));
                if (vm.EsConstante) {
                    $("#sumAprobadoPropiosCorriente").html(ConvertirNumero(sumAprobadoCorrientePropios));
                }

                const val = value;
                const decimalCnt = val.split('.')[1] ? val.split('.')[1].length : 0;
                var total = value = decimalCnt && decimalCnt > 2 ? value : parseFloat(val).toFixed(2);
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
            });
        }

        vm.actualizarValores = function (response) {
            vm.DisableEdit = true;
            vm.disabled = true;
            vm.mensaje = '';

            tramiteVigenciaFuturaServicio.GuardarInformacionPresupuestalValores(vm.informacionPresupuestal).then(function (response) {
                if (response.data && (response.statusText === "OK" || response.status === 200)) {
                    if (!vm.permiteEditar) {
                        utilidades.mensajeSuccess('Si requiere modificar los valores, active la edición, realice los ajustes y guarde .', false, false, false, "Los valores fueron guardados y aprobados con éxito.");
                    } else {
                        utilidades.mensajeSuccess('', false, false, false, "Los valores fueron editados, guardados y aprobados con éxito.");
                        guardarCapituloModificado();
                    }
                        //vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
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

                utilidades.mensajeWarning("¿Esta seguro que desea continuar?",
                    function funcionContinuar() {
                        
                        vm.permiteEditar = false;
                        $("#Editar").html("EDITAR");
                        vm.disabled = true;
                       
                        tramiteVigenciaFuturaServicio.ObtenerInformacionPresupuestalValores(vm.tramiteid).then(
                            function (response) {
                                if (response.data != null && response.data != "") {
                                    new utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada");
                                    vm.init()
                                } else {
                                    new utilidades.mensajeError("Error al realizar la operación");
                                }
                            });                       
                    },
                    function funcionCancelar(reason) {
                        console.log("reason", reason);
                    },
                    "Aceptar",
                    "Cancelar",
                    "Los datos que posiblemente haya diligenciado se perderán."
                )
            }
        }

        vm.mostrarTab = function (origen) {

             vm.SeleccionCosto = (origen == 1) ? 'aprobado' : 'solicitado';
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.limpiarErrores = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-pregunta-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-ValorTotal-pregunta-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            if (vm.informacionPresupuestal.ResumensolicitadoFuentesVigenciaFutura !== undefined)

                vm.informacionPresupuestal.ResumensolicitadoFuentesVigenciaFutura.forEach(p => {
                    var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-" + p.Vigencia);
                    if (campoObligatorioJustificacion != undefined) {
                        campoObligatorioJustificacion.innerHTML = "";
                        campoObligatorioJustificacion.classList.add('hidden');
                    }
                }
            );
           


           



        }

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores(errores);
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
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
        }



        vm.validarValoresVigenciainformacionpresupuestalresumen = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-pregunta-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }
        vm.validarValoresVigenciainformacionpresupuestalValoresTotales = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-ValorTotal-pregunta-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValoresVigenciaInformacionPresupuestalValoresTotalesDetalles = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }


        vm.errores = {
            'VFO010': vm.validarValoresVigenciainformacionpresupuestalresumen,
            'VFO011': vm.validarValoresVigenciainformacionpresupuestalValoresTotales,
            'VFO011-': vm.validarValoresVigenciaInformacionPresupuestalValoresTotalesDetalles,
        }

        /* ------------------------ FIN Validaciones ---------------------------------*/

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
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


        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-informacionpresupuestalresumen');
            vm.seccionCapitulo = span.textContent;


        }

    }

    angular.module('backbone').component('resumenInformacionPresupuestalAprobados', {
        templateUrl: "src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/informacionPresupuestal/resumenInformacionPresupuestalAprobados/resumenInformacionPresupuestalAprobados.html",
        controller: resumenInformacionPresupuestalAprobadosController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            tramiteid: '@',
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