(function () {
    'use strict';

    costoActividadesSinTramiteSgpController.$inject = ['$scope', 'costoActividadesSinTramiteSgpServicio', '$sessionStorage', '$uibModal', 'utilidades', 'utilsValidacionSeccionCapitulosServicio', 'justificacionCambiosServicio'];

    function costoActividadesSinTramiteSgpController(
        $scope,
        costoActividadesSinTramiteSgpServicio,
        $sessionStorage,
        $uibModal,
        utilidades,
        utilsValidacionSeccionCapitulosServicio,
        justificacionCambiosServicio
    ) {
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "recursossgpcostosdelasactisintramitesgp";
        vm.setValidacionErrores = false;
        vm.erroresActivos = null;
        vm.init = init;
        vm.plus = true;
        const listaResumen = [];
        vm.listaResumen;
        let resumenSubtotal = {};
        vm.mensaje = "";
        vm.obj1 = false;
        vm.longMaxText = 200;
        vm.listaCostoAjuste = [];
        vm.listaCostoFirme = [];
        vm.listaCostoMGA = [];
        vm.tblObj = false;
        vm.total = 0;
        vm.pagina = 0;
        vm.numVigencias = [];
        vm.vigencias = [];
        vm.vigenciasTotales = [];
        vm.costoTotal = 0;
        vm.habilitaEditar = habilitaEditar;
        vm.Cancelar = Cancelar;
        vm.Guardar = Guardar;
        vm.vigenciaActual = new Date().getFullYear();
        vm.recorrerObjetivos = recorrerObjetivos;
        vm.recorrerObjetivosNumber = recorrerObjetivosNumber;
        vm.datos;
        vm.validarErrorFuentesCostosHijo = null;
        vm.refrescarResumenHijo = null;
        vm.AgregarEntregable = AgregarEntregable;
        vm.EliminarEntregable = EliminarEntregable;
        vm.productoAuxiliar = null;
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.soloLectura = false;
        vm.componentesRefresh = [
            'datosgeneralessgphorizontesintramitesgp'
        ];
        function init() {
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            vm.vigenciaActual = new Date().getFullYear();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificarrefresco({ handler: vm.refrescarResumenFuentes, nombreComponente: vm.nombreComponente });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });

            vm.ObtenerResumenObjetivosProductosActividades(vm.BPIN);
        }

        vm.validateFormat = function (event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 14;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 18;

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

                    if ((n[1].length == 2 && n[1] > 99) || (n[1].length > 2 && n[1] > 99)) {
                        event.preventDefault();
                    }
                }
            } else {
                if (tamanio > 14 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 14) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 18) {
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
            var tamanioPermitido = 14;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                var indicePunto = event.target.value.toString().indexOf(".");
                var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
                if (decimales > 2) {
                }
                tamanioPermitido = 18;

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

                    if ((n[1].length == 2 && n[1] > 99) || (n[1].length > 2 && n[1] > 99)) {
                        event.preventDefault();
                    }
                }
            }
        }

        function guardarCapituloModificado() {
            var data = {
                ProyectoId: $sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: $('#id-capitulo-recursossgpcostosdelasactisintramitesgp').text(),
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

        function Guardar(producto) {

            producto.HabilitaEditar = false;
            producto.ProyectoId = vm.datos.Proyectoid;

            costoActividadesSinTramiteSgpServicio.GuardarSgp(producto, usuarioDNP)
                .then(function (response) {
                    let exito = response.data;
                    if (response.data.Status) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                        vm.ObtenerResumenObjetivosProductosActividades(vm.BPIN, true);

                        guardarCapituloModificado();
                        return;
                    }
                    else {
                        var mensajeError = JSON.parse(response.data.Message);
                        var mensajeReturn = '';
                        console.log(mensajeError);
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
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });
        }

        function habilitaEditar(productos) {

            vm.productoAuxiliar = JSON.stringify(vm.datos.Objetivos);
            productos.HabilitaEditar = true;
        }

        function AgregarEntregable(productos, objectivo) {

            productos.objectivo = objectivo;

            let modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/costoActividades/modal/agregarEntregableModalSinTramiteSgp.html',
                controller: 'agregarEntregableModalSinTramiteSgpController',
                controllerAs: "vm",
                size: 'lg',
                openedClass: "consola-modal-soportesDNP",
                resolve: {
                    Producto: function () {
                        return productos;
                    }
                },
            });
            modalInstance.result.then(data => {
                if (data != null) {
                    console.log(data);

                    costoActividadesSinTramiteSgpServicio.AgregarEntregableSgp(data, usuarioDNP).then(function (response) {
                        let exito = response.data;
                        if (exito) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            vm.ObtenerResumenObjetivosProductosActividades(vm.BPIN);
                            vm.refrescarResumenHijo();
                            guardarCapituloModificado();
                        }
                        else {
                            utilidades.mensajeError("Error al realizar la operación", false);
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
            });
        }

        function EliminarEntregable(entregable) {

            utilidades.mensajeWarning('¿Se eliminará la línea de información. Los datos que allí haya incluido se perderán. Está seguro de eliminar?', function () {
                costoActividadesSinTramiteSgpServicio.EliminarEntregableSgp(entregable, usuarioDNP)
                    .then(function (response) {
                        let exito = response.data;
                        if (exito) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                            vm.ObtenerResumenObjetivosProductosActividades(vm.BPIN);
                            vm.refrescarResumenHijo();
                            guardarCapituloModificado();
                            return;
                        }
                        else {
                            utilidades.mensajeError("Error al realizar la operación", false);
                        }
                    })
                    .catch(error => {
                        if (error.status == 400) {
                            utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                            return;
                        }
                        utilidades.mensajeError("Error al realizar la operación");
                    });
            }, function () { });
        }
        function OkCancelar() {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }
        function Cancelar(productos) {

            utilidades.mensajeWarning("Los posibles datos que haya diligenciado se perderan. ¿Está seguro de continuar?", function funcionContinuar() {
                OkCancelar();
                productos.HabilitaEditar = false;
                vm.datos.Objetivos = JSON.parse(vm.productoAuxiliar);
                recorrerObjetivos(vm.datos.Objetivos);

            }, function funcionCancelar(reason) {
                return;
            }, null, null, "Advertencia");
        }

        function recorrerObjetivosNumber(event, Objetivos) {

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

            recorrerObjetivos(Objetivos);
        }

        function recorrerObjetivos(Objetivos) {

            vm.listaCostoAjuste = [];
            vm.listaCostoAjuste = [];

            Objetivos.forEach(element => recorrerProductos(element));

            const suma = (a, b) => a + b;
            vm.costoTotal = vm.listaCostoAjuste.reduce(suma);
        }

        function recorrerProductos(Productos) {
            var totalAjuste = 0;
            var totalFirme = 0;
            var totalMGA = 0;
            Productos.Productos.forEach(producto => {
                totalFirme += extraerCostoFirme(producto);
                totalAjuste += extraerCostoAjuste(producto);
                totalMGA += extraerCostoMGA(producto);
                contarVigencias(producto);
                vm.vigenciasTotales.push(vm.vigencias);
                vm.vigencias = [];

                angular.forEach(producto.Vigencias, function (vigencia) {

                    vigencia.TotalVigenciaAJuste = 0;
                    vigencia.TotalVigenciaFirme = 0;
                    vigencia.TotalVigenciaMGA = 0;

                    angular.forEach(vigencia.EntregablesActividades, function (entregable) {

                        if (producto.CatalogoEntregables && entregable.EntregableActividad) {

                            let toDel = producto.CatalogoEntregables.map(function (e) { return e.EntregableNombre; }).indexOf(entregable.EntregableActividad);

                            if (toDel > -1) {
                                producto.CatalogoEntregables.splice(toDel, 1);
                            }
                        }

                        if (entregable.Vigencia == vigencia.Vigencia) {
                            vigencia.TotalVigenciaAJuste += parseFloat(entregable.CostoAjusteProyecto);
                            vigencia.TotalVigenciaFirme += parseFloat(entregable.CostoFirmeProyecto);
                            vigencia.TotalVigenciaMGA += parseFloat(entregable.CostoMGAProyecto);
                        }
                        entregable.TotalCostoAjusteProyecto = 0;
                        entregable.TotalCostoFirmeProyecto = 0;
                        entregable.TotalCostoMGAProyecto = 0;

                        angular.forEach(producto.Vigencias, function (vigenciaAux) {
                            angular.forEach(vigenciaAux.EntregablesActividades, function (entregableAux) {

                                if (entregable.EntregableActividadId == entregableAux.EntregableActividadId) {
                                    entregable.TotalCostoAjusteProyecto += parseFloat(entregableAux.CostoAjusteProyecto);
                                    entregable.TotalCostoFirmeProyecto += parseFloat(entregableAux.CostoFirmeProyecto);
                                    entregable.TotalCostoMGAProyecto += parseFloat(entregableAux.CostoMGAProyecto);
                                }
                            });
                        });
                    });
                });
            });

            vm.listaCostoFirme.push(totalFirme);
            vm.listaCostoAjuste.push(totalAjuste);
            vm.listaCostoMGA.push(totalMGA);
        }
        function contarVigencias(productos) {
            productos.Vigencias.forEach(element => vm.vigencias.push(element.Vigencia))
            vm.numVigencias.push(vm.vigencias.length);
        }
        function extraerCostoFirme(objeto) {
            return objeto.CostoFirme;
        }

        function extraerCostoAjuste(objeto) {
            return objeto.CostoAjuste;
        }

        function extraerCostoMGA(objeto) {
            return objeto.CostoMGA;
        }

        vm.ObtenerResumenObjetivosProductosActividades = function (bpin, guardado = false) {
            vm.listaCostoAjuste = [];
            vm.listaCostoAjuste = [];
            costoActividadesSinTramiteSgpServicio.ObtenerResumenObjetivosProductosActividadesSgp($sessionStorage.idInstancia).then(

                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.datos = respuesta.data;
                        recorrerObjetivos(vm.datos.Objetivos);
                        console.log("mostrando los objetivos");
                        console.log(vm.datos.Objetivos);
                        vm.refrescarResumenHijo();
                        vm.soloLectura = $sessionStorage.soloLectura;
                        if (guardado) {
                            vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                        }
                    }
                }
            );

        };

        vm.RefrescarSoloResumen = function (bpin) {
            costoActividadesSinTramiteSgpServicio.ObtenerResumenObjetivosProductosActividadesSgp($sessionStorage.idInstancia).then(
                function (respuesta) {
                    if (respuesta.data != null && respuesta.data != "") {
                        vm.datos = respuesta.data;

                        vm.refrescarResumenHijo();
                    }
                }
            );

        };

        //Métodos
        vm.mostrarOcultarFlujo = mostrarOcultarFlujo;
        vm.mostrarOcultar = mostrarOcultar;
        vm.mostrarOcultarTB = mostrarOcultarTB;
        vm.paginacionAnt = paginacionAnt;
        vm.paginacionSig = paginacionSig;
        vm.cambiarPagina = cambiarPagina;
        vm.validarLongitud = validarLongitud;
        vm.validarLongitudObjetivo = validarLongitudObjetivo;
        vm.clickVerMas = clickVerMas;

        function validarLongitud(valor) {
            if (valor.length < vm.longMaxText) {
                return true;
            }
            else return false;
        }
        function clickVerMas(ide, texto) {
            $("#" + ide).html(texto);
        }

        function validarLongitudObjetivo(valor) {
            if (valor.length < vm.longMaxText) {
                return true;
            }
            else return false;
        }
        function clickVerMas(ide, texto) {
            $("#" + ide).html(texto);
        }
        function cambiarPagina(index) {
            vm.pagina = index;
        }

        function paginacionSig(index) {
            if (vm.pagina + 3 < vm.numVigencias[index]) vm.pagina += 3;
        }

        function paginacionAnt(index) {
            if (vm.pagina > 3) vm.pagina -= 3;
            else vm.pagina = 0;
        }

        function mostrarOcultarFlujo() {
            vm.mostrarFlujo = !vm.mostrarFlujo;
            if (vm.mostrarFlujo) {
                vm.mensaje = vm.mensajeCompleto;
            }
            else {
                vm.mensaje = vm.mensajeCompleto.substring(0, 78) + "...VER MAS";
            }
        }

        function mostrarOcultarProducto(productoId) {
            var variable = $("#ico" + fuenteId)[0].innerText;
            var listaVigencias = [];
            if (variable === "+") {
                $("#ico" + fuenteId).html('-');
                listaSinCofin.forEach(objLista => {
                    if (objLista.FuenteId == fuenteId && objLista.vigencias != null) {
                        for (var ov = 0; ov < objLista.vigencias.length; ov++) {
                            listaVigencias.push({
                                fuenteId: fuenteId,
                                vigencia: objLista.vigencias[ov].Vigencia,
                                ValorTotal: objLista.vigencias[ov].ValorTotal
                            });
                        }
                    }
                });
                vm.ListaVigencias = listaVigencias;
            }
            else {
                $("#ico" + fuenteId).html('+');
            }
        }

        function mostrarOcultar(objeto) {
            var variable = $("#ico" + objeto).attr("src");

            if (variable === "Img/btnMas.svg") {
                $("#ico" + objeto).attr("src", "Img/btnMenos.svg");
            }
            else {
                $("#ico" + objeto).attr("src", "Img/btnMas.svg");
            }
        }

        function mostrarOcultarTB(objeto) {
            var variable = $("#ico" + objeto).attr("src");

            if (variable === "Img/btnMasTb.svg") {
                $("#ico" + objeto).attr("src", "Img/btnMenosTb.svg");
            }
            else {
                $("#ico" + objeto).attr("src", "Img/btnMasTb.svg");
            }
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.init();
            }
        }

        /* ------------------------- Gestión de errores -------------------------------- */

        vm.limpiarErrores = function () {
            vm.erroresActivos.forEach(p => {
                vm.validarErrorActividades(p.Error, '', p.Data)

            });
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-Valida-Costo");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
            var campoObligatorioJustificacion1 = document.getElementById(vm.nombreComponente + "-Valida-Costo1");
            if (campoObligatorioJustificacion1 != undefined) {
                campoObligatorioJustificacion1.innerHTML = "";
                campoObligatorioJustificacion1.classList.add('hidden');
            }
        }

        vm.ejecutarErrores = function () {
            vm.limpiarErrores();
            vm.erroresActivos.forEach(p => {
                if (vm.errores[p.Error] != undefined) {
                    vm.errores[p.Error](p.Error, p.Descripcion, p.Data);
                }
            });
        }

        vm.validarErroresActivos = function (codError) {
            if (vm.erroresActivos != null) {
                vm.erroresActivos = vm.erroresActivos.filter(p => p.Error != codError);
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: (vm.erroresActivos.length <= 0) });
            }
        }

        /**
         * Función validación Error = COST001
         * @param {any} codError
         * @param {any} descErrores
         */
        vm.validarErrorActividades = function (codError, descErrores, data) {
            if (data != null) {
                var dataObj = JSON.parse(data);
                dataObj.forEach(p => {
                    var objetivoEspecificoEl = document.getElementById("alert-objetivo-" + p.ObjetivoId);
                    var productoEl = document.getElementById("alert-objetivo-" + p.ObjetivoId + '-producto-' + p.ProductoId);
                    var errorEl = document.getElementById("alert-objetivo-" + p.ObjetivoId + '-producto-' + p.ProductoId + "-error");
                    var errorHtml = '<img class="img-advertencia mr-2" src="Img/u11.svg"/>';
                    if (descErrores != '') {
                        if (objetivoEspecificoEl != undefined) objetivoEspecificoEl.innerHTML = errorHtml;
                        if (productoEl != undefined) productoEl.innerHTML = errorHtml;
                    } else {
                        if (objetivoEspecificoEl != undefined) objetivoEspecificoEl.innerHTML = ''
                        if (productoEl != undefined) productoEl.innerHTML = ''
                    }
                    errorHtml += descErrores
                    if (errorEl != undefined) errorEl.innerHTML = errorHtml;
                });
            }
        }


        /**
         * Función validación Error = COST004
         * @param {any} codError
         * @param {any} descErrores
         */
        vm.validarErrorActividadesNuevas = function (codError, descErrores, data) {
            if (data != null) {
                var dataObj = JSON.parse(data);
                dataObj.forEach(p => {
                    var objetivoEspecificoEl = document.getElementById("alert-objetivo-" + p.ObjetivoId);
                    var productoEl = document.getElementById("alert-objetivo-" + p.ObjetivoId + '-producto-' + p.ProductoId);
                    var errorEl = document.getElementById("alert-objetivo-" + p.ObjetivoId + '-producto-' + p.ProductoId + "-error");
                    var errorHtml = '<img class="img-advertencia mr-2" src="Img/u11.svg"/>';
                    var errorActivity = document.getElementById("alert-" + p.ObjetivoId + p.ProductoId + p.ActividadId);

                    if (descErrores != '') {
                        if (objetivoEspecificoEl != undefined) objetivoEspecificoEl.innerHTML = errorHtml;
                        if (productoEl != undefined) productoEl.innerHTML = errorHtml;
                        if (errorActivity != undefined) errorActivity.classList.remove('hidden');
                    } else {
                        if (objetivoEspecificoEl != undefined) objetivoEspecificoEl.innerHTML = ''
                        if (productoEl != undefined) productoEl.innerHTML = ''
                        if (errorActivity != undefined) errorActivity.classList.add('hidden');
                    }
                    errorHtml += descErrores
                    if (errorEl != undefined) errorEl.innerHTML = errorHtml;
                });
            }
        }

        /**
         * Función validación Error = COST005
         * @param {any} codError
         * @param {any} descErrores
         */
        vm.validarErrorCostosVsVigenciasF = function (codError, descErrores, data) {
            if (data != null) {
                var dataObj = JSON.parse(data);
                dataObj.forEach(p => {
                    var objetivoEspecificoEl = document.getElementById("alert-objetivo-" + p.ObjetivoId);
                    var productoEl = document.getElementById("alert-objetivo-" + p.ObjetivoId + '-producto-' + p.ProductoId);
                    var errorEl = document.getElementById("alert-objetivo-" + p.ObjetivoId + '-producto-' + p.ProductoId + "-error");
                    var errorHtml = '<img class="img-advertencia mr-2" src="Img/u11.svg"/>';
                    if (descErrores != '') {
                        if (objetivoEspecificoEl != undefined) objetivoEspecificoEl.innerHTML = errorHtml;
                        if (productoEl != undefined) productoEl.innerHTML = errorHtml;
                    } else {
                        if (objetivoEspecificoEl != undefined) objetivoEspecificoEl.innerHTML = ''
                        if (productoEl != undefined) productoEl.innerHTML = ''
                    }
                    errorHtml += descErrores
                    if (errorEl != undefined) errorEl.innerHTML = errorHtml;
                });
            }
        }

        /**
         * Función validación Error = COST003 - Referencia a sub componente ResumenCostosAjustes
         */

        vm.validarErrorFuentesCostos = function () {
            vm.validarErrorFuentesCostosHijo();
        };

        /* --------------------------------- Notificación de Validaciones ---------------------------*/

        /**
         * Función que recibe listado de errores de su componente padre por medio del binding notificacionvalidacion
         * @param {any} errores
         */
        vm.notificacionValidacionPadre = function (errores) {
            if (errores != undefined) {
                var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
                vm.erroresActivos = erroresFiltrados.erroresActivos;
                vm.ejecutarErrores();
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: erroresFiltrados.isValid });
                vm.setValidacionErrores = (vm.erroresActivos.find(p => p.Error == "COST003") != -1)
            }
        };

        vm.validarErrorCostos = function (errores) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "-Valida-Costo");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }

        vm.validarErrorCostos1 = function (errores) {
            var campomensajeerror = document.getElementById(vm.nombreComponente + "-Valida-Costo1");
            if (campomensajeerror != undefined) {
                campomensajeerror.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span class='pl-2'>" + errores + "</span>";
                campomensajeerror.classList.remove('hidden');
            }
        }

        vm.validacionErroresCost003 = function (handler) {
            vm.validarErrorFuentesCostosHijo = handler;
        }

        vm.refrescarResumen = function (handler) {
            vm.refrescarResumenHijo = handler;
        }

        vm.refrescarResumenFuentes = function () {
            vm.refrescarResumenHijo();
        }

        vm.errores = {
            'COST001': vm.validarErrorActividades,
            'COST001': vm.validarErrorCostos,
            'COST002': vm.validarErrorActividades,
            'COST002': vm.validarErrorCostos1,
            'COST003': vm.validarErrorFuentesCostos,
            'COST004': vm.validarErrorActividadesNuevas,
            'COST005': vm.validarErrorCostosVsVigenciasF,
        }
    }

    angular.module('backbone').component('costoActividadesSinTramiteSgp', {
        templateUrl: "src/app/formulario/ventanas/SGP/ajustesProyectoSGP/componentes/recursosSGP/costoActividades/costoActividadesSinTramiteSgp.html",
        controller: costoActividadesSinTramiteSgpController,
        controllerAs: "vm",
        location: "es-COP",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            notificacioncambios: '&'
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
    });

})();