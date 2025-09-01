    (function () {
    'use strict';

    ejecutorProSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'ejecutorProSgrServicio',
        'justificacionCambiosServicio',
        '$scope',
        '$timeout'
    ];

    function ejecutorProSgrController(
        utilidades,
        $sessionStorage,
        ejecutorProSgrServicio,
        justificacionCambiosServicio,
        $scope,
        $timeout
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        vm.nombreComponente = "sgrviabilidadpreviosejecutorejecutorpropuesto";
        vm.Bpin = $sessionStorage.idObjetoNegocio;
        
        vm.idInstancia = $sessionStorage.idInstancia;
        
        vm.proyectoId = $sessionStorage.proyectoId;

        vm.buscar = buscar;
        vm.onClickCancelar = onClickCancelar;
        vm.onClickAsociar = onClickAsociar;
        vm.restablecerBusqueda = restablecerBusqueda;
        vm.eliminarEjecutor = eliminarEjecutor;

        vm.mostrarBt = false;
        vm.disabled = false;
        vm.activar = true;
        vm.permiteEditar = false;
        
        vm.habilitaOperacionCredito = false;
        vm.erroresComponente = [];

        vm.Valores =
        {
            ProyectoId: 0,
            BPIN: "",
            Criterios: [
                {
                    NombreTipoValor: "",
                    Habilita: false,
                    Valor: 0
                }
            ]
        };

        vm.ConvertirNumero = ConvertirNumero;
           
        vm.init = function () {

            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificacioninicio({ handlerInicio: vm.notificacionInicioPadre, nombreComponente: vm.nombreComponente, esValido: true });

           

        };

        //vm.init();
        vm.notificacionInicioPadre = function (nombreModificado, errores) {
            if (nombreModificado == vm.nombreComponente) {

                if (errores != null && errores != undefined) {
                    vm.erroresComponente = errores;
                }

                vm.disabled = $sessionStorage.soloLectura;
                vm.dsblBtn = $sessionStorage.soloLectura;

                ConsultaTodosTiposEntidades();
                ConsultaEjecutoresAsociados();

                if ($scope.$parent !== undefined && $scope.$parent.$parent !== undefined && $scope.$parent.$parent.vm !== undefined && $scope.$parent.$parent.vm.HabilitarGuardarPaso !== undefined && !$scope.$parent.$parent.vm.HabilitarGuardarPaso) {
                    bloquearControles();
                }
            }
        }
        
        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                vm.permiteEditar = true;
                $("#EditarDG").html("CANCELAR");
                vm.activar = false;
            }
            else {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    OkCancelar();

                    vm.permiteEditar = false;
                    $("#EditarDG").html("EDITAR");
                    vm.activar = true;
                    vm.init();

                }, function funcionCancelar(reason) {
                    return;
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán.");
            }
        }
        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }

        function RestablecerModoNoEdicion() {
            vm.permiteEditar = false;
            $("#EditarDG").html("EDITAR");
            vm.activar = true;
        }

        vm.actualizainput2 = function (event) {
            var sum = 0;
            var ValorFinanciero = 0;
            var ValorTotalProyecto = 0;
            var ValorCredito = 0;

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

                value = parseFloat(value.replaceAll(".", "").replace(",", "."));

                ValorFinanciero = $("#input5")[0].value === 'N/A' ? 0 : parseFloat($("#input5")[0].value.replaceAll(".", "").replace(",", "."));

                ValorCredito = $("#input4")[0].value === 'N/A' ? 0 : parseFloat($("#input4")[0].value.replaceAll(".", "").replace(",", "."));

                ValorTotalProyecto = $("#label1")[0].innerText === '' ? 0 : parseFloat($("#label1")[0].innerText.replaceAll(".", "").replace(",", "."));

                sum = ValorTotalProyecto + ValorFinanciero + value;

                if (value > ValorCredito) {
                    utilidades.mensajeError('El Costo administración patrimonio autónomo debe ser igual o menor al valor del crédito.');
                    return;
                }

                $("#label6").html(ConvertirNumero(sum));

                const val = value;
                const decimalCnt = val.toString().split('.')[1] ? val.toString().split('.')[1].length : 0;
                var total = value = decimalCnt && decimalCnt > 2 ? value : parseFloat(val).toFixed(2);
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
            });
        }

        vm.actualizainput4 = function (event) {
            var sum = 0;
            var ValorFinanciero = 0;
            var ValorTotalProyecto = 0;

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

                value = parseFloat(value.replaceAll(".", "").replace(",", "."));

                ValorFinanciero = $("#input5")[0].value === '' ? 0 : parseFloat($("#input5")[0].value.replaceAll(".", "").replace(",", "."));

                ValorTotalProyecto = $("#label1")[0].innerText === '' ? 0 : parseFloat($("#label1")[0].innerText.replaceAll(".", "").replace(",", "."));

                sum = ValorFinanciero + value;

                if (sum > ValorTotalProyecto) {
                    utilidades.mensajeError('El valor total del crédito debe ser igual o menor al valor total del proyecto.');
                    return;
                }

                $("#label3").html(ConvertirNumero(sum));

                const val = value;
                const decimalCnt = val.toString().split('.')[1] ? val.toString().split('.')[1].length : 0;
                var total = value = decimalCnt && decimalCnt > 2 ? value : parseFloat(val).toFixed(2);
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
            });
        }

        vm.actualizainput5 = function (event) {
            var sum = 0;
            var ValorPatrimonio = 0;
            var ValorTotalCredito = 0;
            var ValorTotalProyecto = 0;

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

                value = parseFloat(value.replaceAll(".", "").replace(",", "."));

                ValorTotalCredito = $("#input4")[0].value === '' ? 0 : parseFloat($("#input4")[0].value.replaceAll(".", "").replace(",", "."));

                sum = ValorTotalCredito + value;

                $("#label3").html(ConvertirNumero(sum));


                ValorTotalProyecto = $("#label1")[0].innerText === '' ? 0 : parseFloat($("#label1")[0].innerText.replaceAll(".", "").replace(",", "."));

                if (sum > ValorTotalProyecto) {
                    utilidades.mensajeError('El valor total del crédito debe ser igual o menor al valor total del proyecto.');
                    return;
                }

                sum = 0;

                ValorPatrimonio = !vm.Valores.Criterios[1].Habilita ? 0 : $("#input2")[0].value === '' ? 0 : parseFloat($("#input2")[0].value.replaceAll(".", "").replace(",", "."));

                sum = ValorTotalProyecto + ValorPatrimonio + value;

                $("#label6").html(ConvertirNumero(sum));

                const val = value;
                const decimalCnt = val.toString().split('.')[1] ? val.toString().split('.')[1].length : 0;
                var total = value = decimalCnt && decimalCnt > 2 ? value : parseFloat(val).toFixed(2);
                return new Intl.NumberFormat('es-co', { minimumFractionDigits: 2, }).format(total);
            });
        }

        function restablecerBusqueda() {
            limpiarCamposFiltro();
            buscarN(true);
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
        }
                
        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: vm.idInstancia,
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
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia
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

        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        vm.changeEntidad = function () {
            var idTipoE = document.getElementById("ddlTipoEntidad").value;
            idTipoE = idTipoE.replace('number:', '');

            ejecutorProSgrServicio.ObtenerEjecutorByTipoEntidad(idTipoE).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaFiltroEntidades = response.data;
                    }
                });
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
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

            event.target.value = event.target.value.replace(",", ".");

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 16;
            var tamanio = event.target.value.length;
            var decimal = false;
            decimal = event.target.value.toString().includes(".");
            if (decimal) {
                tamanioPermitido = 19;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    event.target.value = newValue;
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
                else {
                    var n2 = "";
                    newValue = [n[0], n2].join(".");
                    event.target.value = newValue;
                }
            }
            else {
                if (tamanio > tamanioPermitido && event.keyCode != 44) {
                    event.target.value = event.target.value.slice(0, tamanioPermitido);
                    event.preventDefault();
                }
            }
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                var erroresJson = (erroresRelacionconlapl == undefined || erroresRelacionconlapl.Errores == "") ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                var isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    erroresJson[vm.nombreComponente].forEach(p => {
                        var nameArr = p.Error.split('-');
                        var TipoError = nameArr[0].toString();
                        if (p.Error == 'EJECPRE1') {
                            vm.validarErroresPreguntas(p.Error, p.Descripcion, false);
                            vm.validarValores(p.Error, false);
                        } else if (TipoError == 'SGRERRSEC') {
                            vm.validarSeccion(TipoError, nameArr[1].toString(), p.Descripcion, false);
                        } else {
                            vm.validarValores(p.Error, false);
                        }
                    });
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarErroresPreguntas = function (error, Descripcion, esValido) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "-mensaje-error");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + Descripcion + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }

        vm.validarValores = function (error, esValido) {
            var idSpanAlertComponent = document.getElementById("alert-" + error);
            if (idSpanAlertComponent != undefined) {
                if (!esValido) {
                    idSpanAlertComponent.classList.add("ico-advertencia");
                } else {
                    idSpanAlertComponent.classList.remove("ico-advertencia");
                }
            }
        }

        vm.validarSeccion = function (tipoError, seccion, errores, esValido) {
            var campomensajeerror = document.getElementById(tipoError + seccion);
            if (campomensajeerror != undefined) {
                if (!esValido) {
                    campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                    campomensajeerror.classList.remove('hidden');
                } else {
                    campomensajeerror.classList.remove("ico-advertencia");
                }
            }
        }

        vm.limpiarErrores = function () {
            var idSpanAlertComponent = document.getElementById("alert-OCDG_VTC");
            idSpanAlertComponent.classList.remove("ico-advertencia");

            var idSpanAlertComponent1 = document.getElementById("alert-OCDG_VF");
            idSpanAlertComponent1.classList.remove("ico-advertencia");

            var idSpanAlertComponent2 = document.getElementById("alert-OCDG_VP");
            idSpanAlertComponent2.classList.remove("ico-advertencia");

            vm.validarValores(vm.nombreComponente, true);

            var errorElements = document.getElementsByClassName('errorSeccionDelegarViabilidadPrevios');
            var testDivs = Array.prototype.filter.call(errorElements, function (errorElement) {
                errorElement.innerHTML = "";
                errorElement.classList.add('hidden');
            });
        
            var campomensajeerror2 = document.getElementById(vm.nombreComponente + "-mensaje-error");
            campomensajeerror2.innerHTML = "";
            campomensajeerror2.classList.add('hidden');
            
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                ObtenerOperacionesCredito();
                eliminarCapitulosModificados();
            }
        }

        function buscarN(restablecer) {
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusq.svg";
            if (restablecer)
                vm.BusquedaRealizada = false;
            else
                vm.BusquedaRealizada = true;
        }

        function limpiarCamposFiltro() {
            vm.ejecutorFiltro.nit = "";
            vm.ejecutorFiltro.tipoEntidadId = null;
            vm.ejecutorFiltro.entidadId = null;
        }

        $scope.$on("BloquearPorCTUSPendiente", function (evt, data) {
            bloquearControles();
        });

        function bloquearControles() {
            $("#txtNIT").attr('disabled', true);
            $("#ddlTipoEntidad").attr('disabled', true);
            $("#ddlEntidad").attr('disabled', true);
            $("#btnAsociar").attr('disabled', true);
            $("#btnEliminar").attr('disabled', true);
            vm.disabled = true;
            vm.showBtn = false;
            vm.dsblBtn = true;
        }
        function ConsultaEjecutoresAsociados() {
            ejecutorProSgrServicio.ObtenerEjecutoresAsociados(vm.proyectoId).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaEjecutoresAsociados = response.data;
                        vm.dsblBtn = true;
                        vm.showBtn = false;
                    } else {
                        if (!vm.disabled) {
                            $("#ddlEntidad").empty();
                            vm.listaEjecutoresAsociados = null;
                            vm.dsblBtn = false;
                            vm.showBtn = true;
                        }
                    }
                    if (vm.erroresComponente != null && vm.erroresComponente != undefined) {
                        $timeout(function () {
                            vm.notificacionValidacionPadre(vm.erroresComponente);
                        }, 600);
                    }
                });
        }
        function ConsultaTodosTiposEntidades() {
            ejecutorProSgrServicio.catalogoTodosTiposEntidades().then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaTipoEntidades = response.data;
                    } else {
                        $("#ddlEntidad").empty();
                    }
                });
        }
        function eliminarEjecutor(entity) {
            var proyectoEjecutorId = entity.Id;

            utilidades.mensajeWarning(
                "¿Está seguro de continuar?",
                function funcionContinuar() {

                    ejecutorProSgrServicio.EliminarEjecutorAsociado(proyectoEjecutorId).then(function () {
                        ConsultaEjecutoresAsociados();
                        eliminarCapitulosModificados();
                        vm.listaEjecutores = null;
                        limpiarCamposFiltro();
                        utilidades.mensajeSuccess("El ejecutor asociado fue eliminado con éxito.", false, false, false);
                    })
                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "El ejecutor asociado será eliminado.");
        }

        vm.eliminarEjecutor = function (entity) {
            eliminarEjecutor(entity);
        }
        function restablecerBusqueda() {
            limpiarCamposFiltro();
            buscarN(true);
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
        }
        function onClickAsociar() {

            var idEjecutor = "";
            var d = document.getElementsByName('radio');
            for (var i = 0; i < d.length; i++) {
                console.log(d[i]);
                if (d[i].checked) {
                    idEjecutor = d[i].value;
                    vm.mostrarBt = false;
                }
            }

            ejecutorProSgrServicio.CrearEjecutorAsociado(vm.proyectoId, idEjecutor, 1).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaTipoEntidades = response.data;
                    } else {
                        utilidades.mensajeSuccess("", false, () => { console.log('El ejecutor propuesto ha sido asociado y guardado con éxito.'); }, null, "El ejecutor propuesto ha sido asociado y guardado con éxito.");
                        ConsultaEjecutoresAsociados();
                        guardarCapituloModificado();
                        limpiarCamposFiltro();
                        vm.limpiarErrores();
                        vm.listaEjecutores = null;
                    }
                });

        }
        function onClickCancelar() {
            vm.listaEjecutores = null;
            vm.mostrarBt = false;
            //utilidades.mensajeInformacionN("", null, null, "Entidad(es) cancelada(s)");

        }
        function buscar() {
            var nit = document.getElementById("txtNit").value;
            var tipoEntidadId = document.getElementById("ddlTipoEntidad").value;
            tipoEntidadId = tipoEntidadId.replace('number:', '');
            var entidadId = document.getElementById("ddlEntidad").value;
            entidadId = entidadId.replace('number:', '');

            ejecutorProSgrServicio.ObtenerEjecutores(nit, tipoEntidadId, entidadId).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.BusquedaRealizada = true;
                        vm.mostrarBt = true;
                        vm.listaEjecutores = response.data;
                        vm.cantidadDeProyectos = response.data.length;
                        vm.totalRegistros = response.data.length;
                    } else {
                        vm.BusquedaRealizada = true;
                        vm.listaEjecutores = null;
                        vm.mostrarBt = false;
                        vm.cantidadDeProyectos = 0;
                        utilidades.mensajeError("No se encontraron resultados para los criterios de búsqueda.");
                    }
                });
        }
    }

    angular.module('backbone').component('ejecutorProSgr', {
        templateUrl: "src/app/formulario/ventanas/SGR/viabilidadSGR/previos/ejecutorViabilidad/ejecutor/ejecutorProSgr.html",
        controller: ejecutorProSgrController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioninicio: '&',
            notificacioncambios: '&'
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
        });;
})();