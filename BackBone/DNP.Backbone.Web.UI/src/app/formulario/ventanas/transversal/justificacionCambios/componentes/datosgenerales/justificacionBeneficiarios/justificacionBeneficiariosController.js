(function () {
    'use strict';

    justificacionBeneficiariosController.$inject = [
        '$scope',
        '$sessionStorage',
        'constantesBackbone',
        'justificacionBeneficiariosServicio',
        'utilidades',
        'justificacionCambiosServicio'
    ];



    function justificacionBeneficiariosController(
        $scope,
        $sessionStorage,
        constantesBackbone,
        justificacionBeneficiariosServicio,
        utilidades,
        justificacionCambiosServicio

    ) {

        $sessionStorage.vigenciabeneficiariosTotales = '';
        var vm = this;
        vm.init = init;
        vm.lang = "es";
        vm.nombreComponente = "datosgeneralesbeneficiarios";
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.idProyecto = $sessionStorage.proyectoId;
        vm.codigoBpin = $sessionStorage.idObjetoNegocio;
        vm.habilitaGuardar = false;
        vm.habilitaGuardarProducto = false;
        vm.habilitaAlertaError = false;
        vm.AlertaMensajeError = "";
        vm.ClasesbtnGuardar = "btnguardarDisabledDNP";
        vm.ClasesbtnGuardarProducto = "btnguardarDisabledDNP";
        vm.ClasesbtnGuardarLocalizacion = "btnguardarDisabledDNP";
        vm.ClasesinputIni = " ";
        vm.ClasesinputFin = " ";
        vm.mensaje1 = "";
        vm.mensaje2 = "";
        vm.habilitarEditar = habilitarEditar;
        vm.habilitarEditarProducto = habilitarEditarProducto;
        vm.habilitarEditarLocalizacion = habilitarEditarLocalizacion;
        vm.habilitar = false;
        vm.habilitarFinal = false;
        vm.habilitarFinalProducto = false;
        vm.verBotones = false;
        vm.Guardar = Guardar;
        vm.Usuario = usuarioDNP;
        vm.mostrarJustOcultar = mostrarJustOcultar;
        vm.beneficiariosTotales = null;
        vm.beneficiariosTotalesAuxiliar = null;
        vm.productoAuxiliar = null;
        vm.ErrorTotales = false;
        vm.ErrorBeneficiariosTotales = "";
        vm.longMaxText = 30;
        vm.guiMacroproceso = justificacionCambiosServicio.getIdEtapa($sessionStorage.etapa);
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.ProductosValidos = true;
        vm.EsValidacion = false;
        vm.TieneErrorCapitulo = false;
        vm.currentYear = new Date().getFullYear();
        vm.alertaBeneficiariosLoc = false;
        vm.obtenerJustificacion;
        vm.idProyecto = $sessionStorage.proyectoId;
        vm.justificacionesBeneficiarios = {
            Total: '',
            Producto: '',
            Localizacion:''
        };
        vm.justificacionesBeneficiariosGuardar = {};
        vm.componentesRefresh = [
            'recursoscostosdelasacti',
            'datosgeneraleslocalizaciones',
            'datosgeneralesindicadoresdepr',
            'datosgeneraleshorizonte'
        ];
        vm.paramJustificacion = {
            ProyectoId: vm.idProyecto,
            SeccionCapituloId: 0,
            InstanciaId: $sessionStorage.idInstancia,
            Justificacion: ''

        };
        vm.parametros = {

            idInstancia: $sessionStorage.idInstancia,
            idFlujo: $sessionStorage.idFlujoIframe,
            idNivel: $sessionStorage.idNivel,
            idProyecto: vm.idProyecto,
            idProyectoStr: $sessionStorage.idObjetoNegocio,
            Bpin: vm.Bpin

        };
        vm.erroresActivos = [];
        vm.mostrarBotones = function (tab, producto) {
            producto.mostrarBotones = tab == 1;
        }

        vm.verCaracteristica = function (localizacion) {
            localizacion.mostrarCaracteristisca = true;
        }

        vm.ocultarCaracteristica = function (localizacion) {
            localizacion.mostrarCaracteristisca = false;
        }
        function init() {
            vm.obtenerJustificacion();
            vm.ObtenerbeneficiariosTotales(vm.BPIN);
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });

            vm.notificacionvalidacion({ handler: vm.notificacionValidacion, nombreComponente: vm.nombreComponente, esValido: true });
        };

        function Guardar(idTab) {
            if (idTab == "1") {
                if (document.getElementById("justificacionBeneficiarioTotal").value == '') {
                    swal('', "Debe ingresar una justificación en la sesión de 'Beneficiarios totales'", 'error');
                    return;
                }
                vm.justificacionesBeneficiariosGuardar = vm.justificacionesBeneficiarios;
                vm.justificacionesBeneficiariosGuardar.Total = document.getElementById("justificacionBeneficiarioTotal").value;
            }
            else if (idTab == "2") {
                if (document.getElementById("justificacionBeneficiarioProducto").value == '') {
                    swal('', "Debe ingresar una justificación en la sesión de 'Beneficiarios del producto'", 'error');
                    return;
                }
                vm.justificacionesBeneficiariosGuardar = vm.justificacionesBeneficiarios;
                vm.justificacionesBeneficiariosGuardar.Producto = document.getElementById("justificacionBeneficiarioProducto").value;
            }
            else if (idTab == "3") {
                if (document.getElementById("justificacionBeneficiarioLocalizacion").value == '') {
                    swal('', "Debe ingresar una justificación en la sesión de 'Beneficiarios por localización y característica poblacional'", 'error');
                    return;
                }
                vm.justificacionesBeneficiariosGuardar = vm.justificacionesBeneficiarios;
                vm.justificacionesBeneficiariosGuardar.Localizacion = document.getElementById("justificacionBeneficiarioLocalizacion").value;
            }

            var seccionCapituloId = document.getElementById("id-capitulo-datosgeneralesbeneficiarios");
            var seccionCapituloIdValor = seccionCapituloId != undefined ? seccionCapituloId.innerHTML : 0;
            vm.paramJustificacion.ProyectoId = vm.idProyecto;
            vm.paramJustificacion.Justificacion = JSON.stringify(vm.justificacionesBeneficiariosGuardar);
            vm.paramJustificacion.SeccionCapituloId = seccionCapituloIdValor;
            vm.paramJustificacion.AplicaJustificacion = 1;
            vm.paramJustificacion.Modificado = 1;
            utilidades.mensajeWarning("Se va a actualizar la Justificación, desea Continuar?", function funcionContinuar() {
                justificacionCambiosServicio.guardarCambiosFirme(vm.paramJustificacion).then(function (response) {

                    if (response.statusText === "OK" || response.status === 200) {
                        parent.postMessage("cerrarModal", window.location.origin);
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.justificacionesBeneficiarios.Total = document.getElementById("justificacionBeneficiarioTotal").value;
                        if (idTab == "1") {
                            vm.habilitarFinal = !vm.habilitarFinal;
                            vm.habilitaGuardar = !vm.habilitaGuardar;
                            vm.ClasesbtnGuardar = vm.habilitaGuardar ? "btnguardarDNP" : "btnguardarDisabledDNP";
                        }
                        else if (idTab == "2") {
                            vm.habilitarFinalProducto = !vm.habilitarFinalProducto;
                            vm.habilitaGuardarProducto = !vm.habilitaGuardarProducto;
                            vm.ClasesbtnGuardarProducto = vm.habilitaGuardarProducto ? "btnguardarDNP" : "btnguardarDisabledDNP";
                        }
                        else if (idTab == "3") {
                            vm.habilitarFinalLocalizacion = !vm.habilitarFinalLocalizacion;
                            vm.habilitaGuardarLocalizacion = !vm.habilitaGuardarLocalizacion;
                            vm.ClasesbtnGuardarLocalizacion = vm.habilitaGuardarLocalizacion ? "btnguardarDNP" : "btnguardarDisabledDNP";
                        }
                    } else {
                        swal('', response.data.Mensaje, 'warning');
                    }
                });

            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            });

        }

        function habilitarEditar() {

            if (vm.habilitaGuardar == true) {
                utilidades.mensajeWarning("La justificación que haya diligenciado en la sesión 'Beneficiarios totales' se perdera. ¿esta seguro de continuar?", function funcionContinuar() {
                    
                    document.getElementById("justificacionBeneficiarioTotal").value = vm.justificacionesBeneficiarios.Total;
                    OkCancelar();


                    ToggleEditarCancelarTotales();

                }, function funcionCancelar(reason) {
                    return;
                });

            }
            else {

                ToggleEditarCancelarTotales();
            }
        }

        function ToggleEditarCancelarTotales() {
            vm.habilitarFinal = !vm.habilitarFinal;
            vm.habilitaGuardar = !vm.habilitaGuardar;
            vm.ClasesbtnGuardar = vm.habilitaGuardar ? "btnguardarDNP" : "btnguardarDisabledDNP";
        }

        function habilitarEditarProducto() {

            if (vm.habilitaGuardarProducto == true) {


                utilidades.mensajeWarning("La justificación que haya diligenciado en la sesión 'Beneficiarios del producto' se perdera. ¿esta seguro de continuar?", function funcionContinuar() {
                    document.getElementById("justificacionBeneficiarioProducto").value = vm.justificacionesBeneficiarios.Producto;
                    OkCancelar();

                    ToggleEditarCancelarProducto();

                }, function funcionCancelar(reason) {
                    return;
                });
            }
            else {

                ToggleEditarCancelarProducto();
            }
        }

        function habilitarEditarLocalizacion() {

            if (vm.habilitaGuardarLocalizacion == true) {


                utilidades.mensajeWarning("La justificación que haya diligenciado en la sesión 'Beneficiarios por localización y característica poblacional' se perdera. ¿esta seguro de continuar?", function funcionContinuar() {
                    document.getElementById("justificacionBeneficiarioLocalizacion").value = vm.justificacionesBeneficiarios.Localizacion;
                    OkCancelar();

                    ToggleEditarCancelarLocalizacion();

                }, function funcionCancelar(reason) {
                    return;
                });
            }
            else {
                vm.productoAuxiliar = JSON.stringify(vm.beneficiariosTotales.BeneficiariosProducto);

                ToggleEditarCancelarLocalizacion();
            }
        }

        function ToggleEditarCancelarProducto() {

            vm.habilitarFinalProducto = !vm.habilitarFinalProducto;
            vm.habilitaGuardarProducto = !vm.habilitaGuardarProducto;
            vm.ClasesbtnGuardarProducto = vm.habilitaGuardarProducto ? "btnguardarDNP" : "btnguardarDisabledDNP";
        }

        function ToggleEditarCancelarLocalizacion() {

            vm.habilitarFinalLocalizacion = !vm.habilitarFinalLocalizacion;
            vm.habilitaGuardarLocalizacion = !vm.habilitaGuardarLocalizacion;
            vm.ClasesbtnGuardarLocalizacion = vm.habilitaGuardarLocalizacion ? "btnguardarDNP" : "btnguardarDisabledDNP";
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.ObtenerbeneficiariosTotales(vm.BPIN);
            }
        }

        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "Se ha cancelado la edición.");
            }, 500);
        }

        vm.ObtenerbeneficiariosTotales = function (bpin) {

            var idInstancia = $sessionStorage.idNivel;

            return justificacionBeneficiariosServicio.ObtenerJustificacionbeneficiariosTotales(bpin, usuarioDNP, idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {

                        vm.beneficiariosTotales = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));

                        console.log('BeneficiariosTotales: ', vm.beneficiariosTotales);
                    }
                });
        };

        vm.obtenerJustificacion = function () {
            setTimeout(function () {
                var seccionCapituloId = document.getElementById("id-capitulo-datosgeneralesbeneficiarios");
                vm.seccionCapituloId = seccionCapituloId != undefined && seccionCapituloId.innerHTML != '' ? seccionCapituloId.innerHTML : 0;
                return justificacionBeneficiariosServicio.obtenerCapitulosModificados(vm.guiMacroproceso, vm.idProyecto, vm.idInstancia, vm.seccionCapituloId).then(
                    function (respuesta) {
                        if (respuesta.data != null && respuesta.data != "") {
                            try {
                                vm.justificacionesBeneficiarios = jQuery.parseJSON(respuesta.data[0].Justificacion);
                            }
                            catch { }
                            document.getElementById("justificacionBeneficiarioTotal").value = vm.justificacionesBeneficiarios.Total;
                            document.getElementById("justificacionBeneficiarioProducto").value = vm.justificacionesBeneficiarios.Producto;
                            document.getElementById("justificacionBeneficiarioLocalizacion").value = vm.justificacionesBeneficiarios.Localizacion;
                            
                        }
                        else {
                            document.getElementById("justificacionBeneficiarioTotal").value = vm.justificacionesBeneficiarios.Total;
                            document.getElementById("justificacionBeneficiarioProducto").value = vm.justificacionesBeneficiarios.Producto;
                            document.getElementById("justificacionBeneficiarioLocalizacion").value = vm.justificacionesBeneficiarios.Localizacion;
                        }
                    }
                );
            }, 2000);

        }

        function mostrarJustOcultar(objeto) {
            var variable = $("#icoObjetivoJust" + objeto).attr("src");

            if (variable === "Img/btnMas.svg") {
                $("#icoObjetivoJust" + objeto).attr("src", "Img/btnMenos.svg");
            }
            else {
                $("#icoObjetivoJust" + objeto).attr("src", "Img/btnMas.svg");
            }
        }

        vm.mostrarOcultarProductoJust = function (objeto) {
            var variable = $("#icoJust" + objeto).attr("src");

            if (variable === "Img/btnMasn2.svg") {
                $("#icoJust" + objeto).attr("src", "Img/btnMenosn2.svg");
            }
            else {
                $("#icoJust" + objeto).attr("src", "Img/btnMasn2.svg");
            }
        }

        vm.mostrarOcultarProductoJustLoc = function (objeto) {
            var variable = $("#icoJustloc" + objeto).attr("src");

            if (variable === "Img/btnMasn2.svg") {
                $("#icoJustloc" + objeto).attr("src", "Img/btnMenosn2.svg");
            }
            else {
                $("#icoJustloc" + objeto).attr("src", "Img/btnMasn2.svg");
            }

            if (vm.productoConsultado != undefined) {
                if (vm.productoConsultado !== objeto) {
                    $("#icoJustloc" + vm.productoConsultado).attr("src", "Img/btnMasn2.svg");
                }
            }

            vm.productoConsultado = objeto;
        }

        vm.filterLocalizacion = function (item) {
            if (item.LocalizacionProducto != null)
                return true;
            else
                return false;
        }

        vm.filterProducto = function (item) {
            if (item.DetalleBeneficiariosProductoActual == null && item.DetalleBeneficiariosProductoFirme == null)
                return false;
            else
                return true;
        }

        vm.verificarProducto = function (data) {
            var regresa = false;
            if (data != undefined) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].DetalleBeneficiariosProductoActual != null || data[i].DetalleBeneficiariosProductoFirme != null) {
                        regresa = true;
                        i = data.length;
                    }
                }
            }
            return regresa;
            
        }

        vm.verificarLocalizacion = function (data) {
            var regresa = false;
            if (data != undefined) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].LocalizacionProducto != null) {
                        regresa = true;
                        i = data.length;
                    }
                }
            }
            return regresa;

        }

        /* ------------------------ Validaciones ---------------------------------*/
        vm.notificacionValidacion = function (errores) {
            console.log("Validación  - Justificación Beneficiario");
            vm.limpiarErrores();
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => p.Capitulo == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                }
                var isValid = (erroresJson == null || erroresJson.length == 0);
                if (!isValid) {
                    erroresJson.errores.forEach(p => {
                        if (vm.errores[p.Error] != undefined) {
                            vm.erroresActivos.push({
                                Error: p.Error,
                                Descripcion: p.Descripcion
                            });
                            vm.errores[p.Error](p.Error, p.Descripcion);
                        }
                    });
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        };

        vm.limpiarErrores = function () {
            var campoObligatorioJustificacionLoc = document.getElementById("justificacionBeneficiarios-justificacion-error");
            var ValidacionJustificacionLoc = document.getElementById("justificacionBeneficiarios-justificacion-error-mns");
            if (ValidacionJustificacionLoc != undefined) {
                ValidacionJustificacionLoc.innerHTML = '';
                campoObligatorioJustificacionLoc.classList.add('hidden');
            }

            var campoObligatorioJustificacionLocP = document.getElementById("justificacionBeneficiarios-justificacionP-error");
            var ValidacionJustificacionLocP = document.getElementById("justificacionBeneficiarios-justificacionP-error-mns");
            if (ValidacionJustificacionLocP != undefined) {
                ValidacionJustificacionLocP.innerHTML = '';
                campoObligatorioJustificacionLocP.classList.add('hidden');
            }

            var campoObligatorioJustificacionLocL = document.getElementById("justificacionBeneficiarios-justificacionL-error");
            var ValidacionJustificacionLocL = document.getElementById("justificacionBeneficiarios-justificacionL-error-mns");
            if (ValidacionJustificacionLocL != undefined) {
                ValidacionJustificacionLocL.innerHTML = '';
                campoObligatorioJustificacionLocL.classList.add('hidden');
            }
        }

        vm.ejecutarErrores = function () {
            vm.erroresActivos.forEach(p => {
                if (vm.errores[p.Error] != undefined) {
                    vm.errores[p.Error](p.Error, p.Descripcion);
                }
            });
        }

        vm.validarErroresActivos = function (codError) {
            if (vm.erroresActivos != null) {
                vm.erroresActivos = vm.erroresActivos.filter(function (value, index, arr) {
                    return value.Error != codError;
                });
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: (vm.erroresActivos.length <= 0) });
            }
        }

        vm.validarJustificacion = function (errores, descripcion) {
            var campoObligatorioJustificacionLoc = document.getElementById("justificacionBeneficiarios-justificacion-error");
            var ValidacionJustificacionLoc = document.getElementById("justificacionBeneficiarios-justificacion-error-mns");

            if (campoObligatorioJustificacionLoc != undefined) {
                if (ValidacionJustificacionLoc != undefined) {
                    ValidacionJustificacionLoc.innerHTML = '<span>' + descripcion + "</span>";
                    campoObligatorioJustificacionLoc.classList.remove('hidden');
                }
            }

            var campoObligatorioJustificacionLocP = document.getElementById("justificacionBeneficiarios-justificacionP-error");
            var ValidacionJustificacionLocP = document.getElementById("justificacionBeneficiarios-justificacionP-error-mns");

            if (campoObligatorioJustificacionLocP != undefined) {
                if (ValidacionJustificacionLocP != undefined) {
                    ValidacionJustificacionLocP.innerHTML = '<span>' + descripcion + "</span>";
                    campoObligatorioJustificacionLocP.classList.remove('hidden');
                }
            }

            var campoObligatorioJustificacionLocL = document.getElementById("justificacionBeneficiarios-justificacionL-error");
            var ValidacionJustificacionLocL = document.getElementById("justificacionBeneficiarios-justificacionL-error-mns");

            if (campoObligatorioJustificacionLocL != undefined) {
                if (ValidacionJustificacionLocL != undefined) {
                    ValidacionJustificacionLocL.innerHTML = '<span>' + descripcion + "</span>";
                    campoObligatorioJustificacionLocL.classList.remove('hidden');
                }
            }
        }

        vm.errores = {
            'JUST001': vm.validarJustificacion
        }
    }

    angular.module('backbone').component('datosgeneralesbeneficiarios', {

        templateUrl: "src/app/formulario/ventanas/transversal/justificacionCambios/componentes/datosgenerales/justificacionBeneficiarios/justificacionBeneficiarios.html",
        controller: justificacionBeneficiariosController,
        controllerAs: "vm",
        bindings: {
            justificacioncapitulo: '@',
            notificacioncambios: '&',
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
        }
    }).directive('stringToNumber', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$parsers.push(function (value) {

                    return '' + value;
                });
                ngModel.$formatters.push(function (value) {
                    return parseInt(value);
                });
            }
        };
    });

})();